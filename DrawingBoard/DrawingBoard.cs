// uncomment the define below, to activate the illustration code on the question 'how to move points of a polyline programmatically'
// as in http://drawingboard.codeplex.com/Thread/View.aspx?ThreadId=238338
//
//#define _DEMO_FOR_FERNANDO

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace DrawingBoard
{
    public enum RedimStatus
    {
        Idle = -1,
        None = 0,
        Poly,
        NewPoint,
        MoveSelected,
        Rotate,
        Zoom,
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW
    }

    public enum EditOption
    {
        None = 0,
        Select,
        Ellipse,
        Line,
        Poly,
        Pen,
        Rect,
        RoundRect,
        Arc,
        SimpleText,
        RichText,
        Image,
        AcquireImage,
    }

    public enum EditStatus
    {
        None,
        DrawRect,
        SelectRect,
        Redim,
    }

    /// <summary>
    /// All known operations that can be mapped to keys for keyboard handling
    /// </summary>
    public enum KeyOperation
    {
        None = 0,                   // no operation. for good enumeration habits, it is wise to have a default do-nothing operation.
        SelectMode,           // fall back to 'select' mode.  This is handy if 'Sticky mode' is on, and you want to get rid of it.  A good key to bind it to, would be 'Keys.Escape' for example
        DeSelect,             // clear the selection
        RectMode,
        LineMode,
        EllipseMode,
        RoundRectangleMode,
        SimpleTextMode,
        RichTextMode,
        ImageMode,
        AcquireImageMode,
        ArcMode,
        PolyMode,
        PenMode,
        Load,
        Save,
        LoadObjects,
        SaveSelectedObjects,
        PrintPreview,
        Print,
        GridOff,
        Grid3,
        Grid5,
        Grid8,
        Grid10,
        Grid12,
        Grid15,
        Grid20,
        GridCustom,
        Undo,
        Redo,
        Group,
        UnGroup,
        PolyMerge,
        PolyDeletePoints,
        PolyExtendPoints,
        PolyMirrorX,
        PolyMirrorY,
        PolyMirrorXY,
        ToFront,
        ToBack,
        DeleteSelected,
        CopySelected,
    }

    public delegate void KeyOperationHandler(object sender, KeyOperation keyOperation);

    public partial class DrawingBoard : UserControl
    {
        const int DEFAULT_BOUNDEDCANVAS_WIDTH      = 810;           // these defaults match a typical A4 paper size
        const int DEFAULT_BOUNDEDCANVAS_HEIGHT     = 1140;

        private readonly Cursor         _addPointCursor = getCursor("newPoint3.cur", Cursors.Cross);
        private bool                    _isBoundedCanvas = true;
        private CompositingQuality      _compositingQuality = CompositingQuality.Default;
        private int                     _gridSize;
        private Color                   _gridColor;
        private InterpolationMode       _interpolationMode = InterpolationMode.Default;
        private SmoothingMode           _smoothingMode = SmoothingMode.AntiAlias;
        private TextRenderingHint       _textRenderingHint = TextRenderingHint.AntiAlias;
        private float                   _zoom = 1;
        private Color                   _creationFillColor;
        private Color                   _creationPenColor;
        private float                   _creationPenWidth;
        private bool                    _isCreationFilled;
        private RichtTextForm           _richTextForm;
        private bool                    _fitToGrid = true;
        private bool                    _isMouseSelecting;
        private Bitmap                  _staticDataBitmap;
        private RichTextBox             _richTextBox;
        private RedimStatus             _redimStatus = RedimStatus.Idle;
        private EditStatus              _editStatus = EditStatus.None;
        private bool                    _strobeOmitBackgroundElements;
        private ShapesCollection        _shapesCollection;
        private Point                   _penPrec;
        private Point                   _startD;
        private Point                   _start;
        private Point                   _temp;
        private readonly List<PointWr>  _penPoints = new List<PointWr>();
        private readonly List<PointWr>  _constructionPenPoints = new List<PointWr>();

        private readonly Dictionary<Keys, KeyOperation> _keyMappings = new Dictionary<Keys, KeyOperation>();            // for keyboard-to-operation mapping, as requested by http://drawingboard.codeplex.com/workitem/11504. FV, december 2010

        /// <summary>
        /// The current Edit Option
        /// </summary>
        public EditOption EditOption { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public DrawingBoard()
        {
            SetStyle(ControlStyles.UserPaint |
                    ControlStyles.Opaque | 
                    ControlStyles.DoubleBuffer |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.AllPaintingInWmPaint | 
                    ControlStyles.ResizeRedraw, true);

            InitializeComponent();
            Init();
        }

        /// <summary>
        /// We override this method with a NOP (not calling the base class!) in combination with the AllPaintingInWmPaint control style, and double-buffering,
        /// to reduce flicker!
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // intentional NOP
        }

        /// <summary>
        /// Draw the background
        /// </summary>
        /// <param name="gfx"></param>
        /// <param name="rect"></param>
        private void DrawBackground(Graphics gfx, Rectangle rect)
        {
            if (IsBoundedCanvas)
            {
                // draw the paper 'outside' area
                if (ShowPaperOutside)
                {
                    gfx.Clear(PaperOutsideColor);
                    using (var brush = new SolidBrush(BackColor))
                    {
                        gfx.FillRectangle(brush, rect);
                    }
                }else
                    gfx.Clear(BackColor);

                // draw the canvas bounds
                using (var pen = new Pen(Color.Blue, 0.5f) {DashStyle = DashStyle.Dash})
                {
                    gfx.DrawRectangle(pen, rect);
                }
            }
            else
                gfx.Clear(BackColor);
        }

        /// <summary>
        /// This is the correct way of painting a userdraw control... by overriding the onpaint method
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var gfx = e.Graphics;
            if(DesignMode)
            {
                gfx.DrawString(GetType().FullName + " in Design Mode", Font, Brushes.Green, 10,10);
                return;
            }

            /*
            int startTime = System.DateTime.Now.Second * 10000 + System.DateTime.Now.Millisecond;
            System.Console.WriteLine("startTime:  {0}", startTime.ToString());
            */

            if (_fitToGrid && GridSize > 0)
            {
                _start.X = GridSize*(_start.X/GridSize);
                _start.Y = GridSize*(_start.Y/GridSize);
            }

            // setup the graphics device according to the published preferences
            GraphicSetUp(gfx);


            // the page rect
            int left = (int)((1 + CanvasOriginX) * Zoom);
            int top = (int)((1 + CanvasOriginY) * Zoom);
            int width = (int)(BoundedCanvasWidth * Zoom);
            int height = (int)(BoundedCanvasHeight * Zoom);
            var paperRect = new Rectangle(left, top, width, height);

            // if the _strobeOmitBackgroundElements flag is ON, we don't need to redraw any background elements
            if (!_strobeOmitBackgroundElements)
            {
                // Redraw static objects
                // in the back Layer 
                using (var backGfx = Graphics.FromImage(_staticDataBitmap))
                {
                    GraphicSetUp(backGfx);

                    // draw the background
                    DrawBackground(backGfx, paperRect);

                    #region Background Image
                    if (BackgroundImage != null)
                    {
                        // Initialize the color matrix.
                        // Note the value 0.8 in row 4, column 4.
                        float[][] matrixItems ={ new float[] {1, 0, 0, 0, 0},
                                                   new float[] {0, 1, 0, 0, 0},
                                                   new float[] {0, 0, 1, 0, 0},
                                                   new float[] {0, 0, 0, (float)BackgroundImageAlpha / 255.0f, 0}, 
                                                   new float[] {0, 0, 0, 0, 1}};
                        var colorMatrix = new ColorMatrix(matrixItems);

                        // Create an ImageAttributes object and set its color matrix.
                        var imageAtt = new ImageAttributes();
                        imageAtt.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                        int ddx = CanvasOriginX + BackgroundImageX;
                        int ddy = CanvasOriginY + BackgroundImageY;
                        int ww = BackgroundImage.Width;
                        int hh = BackgroundImage.Height;
                        int maxW = IsBoundedCanvas ? BoundedCanvasWidth : Width;
                        int maxH = IsBoundedCanvas ? BoundedCanvasHeight : Height;
                        switch (BackgroundImageLayout)
                        {
                            case ImageLayout.None:
                                backGfx.DrawImage(BackgroundImage, new Rectangle((int)(ddx * Zoom), (int)(ddy * Zoom), (int)(ww * Zoom), (int)(hh * Zoom)),
                                                0, 0, BackgroundImage.Width, BackgroundImage.Height, GraphicsUnit.Pixel, imageAtt);
                                break;                                
                            case ImageLayout.Tile:
                                {
                                    int h = 0;
                                    gfx.SetClip(new RectangleF(ddx * Zoom, ddy * Zoom, maxW * Zoom, maxH * Zoom), CombineMode.Replace);
                                    while (h < maxH)
                                    {
                                        int w = 0;
                                        while (w < maxW)
                                        {
                                            backGfx.DrawImage(BackgroundImage, new Rectangle((int)((ddx + w) * Zoom), (int)((ddy + h) * Zoom), (int)(ww * Zoom), (int)(hh * Zoom)), 0, 0, BackgroundImage.Width, BackgroundImage.Height, GraphicsUnit.Pixel, imageAtt);
                                            w += ww;
                                        }
                                        h += hh;
                                    }
                                    gfx.ResetClip();
                                }
                                break;
                            case ImageLayout.Zoom:
                            case ImageLayout.Center:
                                backGfx.DrawImage(BackgroundImage, new Rectangle((int)(ddx * Zoom + (maxW - ww) * Zoom / 2),(int)(ddy * Zoom + (maxH - hh) * Zoom / 2),(int)(ww * Zoom), (int)(hh * Zoom)),
                                                0, 0, BackgroundImage.Width, BackgroundImage.Height, GraphicsUnit.Pixel, imageAtt);
                                break;
                            case ImageLayout.Stretch:
                                backGfx.DrawImage(BackgroundImage, new Rectangle((int)(ddx * Zoom), (int)(ddy * Zoom), (int)(maxW * Zoom), (int)(maxH * Zoom)),
                                                0, 0, BackgroundImage.Width, BackgroundImage.Height, GraphicsUnit.Pixel, imageAtt);
                                break;
                        }
                    }
                    #endregion 

                    // Render the grid
                    if (GridSize > 0)
                    {
                        using (var pen = new Pen(GridColor))
                        {
                            var nX = (int) (Width/(GridSize*Zoom));
                            for (int i = 0; i <= nX; i++)
                                backGfx.DrawLine(pen, i*GridSize*Zoom, 0, i*GridSize*Zoom, Height);
                            var nY = (int) (Height/(GridSize*Zoom));
                            for (int i = 0; i <= nY; i++)
                                backGfx.DrawLine(pen, 0, i*GridSize*Zoom, Width, i*GridSize*Zoom);
                        }
                    }

                    // Draws unselected objects
                    _shapesCollection.DrawUnselected(backGfx, CanvasOriginX, CanvasOriginY, Zoom);
                }
            }
            _strobeOmitBackgroundElements = false;


            // draw the background
            DrawBackground(gfx, paperRect);

            // draw the background image with static objects
            gfx.DrawImageUnscaled(_staticDataBitmap, 0, 0);

            // draw the dynamic objects on the buffer
            _shapesCollection.DrawSelected(gfx, CanvasOriginX, CanvasOriginY, Zoom);

            //////////////////////////////////////////////////////////////////
            // draw the graphics effects (creation and selection)
            #region Creation/Selection/PenPoints plus A4 margin

            // draw temporary construction Rect/Line
            int sx = (int)((_start.X + CanvasOriginX) * Zoom);
            int sy = (int)((_start.Y + CanvasOriginY) * Zoom); 

            if (_isMouseSelecting && _editStatus == EditStatus.DrawRect)
            {
                using (var pen = new Pen(Color.Red, 1.5f)
                                       {
                                           DashStyle = DashStyle.Dot, StartCap = LineCap.DiamondAnchor
                                       })
                {
                    if (EditOption == EditOption.Line || EditOption == EditOption.Poly)
                        gfx.DrawLine(pen, sx, sy, (_temp.X + CanvasOriginX)*Zoom, (_temp.Y + CanvasOriginY)*Zoom);
                    else
                    {
                        int ex = (int) ((_temp.X + CanvasOriginX)*Zoom);
                        int ey = (int) ((_temp.Y + CanvasOriginY)*Zoom);
                        ShapesCollection.SanitizeRect(ref sx, ref sy, ref ex, ref ey);
                        gfx.DrawRectangle(pen, sx, sy, ex - sx, ey - sy);
                    }
                }
            }

            // draw selection Rect
            if (_isMouseSelecting && _editStatus == EditStatus.SelectRect)
            {
                using (var pen = new Pen(Color.Green, 1.5f) {DashStyle = DashStyle.Dash})
                {
                    int ex = (int)((_temp.X + CanvasOriginX) * Zoom);
                    int ey = (int)((_temp.Y + CanvasOriginY) * Zoom);
                    ShapesCollection.SanitizeRect(ref sx, ref sy, ref ex, ref ey);
                    gfx.DrawRectangle(pen, sx, sy, ex - sx, ey - sy);
                }
            }

            //Draw Pen construction shape
            if (_constructionPenPoints.Count > 0)
            {
                using (var pen = new Pen(Color.Red, 1.5f)
                                       {
                                           DashStyle = DashStyle.Dot,
                                           StartCap = LineCap.DiamondAnchor
                                       })
                {
                    var myArr = new PointF[_constructionPenPoints.Count];
                    int i = 0;
                    foreach (var p in _constructionPenPoints)
                        myArr[i++] = new PointF((_start.X + p.X + CanvasOriginX)*Zoom, (_start.Y + p.Y + CanvasOriginY)*Zoom); // p.point;

                    if (myArr.Length > 1)
                        gfx.DrawCurve(pen, myArr);
                }
            }

            #endregion

            //////////////////////////////////////////////////////////////////
            /*
            int endTime = System.DateTime.Now.Second * 10000 + System.DateTime.Now.Millisecond;
            System.Console.WriteLine("endTime:  {0}", endTime.ToString());
            */
        }


        //Graphic
        [Category("Graphics"), Description("Interpolation Mode")]
        public InterpolationMode InterpolationMode
        {
            get { return _interpolationMode; }
            set { _interpolationMode = value; }
        }


        [Category("Graphics"), Description("Smoothing Mode")]
        public SmoothingMode SmoothingMode
        {
            get { return _smoothingMode; }
            set { _smoothingMode = value; }
        }

        [Category("Graphics"), Description("Text Rendering Hint")]
        public TextRenderingHint TextRenderingHint
        {
            get { return _textRenderingHint; }
            set { _textRenderingHint = value; }
        }

        [Category("Graphics"), Description("Compositing Quality")]
        public CompositingQuality CompositingQuality
        {
            get { return _compositingQuality; }
            set { _compositingQuality = value; }
        }


        [Category("Canvas"), Description("Canvas")]
        public string ObjectType
        {
            get { return "Canvas"; }
        }

        [Category("Grid"), Description("Grid Size")]
        public int GridSize
        {
            get { return _gridSize; }
            set
            {
                if (value >= 0)
                {
                    _gridSize = value;
                }
                if (_gridSize > 0)
                {
                    CanvasOriginX = _gridSize*(CanvasOriginX/_gridSize);
                    CanvasOriginY = _gridSize*(CanvasOriginY/_gridSize);
                }
                Invalidate();
            }
        }

        [Category("Grid"), Description("Grid Color")]
        public Color GridColor
        {
            get { return _gridColor; }
            set
            {
                if (value == _gridColor)
                    return;
                _gridColor = value;
                Invalidate();
            }
        }

        [Category("Canvas"), Description("Is the canvas bounded")]
        public bool IsBoundedCanvas
        {
            get { return _isBoundedCanvas; }
            set { _isBoundedCanvas = value; }
        }

        [Category("Canvas"), Description("Show Outside of Paper Area")]
        public bool ShowPaperOutside
        {
            get; set;
        }
        [Category("Canvas"), Description("Outside Paper Area Color")]
        public Color PaperOutsideColor
        {
            get; set;
        }

        [Category("Canvas"), Description("Canvas OriginX")]
        public int CanvasOriginX { get; set; }

        [Category("Canvas"), Description("Canvas OriginY")]
        public int CanvasOriginY { get; set; }

        [Category("Canvas"), Description("Bounded Canvas Width")]
        public int BoundedCanvasWidth { get; set; }

        [Category("Canvas"), Description("Bounded Canvas Height")]
        public int BoundedCanvasHeight { get; set; }

        [Category("Canvas"), Description("Canvas Zoom")]
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                if (Zoom == value)
                    return;
                if (value > 0)
                {
                    _zoom = value;
                    Invalidate();
                }
                else
                {
                    _zoom = 1;
                    Invalidate();
                }
            }
        }

        [Category("Appearance"), Description("Background Image OriginX")]
        public int BackgroundImageX { get; set; }

        [Category("Appearance"), Description("Background Image OriginY")]
        public int BackgroundImageY { get; set; }

        [Category("Appearance"), Description("Background Image Alpha")]
        public byte BackgroundImageAlpha { get; set; }

        // the 'Sticky Edit Option' is the behaviour that keeps the current tool active (true) versus switching back to selection mode, each time the user added a shape (false) 
        [Category("Behavior"), Description("Toolbox Sticky Edit Option")]
        public bool StickyEditOption { get; set; }

        public event OptionChanged OnOptionChanged;
        public event ObjectSelected OnShapeSelected;
        public event KeyOperationHandler OnKeyOperation;

        private void TiggerOnKeyOperation(KeyOperation keyOperation)
        {
            var handler = OnKeyOperation;
            if (handler != null)
                handler(this, keyOperation);
        }

        public static Cursor getCursor(string a, Cursor defCur)
        {
            try
            {
                return new Cursor(a);
            }
            catch
            {
                return defCur;
            }
        }


        public void ZoomIn()
        {
            Zoom = (int) (Zoom + 1);
        }

        public void ZoomOut()
        {
            if (Zoom > 1)
            {
                Zoom = (int) (Zoom - 1);
            }
        }

        public void MergePolygons()
        {
            _shapesCollection.mergePolygons();
            Invalidate();
        }

        public void DeletePoints()
        {
            _shapesCollection.RemovePoint();
            Invalidate();
        }

        public void ExtPoints()
        {
            _shapesCollection.PolysFromSelectedPoints();
            Invalidate();
        }


        public void XMirror()
        {
            _shapesCollection.XMirror();
            Invalidate();
        }

        public void YMirror()
        {
            _shapesCollection.YMirror();
            Invalidate();
        }

        public void Mirror()
        {
            _shapesCollection.Mirror();
            Invalidate();
        }

        protected virtual void Init()
        {
            BoundedCanvasWidth = DEFAULT_BOUNDEDCANVAS_WIDTH;
            BoundedCanvasHeight = DEFAULT_BOUNDEDCANVAS_HEIGHT;

            StickyEditOption = true;
            BackgroundImageAlpha = 255;
            GridColor = Color.Gainsboro;
            SetEditStatus(EditStatus.None);
            EditOption = EditOption.Select;
            _shapesCollection = new ShapesCollection();

            _richTextForm = new RichtTextForm();
            _richTextBox = _richTextForm.richTextBox1;

            _creationPenColor = Color.Black;
            _creationPenWidth = 1f;
            _creationFillColor = Color.Black;
            _isCreationFilled = false;

            OnOptionChanged += FakeOptionChange;
            OnShapeSelected += FakeObjectSelected;

            _staticDataBitmap = new Bitmap(Width, Height);

            SetDefaultKeyMappings();
        }

        private void FakeOptionChange(object sender, OptionEventArgs e)
        {
        }

        private void FakeObjectSelected(object sender, PropertyEventArgs e)
        {
        }


        private void SetEditStatus(EditStatus status)
        {
            _editStatus = status;
        }

        private void changeOption(EditOption option)
        {
            EditOption = option;
            // Notify Option change to "listening object" (i.e: toolBbox)
            OnOptionChanged(this, new OptionEventArgs(EditOption)); // raise event
        }

        public void PrintPreview(float zoom)
        {
            InitializePrintPreviewControl(zoom);
        }

        public bool UndoEnabled()
        {
            return _shapesCollection.UndoEnabled();
        }

        public bool RedoEnabled()
        {
            return _shapesCollection.RedoEnabled();
        }

        public void Undo()
        {
            // this.s = this.undoB.Undo();
            // this.redraw();
            _shapesCollection.Undo();
            _shapesCollection.deSelect();
            Invalidate();
        }

        public void Redo()
        {
            _shapesCollection.Redo();
            _shapesCollection.deSelect();
            Invalidate();
        }


        public void addPointSet(List<PointWr> a)
        {
            var tmpa = new List<PointWr>();
            foreach (PointWr p in a)
                tmpa.Add(new PointColor(p.point) {col = Color.Red});

            //this.s.addPoly(0, 0, 0, 0, Color.Black, Color.Black, 1, false, a, true);
            _shapesCollection.addColorPoinySet(0, 0, 0, 0, Color.Black, Color.Black, 1, false, tmpa, true);
        }

        public List<PointWr> getPointSet()
        {
            if (_shapesCollection.selectedElement != null)
                if (_shapesCollection.selectedElement is PointSet)
                    return ((PointSet) _shapesCollection.selectedElement).getRealPosPoints();

            return null;
        }


        public void propertyChanged()
        {
            _shapesCollection.Propertychanged();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (Width <= 0 || Height <= 0) 
                return;
            _staticDataBitmap = new Bitmap(Width, Height);
            Invalidate();
        }

        #region DRAWING

        private void GraphicSetUp(Graphics gfx)
        {
            gfx.CompositingQuality    = CompositingQuality;
            gfx.TextRenderingHint     = TextRenderingHint;
            gfx.SmoothingMode         = SmoothingMode;
            gfx.InterpolationMode     = InterpolationMode;
        }

        #endregion

        #region MOUSE EVENT MANAGMENT

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _start.X = (int) ((e.X)/Zoom - CanvasOriginX);
            _start.Y = (int) ((e.Y)/Zoom - CanvasOriginY);

            if ((e.Button & MouseButtons.Left) != 0)
            {
                #region START LEFT MOUSE BUTTON PRESSED

                _isMouseSelecting = true;
                switch (EditOption)
                {
                    case EditOption.Select:

                        if (_redimStatus != RedimStatus.None)
                        {
                            // I'm over an object or Object redim handle
                            SetEditStatus(EditStatus.Redim);
                        }
                        else
                        {
                            // pressing mouse in an empty space means: starting a select rect
                            SetEditStatus(EditStatus.SelectRect);
                        }

                        break;
                    case EditOption.Pen:
                        _penPoints.Clear();
                        _constructionPenPoints.Clear();
                        _penPrec.X = _start.X;
                        _penPrec.Y = _start.Y;
                        _penPoints.Add(new PointWr(0, 0));
                        _constructionPenPoints.Add(new PointWr(0, 0));
                        break;
                    default:
                        SetEditStatus(EditStatus.DrawRect); // starting a new object
                        break;
                }

                #endregion
            }
            else if((e.Button & MouseButtons.Right) != 0)
            {
                #region START RIGHT MOUSE BUTTON PRESSED

                    _startD.X = (int)((e.X - CanvasOriginX * Zoom));
                    _startD.Y = (int)((e.Y - CanvasOriginY * Zoom));
                    Cursor = Cursors.Cross;

                #endregion
            }
        }
        

   #if _DEMO_FOR_FERNANDO
        // this demo for Fernando, illustrates how to move some points programmatically, of a polyline
        // answer to the board question on 14 december 2010, http://drawingboard.codeplex.com/Thread/View.aspx?ThreadId=238338
        //
        // I guess this code can be ripped out again later.. I'll leave it here for now, just for illustration purposes.  OK for everyone? ;-)
        


        // some hacky member variable for this demo, to remember our 'old' mouse location in between mouse moves, so we can calculate the 'delta' movement
        private Point oldMouse;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // call the baseclass first, being a good .Net citizen
            base.OnMouseMove(e);

            /////////////////////////////
            // some demo code to show how to move points of a polyline (a 'PointSet', in Drawingboard lingo) programmatically.
            // actually, we move it by mouse (with the MIDDLE mouse button pressed), but the mouse delta movement could as well be you granny's shoesize... just demonstrating!
            if ((e.Button & MouseButtons.Middle) != 0)
            {
                bool redraw = false;                        // a flag that signals us if we need to redraw when done... quick hack

                int dx = (int) ((e.X - oldMouse.X)/Zoom);   // get the screen-relative mouse movement, from the current and precious mouse location, and the DrawingBoard's zoom.
                int dy = (int) ((e.Y - oldMouse.Y)/Zoom);

                // let's just iterate over all shapes in the _shapesCollection, and move some points of the polylines ('PoinSet' shapes, in DrawingBoard lingo)
                foreach (var v in _shapesCollection.List)
                {
                    if (v is PointSet)                      // we only want to move polylines in this demo, no other shapes
                    {
                        var ps = v as PointSet;             // cast v to a PointSet for convenience

                        // let's move all points of the polyline.  (if you want to move only the first, then... guess what.. do it on the first one only, likke illustrated below, in comments ;-)
                        foreach (var point in ps.Points)   
                        {
                            point.X += dx;                  // we add to the coordinates... do your own mojo with the coordinates
                            point.Y += dy;
                        }

                        // or.. example: to move only the first point, we could do something like:
                        // ps.Points[0].X += dx;
                        // ps.Points[0].Y += dy;


                        // since we have movement, we will want to redraw later, when done!
                        redraw = true;                      

                        ///////////////////////////////////////////////////
                        // and finally, some cosmetics:
                        //
                        // for selection handle cleanup, we just need to update the handles, if there are any, so they move along
                        if (_shapesCollection.selectedElement != null)
                        {
                            
                            if (_shapesCollection.selectedElement is PointSet)
                            {
                                ((PointSet) _shapesCollection.selectedElement).setupSize();
                                _shapesCollection.sRec = new SelPoly(_shapesCollection.selectedElement);
                            }
                            
                            if (_redimStatus != RedimStatus.None)
                                _shapesCollection.EndMove();

                            if (_shapesCollection.sRec != null)
                                _shapesCollection.sRec.endMoveRedim();
                        }

                    }
                }

                // if we needed to redraw, do it now! (otherwise our change wouldn't be visible straight away)
                if(redraw)
                    Invalidate();
            }

            // just remember our old mouse location for next time.... demo code of course
            oldMouse = e.Location;          

            // by all means: this is JUST DEMO CODE... never hardcode stuff in this way.. it's just to illustrate how to move points of a polyline programmatically.

            ////////
#else
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // call the baseclass first, being a good .Net citizen
            base.OnMouseMove(e);

#endif
            // handle the left mouse button, etc.
            if ((e.Button & MouseButtons.Left) != 0)
            {

                if (_isMouseSelecting)
                {
                    _temp.X = (int) (e.X/Zoom);
                    _temp.Y = (int) (e.Y/Zoom);
                    if (_fitToGrid && GridSize > 0)
                    {
                        _temp.X = GridSize*(int) ((e.X/Zoom)/GridSize);
                        _temp.Y = GridSize*(int) ((e.Y/Zoom)/GridSize);
                    }
                    _temp.X -= CanvasOriginX;
                    _temp.Y -= CanvasOriginY;
                }

                if (_editStatus == EditStatus.Redim)
                {
                    var tmpX = (int) (e.X/Zoom - CanvasOriginX);
                    var tmpY = (int) (e.Y/Zoom - CanvasOriginY);
                    int tmpstartX = _start.X;
                    int tmpstartY = _start.Y;
                    if (_fitToGrid && GridSize > 0)
                    {
                        tmpX = GridSize*(int) ((e.X/Zoom - CanvasOriginX)/GridSize);
                        tmpY = GridSize*(int) ((e.Y/Zoom - CanvasOriginY)/GridSize);
                        tmpstartX = GridSize*(_start.X/GridSize);
                        tmpstartY = GridSize*(_start.Y/GridSize);
                        _shapesCollection.FitToGrid(GridSize);
                        _shapesCollection.sRec.FitToGrid(GridSize);
                    }

                    switch (_redimStatus)
                    {
                            //Poly's point
                        case RedimStatus.Poly:
                            // Move selected
                            if (_shapesCollection.selectedElement != null && _shapesCollection.sRec != null)
                            {
                                //s.movePoint(tmpstartX - temp.X, tmpstartY - temp.Y);
                                _shapesCollection.MovePoint(tmpstartX - tmpX, tmpstartY - tmpY);
                            }
                            if (_fitToGrid && GridSize > 0)
                            {
                                _shapesCollection.FitToGrid(GridSize);
                                if (_shapesCollection.sRec != null) _shapesCollection.sRec.FitToGrid(GridSize);
                            }
                            break;
                        case RedimStatus.MoveSelected:
                            // Move selected
                            if (_shapesCollection.selectedElement != null && _shapesCollection.sRec != null)
                            {
                                _shapesCollection.Move(tmpstartX - tmpX, tmpstartY - tmpY);

                                _shapesCollection.sRec.move(tmpstartX - tmpX, tmpstartY - tmpY);
                            }
                            break;
                        case RedimStatus.Rotate:
                            // rotate selected
                            if (_shapesCollection.selectedElement != null && _shapesCollection.sRec != null)
                            {
                                _shapesCollection.selectedElement.Rotate(tmpX, tmpY);
                                _shapesCollection.sRec.Rotate(tmpX, tmpY);
                            }
                            break;
                        case RedimStatus.Zoom:
                            // rotate selected
                            if (_shapesCollection.selectedElement != null && _shapesCollection.sRec != null)
                            {
                                if (_shapesCollection.selectedElement is Group)
                                {
                                    ((Group) _shapesCollection.selectedElement).setZoom(tmpX, tmpY);
                                    _shapesCollection.sRec.setZoom(((Group) _shapesCollection.selectedElement).GrpZoomX,
                                                         ((Group) _shapesCollection.selectedElement).GrpZoomY);
                                }
                            }
                            break;
                        default:
                            // redim selected
                            if (_shapesCollection.selectedElement != null && _shapesCollection.sRec != null)
                            {
                                _shapesCollection.selectedElement.Redim(tmpX - tmpstartX, tmpY - tmpstartY, _redimStatus);
                                _shapesCollection.sRec.Redim(tmpX - tmpstartX, tmpY - tmpstartY, _redimStatus);
                            }
                            break;
                    }
                }
                else
                {
                    if (EditOption == EditOption.Pen)
                    {
                        //this.s.addEllipse(temp.X,temp.Y,temp.X+1,temp.Y+1,Color.Blue,Color.Blue,1f,false);

                        var pt = new PointWr(_temp.X - _start.X, _temp.Y - _start.Y);
                        _constructionPenPoints.Add(pt);
                        if (Math.Sqrt(Math.Pow(_penPrec.X - _temp.X, 2) + Math.Pow(_penPrec.Y - _temp.Y, 2)) > 15)
                        {
                            _penPoints.Add(pt);
                            _penPrec.X = _temp.X;
                            _penPrec.Y = _temp.Y;
                        }
                    }
                }
                _strobeOmitBackgroundElements = true;
                Refresh();
            }
            else if ((e.Button & MouseButtons.Right) != 0)
            {
                CanvasOriginX = (int)((e.X - _startD.X) / Zoom);
                CanvasOriginY = (int)((e.Y - _startD.Y) / Zoom);
                if (_fitToGrid && GridSize > 0)
                {
                    CanvasOriginX = GridSize * ((CanvasOriginX) / GridSize);
                    CanvasOriginY = GridSize * ((CanvasOriginY) / GridSize);
                }
                Invalidate();
            }
            else
            {
                if (EditOption == EditOption.Select)
                {
                    if (_shapesCollection.sRec != null)
                    {
                        RedimStatus st = _shapesCollection.sRec.isOver((int) (e.X/Zoom) - CanvasOriginX,
                                                             (int) (e.Y/Zoom - CanvasOriginY));
                        _redimStatus = st;

                        switch (st)
                        {
                            case RedimStatus.NewPoint:
                                Cursor = Cursors.SizeAll;
                                Cursor = _addPointCursor;
                                /*To change the cursor
                                Cursor cc = new Cursor("NewPoint.ico");
                                this.Cursor = cc;
                                */
                                break;
                            case RedimStatus.Poly:
                                Cursor = Cursors.SizeAll;
                                break;
                            case RedimStatus.Rotate:
                                Cursor = Cursors.SizeAll;
                                break;
                            case RedimStatus.MoveSelected:
                                Cursor = Cursors.Hand;
                                break;
                            case RedimStatus.NW:
                                Cursor = Cursors.SizeNWSE;
                                break;
                            case RedimStatus.N:
                                Cursor = Cursors.SizeNS;
                                break;
                            case RedimStatus.NE:
                                Cursor = Cursors.SizeNESW;
                                break;
                            case RedimStatus.E:
                                Cursor = Cursors.SizeWE;
                                break;
                            case RedimStatus.SE:
                                Cursor = Cursors.SizeNWSE;
                                break;
                            case RedimStatus.S:
                                Cursor = Cursors.SizeNS;
                                break;
                            case RedimStatus.SW:
                                Cursor = Cursors.SizeNESW;
                                break;
                            case RedimStatus.W:
                                Cursor = Cursors.SizeWE;
                                break;
                            case RedimStatus.Zoom:
                                Cursor = Cursors.SizeNWSE;
                                break;
                            default:
                                Cursor = Cursors.Default;
                                _redimStatus = RedimStatus.None;
                                break;
                        }
                    }
                    else
                    {
                        Cursor = Cursors.Default;
                        _redimStatus = RedimStatus.None;
                    }
                }
                else
                {
                    Cursor = Cursors.Default;
                    _redimStatus = RedimStatus.None;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if ((e.Button & MouseButtons.Left) != 0)
            {
                #region left up
                int sx = _start.X;
                int sy = _start.Y;
                var tmpX = (int) ((e.X)/Zoom - CanvasOriginX);
                var tmpY = (int) ((e.Y)/Zoom - CanvasOriginY);
                if (_fitToGrid && GridSize > 0)
                {
                    tmpX = GridSize*(int) ((e.X/Zoom - CanvasOriginX)/GridSize);
                    tmpY = GridSize*(int) ((e.Y/Zoom - CanvasOriginY)/GridSize);
                }
                var lineX = tmpX;
                var lineY = tmpY;
                ShapesCollection.SanitizeRect(ref sx, ref sy, ref tmpX, ref tmpY);

                switch (EditOption)
                {
                    #region selectrect

                    case EditOption.Select:
                        if (_editStatus != EditStatus.Redim)
                            _shapesCollection.click((int) ((e.X)/Zoom - CanvasOriginX), (int) ((e.Y)/Zoom - CanvasOriginY), _richTextBox);
                        else
                        {
                            if (_shapesCollection.selectedElement is PointSet)
                            {
                                //POLY MANAGEMENT START
                                _shapesCollection.AddPoint();
                                if (_fitToGrid && GridSize > 0)
                                    _shapesCollection.FitToGrid(GridSize);

                                switch (_redimStatus)
                                {
                                    case RedimStatus.Rotate:
                                        _shapesCollection.selectedElement.CommitRotate(tmpX, tmpY);
                                        //this.s.sRec = new SelPoly(this.s.selectedElement);//create handling rect                                     
                                        break;
                                    default:
                                        break;
                                } //POLY MANAGEMENT END
                            }
                        }

                        if (_editStatus == EditStatus.SelectRect)
                        {
                            if (((tmpX - sx) + (tmpY - sy)) > 12)
                            {
                                // manage multi object selection
                                _shapesCollection.multiSelect(sx, sy, tmpX, tmpY, _richTextBox);
                            }
                        }

                        SetEditStatus(EditStatus.None);
                        break;

                        #endregion

                        #region Rect

                    case EditOption.Rect:

                        if (_editStatus == EditStatus.DrawRect)
                        {
                            _shapesCollection.addRect(sx, sy, tmpX, tmpY, _creationPenColor, _creationFillColor, _creationPenWidth, _isCreationFilled);
                            if(!StickyEditOption)
                                changeOption(EditOption.Select);
                        }
                        break;

                        #endregion

                        #region Arc

                    case EditOption.Arc:
                        if (_editStatus == EditStatus.DrawRect)
                        {
                            _shapesCollection.addArc(sx, sy, tmpX, tmpY, _creationPenColor, _creationFillColor, _creationPenWidth,_isCreationFilled);
                            if (!StickyEditOption)
                                changeOption(EditOption.Select);
                        }
                        break;

                        #endregion

                        #region Poly and Pen

                    case EditOption.Pen:
                        //if (this.Status == EditStatus.DrawRect)
                        //{
                        _shapesCollection.addPoly(_start.X, _start.Y, lineX, lineY, _creationPenColor, _creationFillColor, _creationPenWidth, _isCreationFilled, _penPoints, true);
                        _penPoints.Clear();
                        _constructionPenPoints.Clear();
                        if (!StickyEditOption)
                            changeOption(EditOption.Select);
                        //}
                        break;

                    case EditOption.Poly: //polygon/pointSet/curvedshape..
                        if (_editStatus == EditStatus.DrawRect)
                        {
                            var aa = new List<PointWr> {new PointWr(0, 0), new PointWr(tmpX - sx, tmpY - sy)};
                            //aa.Add(new PointWr(System.Math.Abs(sx - tmpX), System.Math.Abs(sy - tmpY)));
                            _shapesCollection.addPoly(sx, sy, tmpX, tmpY, _creationPenColor, _creationFillColor, _creationPenWidth, _isCreationFilled, aa, false);
                            //if (!StickyEditOption)    // for Poly, we disable the stickey edit option, to give the user a chance to edit the polygon right after creating it
                                changeOption(EditOption.Select);
                        }
                        break;

                        #endregion

                        #region RRect

                    case EditOption.RoundRect: //DrawRRect

                        if (_editStatus == EditStatus.DrawRect)
                        {
                            _shapesCollection.addRRect(sx, sy, tmpX, tmpY, _creationPenColor, _creationFillColor, _creationPenWidth, _isCreationFilled);
                            if (!StickyEditOption)
                                changeOption(EditOption.Select);
                        }
                        break;

                        #endregion

                        #region Ellipse

                    case EditOption.Ellipse: //DrawEllipse

                        if (_editStatus == EditStatus.DrawRect)
                        {
                            _shapesCollection.addEllipse(sx, sy, tmpX, tmpY, _creationPenColor, _creationFillColor, _creationPenWidth, _isCreationFilled);
                            if (!StickyEditOption)
                                changeOption(EditOption.Select);
                        }
                        break;

                        #endregion

                        #region DrawTextBox

                    case EditOption.RichText: //DrawTextBox
                        if (_editStatus == EditStatus.DrawRect)
                        {
                            Cursor = Cursors.WaitCursor;
                            _richTextForm.ShowDialog();
                            Cursor = Cursors.Arrow;
                            _shapesCollection.addTextBox(sx, sy, tmpX, tmpY, _richTextBox, _creationPenColor, _creationFillColor, _creationPenWidth, _isCreationFilled);
                            if (!StickyEditOption)
                                changeOption(EditOption.Select);
                        }
                        break;

                        #endregion

                        #region DrawSimpleTextBox

                    case EditOption.SimpleText: //DrawSimpleTextBox
                        if (_editStatus == EditStatus.DrawRect)
                        {
                            Cursor = Cursors.WaitCursor;
                            _richTextForm.ShowDialog();
                            Cursor = Cursors.Arrow;
                            _shapesCollection.addSimpleTextBox(sx, sy, tmpX, tmpY, _richTextBox, _creationPenColor, _creationFillColor, _creationPenWidth, _isCreationFilled);
                            if (!StickyEditOption)
                                changeOption(EditOption.Select);
                        }
                        break;

                        #endregion

                        #region ImgBox

                    case EditOption.Image:

                        if (_editStatus == EditStatus.DrawRect)
                        {
                            string fileName = LoadImage();
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                _shapesCollection.addImgBox(sx, sy, tmpX, tmpY, fileName, _creationPenColor, _creationPenWidth);
                                if (!StickyEditOption)
                                    changeOption(EditOption.Select);
                            }
                        }
                        break;

                        #endregion

                    #region Acquired ImgBox

                    case EditOption.AcquireImage:

                        if (_editStatus == EditStatus.DrawRect)
                        {
                            string errMsg;
                            var bitmap = WIALib.WiaLib.AcquireImage(out errMsg);
                            if (bitmap == null)
                                MessageBox.Show(errMsg);
                            else
                            {
                                _shapesCollection.addImgBox(sx, sy, tmpX, tmpY, bitmap, _creationPenColor, _creationPenWidth);
                                if (!StickyEditOption)
                                    changeOption(EditOption.Select);
                            }
                        }
                        break;

                    #endregion
                        #region Line

                    case EditOption.Line: //Draw Line

                        if (_editStatus == EditStatus.DrawRect)
                        {
                            _shapesCollection.addLine(_start.X, _start.Y, lineX, lineY, _creationPenColor, _creationPenWidth);
                            if (!StickyEditOption)
                                changeOption(EditOption.Select);
                        }
                        break;

                        #endregion

                    default:
                        //this.Status = string.Empty;
                        SetEditStatus(EditStatus.None);
                        break;
                }

                // store start X,Y,X1,Y1 of selected item
                if (_shapesCollection.selectedElement != null)
                {
                    if (_shapesCollection.selectedElement is PointSet)
                    {
                        //POLY MANAGEMENT START
                        ((PointSet) _shapesCollection.selectedElement).setupSize();
                        _shapesCollection.sRec = new SelPoly(_shapesCollection.selectedElement); //create handling rect
                    }

                    if (_redimStatus != RedimStatus.None)
                        _shapesCollection.EndMove();

                    if (_shapesCollection.sRec != null)
                        _shapesCollection.sRec.endMoveRedim();
                }
                // show properties
                var e1 = new PropertyEventArgs(_shapesCollection.getSelectedArray(), _shapesCollection.RedoEnabled(), _shapesCollection.UndoEnabled());
                OnShapeSelected(this, e1); // raise event

                Invalidate(); //redraw all=true 

                _isMouseSelecting = false; // end pressing SX
                #endregion
            }
            else if((e.Button & MouseButtons.Right) != 0)
            {
                #region right up
                    // show properties
                    var e1 = new PropertyEventArgs(_shapesCollection.getSelectedArray(), _shapesCollection.RedoEnabled(), _shapesCollection.UndoEnabled());
                    OnShapeSelected(this, e1); // raise event
                #endregion
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            switch (EditOption)
            {
                case EditOption.Select:
                    //if (this.Status != "redim")
                    //{
                    if (_shapesCollection.selectedElement != null)
                    {
                        if (_shapesCollection.selectedElement is BoxText || _shapesCollection.selectedElement is Group)
                        {
                            Cursor = Cursors.WaitCursor;
                            _shapesCollection.selectedElement.ShowEditor(_richTextForm);
                            Cursor = Cursors.Arrow;
                        }
                        if (_shapesCollection.selectedElement is ImageBox)
                        {
                            //string f_name = this.imgLoader();
                            ((ImageBox) _shapesCollection.selectedElement).Load_IMG();
                        }
                        if (_shapesCollection.selectedElement is Group)
                        {
                            //string f_name = this.imgLoader();
                            ((Group) _shapesCollection.selectedElement).Load_IMG();
                        }
                        if (_shapesCollection.selectedElement is PointColorSet && _redimStatus == RedimStatus.Poly)
                            ((PointColorSet) _shapesCollection.selectedElement).dbl_Click();
                    }
                    SetEditStatus(EditStatus.None);
                    //}
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region SELECTED SHAPE COMMANDS

        /// <summary>
        /// Set the pen color for any fresh created elements in the future, and also for all currently selected elements
        /// </summary>
        /// <param name="col"></param>
        public void setPenColor(Color col)
        {
            _creationPenColor = col;         // set the pen color for any fresh creations in the future

            // and update the pen color for any currently selected elements
            bool redraw = false;
            foreach (var element in _shapesCollection.SelectedElements)
            {
                if (col != element.PenColor)
                {
                    element.PenColor = col;
                    redraw = true;
                }
            }
            if (redraw)
                Invalidate();
        }

        /// <summary>
        /// /// Set the fill color for any fresh created elements in the future, and also for all currently selected elements
        /// </summary>
        /// <param name="col"></param>
        public void setFillColor(Color col)
        {
            _creationFillColor = col;         // set the fill color for any fresh creations in the future

            // and update the fill color for any currently selected elements
            bool redraw = false;
            foreach (var element in _shapesCollection.SelectedElements)
            {
                if (col != element.FillColor)
                {
                    element.FillColor = col;
                    redraw = true;
                }
            }
            if (redraw)
                Invalidate();
        }

        /// <summary>
        /// Set the filled state for any fresh created elements in the future, and also for all currently selected elements
        /// </summary>
        /// <param name="filled"></param>
        public void setFilled(bool filled)
        {
            _isCreationFilled = filled;         // set the filled flag for any fresh creations in the future

            // and update the filled state for any currently selected elements
            bool redraw = false;
            foreach (var element in _shapesCollection.SelectedElements)
            {
                if(filled != element.IsFilled)
                {
                    element.IsFilled = filled;
                    redraw = true;
                }
            }
            if(redraw)
                Invalidate();
        }

        /// <summary>
        /// Set the penwidth for any fresh created elements in the future, and also for all currently selected elements
        /// </summary>
        /// <param name="penWidth"></param>
        public void setPenWidth(float penWidth)
        {
            _creationPenWidth = penWidth;          // set the penwidth for any fresh creations in the future

            // and update the penwidth for any currently selected elements
            bool redraw = false;
            foreach (var element in _shapesCollection.SelectedElements)
            {
                if (penWidth != element.PenWidth)
                {
                    element.PenWidth = penWidth;
                    redraw = true;
                }
            }

            if (redraw)
                Invalidate();
        }


        public void GroupSelected()
        {
            _shapesCollection.GroupSelected();

            // show properties
            OnShapeSelected(this, new PropertyEventArgs(_shapesCollection.getSelectedArray(), _shapesCollection.RedoEnabled(), _shapesCollection.UndoEnabled())); // raise event
            Invalidate();
        }


        public void UnGroupSelected()
        {
            _shapesCollection.UnGroupSelected();
            // show properties
            var e1 = new PropertyEventArgs(_shapesCollection.getSelectedArray(), _shapesCollection.RedoEnabled(), _shapesCollection.UndoEnabled());
            OnShapeSelected(this, e1); // raise event

            Invalidate();
        }


        public void RemoveSelected()
        {
            _shapesCollection.RemoveSelected();
            Invalidate();
        }

        public void CopyMultiSelected()
        {
            _shapesCollection.CopyMultiSelected(-25, -15);
            Invalidate();
        }

        public void ToFront()
        {
            _shapesCollection.ToFront();
            Invalidate();
        }

        public void ToBack()
        {
            _shapesCollection.ToBack();
            Invalidate();
        }

        #endregion

        #region LOAD/SAVE

        public bool LoadFromFile()
        {
            try
            {
                using (var dlg = new OpenFileDialog
                                     {
                                         DefaultExt = "shape",
                                         Title = "Load shape",
                                         Filter = "frame files (*.shape)|*.shape|All files (*.*)|*.*"
                                     })
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        using (var stream = dlg.OpenFile())
                        {
                            var BinaryRead = new BinaryFormatter();
                            _shapesCollection = (ShapesCollection) BinaryRead.Deserialize(stream);
                            //g_l = (ExtGrpLst)BinaryRead.Deserialize(StreamRead);
                            stream.Close();

                            _shapesCollection.AfterLoad();

                            Invalidate();
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception:" + e, "Load error:");
            }
            return false;
        }

        public bool SaveToFile()
        {
            try
            {
                using (var dlg = new SaveFileDialog
                                     {
                                         DefaultExt = "shape",
                                         Title = "Save as shape",
                                         Filter = "shape files (*.shape)|*.shape|All files (*.*)|*.*"
                                     })
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        using (var stream = dlg.OpenFile())
                        {
                            var BinaryWrite = new BinaryFormatter();
                            BinaryWrite.Serialize(stream, _shapesCollection);
                            stream.Close();
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception:" + e, "Save error:");
            }
            return false;
        }

        public bool SaveSelected()
        {
            var a = _shapesCollection.getSelectedList();
            if ((a != null) && (a.Count > 0))
            {
                try
                {
                    using (var dlg = new SaveFileDialog
                                         {
                                             DefaultExt = "sobj", Title = "Save as sobj", Filter = "sobj files (*.sobj)|*.sobj|All files (*.*)|*.*"
                                         })
                    {
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            using (var stream = dlg.OpenFile())
                            {
                                var BinaryWrite = new BinaryFormatter();
                                BinaryWrite.Serialize(stream, a);
                                stream.Close();
                            }
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Exception:" + e, "Save error:");
                }
            }
            return false;
        }

        public bool LoadObj()
        {
            try
            {
                using (var dlg = new OpenFileDialog
                                     {
                                         DefaultExt = "sobj",
                                         Title = "Load sobj",
                                         Filter = "frame files (*.sobj)|*.sobj|All files (*.*)|*.*"
                                     })
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        using (var stream = dlg.OpenFile())
                        {
                            var BinaryRead = new BinaryFormatter();
                            var a = (List<Element>) BinaryRead.Deserialize(stream);
                            //this.s = (Shapes)BinaryRead.Deserialize(StreamRead);
                            _shapesCollection.setList(a);
                            stream.Close();
                            _shapesCollection.AfterLoad();
                            Invalidate();
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception:" + e, "Load error:");
            }
            return false;
        }

        #endregion

        #region IMG LOADER

        public string LoadImage()
        {
            try
            {
                using (var dlg = new OpenFileDialog
                                     {
                                         Title = "Load background image",
                                         Filter = "jpg files (*.jpg)|*.jpg|bmp files (*.bmp)|*.bmp|All files (*.*)|*.*"
                                     })
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                        return (dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        #endregion

        #region Print and Preview

        public void Print()
        {
            _shapesCollection.deSelect();

            using (var printPreviewFrm = new PrintPreview {PrintPreviewControl1 = {Name = "PrintPreviewControl1"}})
            {
                printPreviewFrm.PrintPreviewControl1.Document = printPreviewFrm.docToPrint;
                printPreviewFrm.PrintPreviewControl1.Zoom = 1;
                printPreviewFrm.PrintPreviewControl1.Document.DocumentName = "Printpreview";
                printPreviewFrm.PrintPreviewControl1.UseAntiAlias = true;
                printPreviewFrm.docToPrint.PrintPage += docToPrint_PrintPage;

                // print it
                printPreviewFrm.docToPrint.Print();
            }
        }

        private void InitializePrintPreviewControl(float zoom)
        {
            // make sure no selection is showing up
            _shapesCollection.deSelect();

            using (var printPreviewFrm = new PrintPreview {PrintPreviewControl1 = {Name = "Print Preview"}})
            {
                printPreviewFrm.PrintPreviewControl1.Document = printPreviewFrm.docToPrint;

                // Set the zoom
                printPreviewFrm.PrintPreviewControl1.Zoom = zoom;

                // Set the document name. This will show be displayed when the document is loading into the control.
                printPreviewFrm.PrintPreviewControl1.Document.DocumentName = "Print Preview";

                // Set the UseAntiAlias property to true so fonts are smoothed by the operating system.
                printPreviewFrm.PrintPreviewControl1.UseAntiAlias = true;

                // Add the control to the form.
                //printPreviewFrm.Controls.Add(printPreviewFrm.PrintPreviewControl1);

                // Associate the event-handling method with the document's PrintPage event.
                printPreviewFrm.docToPrint.PrintPage += docToPrint_PrintPage;

                // show the print preview dialog
                printPreviewFrm.ShowDialog();
            }
        }

        // The PrintPreviewControl will display the document
        // by handling the documents PrintPage event
        private void docToPrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            if (BackgroundImage != null)
                g.DrawImage(BackgroundImage, 0, 0);
            _shapesCollection.Draw(g, 0, 0, 1);
        }

        #endregion

        ////////////////////////////////////////////////////////
        #region Keyboard Handling

            /// <summary>
            /// Set a key mapping to a certain operation.
            /// note: operations can be mapped to multiple keys, if you would like to for some reason.
            /// </summary>
            /// <param name="key"></param>
            /// <param name="keyOperation"></param>
            /// <returns></returns>
            public void SetKeyMapping(Keys key, KeyOperation keyOperation)
            {
                if (_keyMappings.ContainsKey(key))
                    _keyMappings[key] = keyOperation;
                else
                    _keyMappings.Add(key, keyOperation);
            }

            /// <summary>
            /// Clear all key mappings
            /// </summary>
            public void ClearKeyMappings()
            {
                _keyMappings.Clear();
            }

            /// <summary>
            /// Set the default key mappings.
            /// Override to add/replace by your own key-to-operation mappings,
            /// or call ClearKeyMappings() once and multiple times SetKeyMapping() to build up a keymappings table
            /// </summary>
            public virtual void SetDefaultKeyMappings()
            {
                ClearKeyMappings();
                SetKeyMapping(Keys.Escape, KeyOperation.SelectMode);
                SetKeyMapping(Keys.Space, KeyOperation.DeSelect);

                SetKeyMapping(Keys.R, KeyOperation.RectMode);
                SetKeyMapping(Keys.L, KeyOperation.LineMode);
                SetKeyMapping(Keys.E, KeyOperation.EllipseMode);
                SetKeyMapping(Keys.N, KeyOperation.RoundRectangleMode);
                SetKeyMapping(Keys.S, KeyOperation.SimpleTextMode);
                SetKeyMapping(Keys.T, KeyOperation.RichTextMode);
                SetKeyMapping(Keys.I, KeyOperation.ImageMode);
                SetKeyMapping(Keys.A, KeyOperation.AcquireImageMode);
                SetKeyMapping(Keys.C, KeyOperation.ArcMode);
                SetKeyMapping(Keys.P, KeyOperation.PolyMode);
                SetKeyMapping(Keys.H, KeyOperation.PenMode);
                SetKeyMapping(Keys.O, KeyOperation.Load);
                SetKeyMapping(Keys.S, KeyOperation.Save);
                SetKeyMapping(Keys.Home, KeyOperation.LoadObjects);
                SetKeyMapping(Keys.Q, KeyOperation.SaveSelectedObjects);
                SetKeyMapping(Keys.PrintScreen, KeyOperation.PrintPreview);
                SetKeyMapping(Keys.Print, KeyOperation.Print);
                SetKeyMapping(Keys.NumPad0, KeyOperation.GridOff);
                SetKeyMapping(Keys.NumPad1, KeyOperation.Grid3);
                SetKeyMapping(Keys.NumPad2, KeyOperation.Grid5);
                SetKeyMapping(Keys.NumPad3, KeyOperation.Grid8);
                SetKeyMapping(Keys.NumPad4, KeyOperation.Grid10);
                SetKeyMapping(Keys.NumPad5, KeyOperation.Grid12);
                SetKeyMapping(Keys.NumPad6, KeyOperation.Grid15);
                SetKeyMapping(Keys.NumPad7, KeyOperation.Grid20);
                SetKeyMapping(Keys.NumPad8, KeyOperation.GridCustom);
                SetKeyMapping(Keys.U, KeyOperation.Undo);
                SetKeyMapping(Keys.Y, KeyOperation.Redo);
                SetKeyMapping(Keys.G, KeyOperation.Group);
                SetKeyMapping(Keys.F, KeyOperation.UnGroup);
                SetKeyMapping(Keys.M, KeyOperation.PolyMerge);
                SetKeyMapping(Keys.X, KeyOperation.PolyDeletePoints);
                SetKeyMapping(Keys.Z, KeyOperation.PolyExtendPoints);
                SetKeyMapping(Keys.F2, KeyOperation.PolyMirrorX);
                SetKeyMapping(Keys.F3, KeyOperation.PolyMirrorY);
                SetKeyMapping(Keys.F4, KeyOperation.PolyMirrorXY);
                SetKeyMapping(Keys.PageUp, KeyOperation.ToFront);
                SetKeyMapping(Keys.PageDown, KeyOperation.ToBack);
                SetKeyMapping(Keys.Delete, KeyOperation.DeleteSelected);
                SetKeyMapping(Keys.C, KeyOperation.CopySelected);
            }
            
            /// <summary>
            /// We override this method to support key mapping on key down
            /// </summary>
            /// <param name="e"></param>
            protected override void OnKeyDown(KeyEventArgs e)
            {
                base.OnKeyDown(e);

                KeyOperation keyOperation;
                if(_keyMappings.TryGetValue(e.KeyCode, out keyOperation))
                {
                    switch (keyOperation)
                    {
                        case KeyOperation.None:
                            break;
                        case KeyOperation.SelectMode:                   changeOption(EditOption.Select);    break;
                        case KeyOperation.DeSelect:                     _shapesCollection.deSelect(); Invalidate();       break;
                        case KeyOperation.RectMode:                     changeOption(EditOption.Rect);  break;
                        case KeyOperation.LineMode:                     changeOption(EditOption.Line); break;
                        case KeyOperation.EllipseMode:                  changeOption(EditOption.Ellipse); break;
                        case KeyOperation.RoundRectangleMode:           changeOption(EditOption.RoundRect); break;
                        case KeyOperation.SimpleTextMode:               changeOption(EditOption.SimpleText); break;
                        case KeyOperation.RichTextMode:                  changeOption(EditOption.RichText); break;
                        case KeyOperation.ImageMode:                    changeOption(EditOption.Image); break;
                        case KeyOperation.AcquireImageMode:             changeOption(EditOption.AcquireImage); break;
                        case KeyOperation.ArcMode:                      changeOption(EditOption.Arc); break;
                        case KeyOperation.PolyMode:                     changeOption(EditOption.Poly); break;
                        case KeyOperation.PenMode:                      changeOption(EditOption.Pen); break;
                        case KeyOperation.Load:                         LoadFromFile(); break;
                        case KeyOperation.Save:                         SaveToFile(); break;
                        case KeyOperation.LoadObjects:                  LoadObj(); break;
                        case KeyOperation.SaveSelectedObjects:          SaveSelected(); break; break;
                        case KeyOperation.PrintPreview:                 PrintPreview(1f); break;
                        case KeyOperation.Print:                        Print(); break;
                        case KeyOperation.GridOff:                      GridSize = 0; break;
                        case KeyOperation.Grid3:                        GridSize = 3; break;
                        case KeyOperation.Grid5:                        GridSize = 5; break;
                        case KeyOperation.Grid8:                        GridSize = 8; break;
                        case KeyOperation.Grid10:                       GridSize = 10; break;
                        case KeyOperation.Grid12:                       GridSize = 12; break;
                        case KeyOperation.Grid15:                       GridSize = 15; break;
                        case KeyOperation.Grid20:                       GridSize = 20; break;
                        case KeyOperation.GridCustom: break;
                        case KeyOperation.Undo:                         Undo(); break;
                        case KeyOperation.Redo:                         Redo(); break;
                        case KeyOperation.Group:                        GroupSelected(); break;
                        case KeyOperation.UnGroup:                      UnGroupSelected(); break;
                        case KeyOperation.PolyMerge:                    MergePolygons(); break;
                        case KeyOperation.PolyDeletePoints:             DeletePoints(); break;
                        case KeyOperation.PolyExtendPoints:             ExtPoints(); break;
                        case KeyOperation.PolyMirrorX:                  XMirror(); break;
                        case KeyOperation.PolyMirrorY:                  YMirror(); break;
                        case KeyOperation.PolyMirrorXY:                 Mirror(); break;
                        case KeyOperation.ToFront:                      ToFront(); break;
                        case KeyOperation.ToBack:                       ToBack(); break;
                        case KeyOperation.DeleteSelected:               RemoveSelected(); break;
                        case KeyOperation.CopySelected:                 CopyMultiSelected(); break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    TiggerOnKeyOperation(keyOperation);
                }
            }
        #endregion
    }
}