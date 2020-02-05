using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using DrawingBoard2.Shapes;
using DrawingBoard2.Utils;

namespace DrawingBoard2.Handlers
{
    /// <summary>
    /// Handler class which handles with point elements
    /// </summary>
    public class PointHandler : Handler
    {
        #region Variables
        private ShapeElement element;
        private PointElement linkedPoint;
        #endregion

        #region Properties
        /// <summary>
        /// Point to be handled
        /// </summary>
        public PointElement Point
        {
            get { return this.linkedPoint; }
        }
        /// <summary>
        /// Indicates, whether point or handler itself is selected 
        /// </summary>
        public new bool Selected
        {
            get { return this.selected | linkedPoint.Selected; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Handler class which handles with point elements
        /// </summary>
        /// <param name="element"></param>
        /// <param name="hOperator">Handler operator</param>
        /// <param name="point">Point element to be handled</param>
        public PointHandler(ShapeElement element, HandlerOperator hOperator,
            PointElement point) : base(element,hOperator)
        {
            this.FillColor = Color.BlueViolet;
            this.linkedPoint = point;
            this.element = element;
            this.RePosition(element);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Re-Positions handler according to shape region
        /// </summary>
        /// <param name="shape"></param>
        public override void RePosition(ShapeElement shape)
        {
            if (linkedPoint == null)
                return;
            region.X0 = (int)(linkedPoint.X + shape.PosStartX - 2);
            region.Y0 = (int)(linkedPoint.Y + shape.PosStartY - 2);
            region.X1 = region.X0 + 5;
            region.Y1 = region.Y0 + 5;
        }
        /// <summary>
        /// Moves itself and linked point
        /// </summary>
        /// <param name="x">Move by x on X axis</param>
        /// <param name="y">Move by y on Y coordinate</param>
        public override void Move(int x, int y)
        {
            base.Move(x, y);
            this.linkedPoint.X = region.X0 + 2 - element.PosStartX;
            this.linkedPoint.Y = region.Y0 + 2 - element.PosStartY;

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

            if (this.Selected)
                fillPen.Color = Color.Red;

            graphObj.FillRectangle(myBrush,region.GetRectangleF(dx,dy,zoom));
            graphObj.DrawRectangle(whitePen, (region.X0 + dx) * zoom, (region.Y0 + dy) * 
                zoom, (region.X1 - region.X0) * zoom, (region.Y1 - region.Y0) * zoom);

            graphObj.DrawRectangle(fillPen, (region.X0 + dx - 1) * zoom, (region.Y0 + dy - 1)
                * zoom, (region.X1 - region.X0 + 2) * zoom, (region.Y1 - region.Y0 + 2) * zoom);

            myBrush.Dispose();
            whitePen.Dispose();
            fillPen.Dispose();
        }
        #endregion
    }
}
