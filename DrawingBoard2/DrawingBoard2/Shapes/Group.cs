using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Represents shape group
    /// </summary>
    [Serializable]
    public class Group : ShapeElement
    {
        #region Variables
        private List<ShapeElement> elementList = new List<ShapeElement>();
        private bool IsgraphPath = false;
        private bool xMirror = false;
        private bool yMirror = false;
        private GroupDisplay groupDisplay = GroupDisplay.Default;
        #endregion

        #region Properties
        /// <summary>
        /// List of shape elements in group
        /// </summary>
        [Browsable(false)]
        public List<ShapeElement> ElementList
        {
            get { return this.elementList; }
            set { this.elementList = value; }
        }
        /// <summary>
        /// Mirror of the shapes on Y axis
        /// </summary>
        [Category("Layout"), Description("Mirror of the shapes on Y axis")]
        public bool YMirror
        {
            get { return this.yMirror; }
            set { this.yMirror = value; }
        }
        /// <summary>
        /// Mirror of the shapes on X axis
        /// </summary>
        [Category("Layout"), Description("Mirror of the shapes on X axis")]
        public bool XMirror
        {
            get { return this.xMirror; }
            set { this.xMirror = value; }
        }
        /// <summary>
        /// Group display option
        /// </summary>
        [Category("Group Behaviour"), Description("Group display option")]
        public GroupDisplay GroupDisplay
        {
            get { return this.groupDisplay; }
            set { this.groupDisplay = value; }
        }
        /// <summary>
        /// Group Zoom value on X axis
        /// </summary>
        [Category("Group Behaviour"), Description("Group Zoom value on X axis")]
        public new float GroupZoomX
        {
            get { return this.groupZoomX; }
            set { this.groupZoomX = value; }
        }
        /// <summary>
        /// Group Zoom value on Y axis
        /// </summary>
        [Category("Group Behaviour"), Description("Group Zoom value on Y axis")]
        public new  float GroupZoomY
        {
            get { return this.groupZoomY; }
            set { this.groupZoomY = value; }
        }
        /// <summary>
        /// Indicates gradient-coloring is enabled on each shape in group
        /// </summary>
        [Category("Gradient"), Description("Indicates gradient-coloring is enabled on each shape in group")]
        public override bool UseGradientColor
        {
            get
            {
                return base.UseGradientColor;
            }
            set
            {
                base.UseGradientColor = value;

                foreach (ShapeElement element in this.elementList)
                    element.UseGradientColor = value;
            }
        }
        /// <summary>
        /// Overridden EndColorPosition
        /// </summary>
        [Category("Gradient"), Description("Overridden EndColorPosition")]
        public override float EndColorPosition
        {
            get
            {
                return base.EndColorPosition;
            }
            set
            {
                base.EndColorPosition = value;
                foreach (ShapeElement element in this.elementList)
                    element.EndColorPosition = value;
            }
        }
        /// <summary>
        /// Gradient color of the group shapes
        /// </summary>
         [Category("Gradient"), Description("Gradient color of the group shapes")]
        public override Color GradientColor
        {
            get
            {
                return base.GradientColor;
            }
            set
            {
                base.GradientColor = value;
                foreach (ShapeElement element in this.elementList)
                    element.GradientColor = value;
            }
        }
        /// <summary>
        /// Gradient color alpha value of the group shapes
        /// </summary>
         [Category("Gradient"), Description("Gradient color alpha value of the group shapes")]
        public override int GradientAlpha
        {
            get
            {
                return base.GradientAlpha;
            }
            set
            {
                base.GradientAlpha = value;
                foreach (ShapeElement element in this.elementList)
                    element.GradientAlpha = value;
            }
        }
        /// <summary>
        /// Gradient dimension value of the group shapes
        /// </summary>
         [Category("Gradient"), Description("Gradient dimension value of the group shapes")]
        public override int GradientDimension
        {
            get
            {
                return base.GradientDimension;
            }
            set
            {
                base.GradientDimension = value;
                foreach (ShapeElement element in this.elementList)
                    element.GradientDimension = value;
            }
        }
        /// <summary>
        /// Gradient angle value of the group shapes
        /// </summary>
         [Category("Gradient"), Description("Gradient angle value of the group shapes")]
        public override int GradientAngle
        {
            get
            {
                return base.GradientAngle;
            }
            set
            {
                base.GradientAngle = value;
                foreach (ShapeElement element in this.elementList)
                    element.GradientAngle = value;
            }
        }
        /// <summary>
        /// Color alpha value of the group shapes
        /// </summary>
         [Category("Gradient"), Description("Color alpha value of the group shapes")]
        public override int ColorAlpha
        {
            get
            {
                return base.ColorAlpha;
            }
            set
            {
                base.ColorAlpha = value;
                foreach (ShapeElement element in this.elementList)
                    element.ColorAlpha = value;
            }
        }
        /// <summary>
        /// Is group supposed to be added a graphic path or not
        /// </summary>
        [Category("Appearance"), Description("Is group supposed to be added a graphic path or not")]
        public bool IsGraphPath
        {
            get { return this.IsgraphPath; }
            set { this.IsgraphPath = value; }
        }
        /// <summary>
        /// Angle(Rotation) value of the shape group
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Angle(Rotation) value of the shape group")]
        public new int Rotation
        {
            get { return this.rotation; }
            set { this.rotation = value; }
        }
        /// <summary>
        /// Fill color of the group
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Fill color of the group")]
        public override Color FillColor
        {
            get
            {
                return base.FillColor;
            }
            set
            {
                base.FillColor = value;
                foreach (ShapeElement element in this.elementList)
                    element.FillColor = value;
            }
        }
        /// <summary>
        /// Is filling enabled in group or not
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Is filling enabled in group or not")]
        public override bool FillEnabled
        {
            get
            {
                return base.FillEnabled;
            }
            set
            {
                base.FillEnabled = value;

                foreach (ShapeElement element in this.elementList)
                    element.FillEnabled = value;
            }
        }
        /// <summary>
        /// Pen  color of each shape in group
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Pen  color of each shape in group")]
        public override Color PenColor
        {
            get
            {
                return base.PenColor;
            }
            set
            {
                base.PenColor = value;
                foreach (ShapeElement element in this.elementList)
                    element.PenColor = value;
            }
        }
        /// <summary>
        /// Width of pen 
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Width of pen")]
        public override float PenWidth
        {
            get
            {
                return base.PenWidth;
            }
            set
            {
                base.PenWidth = value;
                foreach (ShapeElement element in this.elementList)
                    element.PenWidth = value;
            }
        }
        /// <summary>
        ///  Draw borders around shapes
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Draw borders around shapes")]
        public override bool ShowBorder
        {
            get
            {
                return base.ShowBorder;
            }
            set
            {
                base.ShowBorder = value;
                foreach (ShapeElement element in this.elementList)
                    element.ShowBorder = value;
            }
        }
        /// <summary>
        /// Dash Style of the shape group
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Dash Style of the shape group")]
        public override DashStyle DashStyle
        {
            get
            {
                return base.DashStyle;
            }
            set
            {
                base.DashStyle = value;
                foreach (ShapeElement element in this.elementList)
                    element.DashStyle = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Group of shapes
        /// </summary>
        /// <param name="elementList">Shape elements in group</param>
        public Group(List<ShapeElement> elementList)
        {
            this.isGroup = true;
            this.elementList = elementList;

            int minX = +32000;
            int maxX = -32000;
            int minY = +32000;
            int maxY = -32000;

            foreach (ShapeElement element in elementList)
            {
                if (element.PosStartX < minX)
                    minX = element.PosStartX;
                if (element.PosStartY < minY)
                    minY = element.PosStartY;
                if (element.PosEndX > maxX)
                    maxX = element.PosEndX;
                if (element.PosEndY > maxY)
                    maxY = element.PosEndY;
                element.Selected = false;
            }
            region = new Region(minX, minY, maxX, maxY);
            this.selected = true;
            this.EndMoveRedim();
            this.rotation = 0;
            this.canRotate = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Called at the end of move/redim of the shape. Stores startX|Y|X1|Y1 
        /// for a correct rendering during object move/redim
        /// </summary>
        public override void EndMoveRedim()
        {
            base.EndMoveRedim();

            foreach (ShapeElement element in this.elementList)
                element.EndMoveRedim();
        }
        /// <summary>
        /// Used to degroup a grouped shape. Returns a list of shapes.
        /// </summary>
        /// <returns></returns>
        public override List<ShapeElement> DeGroup()
        {
            return this.elementList;
        }
        /// <summary>
        /// Moves the shape by x,y
        /// </summary>
        public override void Move(int x, int y)
        {
            foreach (ShapeElement element in elementList)
                element.Move(x, y);
            base.Move(x, y);
        }
        /// <summary>
        /// Sets zoom value 
        /// </summary>
        /// <param name="x">Zoom X value</param>
        /// <param name="y">Zoom Y value</param>
        public void SetZoom(int x, int y)
        {
            float dx = (region.X1 - x) * 2;
            float dy = (region.Y1 - y) * 2;

            this.groupZoomX = (region.Width - dx) / region.Width;
            this.groupZoomY = (region.Height - dy) / region.Height;
        }
        /// <summary>
        /// CARE! Since this class contains list of shape elements
        /// This may produce bad results
        /// </summary>
        /// <returns>Created instance</returns>
        public override ShapeElement Copy()
        {
            return CloneUtil<ShapeElement>.DeepClone<ShapeElement>(this);
           
        }
        /// <summary>
        /// Copies itself to undo shape for precaution
        /// </summary>
        public override void Select()
        {
            this.undoShape = this.Copy();
        }
        /// <summary>
        /// Changes dimension of the shape
        /// </summary>
        /// <param name="x">X region</param>
        /// <param name="y">Y region</param>
        /// <param name="direction">Direction of redimension</param>
        public override void Redim(int x, int y, Direction direction)
        {
            base.Redim(x, y, direction);

            foreach (ShapeElement element in this.elementList)
            {
                switch (direction)
                {
                    case Direction.North:
                        if (element.NorthResize == OnGroupResize.Move)
                            element.Move(0, -y);
                        else if (element.NorthResize == OnGroupResize.Resize)
                            element.Redim(0, y, direction);
                        break;
                    case Direction.South:
                        if (element.SouthResize == OnGroupResize.Move)
                            element.Move(0, -y);
                        else if (element.SouthResize == OnGroupResize.Resize)
                            element.Redim(0, y, direction);
                        break;
                    case Direction.East:
                        if (element.EastResize == OnGroupResize.Move)
                            element.Move(-x, 0);
                        else if (element.EastResize == OnGroupResize.Resize)
                            element.Redim(x, 0, direction);
                        break;
                    case Direction.West:
                        if (element.WestResize == OnGroupResize.Move)
                            element.Move(-x, 0);
                        else if (element.WestResize == OnGroupResize.Resize)
                            element.Redim(x, 0, direction);
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Adds each element in the list to the graph path
        /// </summary>
        /// <param name="graphicPath">Grap path that will contain elements</param>
        /// <param name="dx">x region on path</param>
        /// <param name="dy">y region on path</param>
        /// <param name="zoom">Zoom value</param>
        public override void AddToGraphPath(GraphicsPath graphicPath, int dx, int dy, float zoom)
        {
            foreach (ShapeElement element in this.elementList)
                element.AddToGraphPath(graphicPath, dx, dy, zoom);
        }
        /// <summary>
        /// Draws each element in group on graph object
        /// </summary>
        /// <param name="graphObj">Graph object to be drawn on</param>
        /// <param name="dx">X region</param>
        /// <param name="dy">Y region</param>
        /// <param name="zoom">Zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            GraphicsState oldGraphState = graphObj.Save();//store previos trasformation
            Matrix matrix = graphObj.Transform; // get previous trasformation

            PointF point = new PointF(zoom * (region.X0 + dx + (region.X1 - region.X0) / 2),
                zoom * (region.Y0 + dy + (region.Y1 - region.Y0) / 2));
            if (this.Rotation > 0)
                matrix.RotateAt(this.Rotation, point, MatrixOrder.Append); //add a trasformation

            //X MIRROR  //Y MIRROR
            if (this.xMirror || this.yMirror)
            {
                matrix.Translate(-(region.X0 + region.Width / 2 + dx) * zoom,
                    -(region.Y0 + region.Height / 2 + dy) * zoom, MatrixOrder.Append);

                if (this.xMirror)
                    matrix.Multiply(new Matrix(-1, 0, 0, 1, 0, 0), MatrixOrder.Append);
                if (this.yMirror)
                    matrix.Multiply(new Matrix(1, 0, 0, -1, 0, 0), MatrixOrder.Append);

                matrix.Translate((region.X0 + region.Width / 2 + dx) * zoom,
                    (region.Y0 + region.Height / 2 + dy) * zoom, MatrixOrder.Append);
            }

            if (this.groupZoomX > 0 && this.groupZoomY > 0)
            {
                matrix.Translate((-1) * zoom * (region.X0 + dx + 
                    (region.X1 - region.X0) / 2), (-1) * zoom * (region.Y0 + dy +
                    (region.Y1 - region.Y0) / 2), MatrixOrder.Append);
                matrix.Scale(this.groupZoomX, this.groupZoomY, MatrixOrder.Append);
                matrix.Translate(zoom * (region.X0 + dx + (region.X1 - region.X0) / 2),
                    zoom * (region.Y0 + dy + (region.Y1 - region.Y0) / 2), 
                    MatrixOrder.Append);
            }
            graphObj.Transform = matrix;

            if (this.IsGraphPath)
            {
                Brush myBrush = GetBrush(dx, dy, zoom);
                Pen myPen = this.CreatePen(zoom);

                GraphicsPath graphPath = new GraphicsPath();

                foreach (ShapeElement element in this.elementList)
                    element.AddToGraphPath(graphPath, dx, dy, zoom);

                if (this.FillEnabled)
                {
                    graphObj.FillPath(myBrush, graphPath);

                    if (this.ShowBorder)
                        graphObj.DrawPath(myPen, graphPath);
                }
                else
                    graphObj.DrawPath(myPen, graphPath);

                if (myBrush != null)
                    myBrush.Dispose();
                myPen.Dispose();
            }
            else
            {
                //MANAGE This.GroupDisplay
                System.Drawing.Region gregion = new System.Drawing.Region();

                if (GroupDisplay != GroupDisplay.Default)
                {
                    bool first = true;
                    foreach (ShapeElement element in this.elementList)
                    {
                        GraphicsPath graphPath = new GraphicsPath(FillMode.Winding);
                        element.AddToGraphPath(graphPath, dx, dy, zoom);

                        if (first)
                            gregion.Intersect(graphPath);
                        else
                        {
                            switch (GroupDisplay)
                            {
                                case GroupDisplay.Intersect:
                                    gregion.Intersect(graphPath);
                                    break;
                                case GroupDisplay.Xor:
                                    gregion.Xor(graphPath);
                                    break;
                                case GroupDisplay.Exclude:
                                    gregion.Exclude(graphPath);
                                    break;
                                default:
                                    break;
                            }
                        }
                        first = false;
                    }
                    graphObj.SetClip(gregion, CombineMode.Intersect);
                }

                foreach (ShapeElement element in this.elementList)
                    element.Draw(graphObj, dx, dy, zoom);

                if (GroupDisplay != GroupDisplay.Default)
                    graphObj.ResetClip();
            }
            graphObj.Restore(oldGraphState);//restore previos trasformation

            if (this.Selected)
            {
                Brush myBrush = GetBrush(dx, dy, zoom);
                Pen myPen = this.CreatePen(zoom);

                graphObj.DrawRectangle(myPen, Rectangle.Round(region.GetRectangleF(dx,dy,zoom)));
                if (myBrush != null)
                    myBrush.Dispose();
                myPen.Dispose();
            }
            matrix.Dispose();
        }
        #endregion
    }
}
