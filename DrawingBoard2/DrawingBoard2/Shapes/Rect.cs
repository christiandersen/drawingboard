using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Rectangle shape
    /// </summary>
    [Serializable]
    public class Rect : ShapeElement
    {
        #region Constructors
        /// <summary>
        /// Creates Rectangle  shape
        /// </summary>
        public Rect() 
        {
            this.Selected = true;
            this.EndMoveRedim();
            this.Rotation = 0;
            this.canRotate = true;
        }      
        #endregion

        #region Methods
        /// <summary>
        /// Over ridden copy method.
        /// Creates a new instance of itself for deep copy
        /// </summary>
        /// <returns>Created instance</returns>
        public override ShapeElement Copy()
        {
            return CloneUtil<ShapeElement>.DeepClone<ShapeElement>(this);
            //return this.CreateInstanceForCopy() as ShapeElement;
        }
        /// <summary>
        /// Copies itself to undoshape
        /// </summary>
        public override void Select()
        {
            this.undoShape = this.Copy();
        }
        /// <summary>
        /// Adds rectangle to a graph path
        /// </summary>
        /// <param name="graphicPath">Graph path to be added</param>
        /// <param name="dx">X region</param>
        /// <param name="dy">Y region</param>
        /// <param name="zoom">Zoom value</param>
        public override void AddToGraphPath(GraphicsPath graphicPath, int dx, int dy, float zoom)
        {
            graphicPath.AddRectangle(region.GetRectangleF(dx,dy,zoom));
        }
        /// <summary>
        /// Draws rectangle object on the board(graphObj)
        /// </summary>
        /// <param name="graphObj">Graph Object(board) to drawn on</param>
        /// <param name="dx">X region on board</param>
        /// <param name="dy">Y region on board</param>
        /// <param name="zoom">Zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            Brush myBrush = GetBrush(dx, dy, zoom);
            Pen myPen = this.CreatePen(zoom);

            // Create a path and add the object.
            GraphicsPath myPath = new GraphicsPath();
            myPath.AddRectangle(region.GetRectangleF(dx,dy,zoom));

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
            translateMatrix.RotateAt(this.Rotation,region.GetActualregion(dx,dy,zoom));
            myPath.Transform(translateMatrix);

            // Draw the transformed ellipse to the screen.
            if (this.FillEnabled)
            {
                graphObj.FillPath(myBrush, myPath);
                //g.FillPath(br, myPath);
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
