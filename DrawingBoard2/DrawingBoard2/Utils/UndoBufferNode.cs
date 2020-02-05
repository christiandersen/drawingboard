using System;

namespace DrawingBoard2.Utils
{
    /// <summary>
    /// Doubly linked list node
    /// </summary>
    public class UndoBufferNode<T>
    {
        #region Variables
        private UndoBufferNode<T> next = null;
        private UndoBufferNode<T> prev = null;
        private T element = default(T);
        #endregion

        #region Properties
        /// <summary>
        /// Next node 
        /// </summary>
        public UndoBufferNode<T> Next
        {
            get { return this.next; }
            set { this.next = value; }
        }
        /// <summary>
        /// Previous node 
        /// </summary>
        public UndoBufferNode<T> Prev
        {
            get { return this.prev; }
            set { this.prev = value; }
        }
        /// <summary>
        /// Data of current node
        /// </summary>
        public T Element
        {
            get { return this.element; }
            set { this.element = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Create doubly linked list node
        /// </summary>
        /// <param name="data">Data of the node</param>
        public UndoBufferNode(T data)
        {
            this.element = data;
        }
        #endregion
    }
}
