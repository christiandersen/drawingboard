using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DrawingBoard
{

    /// <summary>
    /// shapes collection
    /// </summary>
    [Serializable]
    public class ShapesCollection
    {
        // List of objects on the canvas
        public readonly List<Element> List = new List<Element>();
        // The Handles Obj

        public int minDim = 10;
        public Element selectedElement;
        public AbstractSel sRec;

        //the undo/redo buffer 
        [NonSerialized]
        private UndoBuffer undoB;

        public ShapesCollection()
        {
            InitUndoBuff();
        }

        private void InitUndoBuff()
        {
            undoB = new UndoBuffer(20); // set dim of undo buffer
        }

        public bool IsEmpty()
        {
            return (List.Count > 0);
        }

        public void AfterLoad()
        {
            // UndoBuff is not serialized, I must reinit it after LOAD from file
            InitUndoBuff();
            foreach (var e in List)
                e.AfterLoad();
        }

        /// <summary>
        /// Copy all selected Items
        /// </summary>
        public void CopyMultiSelected(int dx, int dy)
        {
            var tmpList = new List<Element>();
            foreach (var elem in List)
            {
                if (!elem.IsSelected)
                    continue;
                var eL = elem.Copy();
                elem.IsSelected = false;
                eL.move(dx, dy);
                tmpList.Add(eL);
                sRec = new SelRect(eL);
                selectedElement = eL;
                selectedElement.endMoveRedim();
            }
            foreach (var tmpElem in tmpList)
            {
                List.Add(tmpElem);
                // store the operation in undo/redo buffer
                storeDo("I", tmpElem);
            }
        }


        /// <summary>
        /// returns a Copy of selected element
        /// </summary>
        public Element GetCopyOfSelectedElement()
        {
            if (selectedElement == null)
                return null;
            var L = selectedElement.Copy();
            return L;
        }

        /// <summary>
        /// Copy selected Item 
        /// </summary>
        public void CopySelected(int dx, int dy)
        {
            if (selectedElement == null)
                return;
            var element = GetCopyOfSelectedElement();
            element.move(dx, dy);
            deSelect();
            List.Add(element);

            // store the operation in undo/redo buffer
            storeDo("I", element);

            sRec = new SelRect(element);
            //sRec.IsLine = L.IsLine;
            selectedElement = element;
            selectedElement.endMoveRedim();
        }

        /// <summary>
        /// Get direct access to the selected elements
        /// </summary>
        public List<Element> SelectedElements
        {
            get
            {
                var tmpList = new List<Element>();
                foreach (var elem in List)
                    if (elem.IsSelected)
                        tmpList.Add(elem);
                return tmpList;
            }
        }

        /// <summary>
        /// Elimina oggetto selezioanto
        /// </summary>
        public void RemoveSelected()
        {
            var tmpList = SelectedElements;

            if (selectedElement != null)
            {
                //this.List.Remove(selectedElement);
                selectedElement = null;
                sRec = null;
            }

            foreach (var tmpElem in tmpList)
            {
                List.Remove(tmpElem);

                // store the operation in undo/redo buffer
                storeDo("D", tmpElem);
            }
        }

        /// <summary>
        /// Group selected objs
        /// </summary>
        public void GroupSelected()
        {
            var tmpList = SelectedElements;

            if (selectedElement != null)
            {
                //this.List.Remove(selectedElement);
                selectedElement = null;
                sRec = null;
            }

            foreach (var tmpElem in tmpList)
            {
                List.Remove(tmpElem);

                // store the operation in undo/redo buffer
                //storeDo("D", tmpElem);
            }

            var g = new Group(tmpList);

            List.Add(g);
            // store the operation in undo/redo buffer
            //storeDo("I", g);

            sRec = new SelRect(g);
            //sRec.showHandles(true);
            selectedElement = g;
            selectedElement.Select();

            // when grouping / ungrouping, reset the undoBuffer
            undoB = new UndoBuffer(20);
        }

        /// <summary>
        /// Grup selected objs
        /// </summary>
        public void UnGroupSelected()
        {
            var tmpList = SelectedElements;

            if (selectedElement != null)
            {
                //this.List.Remove(selectedElement);
                selectedElement = null;
                sRec = null;
            }
            bool found = false;
            foreach (var tmpElem in tmpList)
            {
                var ungroupedElements = tmpElem.UnGroup();
                if (ungroupedElements == null || ungroupedElements.Count <= 0)
                    continue;
                List.AddRange(ungroupedElements);
                List.Remove(tmpElem);
                found = true;
            }
            if (found)      // when grouping / ungrouping reset undoBuffer
                undoB = new UndoBuffer(20);
        }

        public void MovePoint(int dx, int dy)
        {
            ((SelPoly)sRec).movePoints(dx, dy);
            ((SelPoly)sRec).reCreateCreationHandles((PointSet)selectedElement);
            //sRec = new SelPoly(selectedElement);//create handling rect
        }

        public void AddPoint()
        {
            if (!(sRec is SelPoly))
                return;
            var p = ((SelPoly)sRec).getNewPoint();
            int i = ((SelPoly)sRec).getIndex();
            if (i <= 0)
                return;
            ((PointSet)selectedElement).points.Insert(i - 1, p);
            sRec = new SelPoly(selectedElement); //create handling rect
        }

        public void RemovePoint()
        {
            if (!(sRec is SelPoly))
                return;
            var tmp = ((SelPoly)sRec).getSelPoints();
            if (tmp.Count < ((PointSet)selectedElement).points.Count - 1)
                foreach (var p in tmp)
                    ((PointSet)selectedElement).points.Remove(p);
            sRec = new SelPoly(selectedElement); //create handling rect
        }

        //Creates new polys from selected points
        public void PolysFromSelectedPoints()
        {
            if (!(sRec is SelPoly))
                return;
            var tmp = ((SelPoly)sRec).getSelPoints();
            if (tmp.Count > 1)
            {
                var newL = new List<PointWr>();
                foreach (var p in tmp)
                {
                    //((PointSet)this.selectedElement).points.Remove(p);
                    newL.Add(new PointWr(p.point));
                }
                addPoly(sRec.getX(), sRec.getY(), sRec.getX1(), sRec.getY1(), sRec.PenColor, sRec.FillColor,
                        sRec.PenWidth, sRec.IsFilled, newL, false);
            }
            //sRec = new SelPoly(selectedElement);//create handling rect
        }


        public void Move(int dx, int dy)
        {
            foreach (var e in List)
                if (e.IsSelected)
                    e.move(dx, dy);
        }

        public void FitToGrid(int gridsize)
        {
            foreach (var e in List)
                if (e.IsSelected)
                    e.FitToGrid(gridsize);
        }


        public void EndMove()
        {
            foreach (var e in List)
            {
                if (!e.IsSelected)
                    continue;
                e.endMoveRedim();
                if (!e.IsGroup)
                    storeDo("U", e);
            }
        }

        public void Propertychanged()
        {
            foreach (var e in List)
                if (e.IsSelected)
                    storeDo("U", e);
        }

        private int CountSelected()
        {
            int i = 0;
            foreach (var e in List)
                if (e.IsSelected)
                    i++;
            return i;
        }

        /// <summary>
        /// Returns an array with the selected item. Used for property grid.
        /// </summary>
        public Element[] getSelectedArray()
        {
            var myArray = new Element[CountSelected()];
            int i = 0;
            foreach (var e in List)
            {
                if (!e.IsSelected)
                    continue;
                myArray[i] = e;
                i++;
            }
            return myArray;
        }

        /// <summary>
        /// Returns a List with the selected items. Used for SaveObj.
        /// </summary>
        public List<Element> getSelectedList()
        {
            var tmpL = new List<Element>();
            foreach (var e in List)
                if (e.IsSelected)
                    tmpL.Add(e);
            return tmpL;
        }

        /// <summary>
        /// Returns a List with the selected items. Used for SaveObj.
        /// </summary>
        public void setList(List<Element> a)
        {
            foreach (var e in a)
                List.Add(e);
        }

        /// <summary>
        /// 2 front
        /// </summary>
        public void ToFront()
        {
            if (selectedElement != null)
            {
                List.Remove(selectedElement);
                List.Add(selectedElement);
            }
        }

        /// <summary>
        /// 2 back
        /// </summary>
        public void ToBack()
        {
            if (selectedElement == null)
                return;
            List.Remove(selectedElement);
            List.Insert(0, selectedElement);
            deSelect();
        }

        public void XMirror()
        {
            if (!(selectedElement is PointSet))
                return;
            ((PointSet)selectedElement).CommitMirror(true, false);
            sRec = new SelPoly(selectedElement); //create handling rect
        }

        public void YMirror()
        {
            if (!(selectedElement is PointSet))
                return;
            ((PointSet)selectedElement).CommitMirror(false, true);
            sRec = new SelPoly(selectedElement); //create handling rect
        }

        public void Mirror()
        {
            if (!(selectedElement is PointSet))
                return;

            ((PointSet)selectedElement).CommitMirror(true, true);
            //((PointSet)selectedElement).setupSize();
            sRec = new SelPoly(selectedElement); //create handling rect
        }


        /// <summary>
        /// Deselect 
        /// </summary>
        public void deSelect()
        {
            foreach (var obj in List)
                obj.IsSelected = false;
            selectedElement = null;
            sRec = null;
        }

        /// <summary>
        /// Selects last shape containing x,y
        /// </summary>
        public void click(int x, int y, RichTextBox r)
        {
            sRec = null;
            selectedElement = null;
            foreach (var obj in List)
            {
                obj.IsSelected = false;
                obj.DeSelect();
                if (obj.Contains(x, y))
                    selectedElement = obj; //salvo il riferimento dell'ogg trovato
            }
            if (selectedElement == null)
                return;
            selectedElement.IsSelected = true;
            selectedElement.Select();
            selectedElement.Select(r);
            sRec = selectedElement is PointSet
                       ? (AbstractSel)new SelPoly(selectedElement)
                       : new SelRect(selectedElement);
            //sRec.IsLine = selectedElement.IsLine;
            //sRec.showHandles(selectedElement.AmIaGroup());
        }

        public void mergePolygons()
        {
            bool first = true;
            int minX = 0;
            int minY = 0;
            var tmpPointList = new List<PointWr>();
            var tmpDelPolys = new List<Element>();
            PointSet tmpPS = null;
            foreach (var obj in List)
            {
                if (!(obj.IsSelected && obj is PointSet))
                    continue;
                if (first)
                {
                    first = false;
                    minX = obj.getX();
                    minY = obj.getY();
                    tmpPS = (PointSet)obj;
                }
                else
                {
                    if (minX > obj.getX())
                        minX = obj.getX();
                    if (minY > obj.getY())
                        minY = obj.getY();
                }
                tmpDelPolys.Add(obj);
                tmpPointList.AddRange(((PointSet)obj).getRealPosPoints());
            }
            if (tmpDelPolys.Count <= 1)
                return;

            foreach (var obj in tmpDelPolys)
                List.Remove(obj);

            if (tmpPS != null)
                addPoly(0, 0, tmpPS.getX1(), tmpPS.getY1(), tmpPS.PenColor, tmpPS.FillColor, tmpPS.PenWidth,
                        tmpPS.IsFilled, tmpPointList, false);
        }

        /// <summary>
        /// Sanitize a rectangle's corners to be normally ordered
        /// </summary>
        /// <param name="_startX"></param>
        /// <param name="_startY"></param>
        /// <param name="_endX"></param>
        /// <param name="_endY"></param>
        public static void SanitizeRect(ref int _startX, ref int _startY, ref int _endX, ref int _endY)
        {
            int startX = Math.Min(_startX, _endX);
            int startY = Math.Min(_startY, _endY);
            int endX = Math.Max(_startX, _endX);
            int endY = Math.Max(_startY, _endY);
            _startX = startX;
            _startY = startY;
            _endX = endX;
            _endY = endY;
        }

        /// <summary>
        /// Selects all shapes in imput rectangle 
        /// </summary>
        public void multiSelect(int startX, int startY, int endX, int endY, RichTextBox r)
        {
            SanitizeRect(ref startX, ref startY, ref endX, ref endY);

            sRec = null;
            selectedElement = null;
            foreach (var obj in List)
            {
                obj.IsSelected = false;
                obj.DeSelect(); // to deselect points in polys
                int x = obj.getX();
                int x1 = obj.getX1();
                int y = obj.getY();
                int y1 = obj.getY1();
                int c;
                if (x > x1)
                {
                    c = x;
                    x = x1;
                    x1 = c;
                }
                if (y > y1)
                {
                    c = y;
                    y = y1;
                    y1 = c;
                }
                //if (obj.getX() <= endX && obj.getX1() >= Start.X && obj.getY() <= endY && obj.getY1() >= Start.Y)
                if (!(x <= endX && x1 >= startX && y <= endY && y1 >= startY))
                    continue;
                selectedElement = obj; //salvo il riferimento dell'ogg trovato
                obj.IsSelected = true; //indico l'oggetto trovato come selezionato
                obj.Select();
                obj.Select(r);
                obj.Select(startX, startY, endX, endY);
            }
            if (selectedElement != null)
            {
                if (selectedElement is PointSet)
                    sRec = new SelPoly(selectedElement); //create handling rect
                else
                    sRec = new SelRect(selectedElement); //creo un gestore con maniglie
                //sRec.IsLine = selectedElement.IsLine;//indico se il gestore è per una linea
                //sRec.showHandles(selectedElement.AmIaGroup());
            }
        }

        #region DRAW

        /// <summary>
        /// Draw all shapes
        /// </summary>
        public void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            bool almostOneSelected = false;

            // note how we choose a for() loop here, not a foreach(), because it's about 40% faster (!)... the main shapes could be quite numerous, so they deserve the best speed.
            int len = List.Count;
            for (int t = 0; t < len; t++)
            {
                var obj = List[t];
                obj.Draw(gfx, dx, dy, zoom);
                if (obj.IsSelected)
                    almostOneSelected = true;
            }
            if (almostOneSelected)
                if (sRec != null)
                    sRec.Draw(gfx, dx, dy, zoom);
        }


        /// <summary>
        /// Draw all Unselected shapes
        /// </summary>
        public void DrawUnselected(Graphics gfx, int dx, int dy, float zoom)
        {
            gfx.PageScale = 10;

            // note how we choose a for() loop here, not a foreach(), because it's about 40% faster (!)... the main shapes could be quite numerous, so they deserve the best speed.
            int len = List.Count;
            for (int t = 0; t < len; t++)
            {
                var obj = List[t];
                if (!obj.IsSelected)
                    obj.Draw(gfx, dx, dy, zoom);
            }
        }

        /// <summary>
        /// Draw all Unselected shapes
        /// </summary>
        public void DrawUnselected(Graphics gfx)
        {
            gfx.PageScale = 10;
            //bool almostOneSelected = false;
            // note how we choose a for() loop here, not a foreach(), because it's about 40% faster (!)... the main shapes could be quite numerous, so they deserve the best speed.
            int len = List.Count;
            for (int t = 0; t < len; t++)
            {
                var obj = List[t];
                if (!obj.IsSelected)
                    obj.Draw(gfx, 0, 0, 1);
            }
        }

        /// <summary>
        /// Draw all IsSelected shapes
        /// </summary>
        public void DrawSelected(Graphics gfx, int dx, int dy, float zoom)
        {
            bool almostOneSelected = false;

            // note how we choose a for() loop here, not a foreach(), because it's about 40% faster (!)... the main shapes could be quite numerous, so they deserve the best speed.
            int len = List.Count;
            for (int t = 0; t < len; t++)
            {
                var obj = List[t];
                if (!obj.IsSelected)
                    continue;
                obj.Draw(gfx, dx, dy, zoom);
                almostOneSelected = true;
            }
            if (almostOneSelected && sRec != null)
                sRec.Draw(gfx, dx, dy, zoom);
        }

        /// <summary>
        /// Draw all IsSelected shapes
        /// </summary>
        public void DrawSelected(Graphics gfx)
        {
            bool almostOneSelected = false;

            // note how we choose a for() loop here, not a foreach(), because it's about 40% faster (!)... the main shapes could be quite numerous, so they deserve the best speed.
            int len = List.Count;
            for (int t = 0; t < len; t++)
            {
                var obj = List[t];
                if (!obj.IsSelected)
                    continue;
                obj.Draw(gfx, 0, 0, 1);
                almostOneSelected = true;
            }
            if (almostOneSelected && sRec != null)
                sRec.Draw(gfx, 0, 0, 1);
        }

        #endregion

        #region ADD ELEMENTS METHODS

        /// <summary>
        /// Adds Polygon
        /// </summary>
        public void addPoly(int x, int y, int x1, int y1, Color penC, Color fillC, float penW, bool filled,
                            List<PointWr> aa,
                            bool curv)
        {
            /*if (x1 - minDim <= x)
                x1 = x + minDim;
            if (y1 - minDim <= y)
                y1 = y + minDim;*/

            deSelect();
            var r = new PointSet(x, y, x1, y1, aa)
            {
                PenColor = penC,
                PenWidth = penW,
                FillColor = fillC,
                IsFilled = filled,
                Curved = curv
            };

            List.Add(r);
            // store the operation in undo/redo buffer
            storeDo("I", r);

            sRec = new SelPoly(r);
            selectedElement = r;
            selectedElement.Select();
        }


        /// <summary>
        /// Adds Polygon
        /// </summary>
        public void addColorPoinySet(int x, int y, int x1, int y1, Color penC, Color fillC, float penW, bool filled,
                                     List<PointWr> aa, bool curv)
        {
            deSelect();
            var r = new PointColorSet(x, y, x1, y1, aa)
            {
                PenColor = penC,
                PenWidth = penW,
                FillColor = fillC,
                IsFilled = filled,
                Curved = curv
            };

            List.Add(r);
            // store the operation in undo/redo buffer
            storeDo("I", r);

            sRec = new SelPoly(r);
            selectedElement = r;
            selectedElement.Select();
        }


        /// <summary>
        /// Adds Rect
        /// </summary>
        public void addRect(int x, int y, int x1, int y1, Color penC, Color fillC, float penW, bool filled)
        {
            if (x1 - minDim <= x)
                x1 = x + minDim;
            if (y1 - minDim <= y)
                y1 = y + minDim;

            deSelect();
            var r = new Rect(x, y, x1, y1) { PenColor = penC, PenWidth = penW, FillColor = fillC, IsFilled = filled };

            List.Add(r);
            // store the operation in undo/redo buffer
            storeDo("I", r);

            sRec = new SelRect(r);
            selectedElement = r;
            selectedElement.Select();
        }

        /// <summary>
        /// Adds Arc
        /// </summary>
        public void addArc(int x, int y, int x1, int y1, Color penC, Color fillC, float penW, bool filled)
        {
            if (x1 - minDim <= x)
                x1 = x + minDim;
            if (y1 - minDim <= y)
                y1 = y + minDim;

            deSelect();
            var r = new Arc(x, y, x1, y1) { PenColor = penC, PenWidth = penW, FillColor = fillC, IsFilled = filled };

            List.Add(r);
            // store the operation in undo/redo buffer
            storeDo("I", r);

            sRec = new SelRect(r);
            selectedElement = r;
            selectedElement.Select();
        }

        /// <summary>
        /// Adds Rect
        /// </summary>
        public void addLine(int x, int y, int x1, int y1, Color penC, float penW)
        {
            /*
            if ((x1 == x) && (y1 == y))
            {
                x1 = x + minDim;
                y1 = y + minDim;             
            }
             */

            deSelect();
            var r = new Line(x, y, x1, y1) { PenColor = penC, PenWidth = penW };

            List.Add(r);
            // store the operation in undo/redo buffer
            storeDo("I", r);

            sRec = new SelRect(r);

            selectedElement = r;
            selectedElement.Select();
        }

        /// <summary>
        /// Adds TextBox
        /// </summary>
        public void addTextBox(int x, int y, int x1, int y1, RichTextBox t, Color penC, Color fillC, float penW,
                               bool filled)
        {
            if (x1 - minDim <= x)
                x1 = x + minDim;
            if (y1 - minDim <= y)
                y1 = y + minDim;

            deSelect();
            var r = new BoxText(x, y, x1, y1)
            {
                PenColor = penC,
                PenWidth = penW,
                FillColor = fillC,
                IsFilled = filled,
                rtf = t.Rtf
            };
            //Stext r = new Stext(x, y, x1, y1);

            List.Add(r);
            // store the operation in undo/redo buffer
            storeDo("I", r);

            sRec = new SelRect(r);
            selectedElement = r;
            selectedElement.Select();
        }

        /// <summary>
        /// Adds SimpleTextBox
        /// </summary>
        public void addSimpleTextBox(int x, int y, int x1, int y1, RichTextBox t, Color penC, Color fillC, float penW,
                                     bool filled)
        {
            if (x1 - minDim <= x)
                x1 = x + minDim;
            if (y1 - minDim <= y)
                y1 = y + minDim;

            deSelect();
            var r = new SimpleText(x, y, x1, y1)
            {
                Text = t.Text,
                CharFont = t.SelectionFont,
                PenColor = penC,
                PenWidth = penW,
                FillColor = fillC,
                IsFilled = filled
            };

            //r.CharFont = (Font)t.Font.Clone();


            List.Add(r);
            // store the operation in undo/redo buffer
            storeDo("I", r);

            sRec = new SelRect(r);
            selectedElement = r;
            selectedElement.Select();
        }


        /// <summary>
        /// Adds RoundRect
        /// </summary>
        public void addRRect(int x, int y, int x1, int y1, Color penC, Color fillC, float penW, bool filled)
        {
            if (x1 - minDim <= x)
                x1 = x + minDim;
            if (y1 - minDim <= y)
                y1 = y + minDim;

            deSelect();
            var r = new RoundedRect(x, y, x1, y1) { PenColor = penC, PenWidth = penW, FillColor = fillC, IsFilled = filled };

            List.Add(r);
            // store the operation in undo/redo buffer
            storeDo("I", r);

            sRec = new SelRect(r);
            selectedElement = r;
            selectedElement.Select();
        }

        public void addImgBox(int x, int y, int x1, int y1, Bitmap bitmap, Color penC, float penW)
        {
            if (bitmap == null)
                return;

            if (x1 - minDim <= x)
                x1 = x + minDim;
            if (y1 - minDim <= y)
                y1 = y + minDim;

            deSelect();
            var r = new ImageBox(x, y, x1, y1) { PenColor = penC, PenWidth = penW };

            List.Add(r);
            // store the operation in undo/redo buffer
            storeDo("I", r);
            r.img = bitmap;

            sRec = new SelRect(r);
            selectedElement = r;
            selectedElement.Select();
        }

        /// <summary>
        /// Adds ImageBox
        /// </summary>
        public void addImgBox(int x, int y, int x1, int y1, string st, Color penC, float penW)
        {
            if (string.IsNullOrEmpty(st) || !File.Exists(st))
                return;
            var bmp = new Bitmap(st);
            addImgBox(x, y, x1, y1, bmp, penC, penW);
        }

        /// <summary>
        /// Adds Ellipse
        /// </summary>
        public void addEllipse(int x, int y, int x1, int y1, Color penC, Color fillC, float penW, bool filled)
        {
            if (x1 - minDim <= x)
                x1 = x + minDim;
            if (y1 - minDim <= y)
                y1 = y + minDim;

            deSelect();
            var r = new Ellipse(x, y, x1, y1) { PenColor = penC, PenWidth = penW, FillColor = fillC, IsFilled = filled };

            List.Add(r);
            // store the operation in undo/redo buffer
            storeDo("I", r);

            sRec = new SelRect(r);
            selectedElement = r;
            selectedElement.Select();
        }

        #endregion

        #region undo/redo

        public bool UndoEnabled()
        {
            return undoB.unDoable();
        }

        public bool RedoEnabled()
        {
            return undoB.unRedoable();
        }

        public void storeDo(string option, Element e)
        {
            Element olde = null;
            if (e.UndoElement != null)
                olde = e.UndoElement.Copy();
            Element newe = e.Copy();
            var buff = new BuffElement(e, newe, olde, option);
            undoB.Add(buff);
            e.UndoElement = e.Copy();
        }

        public void Undo()
        {
            var buff = (BuffElement)undoB.Undo();
            if (buff != null)
            {
                switch (buff.op)
                {
                    case "U":
                        buff.objRef.CopyFrom(buff.oldE);
                        break;
                    case "I":
                        //buff.objRef.CopyFrom(buff.oldE);
                        List.Remove(buff.objRef);
                        break;
                    case "D":
                        //buff.objRef.CopyFrom(buff.oldE);
                        List.Add(buff.objRef);
                        break;
                    default:
                        break;
                }
            }
        }

        public void Redo()
        {
            var buff = (BuffElement)undoB.Redo();
            if (buff != null)
            {
                switch (buff.op)
                {
                    case "U":
                        buff.objRef.CopyFrom(buff.newE);
                        break;
                    case "I":
                        //buff.objRef.CopyFrom(buff.oldE);
                        List.Add(buff.objRef);
                        break;
                    case "D":
                        //buff.objRef.CopyFrom(buff.oldE);
                        List.Remove(buff.objRef);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}
