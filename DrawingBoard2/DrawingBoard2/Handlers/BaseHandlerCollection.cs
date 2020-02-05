using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

using DrawingBoard2.Shapes;

namespace DrawingBoard2.Handlers
{
    /// <summary>
    /// Base class for handler collection.
    /// Handler collection is collection class for handlers
    /// </summary>
    public class BaseHandlerCollection : ShapeElement
    {
        #region Variables
        /// <summary>
        /// List of handlers in collection
        /// </summary>
        protected List<Handler> handlers = new List<Handler>();
        #endregion

        #region Constructor
        /// <summary>
        /// Handler collection class
        /// </summary>
        /// <param name="element">Shape Element to be handled</param>
        public BaseHandlerCollection(ShapeElement element)
        {
            this.region = element.Region;
            this.Selected = false;
            this.canRotate = element.CanRotate;
            this.rotation = element.Rotation;
            this.groupZoomX = element.GroupZoomX;
            this.groupZoomY = element.GroupZoomY;
            this.IsLine = element.IsThisLine;
            this.isGroup = element.IsGroup;
            this.EndMoveRedim();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Called at the end of move/redim of the shape. Stores startX|Y|X1|Y1 
        /// for a correct rendering during object move/redim
        /// </summary>
        public override void EndMoveRedim()
        {
            base.EndMoveRedim();
            foreach (Handler handler in this.handlers)
                handler.EndMoveRedim();
        }
        /// <summary>
        /// Sets zoom values. Reregions each handler that is contained
        /// </summary>
        /// <param name="x">X zoom value</param>
        /// <param name="y">Y zoom value</param>
        public void SetZoom(float x, float y)
        {
            this.groupZoomX = x;
            this.groupZoomY = y;

            foreach (Handler handler in this.handlers)
                handler.RePosition(this);
        }
        /// <summary>
        /// Rotate element, with a rotation angle from a vertical line from the center
        /// of the shape and a line from the center to the point (x,y)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public override void Rotate(float x, float y)
        {
            base.Rotate(x, y);

            foreach (Handler handler in this.handlers)
                handler.RePosition(this);
        }
        /// <summary>
        /// Moves element at x coordinate by x vlaue and y coordinate by y value 
        /// Also moves each handler that is contained
        /// </summary>
        /// <param name="x">X coordinate move value</param>
        /// <param name="y">Y coordinate move value</param>
        public override void Move(int x, int y)
        {
            base.Move(x, y);

            foreach (Handler handler in this.handlers)
                handler.RePosition(this);
        }
        /// <summary>
        /// Determines whether point(x,y) is contained by this handler or not
        /// </summary>
        /// <param name="x">x region of the point</param>
        /// <param name="y">y region of the point</param>
        /// <returns>True if contains, false if not</returns>
        public HandlerOperator IsOver(int x, int y)
        {
            HandlerOperator hOperator = HandlerOperator.None;

            foreach (Handler handler in this.handlers)
            {
                hOperator = handler.IsOver(x, y);
                if (hOperator != HandlerOperator.None)
                    return hOperator;
            }
            if (this.Contains(x, y))
                return HandlerOperator.Default;

            return HandlerOperator.None;
        }
        /// <summary>
        /// When selected, copies itself to undo shape
        /// </summary>
        public override void Select()
        {
            this.undoShape = this.Copy();
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

            foreach (Handler handler in this.handlers)
            {
                handler.RePosition(this);
            }
        }
        /// <summary>
        /// Draws each handler on the graph object
        /// </summary>
        /// <param name="graphObj">Graphic object to be drawn on</param>
        /// <param name="dx">X region on the graphic object</param>
        /// <param name="dy">Y region on the graphic object</param>
        /// <param name="zoom">Zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            foreach (Handler handler in this.handlers)
                    handler.Draw(graphObj, dx, dy, zoom);
        }
        /// <summary>
        /// Clears handlers
        /// </summary>
        public void Clear()
        {
            this.handlers.Clear();
        }
        #endregion
    }
}
