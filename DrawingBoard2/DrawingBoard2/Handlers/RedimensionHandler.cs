using System;
using System.Drawing;

using DrawingBoard2;
using DrawingBoard2.Shapes;
using DrawingBoard2.Utils;

namespace DrawingBoard2.Handlers
{
    /// <summary>
    /// Handler class which handles with re-dimension of shapes
    /// </summary>
    public class RedimensionHandler : Handler
    {
        #region Constructor
        /// <summary>
        /// Handler class which handles with re-dimension of shapes
        /// </summary>
        /// <param name="element">Shape Element to be handled</param>
        /// <param name="hOperator">Handler operator</param>
        public RedimensionHandler(ShapeElement element, HandlerOperator hOperator)
            : base(element,hOperator)
        {
            this.FillColor = Color.Black;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Re-Positions handler according to shape region
        /// </summary>
        /// <param name="shape"></param>
        public override void RePosition(ShapeElement shape)
        {
            if (shape is Polygon)
                return;
            switch (this.handleOperator)
            {
                case HandlerOperator.RedimNorthWest:
                    region.X0 = shape.PosStartX - 2;
                    region.Y0 = shape.PosStartX - 2;
                    break;
                case HandlerOperator.RedimNorth:
                    region.X0 = shape.PosStartX - 2 + ((shape.PosEndX - shape.PosStartX) / 2);
                    region.Y0 = shape.PosStartX - 2;
                    break;
                case HandlerOperator.RedimNorthEast://"NE":
                    region.X0 = shape.PosEndX - 2;
                    region.Y0 = shape.PosStartY - 2;
                    break;
                case HandlerOperator.RedimEast://"E":
                    region.X0 = shape.PosEndX - 2;
                    region.Y0 = shape.PosStartY - 2 + (shape.PosEndY - shape.PosStartY) / 2;
                    break;
                case HandlerOperator.RedimSouthEast:
                    region.X0 = shape.PosEndX - 2;
                    region.Y0 = shape.PosEndY - 2;
                    break;
                case HandlerOperator.RedimSouth:
                    region.X0 = shape.PosStartX - 2 + (shape.PosEndX - shape.PosStartX) / 2;
                    region.Y0 = shape.PosEndY - 2;
                    break;
                case HandlerOperator.RedimSouthWest:
                    region.X0 = shape.PosStartX - 2;
                    region.Y0 = shape.PosEndY - 2;
                    break;
                case HandlerOperator.RedimWest:
                    region.X0 = shape.PosStartX - 2;
                    region.Y0 = shape.PosStartY - 2 + (shape.PosEndY - shape.PosStartY) / 2;
                    break;
                default:
                    break;
            }
            region.X1 = this.PosStartX + 5;
            region.Y1 = region.Y0 + 5;

        }
        /// <summary>
        /// Draws itself on the graph object
        /// </summary>
        /// <param name="graphObj">Graphic object to be drawn on</param>
        /// <param name="dx">X region on the graphic object</param>
        /// <param name="dy">Y region on the graphic object</param>
        /// <param name="zoom">Zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            SolidBrush myBrush = new SolidBrush(this.FillColor);
            myBrush.Color = DrawingUtils.SetTransparency(Color.Black, 80);
            Pen whitePen = new Pen(Color.White);
            graphObj.FillRectangle(myBrush, region.GetRectangleF(dx,dy,zoom));
            graphObj.DrawRectangle(whitePen, (region.X0 + dx) * zoom, (region.Y0 + dy) * zoom, 
                (region.X1 - region.X0) * zoom, (region.Y1 - region.Y0) * zoom);
            myBrush.Dispose();
            whitePen.Dispose();
        }
        #endregion

    }
}
