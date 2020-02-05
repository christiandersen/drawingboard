using System;
using System.Drawing;
using DrawingBoard2.Shapes;

namespace DrawingBoard2.Handlers
{
    /// <summary>
    /// Base handler class
    /// </summary>
    public abstract class Handler : ShapeElement
    {
        #region Variables
        /// <summary>
        /// Current handler operator
        /// </summary>
        public HandlerOperator handleOperator;
        /// <summary>
        /// Indicates handler is visible or not
        /// </summary>
        public bool visible;
        #endregion

        #region Constructors
        /// <summary>
        /// Base handler class
        /// </summary>
        /// <param name="shape">Shape element to be handled</param>
        /// <param name="hOperator">Handler Operator</param>
        public Handler(ShapeElement shape, HandlerOperator hOperator)
        {
            this.handleOperator = hOperator;
            this.RePosition(shape);   
        }
        #endregion

        #region Methods
        /// <summary>
        /// Indicates whether is handler is selected or not
        /// </summary>
        /// <returns>True if selected unless false</returns>
        [Obsolete("BaseShapeElement already has public property named 'Selected'")]
        public bool IsSelected()
        {
            return this.selected;
        }
        /// <summary>
        /// Determines whether point(x,y) is contained by this handler or not
        /// </summary>
        /// <param name="x">x region of the point</param>
        /// <param name="y">y region of the point</param>
        /// <returns>True if contains, false if not</returns>
        public HandlerOperator IsOver(int x, int y)
        {
            Rectangle rectangle = new Rectangle(region.X0, region.Y0, 
                region.X1 - region.X0,region.Y1 - region.Y0);

            if (rectangle.Contains(x, y))
            {
                this.Selected = true;
                return this.handleOperator;
            }
            this.Selected = false;

            return HandlerOperator.None;
        }
        /// <summary>
        /// Abstract function definition for resposition
        /// Inherited members will override this function for handling repositing shape element
        /// </summary>
        /// <param name="shape">Shape element to be reposition</param>
        public abstract void RePosition(ShapeElement shape);
        #endregion
    }
}
