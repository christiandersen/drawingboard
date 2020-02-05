using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

using DrawingBoard2.Forms;
using DrawingBoard2.Controls;
using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Shape that draws rich text box text(RTF) 
    /// </summary>
    [Serializable]
    public class RichTextBoxShape : ShapeElement
    {
        #region Variables
        private string rtf = string.Empty; //RTF text of richtextbox
        [field: NonSerialized]
        private TransparentRichTextBox transpRichTB = new TransparentRichTextBox();
        #endregion

        #region Properties
        /// <summary>
        /// RTF content of the Rich TextBox
        /// </summary>
        [Editor(typeof(RTFTypeEditor), typeof(System.Drawing.Design.UITypeEditor)),
        Description("RTF content of the Rich TextBox")]
        public string RTF
        {
            get { return this.rtf; }
            set { this.rtf = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates richtextbox shape
        /// </summary>
        public RichTextBoxShape()
        {
            this.selected = true;
            this.EndMoveRedim();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds rectangle to a graph path
        /// </summary>
        /// <param name="graphicPath">Graph path to be added</param>
        /// <param name="dx">X region</param>
        /// <param name="dy">Y region</param>
        /// <param name="zoom">Zoom value</param>
        public override void AddToGraphPath(GraphicsPath graphicPath, int dx, int dy, float zoom)
        {
            graphicPath.AddRectangle(this.region.GetRectangleF(dx, dy, zoom));
        }
        /// <summary>
        /// Over ridden copy method.
        /// Creates a new instance of itself for deep copy
        /// </summary>
        /// <returns>Created instance</returns>
        public override ShapeElement Copy()
        {
            RichTextBoxShape rtbShape = new RichTextBoxShape();
            rtbShape.canRotate = this.canRotate;
            rtbShape.ColorAlpha = this.ColorAlpha;
            rtbShape.DashStyle = this.DashStyle;
            rtbShape.EastResize = this.EastResize;
            rtbShape.EnableCornerNameGeneration = this.EnableCornerNameGeneration;
            rtbShape.EndColorPosition = this.EndColorPosition;
            rtbShape.FillColor = this.FillColor;
            rtbShape.FillEnabled = this.FillEnabled;
            rtbShape.GradientAlpha = this.GradientAlpha;
            rtbShape.GradientAngle = this.GradientAngle;
            rtbShape.GradientColor = this.GradientColor;
            rtbShape.GradientDimension = this.GradientDimension;
            rtbShape.groupZoomX = this.groupZoomX;
            rtbShape.groupZoomY = this.groupZoomY;
            rtbShape.isGroup = this.isGroup;
            rtbShape.IsLine = this.IsLine;
            rtbShape.NorthResize = this.NorthResize;
            rtbShape.oldregion = this.oldregion;
            rtbShape.PenColor = this.PenColor;
            rtbShape.PenWidth = this.PenWidth;
            rtbShape.Region = this.Region;
            rtbShape.Rotation = this.Rotation;
            rtbShape.RTF = this.rtf;
            rtbShape.selected = this.selected;
            rtbShape.ShowBorder = this.ShowBorder;
            rtbShape.SouthResize = this.SouthResize;
            rtbShape.transpRichTB = this.transpRichTB;
            rtbShape.undoShape = this.undoShape;
            rtbShape.UseGradientColor = this.UseGradientColor;
            rtbShape.WestResize = this.WestResize;

            return rtbShape;
        }
        /// <summary>
        /// Copies itself to undoshape
        /// </summary>
        public override void Select()
        {
            this.undoShape = this.Copy();
        }
        /// <summary>
        /// Displays RichTextBox form and fetches rich text
        /// </summary>
        /// <param name="form">Windows Form that displays rich textbox</param>
        public void ShowRichEditor(RichTextBoxForm form)
        {
            form.BackColor = this.FillColor;
            form.FontColor = this.PenColor;
            form.RichTextBox.Rtf = this.rtf;
            form.ShowDialog();

            if (form.Confirmed)
                this.rtf = form.RichTextBox.Rtf;
        }
        /// <summary>
        /// Draws rich text box object on the board(graphObj)
        /// </summary>
        /// <param name="graphObj">Graph Object(board) to drawn on</param>
        /// <param name="dx">X region on board</param>
        /// <param name="dy">Y region on board</param>
        /// <param name="zoom">Zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            Brush myBrush = this.GetBrush(dx, dy, zoom);
            Pen pen = this.CreatePen(zoom);

            if (this.FillEnabled)
                graphObj.FillRectangle(myBrush, this.region.GetRectangleF(dx, dy, zoom));
            if (this.ShowBorder || this.selected)
                graphObj.DrawRectangle(pen, Rectangle.Round(this.region.GetRectangleF(dx, dy, zoom)));
            transpRichTB.BorderStyle = BorderStyle.None;
            transpRichTB.ScrollBars = RichTextBoxScrollBars.None;
            transpRichTB.Rtf = rtf;

            if (graphObj.DpiX < 600)
                transpRichTB.Draw(graphObj, new Region((this.PosStartX + dx) * zoom,
                    (this.PosStartY + dy) * zoom, (this.PosEndX + dx) * zoom, (this.PosEndY + dy) * zoom),
                    1440 / graphObj.DpiX, 1440 / graphObj.DpiY);
            else
                transpRichTB.Draw(graphObj, new Region((this.PosStartX + dx) * zoom,
                    (this.PosStartY + dy) * zoom, (this.PosEndX + dx) * zoom, (this.PosEndY + dy) * zoom),
                    14.4, 14.4);

            pen.Dispose();

            if (myBrush != null)
                myBrush.Dispose();
        }
        #endregion
    }
}
