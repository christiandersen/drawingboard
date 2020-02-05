using System;

namespace DrawingBoard2.Utils
{
    /// <summary>
    /// Manager class that manages undo/redo actions
    /// Keeps actions in a buffer whose structure is doubly linked list 
    /// </summary>
    public class UndoManager<T> where T : class
    {
        #region Variables
        private UndoBufferNode<T> start = null;
        private UndoBufferNode<T> current = null;
        private int bufferSize;
        private int count = 0;
        private bool isAtBottom = true;
        #endregion

        #region Properties
        /// <summary>
        /// True if it can undo last action, unless false
        /// </summary>
        public bool IsUndoable
        {
            get { return !this.isAtBottom; }
        }
        /// <summary>
        /// True if it can redo last action, unless false
        /// </summary>
        public bool IsRedoable
        {
            get
            {
                return this.current != null && current.Next != null;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        ///  Manager class that manages undo/redo actions
        /// </summary>
        /// <param name="size">Size of the buffer</param>
        public UndoManager(int size)
        {
            this.bufferSize = size;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Resets(cleans) buffer
        /// </summary>
        public void ResetBuffer()
        {
            this.count = 0;
            this.isAtBottom = true;
            this.start = null;
            this.current = null;
        }
        /// <summary>
        /// Adds item to the buffer
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void AddItem(T item)
        {
            if (item == null)
                return;
            UndoBufferNode<T> element = new UndoBufferNode<T>(item);

            if (this.count == 0)
                start = current = element;
            else
            {
                element.Prev = current;
                current.Next = element;
                current = element;

                if (this.count == 1)
                    start.Next = current;
            }
            this.count++;

            if (this.bufferSize < this.count)
            {
                this.start = this.start.Next;
                this.start.Prev = null;
                this.count--;
            }
            isAtBottom = false;
        }
        /// <summary>
        /// Undo last element on buffer
        /// </summary>
        /// <returns>undo object</returns>
        public T Undo()
        {
            if (current == null)
                return null;

            T undoObj = current.Element;

            if (current.Prev != null)
            {
                current = current.Prev;
                this.count--;
                this.isAtBottom = false;
            }
            else
                this.isAtBottom = true;

            return undoObj;
        }
        /// <summary>
        /// Redo
        /// </summary>
        /// <returns>Redo object</returns>
        public T Redo()
        {
            if (current == null)
                return null;

            if (!isAtBottom)
            {
                if (current.Next != null)
                {
                    current = current.Next;
                    this.count++;
                }
            }
            else
                isAtBottom = false;
            return current.Element;
        }
        #endregion
    }    
}
