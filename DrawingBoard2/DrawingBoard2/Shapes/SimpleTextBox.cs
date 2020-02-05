using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Simple textbox
    /// </summary>
    [Serializable]
    public class SimpleTextBox : ShapeElement
    {
        #region Variables
        private Font font;
        private StringAlignment textAlignment;
        #endregion

        #region Properties
        /// <summary>
        /// Text 
        /// </summary>        
        [Description("Text of the textbox")]
        public string Text { get; set; }

        /// <summary>
        /// Specifies whether the text in the object is left-aligned, right-aligned, centered, or justified
        /// </summary>
        [Description("Specifies whether the text in the object is left-aligned, right-aligned, centered, or justified")]
        public StringAlignment TextAlignment
        {
            get { return this.textAlignment; }
            set { this.textAlignment = value; }
        }
        /// <summary>
        /// Font of the textbox
        /// </summary>
        [YAXDontSerialize]
        public Font Font
        {
            get { return this.font; }
            set { this.font = value; }
        }
        /// <summary>
        /// XML serializable font
        /// </summary>
        [Browsable(false)]
        public SerializableFont XMLFont
        {
            get { return SerializableFont.FromFont(this.font); }
            set { this.font = value.ToFont(); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create simple text box shape
        /// </summary>
        public SimpleTextBox() 
        {
            this.Selected = true;
            this.EndMoveRedim();
            this.Rotation = 0;
            this.canRotate = true;
        }       
        #endregion

        #region Methods
        /// <summary>
        /// Adss itself(Simple Text Box) to the graph path
        /// </summary>
        /// <param name="graphicPath">Graph path that will be added</param>
        /// <param name="dx">X region on the path</param>
        /// <param name="dy">Y region on the path</param>
        /// <param name="zoom">Zoom value</param>
        public override void AddToGraphPath(GraphicsPath graphicPath, int dx, int dy, float zoom)
        {
            graphicPath.AddRectangle(region.GetRectangleF(dx,dy,zoom));
        }
        /// <summary>
        /// Creates a new instance and returns it
        /// </summary>
        /// <returns></returns>
        public override ShapeElement Copy()
        {
            return CloneUtil<ShapeElement>.DeepClone<ShapeElement>(this);
           
        }
        /// <summary>
        /// Selects itself, copies itself to undo shape
        /// </summary>
        public override void Select()
        {
            this.undoShape = this.Copy();
        }
        /// <summary>
        /// Draws itself on a graph object(board)
        /// </summary>
        /// <param name="graphObj">Board to be drawn on</param>
        /// <param name="dx">X region on the board</param>
        /// <param name="dy">Y region on the board</param>
        /// <param name="zoom">Zoom value of the board</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            GraphicsState oldGraphState = graphObj.Save();//store previos trasformation
            Matrix matrix = graphObj.Transform; // get previous trasformation

            PointF point = region.GetActualregion(dx, dy, zoom);
            if (this.Rotation > 0)
                matrix.RotateAt(this.Rotation, point, MatrixOrder.Append); //add a trasformation

            graphObj.Transform = matrix;
            Brush myBrush = GetBrush(dx, dy, zoom);
            Pen myPen = this.CreatePen(zoom);

            if (this.FillEnabled)
                graphObj.FillRectangle(myBrush, region.GetRectangleF(dx,dy,zoom));

            if (this.ShowBorder || this.Selected)
                graphObj.DrawRectangle(myPen, Rectangle.Round(region.GetRectangleF(dx,dy,zoom)));

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = this.textAlignment;
            stringFormat.LineAlignment = StringAlignment.Near;
            Font tempFont = null;

            if (font != null)
                tempFont = new Font(this.font.FontFamily, this.Font.Size * zoom,
                   this.font.Style);
            else
                tempFont = new System.Drawing.Font("Arial", 12.0f);

            graphObj.DrawString(this.Text, tempFont, new SolidBrush(this.PenColor), 
                region.GetRectangleF(dx,dy,zoom),stringFormat);

            tempFont.Dispose();
            myPen.Dispose();
            if (myBrush != null)
                myBrush.Dispose();

            graphObj.Restore(oldGraphState);//restore previos trasformation
        }
        #endregion
    }
}
