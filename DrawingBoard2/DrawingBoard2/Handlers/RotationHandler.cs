using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using DrawingBoard2;
using DrawingBoard2.Shapes;
using DrawingBoard2.Utils;

namespace DrawingBoard2.Handlers
{
    /// <summary>
    /// Handler class which handles with rotation of shape elements
    /// </summary>
    public class RotationHandler : Handler
    {
        #region Constructor
        /// <summary>
        /// Handler class which handles with rotation of shape elements
        /// </summary>
        /// <param name="element"></param>
        /// <param name="hOperator"></param>
        public RotationHandler(ShapeElement element, HandlerOperator hOperator)
            : base(element,hOperator)
        {
            this.FillColor = Color.Black;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Re-positions handler according to shape region
        /// </summary>
        /// <param name="shape">Shape to be repositioned</param>
        public override void RePosition(ShapeElement shape)
        {
            float midX = (shape.Region.X1 - shape.Region.X0) / 2;
            float midY = (shape.Region.Y1 - shape.Region.Y0) / 2;
            PointF Hp = new PointF(0, -25);
            PointF RotHP = DrawingUtils.RotatePoint(Hp, shape.Rotation);
            midX += RotHP.X;
            midY += RotHP.Y;

            region.X0 = shape.PosStartX + (int)midX - 2;
            region.Y0 = shape.PosStartY + (int)midY - 2;
            this.rotation = shape.Rotation;

            region.X1 = region.X0 + 5;
            region.Y1 = region.Y0 + 5;
        }
        /// <summary>
        /// Draws itself on board
        /// </summary>
        /// <param name="graphObj">Graphic object of the board</param>
        /// <param name="dx">Shift value of x coordinate</param>
        /// <param name="dy">Shift value of y coordinate</param>
        /// <param name="zoom">Current zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            SolidBrush myBrush = new SolidBrush(Color.Black);
            myBrush.Color = DrawingUtils.SetTransparency(Color.Black, 80);

            Pen whitePen = new Pen(Color.White);
            Pen myPen = new Pen(Color.Blue, 1.5f);
            myPen.DashStyle = DashStyle.Dash;

            graphObj.FillRectangle(myBrush, region.GetRectangleF(dx, dy, zoom));
            graphObj.DrawRectangle(whitePen, (region.X0 + dx) * zoom, (region.Y0 + dy) * 
                zoom, (region.X1 - region.X0) * zoom, (region.Y1- region.Y0) * zoom);

            //CENTER POINT
            float midX = (this.region.X1 - this.region.X0) / 2;
            float midY = (this.region.Y1 - this.region.Y0) / 2;

            PointF Hp = new PointF(0, 25);

            PointF RotHP = DrawingUtils.RotatePoint(Hp, this.rotation);

            RotHP.X += region.X0;
            RotHP.Y += region.Y0;
            graphObj.FillEllipse(myBrush, (RotHP.X + midX + dx - 3) * zoom, (RotHP.Y + dy - 3 + 
                midY) * zoom, 6 * zoom, 6 * zoom);
            graphObj.DrawEllipse(whitePen, (RotHP.X + midX + dx - 3) * zoom, (RotHP.Y + dy - 3 +
                midY) * zoom, 6 * zoom, 6 * zoom);
            graphObj.DrawLine(myPen, (region.X0 + midX + dx) * zoom, (region.Y0 + midY + dy)
                * zoom, (RotHP.X + midX + dx) * zoom, (RotHP.Y + midY + dy) * zoom);

            myPen.Dispose();
            myBrush.Dispose();
            whitePen.Dispose();
        }
        #endregion
    }
}
