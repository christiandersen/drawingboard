using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Line shape
    /// <remarks>Note that,this shape has text property, that is written at the center of the shape</remarks>
    /// </summary>
    [Serializable]
    public class Line : ShapeElement
    {
        #region Variables
        private LineCap startCap; //Start line cap of the line
        private LineCap endCap;  //End line cap of the line
        private string text = string.Empty;
        private Color textColor = Color.Black;
        #endregion

        #region Properties
        /// <summary>
        /// Color of the text that is written 
        /// </summary>
        [Category("Appearance"), Description("Color of the text that is written")]
        public Color TextColor
        {
            get { return this.textColor; }
            set { this.textColor = value; }
        }
        /// <summary>
        /// Text that is written in the center of the line
        /// </summary>
        [Category("Appearance"), Description("Text that is written in the center of the line")]
        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }
        /// <summary>
        /// Line cap style of the start of the line
        /// </summary>
        [Category("Appearance"), Description("Line cap style of the start of the line")]
        public LineCap StartCap
        {
            get { return this.startCap; }
            set { this.startCap = value; }
        }
        /// <summary>
        /// Line cap style of the end of the line
        /// </summary>
        [Category("Appearance"), Description("Line cap style of the end of the line")]
        public LineCap EndCap
        {
            get { return this.endCap; }
            set { this.endCap = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Line Shape
        /// </summary>
        public Line()
        {
            this.IsLine = true;
            this.selected = true;
            this.startCap = this.endCap = LineCap.Custom;
            this.EndMoveRedim();
            this.canRotate = false;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deep copies line
        /// </summary>
        /// <returns>Copied(created) element</returns>
        public override ShapeElement Copy()
        {
            return CloneUtil<ShapeElement>.DeepClone<ShapeElement>(this);
        }
        /// <summary>
        /// Select line
        /// </summary>
        public override void Select()
        {
            this.undoShape = this.Copy();
        }
        /// <summary>
        /// Adss line to graphic path
        /// </summary>
        /// <param name="graphicPath">Graphic path that line will be added to</param>
        /// <param name="dx">X region </param>
        /// <param name="dy">Y region</param>
        /// <param name="zoom">Zoom value</param>
        public override void AddToGraphPath(GraphicsPath graphicPath, int dx, int dy, float zoom)
        {
            graphicPath.AddLine((region.X0 + dx) * zoom, (region.Y0 + dy) * zoom,
                (region.X1 + dx) * zoom, (region.Y1 + dy) * zoom);
        }
        /// <summary>
        /// Draw shape on the graphic object
        /// </summary>
        /// <param name="graphObj">Graphic object that will be drawn on</param>
        /// <param name="dx">X region on the graphic object</param>
        /// <param name="dy">Y region on the graphic object</param>
        /// <param name="zoom">Zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            Pen myPen = this.CreatePen(zoom);
            myPen.StartCap = this.startCap;
            myPen.EndCap = this.endCap;

            if (this.selected)
                graphObj.DrawEllipse(myPen, (region.X0 + dx) * zoom, (region.Y0 + dy)
                    * zoom, 3, 3);

            if (region.X0 == region.X1 && region.Y0 == region.Y1)
                graphObj.DrawEllipse(myPen, (region.X0 + dx) * zoom,
                    (region.Y0 + dy) * zoom, 3, 3);
            else
            {
                SolidBrush brush = new SolidBrush(this.PenColor);
                SolidBrush textBrush = new SolidBrush(this.textColor);

                graphObj.DrawLine(myPen, (region.X0 + dx) * zoom,
                    (region.Y0 + dy) * zoom, (region.X1 + dx) * zoom,
                    (region.Y1 + dy) * zoom);

                if (!this.text.Equals(string.Empty))
                    graphObj.DrawString(this.text, SystemFonts.CaptionFont, textBrush, region.MidPointF);

                if (this.generateCornerNames)
                {
                    graphObj.DrawString("A", SystemFonts.CaptionFont, brush,
                        (region.X0 + dx - 10) * zoom, (region.Y0 + dy) * zoom);
                    graphObj.DrawString("B", SystemFonts.CaptionFont, brush,
                        (region.X1 + dx + 10) * zoom, (region.Y1 + dy) * zoom);
                }
                brush.Dispose();
                textBrush.Dispose();
            }
            myPen.Dispose();
        }
        #endregion
    }
}
