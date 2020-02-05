using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

using DrawingBoard2;
using DrawingBoard2.Shapes;

namespace DrawingBoard2.Handlers
{
    /// <summary>
    /// Handler collection inherited from <see cref="DrawingBoard2.Handlers.BaseHandlerCollection"/>
    /// </summary>
    public class ShapeHandlerCollection : BaseHandlerCollection
    {
        #region Constructor
        /// <summary>
        /// Handler collection
        /// </summary>
        /// <param name="element">Shape element to be handled</param>
        public ShapeHandlerCollection(ShapeElement element)
            : base(element)
        {
            this.SetupHandlers();
        }
        #endregion

        #region Methods
        /// <summary>
        ///Set ups handlers
        /// </summary>
        public void SetupHandlers()
        {
            if (!this.isGroup)
            {
                this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimNorthWest));
                this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimSouthEast));

                if (!this.IsThisLine)
                {
                    this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimNorth));

                    if (this.canRotate)
                        this.handlers.Add(new RotationHandler(this, HandlerOperator.Rotation));
                    this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimNorthEast));
                    this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimEast));
                    this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimSouth));
                    this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimSouthWest));
                    this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimWest));
                }
            }
            else
            {
                this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimNorth));
                if (this.canRotate)
                    this.handlers.Add(new RotationHandler(this, HandlerOperator.Rotation));
                this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimEast));
                this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimSouth));
                this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimWest));
                this.handlers.Add(new ZoomHandler(this,HandlerOperator.Zoom));
            }
        }
        /// <summary>
        /// Draws itself on board(graph object)
        /// </summary>
        /// <param name="graphObj">Graphic object to be drawn on</param>
        /// <param name="dx">X region on the graphic object</param>
        /// <param name="dy">Y region on the graphic object</param>
        /// <param name="zoom">Zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            base.Draw(graphObj, dx, dy, zoom);
            Pen myPen = new Pen(Color.Blue, 1.5f);
            myPen.DashStyle = DashStyle.Dash;

            if (this.IsThisLine)
                graphObj.DrawLine(myPen, (region.X0 + dx) * zoom, (region.Y0 + dy) * zoom, 
                    (region.X1 + dx) * zoom, (region.Y1 + dy) * zoom);
            else
                graphObj.DrawRectangle(myPen, (region.X0 + dx) * zoom, (region.Y0 + dy) * zoom,
                    (region.X1 - region.X0) * zoom, (region.Y1 - region.Y0) * zoom);
            myPen.Dispose();
        }
        #endregion
    }
}
