using System;
using DrawingBoard2.Shapes;

namespace DrawingBoard2
{
    /// <summary>
    /// Undo Buffer element class that is used for buffering actions for undo/redo
    /// </summary>
    public class UndoElement
    {
        #region Variables
        private ShapeElement currentShape = null;
        private ShapeElement oldShape = null;
        private ShapeElement newShape = null;
        private BufferOperation operation;
        #endregion

        #region Properties
        /// <summary>
        /// Current shape element ( after action )
        /// </summary>
        public ShapeElement CurrentShape
        {
            set { this.currentShape = value; }
            get { return this.currentShape; }
        }
        /// <summary>
        /// Old shape element (before action)
        /// </summary>
        public ShapeElement OldShape
        {
            get { return this.oldShape; }
            set { this.oldShape = value; }
        }
        /// <summary>
        /// New Shape element (after action) 
        /// </summary>
        public ShapeElement NewShape
        {
            get { return this.newShape; }
            set { this.newShape = value; }
        }
        /// <summary>
        /// Operation Insert/Delete/Update
        /// </summary>
        public BufferOperation BufferOperation
        {
            get { return this.operation; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Buffer element class that is used for buffering actions for undoing/redoing 
        /// </summary>
        /// <param name="currentShape">Current shape</param>
        /// <param name="operation">Opertion</param>
        public UndoElement(ShapeElement currentShape, BufferOperation operation)
        {
            this.currentShape = currentShape;

            if (currentShape.UndoShape != null)
                this.oldShape = currentShape.UndoShape.Copy();
            this.newShape = currentShape.Copy();
            this.operation = operation;
        }
        #endregion
    }
}
