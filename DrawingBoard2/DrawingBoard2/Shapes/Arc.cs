using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using DrawingBoard2.Utils;


namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Arc shape
    /// </summary>
    [Serializable]
    public class Arc : ShapeElement
    {
        #region Variables
        private int startAngle;
        private int angleLength;
        private LineCap startCap;
        private LineCap endCap;
        #endregion

        #region Properties
        /// <summary>
        /// One Corner Point of Arc
        /// </summary>
        [YAXDontSerialize]
        [Browsable(false)]
        private PointF CornerA
        {
            get
            {
                if(this.startAngle > 90)
                    return new PointF(this.region.X0 + Math.Abs(180 - startAngle) / 180.0f * this.region.Width,
                        this.region.Y0 + Math.Abs(270 - this.startAngle) / 180.0f * this.region.Height);
                return new PointF(this.region.X0 + Math.Abs(180 - startAngle) / 180.0f * this.region.Width,
                        this.region.Y0 + Math.Abs(90 + this.startAngle) / 180.0f * this.region.Height);
            }
        }
        /// <summary>
        /// One Corner Point of Arc
        /// </summary>
        [YAXDontSerialize]
        [Browsable(false)]
        private PointF CornerB
        {
            get
            {
                int total = this.angleLength + this.startAngle;

                if (total > 90)
                    return new PointF(this.region.X0 + Math.Abs(180 - startAngle) / 180.0f * this.region.Width,
                        this.region.Y0 + Math.Abs(270 - total) / 180.0f * this.region.Height);
                return new PointF(this.region.X0 + Math.Abs(180 - total) / 180.0f * this.region.Width,
                        this.region.Y0 + Math.Abs(90 + total) / 180.0f * this.region.Height);
            }
        }
        /// <summary>
        /// Start cap style of arc
        /// </summary>
        [Category("Appearance"), Description("Start cap style of arc")]
        public LineCap StartCap
        {
            get { return this.startCap; }
            set { this.startCap = value; }
        }
        /// <summary>
        /// End cap style of arc
        /// </summary>
        [Category("Appearance"), Description("End cap style of arc")]
        public LineCap EndCap
        {
            get { return this.endCap; }
            set { this.endCap = value; }
        }
        /// <summary>
        /// Start angle arc shape
        /// </summary>
        [Category("Appearance"), Description("Start angle arc shape")]
        public int StartAngle
        {
            get { return this.startAngle; }
            set { this.startAngle = value; }
        }
        /// <summary>
        /// Angle length of arc
        /// </summary>
        [Category("Appearance"), Description("Angle length of arc")]
        public int AngleLength
        {
            get { return this.angleLength; }
            set { this.angleLength = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create Arc shape
        /// </summary>
        public Arc()
        {
            this.selected = true;
            this.EndMoveRedim();
            this.startAngle = 0;
            this.angleLength = 90;
            this.startCap = LineCap.Custom;
            this.endCap = LineCap.Custom;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates an instance and returns it for deep copy
        /// </summary>
        /// <returns>Created instance for deep copy</returns>
        public override ShapeElement Copy()
        {
            return CloneUtil<ShapeElement>.DeepClone<ShapeElement>(this);
        }
        /// <summary>
        /// Sets itself as undo shape 
        /// </summary>
        public override void Select()
        {
            this.undoShape = this.Copy();
        }
        /// <summary>
        /// Adds itself to graphic path as Arc
        /// </summary>
        /// <param name="graphicPath">Graphic path that will contain arc</param>
        /// <param name="dx">X region on path</param>
        /// <param name="dy">Y region on path</param>
        /// <param name="zoom"></param>
        public override void AddToGraphPath(GraphicsPath graphicPath, int dx, int dy, float zoom)
        {
            graphicPath.AddArc(region.GetRectangleF(dx,dy,zoom), this.startAngle, this.angleLength);
        }
        /// <summary>
        /// Draws arc object on the board(graphObj)
        /// </summary>
        /// <param name="graphObj">Graph Object(board) to drawn on</param>
        /// <param name="dx">X region on board</param>
        /// <param name="dy">Y region on board</param>
        /// <param name="zoom">Zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            Brush myBrush = GetBrush(dx, dy, zoom);
            Pen myPen = this.CreatePen(zoom);
  
            myPen.EndCap = this.endCap;
            myPen.StartCap = this.startCap;

            if (this.selected)
            {
                Pen tempPen = new Pen(this.PenColor, ScaledPenWidth(zoom));
                tempPen.Width = 0.5f;
                tempPen.DashStyle = DashStyle.Dot;
                graphObj.DrawEllipse(tempPen,region.GetRectangleF(dx,dy,zoom));
                tempPen.Dispose();
            }

            // Create a path and add the object.
            GraphicsPath myPath = new GraphicsPath();
            myPath.AddArc(region.GetRectangleF(dx,dy,zoom), this.startAngle, this.angleLength);

            // Draw the transformed ellipse to the screen.
            if (this.FillEnabled)
            {
                graphObj.FillPath(myBrush, myPath);
                if (this.ShowBorder)
                    graphObj.DrawPath(myPen, myPath);
            }
            else
                graphObj.DrawPath(myPen, myPath);

            if (this.generateCornerNames)
            {
                using (SolidBrush brush = new SolidBrush(this.PenColor))
                {
                    graphObj.DrawString("A", SystemFonts.CaptionFont, brush, this.CornerA);
                    graphObj.DrawString("B", SystemFonts.CaptionFont, brush, this.CornerB);
                }
            }
            myPath.Dispose();
            myPen.Dispose();

            if (myBrush != null)
                myBrush.Dispose();
        }
        #endregion
    }
}