using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Ellipse shape
    /// </summary>
    [Serializable]
    public class Ellipse : ShapeElement
    {
        #region Constructor
        /// <summary>
        /// Ellipse shape
        /// </summary>
        public Ellipse()
        {
            this.selected = true;
            this.EndMoveRedim();
            this.canRotate = true;
            this.rotation = 0;
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
        /// Adds itself to graphic path as Ellipse
        /// </summary>
        /// <param name="graphicPath">Graphic path that will contain Ellipse</param>
        /// <param name="dx">X region on path</param>
        /// <param name="dy">Y region on path</param>
        /// <param name="zoom"></param>
        public override void AddToGraphPath(GraphicsPath graphicPath, int dx, int dy, float zoom)
        {
            graphicPath.AddEllipse(region.GetRectangleF(dx, dy, zoom));
        }
        /// <summary>
        /// Draws ellipse on the board(graph object)
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
            myPath.AddEllipse(region.GetRectangleF(dx, dy, zoom));
            Matrix translateMatrix = new Matrix();
            translateMatrix.RotateAt(this.Rotation, region.GetActualregion(dx, dy, zoom));
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
            
            if (this.generateCornerNames)
                myPath.AddString("o", SystemFonts.CaptionFont.FontFamily,
                   (int)SystemFonts.CaptionFont.Style, 12,region.MidPointF,StringFormat.GenericDefault);
            

            myPath.Dispose();
            myPen.Dispose();
            if (myBrush != null)
                myBrush.Dispose();
        }
        #endregion
    }
}
