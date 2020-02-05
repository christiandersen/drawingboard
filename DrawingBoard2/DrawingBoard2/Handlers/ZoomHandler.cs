using System;
using System.Drawing;

using DrawingBoard2;
using DrawingBoard2.Shapes;
using DrawingBoard2.Utils;

namespace DrawingBoard2.Handlers
{
    /// <summary>
    /// Handler class that handles with zooming operation
    /// </summary>
    public class ZoomHandler : Handler
    {
        #region Constructor
        /// <summary>
        /// Handler class that handles with zooming operation
        /// </summary>
        /// <param name="element">Shape element to be zoomed</param>
        /// <param name="hOperator">Handler operator</param>
        public ZoomHandler(ShapeElement element, HandlerOperator hOperator)
            : base(element,hOperator)
        {
            this.FillColor = Color.Black;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Re-positions handler according to shape region
        /// </summary>
        /// <param name="shape">Shape Element</param>
        public override void RePosition(ShapeElement shape)
        {
            float zx = (shape.Region.Width - (shape.Region.Width * shape.GroupZoomX)) / 2;
            float zy = (shape.Region.Height - (shape.Region.Height * shape.GroupZoomY)) / 2;
            region.X0 = (int)((shape.PosEndX - 2) - zx);
            region.Y0 = (int)((shape.PosEndY - 2) - zy);
            region.X1 = region.X0 + 5;
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
            myBrush.Color = DrawingUtils.SetTransparency(myBrush.Color, 80);
            Pen whitePen = new Pen(Color.White);
            Pen fillPen = new Pen(this.FillColor);

            graphObj.FillRectangle(myBrush, region.GetRectangleF(dx, dy, zoom));
            graphObj.DrawRectangle(whitePen, (region.X0 + dx) * zoom, (region.Y0 + dy) * zoom,
                (region.X1 - region.X0) * zoom, (region.Y1 - region.Y0) * zoom);

            graphObj.DrawRectangle(fillPen, (region.X0 + dx - 1) * zoom, (region.Y0 + dy - 1) * zoom,
                (region.X1 - region.X0 + 2) * zoom, (region.Y1 - region.Y0 + 2) * zoom);

            myBrush.Dispose();
            whitePen.Dispose();
            fillPen.Dispose();
        }
        #endregion
    }
}
