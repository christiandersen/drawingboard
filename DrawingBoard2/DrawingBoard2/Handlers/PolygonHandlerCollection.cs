using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

using DrawingBoard2;
using DrawingBoard2.Shapes;

namespace DrawingBoard2.Handlers
{
    /// <summary>
    /// Handler collection class which is collection of handlers which handle with polygons and pointsets
    /// </summary>
    public class PolygonHandlerCollection : BaseHandlerCollection
    {
        #region Constructor
        /// <summary>
        /// Handler collection class which is collection of handlers which handle with polygons and pointsets
        /// </summary>
        /// <param name="element">Shape Element to be handled</param>
        public PolygonHandlerCollection(ShapeElement element)
            : base(element)
        {
            if(element is Polygon)
                this.Setup(element as Polygon);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets up handlers for polygon handler
        /// </summary>
        /// <param name="polygon">Polygon to be handled</param>
        public void Setup(Polygon polygon)
        {
            if (this.canRotate)
                this.handlers.Add(new RotationHandler(this,HandlerOperator.Rotation));

            PointElement pointElement = null;
            int counter = 0;
            int minx = 0;
            int miny = 0;
            int maxx = 0;
            int maxy = 0;

            foreach (PointElement point in polygon.Points)
            {
                counter++;
                this.handlers.Add(new PointHandler(this, HandlerOperator.Polygon, point));

                if (pointElement != null)
                {
                    minx = Math.Min(point.X, pointElement.X);
                    miny = Math.Min(point.Y, pointElement.Y);
                    maxx = Math.Max(point.X, pointElement.X);
                    maxy = Math.Max(point.Y, pointElement.Y);

                    PointElement newPoint = new PointElement(minx + (int)((maxx - minx) / 2),
                           miny + (int)((maxy - miny) / 2));
                    this.handlers.Add(new NewPointHandler(this, HandlerOperator.NewPoint,
                        newPoint, counter));
                }
                pointElement = point;
            }
            if (counter > 0)
            {
                PointElement newPoint = new PointElement(pointElement.X + 7, pointElement.Y + 7);
                this.handlers.Add(new NewPointHandler(this, HandlerOperator.NewPoint,
                    newPoint, counter + 1));
            }
            this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimSouthEast));
            this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimSouth));
            this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimEast));
            this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimWest));
            this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimSouthWest));
            this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimNorthWest));
            this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimNorth));
            this.handlers.Add(new RedimensionHandler(this, HandlerOperator.RedimNorthEast));
        }
        /// <summary>
        /// Removes existing NewPointHandlers and creates a new NewPointHandler
        /// for each point in pointset
        /// </summary>
        /// <param name="pointSet">Set of point</param>
        public void ReCreateCreationHandlers(Polygon pointSet)
        {
            for (int i = 0; i < this.handlers.Count; i++)
                if (this.handlers[i] is NewPointHandler)
                    handlers.RemoveAt(i--);
            PointElement pointElement = null;
            int counter = 0;
            int minx = 0;
            int miny = 0;
            int maxx = 0;
            int maxy = 0;

            foreach (PointElement point in pointSet.points)
            {
                counter++;
                this.handlers.Add(new PointHandler(this, HandlerOperator.Polygon, point));

                if (pointElement != null)
                {
                    minx = Math.Min(point.X, pointElement.X);
                    miny = Math.Min(point.Y, pointElement.Y);
                    maxx = Math.Max(point.X, pointElement.X);
                    maxy = Math.Max(point.Y, pointElement.Y);

                    PointElement newPoint = new PointElement(minx + (int)((maxx - minx) / 2),
                           miny + (int)((maxy - miny) / 2));
                    this.handlers.Add(new NewPointHandler(this, HandlerOperator.NewPoint,
                        newPoint, counter));
                }
                pointElement = point;
            }
            if (counter > 0)
            {
                PointElement newPoint = new PointElement(pointElement.X + 7, pointElement.Y + 7);
                this.handlers.Add(new NewPointHandler(this, HandlerOperator.NewPoint,
                    newPoint, counter + 1));
            }
        }        
        /// <summary>
        /// Finds selected points among  pointhandlers  and returns
        /// </summary>
        /// <returns>List of pointelement that are selected</returns>
        public List<PointElement> GetSelectedPoints()
        {
            List<PointElement> selectedPoints = new List<PointElement>();

            foreach (Handler handler in this.handlers)
                if (handler is PointHandler & handler.Selected)
                    selectedPoints.Add((handler as PointHandler).Point);

            return selectedPoints;
        }
        /// <summary>
        /// Searchs selected NewPointHandler in handler list
        /// </summary>
        /// <returns>Index of found handler</returns>
        public int GetNewPointIndex()
        {
            foreach (Handler handler in handlers)
                if (handler is NewPointHandler && handler.Selected)
                    return (handler as NewPointHandler).Index;
            return 0;
        }   
        /// <summary>
        /// Searchs selected NewPointHandler in handler list
        /// </summary>
        /// <returns>PointElement of found handler</returns>
        public PointElement GetNewPoint()
        {
            foreach (Handler handler in handlers)
                if (handler is NewPointHandler && handler.Selected)
                    return (handler as NewPointHandler).Point;
            return null;
        }   
        /// <summary>
        /// Moves each point handler in polygon by dx and dy value.
        /// </summary>
        /// <param name="dx">Value to move element at x axis</param>
        /// <param name="dy">Value to move element at y axis</param>
        public void MovePoints(int dx, int dy)
        {
            foreach (Handler handler in handlers)
                if (handler is PointHandler && handler.Selected)
                    handler.Move(dx, dy);
        }
        /// <summary>
        /// Redimension itselfs. When it changes its dimensions , it also
        /// re-regions each of handlers
        /// </summary>
        /// <param name="x"> New x value</param>
        /// <param name="y"> New y value</param>
        /// <param name="direction">Direction that object will move</param>
        public override void Redim(int x, int y, Direction direction)
        {
            base.Redim(x, y, direction);

            foreach (Handler handler in handlers)
            {
                if (handler is NewPointHandler)
                {
                    handler.visible = false;
                }
            }
        }
        /// <summary>
        /// Rotate element, with a rotation angle from a vertical line from the center
        /// of the shape and a line from the center to the point (x,y)
        /// </summary>
        /// <param name="x">Point of x axis </param>
        /// <param name="y">Point of y axis</param>
        public override void Rotate(float x, float y)
        {
            base.Rotate(x, y);

            foreach (Handler handler in this.handlers)
                if (handler is PointHandler | handler is NewPointHandler)
                    handler.visible = false;
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

            Pen myPen = new Pen(Color.Blue, 1f);
            myPen.DashStyle = DashStyle.Dash;
            graphObj.DrawRectangle(myPen, (region.X0 + dx) * zoom, (region.Y0 + dy) * zoom, 
                (region.X1 - region.X0) * zoom, (region.Y1 - region.Y0) * zoom);
            myPen.Dispose();
        }
        #endregion
    }
}
