using System;

using DrawingBoard2.Shapes;

namespace DrawingBoard2
{
    /// <summary>
    /// Event arguments for property change event
    /// </summary>
    public class PropertyEventArgs : EventArgs
    {
        #region Variables
        private bool undoable;
        private bool redoable;
        private ShapeElement element;
        #endregion

        #region Properties
        /// <summary>
        /// Selected Shape element
        /// </summary>
        public ShapeElement Element
        {
            get { return this.element; }
        }
        /// <summary>
        /// Is situation undoabled
        /// </summary>
        public bool Undoable
        {
            get { return this.undoable; }
        }
        /// <summary>
        /// Is situation redoable
        /// </summary>
        public bool Redoable
        {
            get { return this.redoable; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Event arguments that are raised on object(shape element) selection
        /// </summary>
        /// <param name="element">Selected Element</param>
        /// <param name="undoable">Are shapes undoable</param>
        /// <param name="redoable">Are shapes redoable</param>
        public PropertyEventArgs(bool undoable, bool redoable, ShapeElement element)
        {
            this.undoable = undoable;
            this.redoable = redoable;
            this.element = element;
        }
        #endregion
    }
}
