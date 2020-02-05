namespace DrawingBoard
{

    /// <summary>
    /// Undo buffer Element.
    /// </summary>
    public class BuffElement
    {
        public Element newE; //Start point
        public Element objRef;
        public Element oldE; //Start point
        public string op; // U:Update, I:Insert, D:Delete

        public BuffElement(Element refe, Element newe, Element olde, string o)
        {
            objRef = refe;
            oldE = olde;
            newE = newe;
            op = o;
        }
    }

    /// <summary>
    /// Two Linked List Element
    /// </summary>
    public class BuffObject
    {
        public object elem;
        public BuffObject Next;
        public BuffObject Prec;

        public BuffObject(object o)
        {
            elem = o;
        }
    }

    /// <summary>
    /// Undo buffer. (Two Linked List)
    /// </summary>
    //[Serializable]
    public class UndoBuffer
    {
        private int _count;
        private bool IsAtEnd;
        private BuffObject Bottom;
        private BuffObject Current;
        private BuffObject Top;

        public UndoBuffer(int i)
        {
            BuffSize = i;
            _count = 0;
            Top = null;
            Bottom = null;
            Current = null;
            IsAtEnd = true;
        }

        public int BuffSize { get; set; }

        public int Count
        {
            get { return _count; }
        }

        public void Add(object o)
        {
            if (o == null)
                return;
            var g = new BuffObject(o);
            if (Count == 0)
            {
                g.Next = null;
                g.Prec = null;
                Top = g;
                Bottom = g;
                Current = g;
            }
            else
            {
                g.Prec = Current;
                g.Next = null;
                Current.Next = g;
                Top = g;
                Current = g;
                if (Count == 1)
                    Bottom.Next = g;
            }

            //this._N_elem = count();
            _count++;
            if (BuffSize < Count)
            {
                Bottom = Bottom.Next;
                Bottom.Prec = null;
                _count--;
            }
            IsAtEnd = false;
        }

        public object Undo()
        {
            if (Current != null)
            {
                object obj = Current.elem;
                if (Current.Prec != null)
                {
                    Current = Current.Prec;
                    _count--;
                    IsAtEnd = false;
                }
                else
                    IsAtEnd = true;
                return obj;
            }
            //this._N_elem = count();
            return null;
        }

        public object Redo()
        {
            if (Current != null)
            {
                if (!IsAtEnd)
                {
                    if (Current.Next != null)
                    {
                        Current = Current.Next;
                        _count++;
                    }
                }
                else
                    IsAtEnd = false;
                return Current.elem;
            }
            //this._N_elem = count();
            return null;
        }

        public bool unDoable()
        {
            return !IsAtEnd;
        }

        public bool unRedoable()
        {
            return Current != null && Current.Next != null;
        }
    }

}
