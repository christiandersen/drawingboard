using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

using DrawingBoard2.Forms;
using DrawingBoard2.Shapes;
using DrawingBoard2.Handlers;
using DrawingBoard2.Utils;

namespace DrawingBoard2.Controls
{
    /// <summary>
    /// Drawing board control that is used for drawing 2D shapes
    /// </summary>
    //[Designer("DrawingBoard2.Controls.DrawingBoardDesigned, AssemblyContainingTheDesigner")]
    [Designer(typeof(DrawingBoardDesigner))]
    public partial class DrawingBoard : UserControl
    {
        #region Variables
        private DrawingBoardStatus status = DrawingBoardStatus.None;
        private DrawingOption option = DrawingOption.Select;
        private TriangleType triangleType;
        private HandlerOperator redimStatus = HandlerOperator.None;

        private int mouseX;
        private int mouseY;
        private int startX;
        private int startY;
        private int startDX;
        private int startDY;
        private ShapeManager shapeManager = new ShapeManager();
        private float zoom = 1; //Zoom value
        private int dx; //Origin X 
        private int dy; //Origin Y

        //Pen tool variables
        private List<PointElement> visiblePenPoints = new List<PointElement>();
        private List<PointElement> penPoints = new List<PointElement>();
        private int PenPreviousX;
        private int PenPreviousY;

        //Double buffer bitmaps
        private Bitmap bufferBmp; //Buffer layer  (Double buffering)
        private Bitmap backBufferBmp; //Back buffer layer

        //Grid variables
        private int gridSize = 0;
        private bool fitToGrid = true;

        //Graphic
        private CompositingQuality compositinQuality = CompositingQuality.Default;
        private TextRenderingHint textRenderingHint = TextRenderingHint.AntiAlias;
        private SmoothingMode smoothningMode = SmoothingMode.AntiAlias;
        private InterpolationMode interpolationMode = InterpolationMode.Default;

        private bool leftMouseButtonPressed;
        private int tempX; //Temporary variable that holds x region
        private int tempY; //Temporary variable that holds y region

        //Forms
        private RichTextBoxForm richTbForm = new RichTextBoxForm(true);
        private SimpleTextForm simpleTbForm = new SimpleTextForm();
        private PolygonForm polygonForm = new PolygonForm();

        private Color penColor = Color.Black; //Current pen color
        private float penWidth = 1; //Current pen width
        private Color fillColor = Color.White;//Current fill color
        private bool fillEnabled = false;//Filling enabled or not

        //Cursors
        private Cursor addPointCursor = Cursors.Cross;
        private Cursor deletePointCursor = Cursors.Default;
        #endregion

        #region Events
        /// <summary>
        /// ObjectSelectedEvent is fired when any object on drawing board is selected
        /// </summary>
        public event ObjectSelectedEvent OnObjectSelected;
        #endregion

        #region Properties
        /// <summary>
        /// Type of triangle shape that is currently drawn
        /// </summary>
        public TriangleType TriangleType
        {
            get { return this.triangleType; }
            set { this.triangleType = value; }
        }
        /// <summary>
        /// Interpolation mode ,specifies the algorithm that is used when images are scaled or rotated
        /// </summary>
        public InterpolationMode InterPolationMode
        {
            get { return this.interpolationMode; }
            set { this.interpolationMode = value; }
        }
        /// <summary>
        /// Specifies the quality level to use during compositing.
        /// </summary>
        public CompositingQuality CompositingQuality
        {
            get { return this.compositinQuality; }
            set { this.compositinQuality = value; }
        }
        /// <summary>
        /// Indiciates, which shape is currently drawn
        /// </summary>
        public DrawingOption Option
        {
            get { return option; }
            set { option = value; }
        }
        /// <summary>
        /// Rendering quality. Default set as SmoothingMode.AntiAlias
        /// </summary>
        public SmoothingMode SmoothingMode
        {
            get { return this.smoothningMode; }
            set { this.smoothningMode = value; }
        }
        /// <summary>
        ///  Rendering mode for text
        /// </summary>
        public TextRenderingHint TextRenderingHint
        {
            get { return this.textRenderingHint; }
            set { this.textRenderingHint = value; }
        }
        /// <summary>
        /// Current status of the drawing board
        /// See <see cref="DrawingBoard2.DrawingBoardStatus"/>
        /// </summary>
        public DrawingBoardStatus Status
        {
            get { return status; }
            set { status = value; }
        }
        /// <summary>
        /// Current redimension status
        /// See <see cref="DrawingBoard2.HandlerOperator"/>
        /// </summary>
        public HandlerOperator RedimStatus
        {
            get { return redimStatus; }
            set { redimStatus = value; }
        }
        /// <summary>
        /// Column/Row Size of the background grid
        /// </summary>
        public int GridSize
        {
            get { return this.gridSize; }
            set
            {
                if (value < 0)
                    return;
                this.gridSize = value;

                if (gridSize != 0)
                {
                    this.dx = gridSize * (int)(this.dx / gridSize);
                    this.dy = gridSize * (int)(this.dy / gridSize);
                }
                this.ReDraw(true);
            }
        }
        /// <summary>
        /// Current zoom value of the board
        /// </summary>
        public float Zoom
        {
            get { return this.zoom; }
            set
            {
                this.zoom = value > 0 ? value : 1;
                this.ReDraw(true);
            }
        }
        /// <summary>
        /// Origin X
        /// </summary>
        public int Dx
        {
            get { return this.dx; }
            set { this.dx = value; }
        }
        /// <summary>
        /// Origin Y
        /// </summary>
        public int Dy
        {
            get { return this.dy; }
            set { this.dy = value; }
        }
        /// <summary>
        /// Indicates whether is undoing enabled
        /// </summary>
        public bool UndoEnabled
        {
            get { return this.shapeManager.UndoEnabled; }
        }
        /// <summary>
        /// Indicates whether is redoing enabled
        /// </summary>
        public bool RedoEnabled
        {
            get { return this.shapeManager.RedoEnabled; }

        }
        /// <summary>
        /// Pen color of the selected shape element
        /// </summary>
        public Color PenColor
        {
            set
            {
                this.penColor = value;

                if (this.shapeManager.SelectedElement != null)
                    this.shapeManager.SelectedElement.PenColor = value;
            }
        }
        /// <summary>
        /// Fill color of the selected shape element
        /// </summary>
        public Color FillColor
        {
            set
            {
                this.fillColor = value;

                if (this.shapeManager.SelectedElement != null)
                    this.shapeManager.SelectedElement.FillColor = value;
            }
        }
        /// <summary>
        /// Indicates whether selected shape is set as fillable or not
        /// </summary>
        public bool EnableFill
        {
            set
            {
                this.fillEnabled = value;

                if (this.shapeManager.SelectedElement != null)
                    this.shapeManager.SelectedElement.FillEnabled = value;
            }
        }
        /// <summary>
        /// Pen width of the selected shape element
        /// </summary>
        public float PenWidth
        {
            set
            {
                this.penWidth = value;

                if (this.shapeManager.SelectedElement != null)
                    this.shapeManager.SelectedElement.PenWidth = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Drawing board control that is used for drawing 2D shapes
        /// </summary>
        public DrawingBoard()
        {
            InitializeComponent();

            this.bufferBmp = new Bitmap(this.Width, this.Height);
            this.backBufferBmp = new Bitmap(this.Width, this.Height);

            this.MouseWheel += new MouseEventHandler(DrawingBoard_MouseWheel);
        }
        #endregion

        #region Other Methods
        /// <summary>
        /// Sets value to the property of selected shape
        /// </summary>
        /// <param name="shapeProperty">Specifies property to be set</param>
        /// <param name="value">Value to be set to the property</param>
        public void SetProperty(ShapeProperty shapeProperty, object value)
        {
            if (this.shapeManager.SelectedElement == null)
                return;
            string propertyName = Enum.GetName(typeof(ShapeProperty), shapeProperty);
            PropertyInfo property = this.shapeManager.SelectedElement.GetType().GetProperty(propertyName);

            if (property != null && property.CanWrite)
                property.SetValue(this.shapeManager.SelectedElement, value, null);
            this.ReDraw(true);
        }
        /// <summary>
        /// Rotates selected shape
        /// </summary>
        /// <param name="angle">Angle to be added to the selected shape</param>
        public void RotateSelectedElement(int angle)
        {
            if (this.shapeManager.SelectedElement == null)
                return;
            this.shapeManager.SelectedElement.Rotation += angle;
            this.ReDraw(false);
        }
        /// <summary>
        /// Zooms in drawing board by 1 unit
        /// </summary>
        public void ZoomIn()
        {
            this.Zoom = this.zoom + 1;
        }
        /// <summary>
        /// Zooms out drawing board  by 1 unit
        /// </summary>
        public void ZoomOut()
        {
            if (this.Zoom > 0)
                this.Zoom = this.zoom - 1;
        }
        /// <summary>
        /// Merges polygons on board
        /// </summary>
        public void MergePolygons()
        {
            this.shapeManager.MergePolygons();
            this.ReDraw(true);
        }
        /// <summary>
        /// Redo last undo command
        /// </summary>
        public void Redo()
        {
            this.shapeManager.Redo();
            this.shapeManager.DeSelect();
            this.ReDraw(true);
        }
        /// <summary>
        /// Undo last command(activity)
        /// </summary>
        public void Undo()
        {
            this.shapeManager.Undo();
            this.shapeManager.DeSelect();
            this.ReDraw(true);
        }
        /// <summary>
        /// Removes selected shape from board
        /// </summary>
        public void RemoveSelected()
        {
            this.shapeManager.RemoveSelected();
            this.ReDraw(true);
        }
        /// <summary>
        /// Copies selected element
        /// </summary>
        public void CopySelected()
        {
            this.shapeManager.CopyMultiSelected(25, 15);
            this.ReDraw(true);
        }
        /// <summary>
        /// Brings selected item to front
        /// </summary>
        public new void BringToFront()
        {
            this.shapeManager.BringToFront();
            this.ReDraw(true);
            base.BringToFront();
        }
        /// <summary>
        /// Sends selected item at backward
        /// </summary>
        public new void SendToBack()
        {
            this.shapeManager.SendToBack();
            this.ReDraw(true);
            base.SendToBack();
        }
        /// <summary>
        /// Event that is fired when its painted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingBoard_Paint(object sender, PaintEventArgs e)
        {
            this.ReDraw(false);
        }
        /// <summary>
        /// Sets graphic object
        /// </summary>
        /// <param name="graphObj">Graphic object to be set</param>
        private void GraphicSetup(Graphics graphObj)
        {
            graphObj.CompositingQuality = this.CompositingQuality;
            graphObj.TextRenderingHint = this.TextRenderingHint;
            graphObj.SmoothingMode = this.SmoothingMode;
            graphObj.InterpolationMode = this.InterPolationMode;
        }
        /// <summary>
        /// Clears shapes and redraws board
        /// </summary>
        public void Clear()
        {
            this.shapeManager.Clear();
            this.ReDraw(true);
        }
        /// <summary>
        /// Adds new point set on board. Point set contains list of points
        /// which comes as "points" parameter
        /// </summary>
        /// <param name="points">List of points to be added</param>
        public void AddPointSet(List<PointElement> points)
        {
            List<PointElement> tempList = new List<PointElement>();

            foreach (PointElement element in points)
                tempList.Add(new ColoredPoint(element.Point, Color.Red));
            this.shapeManager.AddColoredPointSet(new Region(0, 0, 0, 0),
                Color.Black, Color.Black, 1, false, tempList, true);
        }
        /// <summary>
        /// Returns pointset if selected element is point set
        /// if not , returns null
        /// </summary>
        /// <returns>Selected PointSet</returns>
        public Polygon GetPointSet()
        {
            return this.shapeManager.SelectedElement == null ? null :
                (this.shapeManager.SelectedElement as Polygon);
        }
        /// <summary>
        /// Function is invoked when board is resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingBoard_Resize(object sender, EventArgs e)
        {
            if (this.Width > 0 & this.Height > 0)
            {
                bufferBmp.Dispose();
                backBufferBmp.Dispose();

                bufferBmp = new Bitmap(this.Width, this.Height);
                backBufferBmp = new Bitmap(this.Width, this.Height);
                this.ReDraw(true);
            }
        }
        #endregion

        #region MOUSE_EVENTS
        private void DrawingBoard_MouseWheel(object sender, MouseEventArgs e)
        {
            this.zoom += e.Delta / 80;

            if (this.zoom > 20)
                this.zoom = 20;
            if (this.zoom < 1)
                this.zoom = 1;
            this.ReDraw(true);
        }
        private void DrawingBoard_MouseDown(object sender, MouseEventArgs e)
        {
            this.mouseX = e.X;
            this.mouseY = e.Y;
            this.startX = (int)((e.X) / Zoom - this.dx);
            this.startY = (int)((e.Y) / Zoom - this.dy);

            if (e.Button == MouseButtons.Left)
            {
                this.leftMouseButtonPressed = true;

                if (this.option == DrawingOption.Select)
                    this.status = this.redimStatus != HandlerOperator.None ?
                        DrawingBoardStatus.Redimension : DrawingBoardStatus.SelectRectangle;
                else if (this.option == DrawingOption.Pen)
                {
                    if (this.penPoints == null)
                        this.penPoints = new List<PointElement>();
                    else
                        this.penPoints.Clear();
                    if (this.visiblePenPoints == null)
                        this.visiblePenPoints = new List<PointElement>();
                    else
                        this.visiblePenPoints.Clear();
                    PenPreviousX = this.startX;
                    PenPreviousY = this.startY;
                    this.penPoints.Add(new PointElement(0, 0));
                    this.visiblePenPoints.Add(new PointElement(0, 0));
                }
                else
                    this.status = DrawingBoardStatus.DrawRectangle;
            }
            else
            {
                this.startDX = this.dx;
                this.startDY = this.dy;
                this.Cursor = Cursors.Cross;
            }
        }
        private void DrawingBoard_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.option != DrawingOption.Select)
                return;

            if (shapeManager.SelectedElement != null)
            {
                if (shapeManager.SelectedElement is RichTextBoxShape ||
                    shapeManager.SelectedElement is Group)
                {
                    this.Cursor = Cursors.WaitCursor;
                    (shapeManager.SelectedElement as RichTextBoxShape).ShowRichEditor(this.richTbForm);
                    this.Cursor = Cursors.Arrow;
                }
                SimpleTextBox simpleTb = shapeManager.SelectedElement as SimpleTextBox;
                if (simpleTb != null)
                {
                    this.Cursor = Cursors.WaitCursor;

                    simpleTbForm.TextBox.Text = simpleTb.Text;
                    simpleTbForm.FontColor = simpleTb.PenColor;
                    simpleTbForm.BackColor = simpleTb.FillColor;
                    simpleTbForm.ShowDialog();
                    simpleTb.Text = simpleTbForm.TextBox.Text;
                    simpleTb.Font = simpleTbForm.Font;
                    simpleTb.FillEnabled = true;
                    simpleTb.FillColor = simpleTbForm.TextBox.BackColor;
                    simpleTb.PenColor = simpleTbForm.TextBox.ForeColor;

                    this.Cursor = Cursors.Arrow;
                }
                if (this.shapeManager.SelectedElement is ImageBox)
                    (shapeManager.SelectedElement as ImageBox).LoadImage();
                if (this.shapeManager.SelectedElement is Group)
                    (shapeManager.SelectedElement as ImageBox).LoadImage();
            }
            this.status = DrawingBoardStatus.None;
        }
        private void DrawingBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.leftMouseButtonPressed)
                {
                    tempX = (int)(e.X / Zoom);
                    tempY = (int)(e.Y / Zoom);
                    if (fitToGrid & this.gridSize > 0)
                    {
                        tempX = this.gridSize * (int)((e.X / Zoom) / this.gridSize);
                        tempY = this.gridSize * (int)((e.Y / Zoom) / this.gridSize);
                    }
                    tempX = tempX - this.dx;
                    tempY = tempY - this.dy;
                }
                if (this.status == DrawingBoardStatus.Redimension)
                {
                    int tmpX = (int)(e.X / Zoom - this.dx);
                    int tmpY = (int)(e.Y / Zoom - this.dy);
                    int tmpstartX = startX;
                    int tmpstartY = startY;

                    if (fitToGrid & this.gridSize > 0)
                    {
                        tmpX = this.gridSize * (int)((e.X / Zoom - this.dx) / this.gridSize);
                        tmpY = this.gridSize * (int)((e.Y / Zoom - this.dy) / this.gridSize);
                        tmpstartX = this.gridSize * (int)(startX / this.gridSize);
                        tmpstartY = this.gridSize * (int)(startY / this.gridSize);
                        shapeManager.FitToGrid(this.gridSize);
                        shapeManager.HanderCollection.FitTogrid(this.gridSize);
                    }
                    if (shapeManager.SelectedElement != null && shapeManager.HanderCollection != null)
                    {
                        switch (this.redimStatus)
                        {
                            case HandlerOperator.Polygon://RedimensionStatus.Polygon:
                                shapeManager.MovePoint(tmpstartX - tmpX, tmpstartY - tmpY);
                                if (fitToGrid & this.gridSize > 0)
                                {
                                    tempX = this.gridSize * (int)((e.X / Zoom) / this.gridSize);
                                    tempY = this.gridSize * (int)((e.Y / Zoom) / this.gridSize);
                                }
                                break;
                            case HandlerOperator.Default:      // Move selected
                                shapeManager.Move(tmpstartX - tmpX, tmpstartY - tmpY);
                                shapeManager.HanderCollection.Move(tmpstartX - tmpX, tmpstartY - tmpY);
                                break;
                            case HandlerOperator.Rotation: // rotate selected
                                shapeManager.SelectedElement.Rotate(tmpX, tmpY);
                                shapeManager.HanderCollection.Rotate(tmpX, tmpY);
                                break;
                            case HandlerOperator.Zoom: //Zooming selected
                                Group tempGroup = shapeManager.SelectedElement as Group;
                                if (shapeManager.SelectedElement is Group)
                                {
                                    tempGroup.SetZoom(tmpX, tmpY);
                                    shapeManager.HanderCollection.SetZoom(tempGroup.GroupZoomX, tempGroup.GroupZoomY);
                                }
                                break;
                            default:
                                if (this.shapeManager.SelectedElement != null &
                                    this.shapeManager.HanderCollection != null)
                                {
                                    shapeManager.SelectedElement.Redim(tmpX - tmpstartX, tmpY -
                                        tmpstartY, ConverterUtil.ToDirection(this.redimStatus));
                                    shapeManager.HanderCollection.Redim(tmpX - tmpstartX, tmpY -
                                        tmpstartY, ConverterUtil.ToDirection(this.redimStatus));
                                }
                                break;
                        }
                    }
                }
                else if (this.option == DrawingOption.Pen)
                {
                    this.visiblePenPoints.Add(new PointElement(tempX - this.startX, tempY - this.startY));

                    if (Math.Sqrt(Math.Pow(PenPreviousX - tempX, 2) +
                        Math.Pow(PenPreviousY - tempY, 2)) > 15)
                    {
                        penPoints.Add(new PointElement(tempX - this.startX, tempY - this.startY));
                        PenPreviousX = this.tempX;
                        PenPreviousY = this.tempY;
                    }
                }
                ReDraw(false);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.dx = (this.startDX + this.mouseX - e.X);
                this.dy = (this.startDY + this.mouseY - e.Y);

                if (fitToGrid && this.gridSize > 0)
                {
                    this.dx = this.gridSize * (int)((this.dx) / this.gridSize);
                    this.dy = this.gridSize * (int)((this.dy) / this.gridSize);
                }
                ReDraw(true);
            }
            else
            {
                if (this.option != DrawingOption.Select ||
                    this.shapeManager.HanderCollection == null)
                {
                    this.Cursor = Cursors.Default;
                    this.redimStatus = HandlerOperator.None;
                    return;
                }
                HandlerOperator status = this.shapeManager.HanderCollection.IsOver((int)(e.X / Zoom) -
                     this.dx, (int)(e.Y / Zoom - this.dy));
                this.redimStatus = status;
                this.Cursor = FormUtils.GetCursor(status);
            }
        }
        private void DrawingBoard_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                int tmpX = (int)((e.X) / Zoom - this.dx);
                int tmpY = (int)((e.Y) / Zoom - this.dy);

                if (fitToGrid & this.gridSize > 0)
                {
                    tmpX = this.gridSize * (int)((e.X / Zoom - this.dx) / this.gridSize);
                    tmpY = this.gridSize * (int)((e.Y / Zoom - this.dy) / this.gridSize);
                }
                Region region = new Region(startX, startY, tmpX, tmpY);

                switch (this.option)
                {
                    case DrawingOption.Select:
                        if (this.Status != DrawingBoardStatus.Redimension)
                            this.shapeManager.Click((int)((e.X) / Zoom - this.dx), (int)((e.Y) / Zoom - this.dy));
                        else
                        {
                            Polygon pointSet = this.shapeManager.SelectedElement
                                as Polygon;
                            if (pointSet != null)
                            {
                                this.shapeManager.AddPoint();
                                if (this.fitToGrid & this.gridSize > 0)
                                    this.shapeManager.FitToGrid(this.gridSize);

                                if (this.redimStatus == HandlerOperator.Rotation &&
                                    this.shapeManager.SelectedElement is Polygon)
                                    (this.shapeManager.SelectedElement as Polygon).CommitRotate();
                            }
                            if (this.status == DrawingBoardStatus.SelectRectangle &&
                                (((e.X) / Zoom - this.dx - this.startX) + ((e.Y) / Zoom - this.dy - this.startY)) > 12)
                                this.shapeManager.MultiSelect(new Region(this.startX,
                                    this.startY, (int)((e.X) / Zoom - this.dx), (int)((e.Y) / Zoom - this.dy)));
                        }
                        this.status = DrawingBoardStatus.None; //Reset Status
                        break;
                    case DrawingOption.Rectangle:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.shapeManager.AddRectangle(region,
                            this.penColor, this.fillColor, this.penWidth, this.fillEnabled);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.Table:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.shapeManager.AddTable(region, this.penColor, this.fillColor,
                                this.penWidth, this.fillEnabled);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.Arc:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.shapeManager.AddArc(region,
                            this.penColor, this.fillColor, this.penWidth, this.fillEnabled);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.Pen:
                        this.shapeManager.AddPointSet(region,
                            this.penColor, this.fillColor, this.penWidth, this.fillEnabled,
                            penPoints, true);
                        this.option = DrawingOption.Select;
                        this.penPoints = null;
                        this.visiblePenPoints = null;
                        break;
                    case DrawingOption.Star:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.shapeManager.AddStar(region, this.penColor, this.fillColor,
                                this.penWidth, this.fillEnabled);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.Trapezoid:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.shapeManager.AddTrapezoid(region, this.penColor, this.fillColor,
                                this.penWidth, this.fillEnabled);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.Polygon:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            List<PointElement> tempPointList = new List<PointElement>();
                            tempPointList.Add(new PointElement(0, 0));
                            tempPointList.Add(new PointElement(tmpX - startX, tmpY - startY));
                            this.shapeManager.AddPointSet(region, this.penColor, this.fillColor,
                                this.penWidth, this.fillEnabled, tempPointList, false);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.Triangle:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.shapeManager.AddTriangle(region, penColor, fillColor, penWidth, fillEnabled, this.triangleType);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.Pentagon:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.shapeManager.AddPentagon(region, penColor, fillColor, penWidth, fillEnabled);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.Cube:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.shapeManager.AddCube(region, penColor, fillColor, penWidth, fillEnabled);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.Hexagon:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.shapeManager.AddHexagon(region, penColor, fillColor, penWidth, fillEnabled);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.RoundedRectangle:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.shapeManager.AddRoundedRectangle(region,
                            this.penColor, this.fillColor, this.penWidth, this.fillEnabled);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.Ellipse:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.shapeManager.AddEllipse(region,
                            this.penColor, this.fillColor, this.penWidth, this.fillEnabled);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.RichTextBox:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            richTbForm.ShowDialog();
                            this.Cursor = Cursors.Arrow;
                            this.shapeManager.AddRichTextBox(region, this.richTbForm.RichTextBox,
                                richTbForm.RichTextBox.ForeColor, richTbForm.RichTextBox.BackColor,
                                this.penWidth, true);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.SimpleTextBox:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            simpleTbForm.ShowDialog();
                            this.Cursor = Cursors.Arrow;
                            this.shapeManager.AddSimpleTextBox(region,
                                this.simpleTbForm.TextBox, simpleTbForm.TextBox.ForeColor,
                                simpleTbForm.TextBox.BackColor, this.penWidth, true);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.RegularPolygon:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            polygonForm.PolgonRegion = region;
                            polygonForm.ShowDialog();
                            this.Cursor = Cursors.Arrow;

                            if (polygonForm.PointsList != null)
                            {
                                this.shapeManager.AddRegularPolygon(region,
                                    this.penColor, this.fillColor, this.penWidth,
                                    this.fillEnabled, polygonForm.PointsList);
                            }
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.ImageBox:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            Bitmap image = FormUtils.GetImageByFileDialog();
                            this.shapeManager.AddImageBox(region, image,
                                this.penColor, this.penWidth);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.Line:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.shapeManager.AddLine(region, penColor,
                                fillColor, penWidth);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.CartesianPlane:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.shapeManager.AddCartesianPlane(region, penColor,
                                fillColor, penWidth);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    case DrawingOption.Pie:
                        if (this.status == DrawingBoardStatus.DrawRectangle)
                        {
                            this.shapeManager.AddPie(region, penColor, fillColor, penWidth,
                                fillEnabled);
                            this.option = DrawingOption.Select;
                        }
                        break;
                    default:
                        this.status = DrawingBoardStatus.None;
                        break;
                }
                if (this.shapeManager.SelectedElement != null)
                {
                    Polygon pointSet = this.shapeManager.SelectedElement
                        as Polygon;

                    if (pointSet != null)
                    {
                        pointSet.SetupSize();
                        this.shapeManager.HanderCollection =
                            new PolygonHandlerCollection(this.shapeManager.SelectedElement);
                    }
                    if (this.redimStatus != HandlerOperator.None)
                        this.shapeManager.EndMove();
                    if (this.shapeManager.HanderCollection != null)
                        this.shapeManager.HanderCollection.EndMoveRedim();

                }
                this.ReDraw(true);
                this.leftMouseButtonPressed = false;
            }
            if (this.OnObjectSelected != null)
                OnObjectSelected(this, new PropertyEventArgs(this.shapeManager.UndoEnabled,
                    shapeManager.RedoEnabled, this.shapeManager.SelectedElement));
        }
        #endregion

        #region Keyboard Events
        /// <summary>
        /// Handles with keyboard input
        /// </summary>
        /// <param name="msg">Windows Message</param>
        /// <param name="keyData">Pressed key data</param>
        /// <returns>Always true</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Delete)
                this.RemoveSelected();
            if (keyData == Keys.Left && this.shapeManager.SelectedElement != null)
            {
                this.shapeManager.MoveLeft();
                this.ReDraw(false);
            }
            if (keyData == Keys.Right && this.shapeManager.SelectedElement != null)
            {
                this.shapeManager.MoveRight();
                this.ReDraw(false);
            }
            if (keyData == Keys.Down && this.shapeManager.SelectedElement != null)
            {
                this.shapeManager.MoveDown();
                this.ReDraw(false);
            }
            if (keyData == Keys.Up && this.shapeManager.SelectedElement != null)
            {
                this.shapeManager.MoveUp();
                this.ReDraw(false);
            }
            if ((keyData & Keys.Control) == Keys.Control)
            {
                if ((keyData & Keys.C) == Keys.C)
                    this.CopySelected();
                if ((keyData & Keys.Z) == Keys.Z)
                    this.Undo();
            }
            return true;
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Redraws shapes on board.
        /// If redrawAll parameters is true then draws all shapes on board
        /// else draws only selected shape
        /// </summary>
        /// <param name="redrawAll">Indicates that draw all or only the selected one</param>
        public void ReDraw(bool redrawAll)
        {
            if (this.fitToGrid & this.gridSize > 0)
            {
                this.startX = this.gridSize * (int)(startX / this.gridSize);
                this.startY = this.gridSize * (int)(startY / this.gridSize);
            }
            Graphics graphObj = this.CreateGraphics();
            this.GraphicSetup(graphObj);

            if (redrawAll)
            {
                // Redraw static objects in the back Layer 
                Graphics backgroundGraphObj;
                backgroundGraphObj = Graphics.FromImage(this.backBufferBmp);
                this.GraphicSetup(backgroundGraphObj);
                backgroundGraphObj.Clear(this.BackColor);

                if (this.BackgroundImage != null)
                    backgroundGraphObj.DrawImage(this.BackgroundImage, 0, 0);

                // Render the grid
                if (this.gridSize > 0)
                {
                    Pen myPen = new Pen(Color.LightGray);
                    int totalVerticalLine = (int)(this.Width / (this.gridSize * Zoom));

                    for (int i = 0; i <= totalVerticalLine; i++)
                        backgroundGraphObj.DrawLine(myPen, i * this.gridSize * Zoom, 0, i * this.gridSize * Zoom, this.Height);

                    int totalHorizontalLine = (int)(this.Height / (this.gridSize * Zoom));
                    for (int i = 0; i <= totalHorizontalLine; i++)
                        backgroundGraphObj.DrawLine(myPen, 0, i * this.gridSize * Zoom, this.Width, i * this.gridSize * Zoom);
                    myPen.Dispose();
                }

                // Draws unselected objects
                shapeManager.DrawUnselectedShapes(backgroundGraphObj,
                    this.dx, this.dy, this.zoom, false);

                backgroundGraphObj.Dispose();
            }
            //Do Double Buffering
            Graphics bufferGraph = Graphics.FromImage(this.bufferBmp);
            bufferGraph.TextRenderingHint = TextRenderingHint.AntiAlias;
            bufferGraph.SmoothingMode = SmoothingMode.AntiAlias;
            bufferGraph.Clear(this.BackColor);

            // Draw the background image with statics objects
            bufferGraph.DrawImageUnscaled(this.backBufferBmp, 0, 0);
            // Now Draw the dynamic objects on the buffer
            shapeManager.DrawSelectedShapes(bufferGraph, this.dx, this.dy, this.Zoom, false);

            // Now I draw the graphics effects (creation and selection )
            #region Creation/Selection/PenPoints plus A4 margin
            //Draw Red creation Rectangle/Line
            if (this.leftMouseButtonPressed & this.Status == DrawingBoardStatus.DrawRectangle)
            {
                Pen myPen = new Pen(Color.Red, 1.5f);
                myPen.DashStyle = DashStyle.Dot;
                myPen.StartCap = LineCap.DiamondAnchor;

                if (this.option == DrawingOption.Line ||
                    this.option == DrawingOption.Polygon)
                    bufferGraph.DrawLine(myPen, (startX + this.dx) * this.Zoom, (startY + this.dy) * this.Zoom, (tempX + this.dx) * this.Zoom, (tempY + this.dy) * this.Zoom);
                else
                    bufferGraph.DrawRectangle(myPen, (this.startX + this.dx) * this.Zoom, (this.startY + this.dy) * this.Zoom, (tempX - this.startX) * this.Zoom, (tempY - this.startY) * this.Zoom);

                myPen.Dispose();
            }
            //Draw selection Rectangle
            if (this.leftMouseButtonPressed & this.Status == DrawingBoardStatus.SelectRectangle)
            {
                Pen myPen = new Pen(Color.Green, 1.5f);
                myPen.DashStyle = DashStyle.Dash;
                bufferGraph.DrawRectangle(myPen, (this.startX + this.dx) * this.Zoom, (this.startY + this.dy) * this.Zoom, (tempX - this.startX) * this.Zoom, (tempY - this.startY) * this.Zoom);
                myPen.Dispose();
            }
            //Draw A4 margin
            Pen a4Pen = new Pen(Color.Blue, 0.5f);
            a4Pen.DashStyle = DashStyle.Dash;
            bufferGraph.DrawRectangle(a4Pen, (1 + this.dx) * this.Zoom, (1 + this.dy) * this.Zoom, 810 * this.Zoom, 1140 * this.Zoom);
            a4Pen.Dispose();

            //Draw Pen construction shape
            if (this.penPoints != null)
            {
                Pen myPen = new Pen(Color.Red, 1.5f);
                myPen.DashStyle = DashStyle.Dot;
                myPen.StartCap = LineCap.DiamondAnchor;

                List<PointF> pointList = new List<PointF>();

                foreach (PointElement point in this.visiblePenPoints)
                    pointList.Add(new PointF((startX + point.X + this.dx) * this.Zoom,
                        (startY + point.Y + this.dy) * this.Zoom));

                if (pointList.Count > 1)
                    bufferGraph.DrawCurve(myPen, pointList.ToArray());
            }
            #endregion

            graphObj.DrawImageUnscaled(bufferBmp, 0, 0);
            bufferGraph.Dispose();
            graphObj.Dispose();
        }
        /// <summary>
        /// Exports board as image
        /// </summary>
        /// <returns>Image of the current status of the board</returns>
        public Bitmap ExportToBitmap()
        {
            if (this.shapeManager.ShapeCount == 0)
                return null;

            float tempZoom = this.zoom;
            int oldGridSize = this.gridSize;

            this.zoom = 1;
            this.gridSize = 0;
            this.shapeManager.UnselectElements();
            ReDraw(true);
            Bitmap exportBmp = this.bufferBmp.Clone() as Bitmap;

            //Crop Drawn Area
            exportBmp = ImageUtil.CropImage(exportBmp, shapeManager.MinX - 20, shapeManager.MinY - 20,
                shapeManager.MaxX + 20, shapeManager.MaxY + 20);

            this.zoom = tempZoom;
            this.gridSize = oldGridSize;
            ReDraw(true);

            return exportBmp;
        }
        #endregion

        #region Serialization Methods
        /// <summary>
        /// Serializes selected shape in binary format
        /// </summary>
        /// <returns>Binary serialized shape element</returns>
        public byte[] SerializeBinarySelected()
        {
            return shapeManager.SerializeBinary();
        }
        /// <summary>
        /// Deserializes single shape element from binary format
        /// </summary>
        /// <param name="binaryData">Binary data to be deserialized</param>
        public void DeserializeBinarySeleceted(byte[] binaryData)
        {
            shapeManager.DeSerializeBinarySelected(binaryData);
            this.ReDraw(true);
        }
        /// <summary>
        /// Serializes selected shape in XML format
        /// </summary>
        /// <returns>Selected shape in XML format</returns>
        public string SerializeXMLSelected()
        {
            return this.shapeManager.SerializeXMLSelected();
        }
        /// <summary>
        /// DeSerializes single shape element
        /// </summary>
        /// <param name="xmlContent">XML content to be deserialized</param>
        public void DeSerializeXMLSelected(string xmlContent)
        {
            this.shapeManager.DeSerializeXMLSelected(xmlContent);
            this.ReDraw(true);
        }
        /// <summary>
        /// Deserializes binary data into shapes
        /// </summary>
        /// <param name="binaryData">Binary data to be deserialized</param>
        public void DeserializeBinary(byte[] binaryData)
        {
            shapeManager.DeSerializeBinary(binaryData);
            this.ReDraw(true);
        }
        /// <summary>
        /// Serializes shapes on the board in binary format
        /// </summary>
        /// <returns>Serialized binary data</returns>
        public byte[] SerializeBinary()
        {
            return shapeManager.SerializeBinary();
        }
        /// <summary>
        /// Serializes all the shapes on board content in xml format
        /// </summary>
        /// <returns>XML output of serialization</returns>
        public string SerializeXML()
        {
            return shapeManager.SerializeXML();
        }
        /// <summary>
        /// DeSerializes xml content as list of shapes and draws shapes on board
        /// </summary>
        /// <param name="xmlContent">XML content to be deserialized</param>
        public void DeSerializeXML(string xmlContent)
        {
            this.shapeManager.DeSerializeXML(xmlContent);
            this.ReDraw(true);
        }
        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DrawingBoard
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.Name = "DrawingBoard";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawingBoard_Paint);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DrawingBoard_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawingBoard_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawingBoard_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawingBoard_MouseUp);
            this.Resize += new System.EventHandler(this.DrawingBoard_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
