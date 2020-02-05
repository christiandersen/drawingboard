using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Rectangle with rounded corners
    /// </summary>
    [Serializable()]
    public class RoundedRect : ShapeElement
    {
        #region Variables
        private int arcsWidth;
        #endregion

        #region Properties
        /// <summary>
        /// Arc width of the round of the roundrectangles
        /// </summary>
        [Category("Appearance"), Description("Arc width of the round of the roundrectangles")]
        public int ArcsWidth
        {
            get{   return arcsWidth; }
            set
            {
                if (value <= 9)
                    arcsWidth = 10;
                else
                    arcsWidth = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a rounded rectangle
        /// </summary>
        public RoundedRect() 
        {
            this.selected = true;
            this.arcsWidth = 20;
            this.rotation = 0;
            this.EndMoveRedim();
            this.canRotate = true; //can rotate?
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
            //return this.CreateInstanceForCopy() as ShapeElement;
        }
        /// <summary>
        /// Sets itself as undo shape 
        /// </summary>
        public override void Select()
        {
            this.undoShape = this.Copy();
        }
        /// <summary>
        /// Adds itself to graphic path as rounded rectangle
        /// </summary>
        /// <param name="graphicPath">Graphic path that will contain rounded rectangle</param>
        /// <param name="dx">X region on path</param>
        /// <param name="dy">Y region on path</param>
        /// <param name="zoom"></param>
        public override void AddToGraphPath(GraphicsPath graphicPath, int dx, int dy, float zoom)
        {
            float n = this.arcsWidth;
            graphicPath.AddArc(new RectangleF((region.X0 + dx) * zoom, (region.Y0 + dy)
                * zoom, n * zoom, n * zoom), 180, 90);
            graphicPath.AddLine((region.X0 + dx + n / 2) * zoom, (region.Y0 + dy) *
                zoom, (region.X1 + dx - n / 2) * zoom, (region.Y0 + dy) * zoom);

            graphicPath.AddArc(new RectangleF((region.X1 + dx - n) * zoom, 
                (region.Y0 + dy) * zoom, n * zoom, n * zoom), 270, 90);
            graphicPath.AddLine((region.X1 + dx) * zoom, (region.Y0 + dy + n / 2) * zoom,
                (region.X1 + dx) * zoom, (region.Y1 + dy - n / 2) * zoom);

            graphicPath.AddArc(new RectangleF((region.X1 + dx - n) * zoom, 
                (region.Y1 + dy - n) * zoom, n * zoom, n * zoom), 0, 90);
            graphicPath.AddLine((region.X0 + dx + n / 2) * zoom, (region.Y1 + dy) * zoom,
                (region.X1 + dx - n / 2) * zoom, (region.Y1 + dy) * zoom);

            graphicPath.AddArc(new RectangleF((region.X0 + dx) * zoom, 
                (region.Y1 + dy - n) * zoom, n * zoom, n * zoom), 90, 90);
            graphicPath.AddLine((region.X0 + dx) * zoom, (region.Y1 + dy - n / 2) * zoom,
                (region.X0 + dx) * zoom, (region.Y0 + dy + n / 2) * zoom);
        }
        /// <summary>
        /// Draws rounded rectangle on the board(graph object)
        /// </summary>
        /// <param name="graphObj">Graph Object(board) to drawn on</param>
        /// <param name="dx">X region on board</param>
        /// <param name="dy">Y region on board</param>
        /// <param name="zoom">Zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            float n = this.arcsWidth;
            Brush myBrush = GetBrush(dx, dy, zoom);
            Pen myPen = this.CreatePen(zoom);

            // Create a path and add the object.
            GraphicsPath myPath = new GraphicsPath();

            myPath.AddArc(new RectangleF((region.X0 + dx) * zoom, (region.Y0 + dy) * zoom, n * zoom, n * zoom), 180, 90);
            myPath.AddLine((region.X0 + dx + n / 2) * zoom, (region.Y0 + dy) * zoom, (region.X1 + dx - n / 2) * zoom, (region.Y0 + dy) * zoom);

            myPath.AddArc(new RectangleF((region.X1 + dx - n) * zoom, (region.Y0 + dy) * zoom, n * zoom, n * zoom), 270, 90);
            myPath.AddLine((region.X1 + dx) * zoom, (region.Y0 + dy + n / 2) * zoom, (region.X1 + dx) * zoom, (region.Y1 + dy - n / 2) * zoom);

            myPath.AddArc(new RectangleF((region.X1 + dx - n) * zoom, (region.Y1 + dy - n) * zoom, n * zoom, n * zoom), 0, 90);
            myPath.AddLine((region.X0 + dx + n / 2) * zoom, (region.Y1 + dy) * zoom, (region.X1 + dx - n / 2) * zoom, (region.Y1 + dy) * zoom);

            myPath.AddArc(new RectangleF((region.X0 + dx) * zoom, (region.Y1 + dy - n) * zoom, n * zoom, n * zoom), 90, 90);
            myPath.AddLine((region.X0 + dx) * zoom, (region.Y1 + dy - n / 2) * zoom, (region.X0 + dx) * zoom, (region.Y0 + dy + n / 2) * zoom);

            if (this.generateCornerNames)
            {
                myPath.AddString("A", SystemFonts.CaptionFont.FontFamily,
                    (int)SystemFonts.CaptionFont.Style, 12, new Point(this.region.X0 - 15,
                        this.region.Y0 - 15), StringFormat.GenericDefault);
                myPath.AddString("B", SystemFonts.CaptionFont.FontFamily,
                    (int)SystemFonts.CaptionFont.Style, 12, new Point(this.region.X1 + 5,
                        this.region.Y0 - 15), StringFormat.GenericDefault);
                myPath.AddString("C", SystemFonts.CaptionFont.FontFamily,
                    (int)SystemFonts.CaptionFont.Style, 12, new Point(this.region.X1 + 5,
                        this.region.Y1 + 5), StringFormat.GenericDefault);
                myPath.AddString("D", SystemFonts.CaptionFont.FontFamily,
                    (int)SystemFonts.CaptionFont.Style, 12, new Point(this.region.X0 - 15,
                        this.region.Y1 + 5), StringFormat.GenericDefault);
            }
            Matrix translateMatrix = new Matrix();
            translateMatrix.RotateAt(this.Rotation, new PointF((region.X0 + dx + (int)(region.X1 - region.X0) / 2) * zoom, (region.Y0 + dy + (int)(region.Y1 - region.Y0) / 2) * zoom));
            myPath.Transform(translateMatrix);

            // Draw the transformed ellipse to the screen.
            if (this.FillEnabled)
            {
                graphObj.FillPath(myBrush, myPath);
                if (this.ShowBorder)
                    graphObj.DrawPath(myPen, myPath);
            }
            else
                graphObj.DrawPath(myPen, myPath);

            myPath.Dispose();
            myPen.Dispose();
            if (myBrush != null)
                myBrush.Dispose();
        }
        #endregion
    }
}
