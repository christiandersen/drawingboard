using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;

namespace DrawingBoard
{
    //Grouped objs properties. When you decide to create a group with the
    // selected objs, U can preset these pprops in the objs:
    // see: 
    //_onGroupXRes 
    //_onGroupX1Res
    //_onGroupYRes 
    //_onGroupY1Res 
    public enum OnGroupResize
    {
        Move,
        Resize,
        Nothing
    } ;

    public enum GroupDisplay
    {
        Default,
        Intersect,
        Xor,
        Exclude
    } ;

    //Point Wrapper: used to store and update points in collection
    [Serializable]
    public class PointWr
    {
        private Point p;
        public bool selected;
        private Point startp;

        public PointWr(Point pp)
        {
            p = pp;
            startp = p;
        }

        public PointWr(int x, int y)
        {
            p.X = x;
            p.Y = y;
            startp = p;
        }

        [Category("Position"), Description("X ")]
        public int X
        {
            get { return p.X; }
            set { p.X = value; }
        }

        [Category("Position"), Description("Y ")]
        public int Y
        {
            get { return p.Y; }
            set { p.Y = value; }
        }

        [Category("Position"), Description(" ")]
        public Point point
        {
            get { return p; }
            set { p = value; }
        }

        public void Zoom(float dx, float dy)
        {
            X = (int) (startp.X*dx);
            Y = (int) (startp.Y*dy);
        }

        public void endZoom()
        {
            startp = p;
        }

        public virtual PointWr copy()
        {
            return new PointWr(X, Y);
        }

        public void RotateAt(float x, float y, int rotAngle)
        {
            float tmpX = X - x;
            float tmpY = Y - y;
            PointF rotPt = RotatePoint(new PointF(tmpX, tmpY), rotAngle);
            rotPt.X = rotPt.X + x;
            rotPt.Y = rotPt.Y + y;
            X = (int) rotPt.X;
            Y = (int) rotPt.Y;
        }

        public void XMirror(int wid)
        {
            X = (-1)*p.X + wid;
            startp = p;
        }

        public void YMirror(int hei)
        {
            Y = (-1)*p.Y + hei;
            startp = p;
        }

        protected static PointF RotatePoint(PointF p, int RotAng)
        {
            double RotAngF = RotAng*Math.PI/180;
            double SinVal = Math.Sin(RotAngF);
            double CosVal = Math.Cos(RotAngF);
            var Nx = (float) (p.X*CosVal - p.Y*SinVal);
            var Ny = (float) (p.Y*CosVal + p.X*SinVal);
            return new PointF(Nx, Ny);
        }
    }

    //Point Color: used to store and update points in collection of gradient brush
    [Serializable]
    public class PointColor : PointWr
    {
        public PointColor(Point pp) :
            base(pp)
        {
        }

        public PointColor(int x, int y) :
            base(x, y)
        {
        }

        public Color col { get; set; }

        public override PointWr copy()
        {
            return new PointColor(X, Y);
        }
    }

    #region Events and Delegates (Used by the controls vectShape and toolBox )

    public delegate void OptionChanged(object sender, OptionEventArgs e);

    public delegate void ObjectSelected(object sender, PropertyEventArgs e);

    public class PropertyEventArgs : EventArgs
    {
        public Element[] element;
        public bool Redoable; //for enable/disable udo/redoBtn
        public bool Undoable; //for enable/disable udo/redoBtn
        //Constructor.
        public PropertyEventArgs(Element[] a, bool r, bool u)
        {
            element = a;
            Undoable = u;
            Redoable = r;
        }
    }

    public class OptionEventArgs : EventArgs
    {
        public EditOption option;

        //Constructor.
        //
        public OptionEventArgs(EditOption s)
        {
            option = s;
        }
    }

    #endregion

    #region Base element Ele and its derived class

    // base Element
    [Serializable]
    public abstract class Element
    {
        //protected PathGradientBrush pgb;//TEST
        //protected PenWR pen=new PenWR(Color.Black);//TEST

        // When you resize the group using the X1 handle (East):
        private int _alpha;
        private int _endalpha = 255;
        private Color _endColor = Color.White;
        private float _endColorPos = 1f;
        private int _gradientAngle;
        private int _gradientLen;
        protected bool _onGroupDoubleClick = true;
        protected OnGroupResize _onGroupX1Res = OnGroupResize.Resize;
        protected OnGroupResize _onGroupXRes = OnGroupResize.Resize;
        // When you resize the group using the Y handle (North):
        // When you resize the group using the Y handle (South):
        protected OnGroupResize _onGroupY1Res = OnGroupResize.Resize;
        protected OnGroupResize _onGroupYRes = OnGroupResize.Resize;

        // When double click on Group obj, forward it to sub-obj

        //LINEAR GRADIENT
        private bool _useGradientLine;
        protected DashStyle Dashstyle { get; set; }
        protected LineCap EndLineCap { get; set; }
        protected LineCap StartLineCap { get; set; }

        //Group obj zoom 
        protected PointF GroupZoom = new PointF(1.0f, 1.0f);

        protected Point Start0;
        protected Point Start1;

        protected Point Location0;
        protected Point Location1;

        public Element UndoElement      { get; set; }
        public virtual bool IsSelected  { get; set; }
        public bool IsDeleted           { get; set; }
        public bool IsLine              { get; set; }
        public bool IsGroup             { get; set; }

        [Description("Rotation Angle.")]
        public int Rotation { get; set; }

        protected Element()
        {
            PenColor = Color.Black;
            PenWidth = 1f;
            FillColor = Color.Black;
            IsFilled = false;
            ShowBorder = true;
            Dashstyle = DashStyle.Solid;
            Alpha = 255;
        }

        /// <summary>
        /// To fill a shape with parallel lines 
        /// </summary>
        protected void FillWithLines(Graphics gfx, int dx, int dy, float zoom, GraphicsPath myPath, float gridSize, float gridRot)
        {
            GraphicsState gs = gfx.Save(); //store previous transformation
            gfx.SetClip(myPath, CombineMode.Intersect);
            Matrix mx = gfx.Transform; // get previous transformation
            var p = new PointF(zoom*(Location0.X + dx + (Location1.X - Location0.X)/2), zoom*(Location0.Y + dy + (Location1.Y - Location0.Y)/2));
            if (Rotation > 0)
                mx.RotateAt(Rotation, p, MatrixOrder.Append); //add a transformation
            mx.RotateAt(gridRot, p, MatrixOrder.Append); //add a transformation
            gfx.Transform = mx;

            int max = Math.Max(Width, Height);
            using (var linePen = new Pen(Color.Gray))
            {
                var nY = (int) (max*3/(gridSize));
                for (int i = 0; i <= nY; i++)
                    gfx.DrawLine(linePen, (Location0.X - max + dx)*zoom, (Location0.Y - max + dy + i*gridSize)*zoom, (Location0.X + dx + max*2)*zoom,(Location0.Y - max + dy + i*gridSize)*zoom);
            }
            gfx.Restore(gs);
            //g.ResetClip();
        }

        /// <summary>
        /// Used to define pen with. 
        /// </summary>
        protected float scaledPenWidth(float zoom)
        {
            if (zoom < 0.1f)
                zoom = 0.1f;
            return PenWidth*zoom;
        }

        /// <summary>
        /// Adapt the shape at the gridsize 
        /// </summary>
        public virtual void FitToGrid(int gridsize)
        {
            Start0.X = gridsize*(Start0.X/gridsize);
            Start0.Y = gridsize*(Start0.Y/gridsize);
            Start1.X = gridsize*(Start1.X/gridsize);
            Start1.Y = gridsize*(Start1.Y/gridsize);
        }

        /// <summary>
        /// Confirm the rotation 
        /// </summary>
        public virtual void CommitRotate(float x, float y)
        {
        }

        /// <summary>
        /// Rotate
        /// </summary>
        public virtual void Rotate(float x, float y)
        {
            float tmp = _rotate(x, y);
            Rotation = (int)tmp;
        }

        /// <summary>
        /// Return a point obtained rotating p by RotAng respect 0,0 
        /// </summary>
        protected static PointF rotatePoint(PointF p, int RotAng)
        {
            double RotAngF = RotAng*Math.PI/180;
            double SinVal = Math.Sin(RotAngF);
            double CosVal = Math.Cos(RotAngF);
            var Nx = (float) (p.X*CosVal - p.Y*SinVal);
            var Ny = (float) (p.Y*CosVal + p.X*SinVal);
            return new PointF(Nx, Ny);
        }

        /// <summary>
        /// Gets a rotation angle from a vertical line from the center of the shape and a line
        /// from the center to the point (x,y)
        /// </summary>
        protected float _rotate(float x, float y)
        {
            //
            var c = new Point((Location0.X + (Location1.X - Location0.X)/2), (Location0.Y + (Location1.Y - Location0.Y)/2));
            float dx = x - c.X;
            float dy = y - c.Y;
            float b;
            float alphVal = 0f;
            if ((dx > 0) && (dy > 0))
            {
                b = 90;
                alphVal = (float) Math.Abs((Math.Atan((dy/dx))*(180/Math.PI)));
            }
            else if ((dx <= 0) && (dy >= 0))
            {
                b = 180;
                if (dy > 0)
                {
                    alphVal = (float) Math.Abs((Math.Atan((dx/dy))*(180/Math.PI)));
                }
                else if (dy == 0)
                {
                    b = 270;
                }
            }
            else if ((dx < 0) && (dy < 0))
            {
                b = 270;
                alphVal = (float) Math.Abs((Math.Atan((dy/dx))*(180/Math.PI)));
            }
            else
            {
                b = 0;
                alphVal = (float) Math.Abs((Math.Atan((dx/dy))*(180/Math.PI)));
            }
            float f = (b + alphVal);
            return f;
        }

        private float getDimX()
        {
            return (float) (Math.Sqrt(Math.Pow(Width, 2) + Math.Pow(Height, 2)) - Width)/2;
        }

        private float getDimY()
        {
            return (float) (Math.Sqrt(Math.Pow(Width, 2) + Math.Pow(Height, 2)) - Height)/2;
        }

        /// <summary>
        /// gets a brush from the properties of the shape
        /// </summary>
        protected Brush GetBrush(int dx, int dy, float zoom)
        {
            if (IsFilled)
            {
                if (UseGradientLineColor)
                {
                    float wid;
                    float hei;
                    if (GradientLength > 0)
                    {
                        wid = GradientLength;
                        hei = GradientLength;
                    }
                    else
                    {
                        wid = ((Location1.X - Location0.X) + 2*getDimX())*zoom;
                        hei = ((Location1.Y - Location0.Y) + 2*getDimY())*zoom;
                    }
                    var br = new LinearGradientBrush(
                        new RectangleF((Location0.X - getDimX() + dx)*zoom, (Location0.Y - getDimY() + dy)*zoom, wid, hei)
                        , ToTransparentColor(FillColor, Alpha)
                        , ToTransparentColor(EndColor, EndAlpha)
                        , GradientAngle
                        , true);
                    br.SetBlendTriangularShape(EndColorPosition, 0.95f);
                    br.WrapMode = WrapMode.TileFlipXY;
                    return br;
                }
                return new SolidBrush(ToTransparentColor(FillColor, Alpha));
            }
            return null;
        }

        /// <summary>
        /// Copy the properties common to all shapes. 
        /// </summary>
        protected static void copyStdProp(Element from, Element to)
        {
            to.Location0.X = from.Location0.X;
            to.Location1.X = from.Location1.X;
            to.Location0.Y = from.Location0.Y;
            to.Location1.Y = from.Location1.Y;
            to.StartLineCap = from.StartLineCap;
            to.Start0.X = from.Start0.X;
            to.Start1.X = from.Start1.X;
            to.Start0.Y = from.Start0.Y;
            to.Start1.Y = from.Start1.Y;
            to.IsLine = from.IsLine;
            to.Alpha = from.Alpha;
            to.Dashstyle = from.Dashstyle;
            to.FillColor = from.FillColor;
            to.IsFilled = from.IsFilled;
            to.PenColor = from.PenColor;
            to.PenWidth = from.PenWidth;
            to.ShowBorder = from.ShowBorder;
            to._onGroupX1Res = from._onGroupX1Res;
            to._onGroupXRes = from._onGroupXRes;
            to._onGroupY1Res = from._onGroupY1Res;
            to._onGroupYRes = from._onGroupYRes;

            to._useGradientLine = from._useGradientLine;
            to._endColor = from._endColor;
            to._endalpha = from._endalpha;
            to._gradientLen = from._gradientLen;
            to._gradientAngle = from._gradientAngle;
            to._endColorPos = from._endColorPos;
        }

        /// <summary>
        /// 2 points distance
        /// </summary>
        protected static int Dist(int x, int y, int x1, int y1)
        {
            return (int) Math.Sqrt(Math.Pow((x - x1), 2) + Math.Pow((y - y1), 2));
        }

        /// <summary>
        /// Make a color darker or lighter
        /// </summary>
        protected static Color ToDarkColor(Color c, int v, int a)
        {
            int r = c.R;
            r = r - v;
            if (r < 0)
                r = 0;
            if (r > 255)
                r = 255;
            int green = c.G;
            green = green - v;
            if (green < 0)
                green = 0;
            if (green > 255)
                green = 255;
            int b = c.B;
            b = b - v;
            if (b < 0)
                b = 0;
            if (b > 255)
                b = 255;
            if (a > 255)
                a = 255;
            if (a < 0)
                a = 0;

            return Color.FromArgb(a, r, green, b);
        }

        /// <summary>
        /// Make a color Tresparent/Solid
        /// </summary>
        protected static Color ToTransparentColor(Color c, int v)
        {
            if (v < 0)
                v = 0;
            if (v > 255)
                v = 255;

            return Color.FromArgb(v, c);
        }


        /// <summary>
        /// true if the shape contains the point x,y
        /// </summary>
        public virtual bool Contains(int x, int y)
        {
            if (IsLine)
            {
                int appo = Dist(x, y, Location0.X, Location0.Y) + Dist(x, y, Location1.X, Location1.Y);
                int appo1 = Dist(Location1.X, Location1.Y, Location0.X, Location0.Y) + 7;
                return appo < appo1;
            }
            return new Rectangle(Location0.X, Location0.Y, Location1.X - Location0.X, Location1.Y - Location0.Y).Contains(x, y);


            // LINES HIT TEST
            /*
            using (var tmpGp = new GraphicsPath())
            {
                AddGp(tmpGp, 0, 0, 1);// AddGp is defined in derived classes

                var p = new Point(x, y);
                using (var pen = new Pen(penColor, penWidth))
                {
                    tmpGp.Widen(pen);
                }

                if (tmpGp.IsVisible(p))
                    return true;
            }             
            return false; 
            */
        }

        /// <summary>
        /// Moves the shape by x,y
        /// </summary>
        public virtual void move(int x, int y)
        {
            Location0.X = Start0.X - x;
            Location0.Y = Start0.Y - y;
            Location1.X = Start1.X - x;
            Location1.Y = Start1.Y - y;
        }

        /* 
        public void move(int x, int startx,int y, int starty)
        {
            int dx = startx - x;
            int dy = starty - y;
            this.X = this.X - dx;
            this.Y = this.Y - dy;
            this.X1 = this.X1 - dx;
            this.Y1 = this.Y1 - dy;
        }
        */

        /// <summary>
        /// Redim the shape 
        /// </summary>
        public virtual void Redim(int x, int y, RedimStatus redimStatus)
        {
            switch (redimStatus)
            {
                case RedimStatus.NW:
                    Location0.X = Start0.X + x;
                    Location0.Y = Start0.Y + y;
                    break;
                case RedimStatus.N:
                    Location0.Y = Start0.Y + y;
                    break;
                case RedimStatus.NE:
                    Location1.X = Start1.X + x;
                    Location0.Y = Start0.Y + y;
                    break;
                case RedimStatus.E:
                    Location1.X = Start1.X + x;
                    break;
                case RedimStatus.SE:
                    Location1.X = Start1.X + x;
                    Location1.Y = Start1.Y + y;
                    break;
                case RedimStatus.S:
                    Location1.Y = Start1.Y + y;
                    break;
                case RedimStatus.SW:
                    Location0.X = Start0.X + x;
                    Location1.Y = Start1.Y + y;
                    break;
                case RedimStatus.W:
                    Location0.X = Start0.X + x;
                    break;
                default:
                    break;
            }

            if (!IsLine)
            {
                // manage redim limits
                if (Location1.X <= Location0.X)
                    Location1.X = Location0.X + 10;
                if (Location1.Y <= Location0.Y)
                    Location1.Y = Location0.Y + 10;
            }
        }

        /// <summary>
        /// Called at the end of move/redim of the shape. Stores Start.X|Y|X1|Y1 
        /// for a correct rendering during object move/redim
        /// </summary>
        public virtual void endMoveRedim()
        {
            Start0.X = Location0.X;
            Start0.Y = Location0.Y;
            Start1.X = Location1.X;
            Start1.Y = Location1.Y;
        }

        #region GET/SET

        /*
        [Editor(typeof(PenTypeEditor),
             typeof(System.Drawing.Design.UITypeEditor))]
        [CategoryAttribute("Apparence"), DescriptionAttribute("Pen")]
        public PenWR PEN
        {
            set
            {
                pen = value;
            }
            get
            {
                return pen;
            }
        }
        */


        /*
        public PathGradientBrush PGB
        {
            set
            {
                pgb = value;
            }
            get
            {
                return pgb;
            }
        }
        */

        /*
        [Editor(typeof(myTypeEditor),
             typeof(System.Drawing.Design.UITypeEditor))]
        public string TEST
        {
            set
            {
                test = value;
            }
            get
            {
                return test;
            }
        }
        */


        [Category("Position"), Description("X ")]
        public int PosX
        {
            get { return Location0.X; }
        }

        [Category("Position"), Description("Y ")]
        public int PosY
        {
            get { return Location0.Y; }
        }

        [Category("Dimension"), Description("Width ")]
        public int Width
        {
            get { return Math.Abs(Location1.X - Location0.X); }
        }

        [Category("Dimension"), Description("Height ")]
        public int Height
        {
            get { return Math.Abs(Location1.Y - Location0.Y); }
        }

        [Category("Group Behaviour"), Description("On Group Resize X")]
        public OnGroupResize OnGrpXRes
        {
            set { _onGroupXRes = value; }
            get { return _onGroupXRes; }
        }

        [Category("Group Behaviour"), Description("On Group Resize X1")]
        public OnGroupResize OnGrpX1Res
        {
            set { _onGroupX1Res = value; }
            get { return _onGroupX1Res; }
        }

        [Category("Group Behaviour"), Description("On Group Resize Y")]
        public OnGroupResize OnGrpYRes
        {
            set { _onGroupYRes = value; }
            get { return _onGroupYRes; }
        }

        [Category("Group Behaviour"), Description("On Group Resize Y1")]
        public OnGroupResize OnGrpY1Res
        {
            set { _onGroupY1Res = value; }
            get
            {
                //return System.Math.Abs(this.Y1 - this.Y);
                return _onGroupY1Res;
            }
        }

        [Category("Group Behaviour"), Description("Manage On Group Double Click")]
        public bool OnGrpDClick
        {
            set { _onGroupDoubleClick = value; }
            get { return _onGroupDoubleClick; }
        }

        [Category("Appearance"), Description("Set Border Dash Style ")]
        public virtual DashStyle dashStyle { get; set; }

        [Category("Appearance"), Description("Show border when filled or contains Text")]
        public virtual bool ShowBorder { get; set; }

        [Category("Appearance"), Description("Pen Color")]
        public virtual Color PenColor { get; set; }

        [Category("Appearance"), Description("Fill Color")]
        public virtual Color FillColor { get; set; }

        [Category("Appearance"), Description("Pen Width")]
        public virtual float PenWidth { get; set; }

        [Category("Appearance"), Description("Filled/Unfilled")]
        public virtual bool IsFilled { get; set; }

        [Category("Appearance"), Description("Opacity")]
        public virtual int Alpha
        {
            get { return _alpha; }
            set { _alpha = value < 0 ? 0 : (value > 255 ? 255 : value); }
        }

        [Category("GradientBrush"), Description("True: use gradient fill color")]
        public virtual bool UseGradientLineColor
        {
            get { return _useGradientLine; }
            set { _useGradientLine = value; }
        }

        [Category("GradientBrush"), Description("End Color Position [0..1])")]
        public virtual float EndColorPosition
        {
            get { return _endColorPos; }
            set
            {
                if (value > 1)
                    value = 1;
                if (value < 0)
                    value = 0;
                _endColorPos = value;
            }
        }

        [Category("GradientBrush"), Description("Gradient End Color")]
        public virtual Color EndColor
        {
            get { return _endColor; }
            set { _endColor = value; }
        }


        [Category("GradientBrush"), Description("End Color Opacity")]
        public virtual int EndAlpha
        {
            get { return _endalpha; }
            set { _endalpha = value < 0 ? 0 : (value > 255 ? 255 : value); }
        }

        [Category("GradientBrush"), Description("Gradient Dimension")]
        public virtual int GradientLength
        {
            get { return _gradientLen; }
            set { _gradientLen = value >= 0 ? value : 0; }
        }

        [Category("GradientBrush"), Description("Gradient Angle")]
        public virtual int GradientAngle
        {
            get { return _gradientAngle; }
            set { _gradientAngle = value; }
        }

        public bool CanRotate
        {
            get; set;
        }

        public int GetRotation()
        {
            return CanRotate ? Rotation : 0;
        }

        public void AddRotation(int a)
        {
            //if (canRotate())
            Rotation += a;
        }

        public int getX()
        {
            return Location0.X;
        }

        public int getY()
        {
            return Location0.Y;
        }

        public int getX1()
        {
            return Location1.X;
        }

        public int getY1()
        {
            return Location1.Y;
        }

        public float getGprZoomX()
        {
            return GroupZoom.X;
        }

        public float getGprZoomY()
        {
            return GroupZoom.Y;
        }

        #endregion

        #region virtual metods

        /// <summary>
        /// Draw this shape to a graphic ogj. 
        /// </summary>
        public virtual void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
        }

        /// <summary>
        /// Add this shape to a graphic path. 
        /// </summary>        
        public void AddGraphPath(GraphicsPath gp, int dx, int dy, float zoom)
        {
            var tmpGp = new GraphicsPath();
            AddGp(tmpGp, dx, dy, zoom); // AddGp is defined in derived classes
            using (var translateMatrix = new Matrix())
            {
                translateMatrix.RotateAt(Rotation, new PointF((Location0.X + dx + (Location1.X - Location0.X) / 2) * zoom, (Location0.Y + dy + (Location1.Y - Location0.Y) / 2) * zoom));
                tmpGp.Transform(translateMatrix);
            }
            gp.AddPath(tmpGp, true);
        }

        /// <summary>
        /// Add this shape to a graphic path. 
        /// </summary>
        public virtual void AddGp(GraphicsPath gp, int dx, int dy, float zoom)
        {
        }

        /// <summary>
        /// Used to ungroup a grouped shape. Returns a list of shapes.
        /// </summary>
        public virtual List<Element> UnGroup()
        {
            return null;
        }

        /// <summary>
        /// Select this shape.
        /// </summary>
        public virtual void Select()
        {
        }

        /// <summary>
        /// Select this shape.
        /// </summary>
        public virtual void Select(RichTextBox r)
        {
        }

        /// <summary>
        /// Select this shape.
        /// </summary>
        public virtual void Select(int sX, int sY, int eX, int eY)
        {
        }

        /// <summary>
        /// Deselct this shape.
        /// </summary>
        public virtual void DeSelect()
        {
        }

        /// <summary>
        /// Used for RTF editor.
        /// </summary>
        public virtual void ShowEditor(RichtTextForm f)
        {
        }

        /// <summary>
        /// Used after the load from file. Manage here the creation of object not serialized.
        /// </summary>
        public virtual void AfterLoad()
        {
        }

        /// <summary>
        /// Copy the properties from another shape
        /// </summary>
        public virtual void CopyFrom(Element element)
        {
        }

        /// <summary>
        /// Clone this shape
        /// </summary>
        public virtual Element Copy()
        {
            return null;
        }

        /// <summary>
        /// Copy the gradient properties. 
        /// </summary>
        protected void copyGradprop(Element element)
        {
            _useGradientLine = element._useGradientLine;
            _endColor = element._endColor;
            _endalpha = element._endalpha;
            _gradientLen = element._gradientLen;
            _gradientAngle = element._gradientAngle;
            _endColorPos = element._endColorPos;
        }

        //public virtual void Rotate(float x, float y)
        //{ }

        #endregion
    }


    /// <summary>
    /// Handle tool for redim/move/rotate shapes
    /// </summary>
    [Serializable]
    public class AbSel : Element
    {
        public AbSel(Element EL)
        {
            Location0.X = EL.getX();
            Location0.Y = EL.getY();
            Location1.X = EL.getX1();
            Location1.Y = EL.getY1();
            IsSelected = false;
            endMoveRedim();
            CanRotate = EL.CanRotate;
            Rotation = EL.GetRotation();
        }

        public void showHandles(bool i)
        {
            IsGroup = i;
        }

        /// <summary>
        /// Su quale maniglia cade il punto x,y? 
        /// </summary>
        public RedimStatus isOver(int x, int y)
        {
            return Contains(x, y) ? RedimStatus.MoveSelected : RedimStatus.Idle;
        }

        public override void Select()
        {
            UndoElement = Copy();
        }

        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            using (var pen = new Pen(Color.Blue, 1.5f) { DashStyle = DashStyle.Dash })
            {
                gfx.DrawRectangle(pen, (Location0.X + dx)*zoom, (Location0.Y + dy)*zoom, (Location1.X - Location0.X)*zoom, (Location1.Y - Location0.Y)*zoom);
            }
        }
    }


    /// <summary>
    /// Handle tool for redim/move/rotate shapes
    /// </summary>
    [Serializable]
    public class SelRectBK : Element
    {
        public SelRectBK(Element EL)
        {
            Location0.X = EL.getX();
            Location0.Y = EL.getY();
            Location1.X = EL.getX1();
            Location1.Y = EL.getY1();
            IsSelected = false;
            endMoveRedim();
            CanRotate = EL.CanRotate;
            Rotation = EL.GetRotation();
        }

        public void showHandles(bool i)
        {
            IsGroup = i;
        }

        /// <summary>
        /// Su quale maniglia cade il punto x,y? 
        /// </summary>
        public RedimStatus isOver(int x, int y)
        {
            var r = new Rectangle(Location0.X - 2, Location0.Y - 2, 5, 5);
            if (!IsGroup)
            {
                //NW
                if (r.Contains(x, y))
                    return RedimStatus.NW;
                //SE
                r.X = Location1.X - 2;
                r.Y = Location1.Y - 2;
                if (r.Contains(x, y))
                    return RedimStatus.SE;
            }
            if (!IsLine)
            {
                //N
                r.X = Location0.X - 2 + (Location1.X - Location0.X)/2;
                r.Y = Location0.Y - 2;
                if (r.Contains(x, y))
                    return RedimStatus.N;
                if (CanRotate)
                {
                    //ROT
                    float midX = (Location1.X - Location0.X)*0.5f;
                    float midY = (Location1.Y - Location0.Y)*0.5f;
                    var Hp = new PointF(0, -25);
                    PointF RotHP = rotatePoint(Hp, Rotation);
                    midX += RotHP.X;
                    midY += RotHP.Y;

                    r.X = Location0.X + (int) midX - 2;
                    r.Y = Location0.Y + (int) midY - 2;
                    if (r.Contains(x, y))
                        return RedimStatus.Rotate;
                }
                if (!IsGroup)
                {
                    //NE
                    r.X = Location1.X - 2;
                    r.Y = Location0.Y - 2;
                    if (r.Contains(x, y))
                        return RedimStatus.NE;
                }
                //E
                r.X = Location1.X - 2;
                r.Y = Location0.Y - 2 + (Location1.Y - Location0.Y)/2;
                if (r.Contains(x, y))
                    return RedimStatus.E;
                //S
                r.X = Location0.X - 2 + (Location1.X - Location0.X)/2;
                r.Y = Location1.Y - 2;
                if (r.Contains(x, y))
                    return RedimStatus.S;
                if (!IsGroup)
                {
                    //SW
                    r.X = Location0.X - 2;
                    r.Y = Location1.Y - 2;
                    if (r.Contains(x, y))
                        return RedimStatus.SW;
                }
                //W
                r.X = Location0.X - 2;
                r.Y = Location0.Y - 2 + (Location1.Y - Location0.Y)/2;
                if (r.Contains(x, y))
                    return RedimStatus.W;
            }

            if (Contains(x, y))
                return RedimStatus.MoveSelected;

            return RedimStatus.Idle;
        }

        public override void Select()
        {
            UndoElement = Copy();
        }

        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            using(var brush = new SolidBrush(Color.Black) {Color = ToTransparentColor(Color.Black, 90)})
            using(var pen = new Pen(Color.Blue, 1.5f) {DashStyle = DashStyle.Dash})
            {
                if (!IsGroup)
                {
                    //NW
                    gfx.FillRectangle(brush, new RectangleF(((Location0.X + dx - 2))*zoom, ((Location0.Y + dy - 2))*zoom, 5*zoom, 5*zoom));
                    gfx.DrawRectangle(Pens.White, (Location0.X + dx - 2)*zoom, (Location0.Y + dy - 2)*zoom, 5*zoom, 5*zoom);
                    //SE
                    gfx.FillRectangle(brush, new RectangleF((Location1.X + dx - 2)*zoom, (Location1.Y + dy - 2)*zoom, (5)*zoom, (5)*zoom));
                    gfx.DrawRectangle(Pens.White, (Location1.X + dx - 2)*zoom, (Location1.Y + dy - 2)*zoom, (5)*zoom, (5)*zoom);
                }
                if (!IsLine)
                {
                    gfx.DrawRectangle(pen, (Location0.X + dx)*zoom, (Location0.Y + dy)*zoom, (Location1.X - Location0.X)*zoom, (Location1.Y - Location0.Y)*zoom);
                    //N
                    gfx.FillRectangle(brush, (Location0.X + dx - 2 + (Location1.X - Location0.X)/2)*zoom, (Location0.Y + dy - 2)*zoom, 5*zoom, 5*zoom);
                    gfx.DrawRectangle(Pens.White, (Location0.X + dx - 2 + (Location1.X - Location0.X)/2)*zoom, (Location0.Y + dy - 2)*zoom, 5*zoom, 5*zoom);
                    if (!IsGroup)
                    {
                        //NE
                        gfx.FillRectangle(brush, (Location1.X + dx - 2)*zoom, (Location0.Y + dy - 2)*zoom, 5*zoom, 5*zoom);
                        gfx.DrawRectangle(Pens.White, (Location1.X + dx - 2)*zoom, (Location0.Y + dy - 2)*zoom, 5*zoom, 5*zoom);
                    }
                    //E
                    gfx.FillRectangle(brush, (Location1.X + dx - 2)*zoom, (Location0.Y + dy - 2 + (Location1.Y - Location0.Y)/2)*zoom, 5*zoom, 5*zoom);
                    gfx.DrawRectangle(Pens.White, (Location1.X + dx - 2)*zoom, (Location0.Y + dy - 2 + (Location1.Y - Location0.Y)/2)*zoom, 5*zoom, 5*zoom);
                    //S
                    gfx.FillRectangle(brush, (Location0.X + dx - 2 + (Location1.X - Location0.X)/2)*zoom, (Location1.Y + dy - 2)*zoom, 5*zoom, 5*zoom);
                    gfx.DrawRectangle(Pens.White, (Location0.X + dx - 2 + (Location1.X - Location0.X)/2)*zoom, (Location1.Y + dy - 2)*zoom, 5*zoom, 5*zoom);
                    if (!IsGroup)
                    {
                        //SW
                        gfx.FillRectangle(brush, (Location0.X + dx - 2)*zoom, (Location1.Y + dy - 2)*zoom, 5*zoom, 5*zoom);
                        gfx.DrawRectangle(Pens.White, (Location0.X + dx - 2)*zoom, (Location1.Y + dy - 2)*zoom, 5*zoom, 5*zoom);
                    }
                    //W
                    gfx.FillRectangle(brush, (Location0.X + dx - 2)*zoom, (Location0.Y + dy - 2 + (Location1.Y - Location0.Y)/2)*zoom, 5*zoom, 5*zoom);
                    gfx.DrawRectangle(Pens.White, (Location0.X + dx - 2)*zoom, (Location0.Y + dy - 2 + (Location1.Y - Location0.Y)/2)*zoom, 5*zoom, 5*zoom);

                    //TEST                
                    if (CanRotate)
                    {
                        //C
                        float midX = (Location1.X - Location0.X)/2;
                        float midY = (Location1.Y - Location0.Y)/2;
                        gfx.FillEllipse(brush, (Location0.X + midX + dx - 3)*zoom, (Location0.Y + dy - 3 + midY)*zoom, 6*zoom, 6*zoom);
                        gfx.DrawEllipse(Pens.White, (Location0.X + midX + dx - 3)*zoom, (Location0.Y + dy - 3 + midY)*zoom, 6*zoom, 6*zoom);
                        // ROT
                        var Hp = new PointF(0, -25);
                        PointF RotHP = rotatePoint(Hp, Rotation);
                        RotHP.X += midX;
                        RotHP.Y += midY;
                        gfx.FillRectangle(brush, (Location0.X + RotHP.X + dx - 3)*zoom, (Location0.Y + dy - 3 + RotHP.Y)*zoom, 6*zoom, 6*zoom);
                        gfx.DrawRectangle(Pens.White, (Location0.X + RotHP.X + dx - 3)*zoom, (Location0.Y + dy - 3 + RotHP.Y)*zoom, 6*zoom, 6*zoom);
                        gfx.DrawLine(pen, (Location0.X + midX + dx)*zoom, (Location0.Y + midY + dy)*zoom, (Location0.X + RotHP.X + dx)*zoom,
                                   (Location0.Y + RotHP.Y + dy)*zoom);
                    }
                }
                else
                {
                    gfx.DrawLine(pen, (Location0.X + dx)*zoom, (Location0.Y + dy)*zoom, (Location1.X + dx)*zoom, (Location1.Y + dy)*zoom);
                }


                // else
                // {
                //     g.DrawRectangle(pen, (this.X + dx) * zoom, (this.Y + dy) * zoom, (this.X1 - this.X) * zoom, (this.Y1 - this.Y) * zoom);
                // }
            }
        }
    }

    /// <summary>
    /// PointSet
    /// </summary>
    [Serializable]
    public class PointSet : Element
    {
        public List<PointWr> points;

        public PointSet(int x, int y, int x1, int y1, IEnumerable<PointWr> a)
        {
            Location0.X = x;
            Location0.Y = y;
            Location1.X = x1;
            Location1.Y = y1;
            IsSelected = true;
            //
            points = new List<PointWr>();
            points.AddRange(a);
            setupSize();
            //
            endMoveRedim();
            Rotation = 0;
            CanRotate = true; //can rotate?
        }

        [Editor(typeof (myTypeEditor),
            typeof (UITypeEditor))]
        [Category("Polygon"), Description("Points")]
        public List<PointWr> Points
        {
            get { return points; }
            set { points = value; }
        }


        [Category("Polygon"), Description("Curved")]
        public bool Curved { get; set; }

        [Category("Polygon"), Description("Closed")]
        public bool Closed { get; set; }


        [Category("1"), Description("Rectangle")]
        public string ObjectType
        {
            get { return "PointSet"; }
        }

        private void RePosition()
        {
            if (points == null) 
                return;
            int minNegativeX = 0;
            int minNegativeY = 0;
            foreach (var p in points)
            {
                minNegativeX = p.X;
                minNegativeY = p.Y;
                break;
            }
            foreach (var p in points)
            {
                if (p.X < minNegativeX)
                    minNegativeX = p.X;
                if (p.Y < minNegativeY)
                    minNegativeY = p.Y;
            }
            //if (minNegativeX < 0 || minNegativeY < 0)
            //{
            foreach (var p in points)
            {
                p.X = p.X - minNegativeX;
                p.Y = p.Y - minNegativeY;
            }
            //}
            Location0.X = Location0.X + minNegativeX;
            Location0.Y = Location0.Y + minNegativeY;
        }

        public List<PointWr> getRealPosPoints()
        {
            var a = new List<PointWr>();
            foreach (var p in points)
                a.Add(new PointWr(p.X + Location0.X, p.Y + Location0.Y));
            return a;
        }

        public void setupSize()
        {
            if (points != null)
            {
                int maxX = 0;
                int maxY = 0;
                foreach (var p in points)
                {
                    if (p.X > maxX)
                        maxX = p.X;
                    if (p.Y > maxY)
                        maxY = p.Y;
                }
                Location1.Y = Location0.Y + maxY;
                Location1.X = Location0.X + maxX;
                RePosition(); //!
            }
        }

        public void addPoint(PointWr p)
        {
            points.Add(p);
            //this.rePos();
        }

        public void rmPoint(PointWr p)
        {
            /*foreach (Point pp in points)
            { 
                if (pp==p)
            }*/
            points.Remove(p);
            //this.points.Add(p);
        }

        #region OVERRIDDEN

        public override void endMoveRedim()
        {
            base.endMoveRedim();
            foreach (var p in points)
            {
                p.endZoom();
            }
        }

        public override void Redim(int x, int y, RedimStatus redimStatus)
        {
            base.Redim(x, y, redimStatus);
            //if (redimSt == RedimStatus.SE || redimSt == RedimStatus.E || redimSt == RedimStatus.S)
            //{
            //MANAGE point set redim as zoom v
            float dx = (Location1.X - Location0.X) / (float) (Start1.X - Start0.X);
            float dy = (Location1.Y - Location0.Y) / (float) (Start1.Y - Start0.Y);
            foreach (var p in points)
                p.Zoom(dx, dy);
            //}
        }

        public override bool Contains(int x, int y)
        {
            int minX = Location0.X;
            int minY = Location0.Y;
            int maxX = Location1.X;
            int maxY = Location1.Y;
            foreach (var p in points)
            {
                if (minX > Location0.X + p.X)
                    minX = Location0.X + p.X;
                if (minY > Location0.Y + p.Y)
                    minY = Location0.Y + p.Y;
                if (maxX < Location0.X + p.X)
                    maxX = Location0.X + p.X;
                if (maxY < Location0.Y + p.Y)
                    maxY = Location0.Y + p.Y;
            }
            return new Rectangle(minX, minY, maxX - minX, maxY - minY).Contains(x, y);
        }

        public override void FitToGrid(int gridsize)
        {
            base.FitToGrid(gridsize);

            if (points != null)
            {
                foreach (var p in points)
                {
                    p.X = gridsize*(p.X/gridsize);
                    p.Y = gridsize*(p.Y/gridsize);
                }
            }
        }

        public override void CommitRotate(float x, float y)
        {
            //base.CommitRotate(x, y);
            //this.Rotation
            if (Rotation > 0)
            {
                //CENTER POINT
                float midX = (float)(Location1.X - Location0.X)/2;
                float midY = (float)(Location1.Y - Location0.Y) / 2;

                foreach (var p in points)
                    p.RotateAt(midX, midY, Rotation);
                Rotation = 0;
            }
        }

        public void CommitMirror(bool xmirr, bool ymirr)
        {
            foreach (var p in points)
            {
                if (xmirr)
                    p.XMirror(Width);
                if (ymirr)
                    p.YMirror(Height);
            }
            //rePos();
            setupSize();
        }


        public override void DeSelect()
        {
            //base.DeSelect();
            foreach (var p in points)
            {
                p.selected = false;
            }
        }

        public override void Select(int sX, int sY, int eX, int eY)
        {
            foreach (var p in points)
            {
                p.selected = false;
                if (new Rectangle(sX, sY, eX - sX, eY - sY).Contains(new Point(p.X + Location0.X, p.Y + Location0.Y)))
                    p.selected = true;
            }
        }


        public override Element Copy()
        {
            var aa = new List<PointWr>();
            if (points != null)
                foreach (var p in points)
                    aa.Add(p.copy());

            var newE = new PointSet(Location0.X, Location0.Y, Location1.X, Location1.Y, aa)
                           {
                               PenColor = PenColor,
                               PenWidth = PenWidth,
                               FillColor = FillColor,
                               IsFilled = IsFilled,
                               dashStyle = dashStyle,
                               Alpha = Alpha,
                               IsLine = IsLine,
                               Rotation = Rotation,
                               ShowBorder = ShowBorder,
                               OnGrpXRes = OnGrpXRes,
                               OnGrpX1Res = OnGrpX1Res,
                               OnGrpYRes = OnGrpYRes,
                               OnGrpY1Res = OnGrpY1Res
                           };

            newE.copyGradprop(this);

            newE.Closed = Closed;
            newE.Curved = Curved;

            return newE;
        }

        public override void CopyFrom(Element element)
        {
            copyStdProp(element, this);
            Rotation = element.Rotation;
            Curved = ((PointSet) element).Curved;
            Closed = ((PointSet) element).Closed;
        }

        public override void Select()
        {
            UndoElement = Copy();
        }

        public override void AddGp(GraphicsPath gp, int dx, int dy, float zoom)
        {
            // To ARRAY
            var myArr = new PointF[points.Count];
            int i = 0;
            foreach (var p in points)
            {
                myArr[i++] = new PointF((p.X + Location0.X + dx)*zoom, (p.Y + Location0.Y + dy)*zoom); // p.point;
            }

            if (i < 2)
                gp.AddLines(myArr);
            else if (Curved)
                gp.AddCurve(myArr);
            else
                gp.AddPolygon(myArr);
        }

        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            //System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(this.fillColor);
            using (var brush = GetBrush(dx, dy, zoom))
            {
                using (var pen = new Pen(PenColor, scaledPenWidth(zoom)) {DashStyle = dashStyle})
                {
                    if (IsSelected)
                    {
                        //brush.Color = this.dark(this.fillColor, 5,this.alpha);
                        pen.Color = Color.Red;
                        pen.Color = ToTransparentColor(pen.Color, 120);
                        pen.Width = pen.Width + 1;
                    }

                    // Create a path and add the object.
                    using (var myPath = new GraphicsPath())
                    {
                        var myArr = new PointF[points.Count];
                        int i = 0;
                        foreach (var p in points)
                        {
                            myArr[i++] = new PointF((p.X + Location0.X + dx)*zoom, (p.Y + Location0.Y + dy)*zoom); // p.point;
                            //if (p.selected)
                            //  g.FillEllipse(Brushes.Green, (p.X + this.X + dx-2) * zoom, (p.Y + this.Y + dy-2) * zoom, 5*zoom, 5*zoom);
                        }

                        if (myArr.Length < 3 || !Curved)
                        {
                            if (Closed && myArr.Length >= 3)
                                myPath.AddPolygon(myArr);
                            else
                                myPath.AddLines(myArr);
                        }
                        else
                        {
                            if (Closed)
                                myPath.AddClosedCurve(myArr);
                            else
                                myPath.AddCurve(myArr);
                            //myPath.AddBeziers(myArr);
                        }


                        //myPath.AddRectangle(new RectangleF((this.X + dx) * zoom, (this.Y + dy) * zoom, (this.X1 - this.X) * zoom, (this.Y1 - this.Y) * zoom));

                        using (var translateMatrix = new Matrix())
                        {
                            translateMatrix.RotateAt(Rotation, new PointF((Location0.X + dx + (Location1.X - Location0.X)/2)*zoom, (Location0.Y + dy + (Location1.Y - Location0.Y)/2)*zoom));
                            myPath.Transform(translateMatrix);
                        }

                        // Draw the transformed obj to the screen.
                        if (IsFilled)
                        {
                            gfx.FillPath(brush, myPath);
                            if (ShowBorder)
                                gfx.DrawPath(pen, myPath);
                        }
                        else
                            gfx.DrawPath(pen, myPath);
                    }
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// A set of color point for Path Gradient Path management 
    /// </summary>
    [Serializable]
    public class PointColorSet : PointSet
    {
        public PointColorSet(int x, int y, int x1, int y1, List<PointWr> a) :
            base(x, y, x1, y1, a)
        {
        }

        public void dbl_Click()
        {
            //base.Select();
            //this.undoEle = this.Copy();
        }

        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            //brush.Color = this.Transparency(this.fillColor, this.alpha);
            using (var pen = new Pen(PenColor, scaledPenWidth(zoom)) {DashStyle = dashStyle})
            {
                if (IsSelected)
                {
                    //brush.Color = this.dark(this.fillColor, 5,this.alpha);
                    pen.Color = Color.Red;
                    pen.Color = ToTransparentColor(pen.Color, 120);
                    pen.Width = pen.Width + 1;
                }

                // Create a path and add the object.
                using (var myPath = new GraphicsPath())
                {
                    var myArr = new PointF[points.Count];
                    var myColorArr = new Color[points.Count];
                    int i = 0;
                    foreach (var  p in points)
                    {
                        myArr[i] = new PointF((p.X + Location0.X + dx)*zoom, (p.Y + Location0.Y + dy)*zoom); // p.point;
                        if (p is PointColor)
                            myColorArr[i++] = ((PointColor) p).col;
                    }

                    if (myArr.Length < 3 || !Curved)
                    {
                        if (Closed && myArr.Length >= 3)
                            myPath.AddPolygon(myArr);
                        else
                            myPath.AddLines(myArr);
                    }
                    else
                    {
                        if (Closed)
                            myPath.AddClosedCurve(myArr);
                        else
                            myPath.AddCurve(myArr);
                    }

                    //PGB
                    using (var brush = new PathGradientBrush(myPath) { SurroundColors = myColorArr, CenterColor = FillColor })
                    {
                        using (var translateMatrix = new Matrix())
                        {
                            translateMatrix.RotateAt(Rotation, new PointF((Location0.X + dx + (Location1.X - Location0.X)/2)*zoom, (Location0.Y + dy + (Location1.Y - Location0.Y)/2)*zoom));
                            myPath.Transform(translateMatrix);
                        }

                        // Draw the transformed obj to the screen.
                        gfx.FillPath(brush, myPath);
                        if (ShowBorder)
                            gfx.DrawPath(pen, myPath);
                    }
                }
            }
        }
    }

    #endregion


}