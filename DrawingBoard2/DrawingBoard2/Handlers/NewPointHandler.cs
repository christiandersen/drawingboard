using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using DrawingBoard2.Shapes;
using DrawingBoard2.Utils;

namespace DrawingBoard2.Handlers
{
    /// <summary>
    /// Handler class which handles with adding new point operation
    /// </summary>
    public class NewPointHandler : Handler
    {
        #region Variables
        private PointElement linkedPoint;
        private ShapeElement element;
        /// <summary>
        /// Index of the handler in the collection
        /// </summary>
        public int Index = 0;
        #endregion

        #region Properties
        /// <summary>
        /// Linked point
        /// </summary>
        public PointElement Point
        {
            get { return this.linkedPoint; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Handler class which handles with adding new point operation
        /// </summary>
        /// <param name="element">Current element</param>
        /// <param name="hndlOperator">Handler Operator</param>
        /// <param name="point">New point</param>
        /// <param name="index">index of the handler </param>
        public NewPointHandler(ShapeElement element, HandlerOperator hndlOperator,
            PointElement point ,int index) : base(element,hndlOperator)
        {
            this.element = element;
            Index = index;
            this.FillColor = Color.YellowGreen;
            this.linkedPoint = point;
            this.RePosition(element);
        }
        #endregion

        #region Methods
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
        /// Re-positions handler according to shape region
        /// </summary>
        /// <param name="element"></param>
        public override void RePosition(ShapeElement element)
        {
            if (this.linkedPoint == null)
                return;
            region.X0 = (int)(linkedPoint.X + element.PosStartX - 1);
            region.Y0 = (int)(linkedPoint.Y + element.PosStartY - 1);
            region.X1 = region.X0 + 3;
            region.Y1 = region.Y0 + 3;
        }
        /// <summary>
        /// Draws itself on the graph object
        /// </summary>
        /// <param name="graphObj">Graphic object to be drawn on</param>
        /// <param name="dx">X position on the graphic object</param>
        /// <param name="dy">Y position on the graphic object</param>
        /// <param name="zoom">Zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            SolidBrush myBrush = new SolidBrush(this.FillColor);
            myBrush.Color = DrawingUtils.SetTransparency(myBrush.Color, 80);
            Pen whitePen = new Pen(Color.White);
            Pen fillPen = new Pen(this.FillColor);

            graphObj.FillRectangle(myBrush,region.GetRectangleF(dx,dy,zoom));
            graphObj.DrawRectangle(whitePen, (region.X0 + dx) * zoom, (region.Y1 + dy) * zoom,
                (region.X1 - region.X0) * zoom, (region.Y1 - region.Y0) * zoom);
            graphObj.DrawRectangle(fillPen, (region.X0 + dx - 1) * zoom, 
                (region.Y0 + dy - 1) * zoom, (region.X1 - region.X0 + 2) * zoom, 
                (region.Y1 - region.Y0 + 2) * zoom);

            myBrush.Dispose();
            whitePen.Dispose();
            fillPen.Dispose();
        }
        #endregion
    }
}
