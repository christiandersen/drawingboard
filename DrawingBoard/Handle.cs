using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawingBoard
{
    [Serializable]
    public abstract class Handle : Element
    {
        public RedimStatus Operation;
        public bool IsVisible = true;

        protected Handle()
        {
        }

        protected Handle(Element e, RedimStatus o)
        {
            Operation = o;
            rePosition(e);
        }

        public RedimStatus isOver(int x, int y)
        {
            var r = new Rectangle(Location0.X, Location0.Y, Location1.X - Location0.X, Location1.Y - Location0.Y);
            if (r.Contains(x, y))
            {
                IsSelected = true;
                return Operation;
            }
            IsSelected = false;
            return RedimStatus.None;
        }

        public virtual void rePosition(Element e)
        {
        }
    }

    /// <summary>
    /// 
    /// Handle object for redim/move/rotate shapes
    /// </summary>
    [Serializable]
    public class RedimHandle : Handle
    {
        public RedimHandle(Element e, RedimStatus o)
            : base(e, o)
        {
            FillColor = Color.Black;
        }

        public override void rePosition(Element e)
        {
            switch (Operation)
            {
                case RedimStatus.NW:
                    Location0.X = e.getX() - 2;
                    Location0.Y = e.getY() - 2;
                    break;
                case RedimStatus.N:
                    Location0.X = e.getX() - 2 + ((e.getX1() - e.getX())/2);
                    Location0.Y = e.getY() - 2;
                    break;
                case RedimStatus.NE:
                    Location0.X = e.getX1() - 2;
                    Location0.Y = e.getY() - 2;
                    break;
                case RedimStatus.E:
                    Location0.X = e.getX1() - 2;
                    Location0.Y = e.getY() - 2 + (e.getY1() - e.getY())/2;
                    break;
                case RedimStatus.SE:
                    Location0.X = e.getX1() - 2;
                    Location0.Y = e.getY1() - 2;
                    break;
                case RedimStatus.S:
                    Location0.X = e.getX() - 2 + (e.getX1() - e.getX())/2;
                    Location0.Y = e.getY1() - 2;
                    break;
                case RedimStatus.SW:
                    Location0.X = e.getX() - 2;
                    Location0.Y = e.getY1() - 2;
                    break;
                case RedimStatus.W:
                    Location0.X = e.getX() - 2;
                    Location0.Y = e.getY() - 2 + (e.getY1() - e.getY())/2;
                    break;
            }
            Location1.X = Location0.X + 5;
            Location1.Y = Location0.Y + 5;
        }


        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            //System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            using (var brush = new SolidBrush(FillColor) {Color = ToTransparentColor(Color.Black, 80)})
            {
                gfx.FillRectangle(brush, new RectangleF((Location0.X + dx)*zoom, (Location0.Y + dy)*zoom, (Location1.X - Location0.X)*zoom, (Location1.Y - Location0.Y)*zoom));
                gfx.DrawRectangle(Pens.White, (Location0.X + dx)*zoom, (Location0.Y + dy)*zoom, (Location1.X - Location0.X)*zoom, (Location1.Y - Location0.Y)*zoom);
            }
        }
    }

    [Serializable]
    public class ZoomHandle : Handle
    {
        public ZoomHandle(Element e, RedimStatus o)
            : base(e, o)
        {
            FillColor = Color.Red;
        }

        public override void rePosition(Element e)
        {
            float zx = (e.Width - (e.Width*e.getGprZoomX()))/2;
            float zy = (e.Height - (e.Height*e.getGprZoomY()))/2;
            Location0.X = (int) ((e.getX1() - 2) - zx);
            Location0.Y = (int) ((e.getY1() - 2) - zy);
            Location1.X = Location0.X + 5;
            Location1.Y = Location0.Y + 5;
            //this._zoomX = ((SelRect)e).zoomX;
            //this._zoomY = ((SelRect)e).zoomY;
        }

        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            using (var fillPen = new Pen(FillColor))
            using (var brush = new SolidBrush(ToTransparentColor(FillColor, 80)))
            {
                gfx.FillRectangle(brush, new RectangleF((Location0.X + dx)*zoom, (Location0.Y + dy)*zoom, (Location1.X - Location0.X)*zoom, (Location1.Y - Location0.Y)*zoom));
                gfx.DrawRectangle(Pens.White, (Location0.X + dx)*zoom, (Location0.Y + dy)*zoom, (Location1.X - Location0.X)*zoom, (Location1.Y - Location0.Y)*zoom);
                gfx.DrawRectangle(fillPen, (Location0.X + dx - 1)*zoom, (Location0.Y + dy - 1)*zoom, (Location1.X - Location0.X + 2)*zoom, (Location1.Y - Location0.Y + 2)*zoom);
            }
        }
    }

    [Serializable]
    public class RotHandle : Handle
    {
        public RotHandle(Element e, RedimStatus o)
            : base(e, o)
        {
        }

        public override void rePosition(Element e)
        {
            float midX = (e.getX1() - e.getX())/2;
            float midY = (e.getY1() - e.getY())/2;
            var Hp = new PointF(0, -25);
            PointF RotHP = rotatePoint(Hp, e.GetRotation());
            midX += RotHP.X;
            midY += RotHP.Y;

            Location0.X = e.getX() + (int) midX - 2;
            Location0.Y = e.getY() + (int) midY - 2;
            Rotation = e.GetRotation();

            Location1.X = Location0.X + 5;
            Location1.Y = Location0.Y + 5;
        }

        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            using (var brush = new SolidBrush(Color.Black) {Color = ToTransparentColor(Color.Black, 80)})
            using (var pen = new Pen(Color.Blue, 1.5f) {DashStyle = DashStyle.Dash})
            {
                gfx.FillRectangle(brush, new RectangleF((Location0.X + dx)*zoom, (Location0.Y + dy)*zoom, (Location1.X - Location0.X)*zoom, (Location1.Y - Location0.Y)*zoom));
                gfx.DrawRectangle(Pens.White, (Location0.X + dx)*zoom, (Location0.Y + dy)*zoom, (Location1.X - Location0.X)*zoom, (Location1.Y - Location0.Y)*zoom);

                //CENTER POINT
                float midX = (Location1.X - Location0.X)/2;
                float midY = (Location1.Y - Location0.Y) / 2;

                var Hp = new PointF(0, 25);
                var RotHP = rotatePoint(Hp, Rotation);
                RotHP.X += Location0.X;
                RotHP.Y += Location0.Y;
                gfx.FillEllipse(brush, (RotHP.X + midX + dx - 3)*zoom, (RotHP.Y + dy - 3 + midY)*zoom, 6*zoom, 6*zoom);
                gfx.DrawEllipse(Pens.White, (RotHP.X + midX + dx - 3) * zoom, (RotHP.Y + dy - 3 + midY) * zoom, 6 * zoom, 6 * zoom);
                gfx.DrawLine(pen, (Location0.X + midX + dx) * zoom, (Location0.Y + midY + dy) * zoom, (RotHP.X + midX + dx) * zoom,
                           (RotHP.Y + midY + dy) * zoom);
            }

        }
    }


    [Serializable]
    public class PointHandle : Handle
    {
        private readonly Element el;
        private readonly PointWr linkedPoint;

        public PointHandle(Element e, RedimStatus o, PointWr p)
        {
            Operation = o;
            FillColor = Color.BlueViolet;
            linkedPoint = p;
            el = e;
            rePosition(e);
        }

        public PointWr getPoint()
        {
            return linkedPoint;
        }

        public override bool IsSelected
        {
            get { return (base.IsSelected || linkedPoint.selected); }
        }

        public override void move(int x, int y)
        {
            base.move(x, y);
            linkedPoint.X = Location0.X + 2 - el.getX();
            linkedPoint.Y = Location0.Y + 2 - el.getY();
        }

        public override void rePosition(Element e)
        {
            Location0.X = (linkedPoint.X + e.getX() - 2);
            Location0.Y = (linkedPoint.Y + e.getY() - 2);
            Location1.X = Location0.X + 5;
            Location1.Y = Location0.Y + 5;
        }

        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            using (var brush = new SolidBrush(ToTransparentColor(FillColor, 80)))
            using (var fillPen = new Pen(IsSelected ? Color.Red : FillColor))
            {
                gfx.FillRectangle(brush, new RectangleF((Location0.X + dx)*zoom, (Location0.Y + dy)*zoom, (Location1.X - Location0.X)*zoom, (Location1.Y - Location0.Y)*zoom));
                gfx.DrawRectangle(Pens.White, (Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom);
                gfx.DrawRectangle(fillPen, (Location0.X + dx - 1) * zoom, (Location0.Y + dy - 1) * zoom, (Location1.X - Location0.X + 2) * zoom, (Location1.Y - Location0.Y + 2) * zoom);
            }
        }
    }

    [Serializable]
    public class NewPointHandle : Handle
    {
        private readonly Element el;
        private readonly PointWr linkedPoint;
        public int Index;

        public NewPointHandle(Element e, RedimStatus o, PointWr p, int i)
        {
            Index = i;
            Operation = o;
            FillColor = Color.YellowGreen;
            linkedPoint = p;
            el = e;
            rePosition(e);
        }

        public PointWr getPoint()
        {
            return linkedPoint;
        }

        public override void move(int x, int y)
        {
            base.move(x, y);
            linkedPoint.X = Location0.X + 2 - el.getX();
            linkedPoint.Y = Location0.Y + 2 - el.getY();
        }

        public override void rePosition(Element e)
        {
            Location0.X = (linkedPoint.X + e.getX() - 1);
            Location0.Y = (linkedPoint.Y + e.getY() - 1);
            Location1.X = Location0.X + 3;
            Location1.Y = Location0.Y + 3;
        }

        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            using(var brush = new SolidBrush( ToTransparentColor(FillColor, 80)))
            using (var fillPen = new Pen(FillColor))
            {
                gfx.FillRectangle(brush, new RectangleF((Location0.X + dx)*zoom, (Location0.Y + dy)*zoom, (Location1.X - Location0.X)*zoom, (Location1.Y - Location0.Y)*zoom));
                gfx.DrawRectangle(Pens.White, (Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom);

                gfx.DrawRectangle(fillPen, (Location0.X + dx - 1)*zoom, (Location0.Y + dy - 1)*zoom, (Location1.X - Location0.X + 2)*zoom, (Location1.Y - Location0.Y + 2)*zoom);
            }
        }
    }


    /// <summary>
    /// 
    /// Abstract Handle collection for redim/move/rotate shapes
    /// </summary>
    [Serializable]
    public abstract class AbstractSel : Element
    {
        //public float zoomX=1;
        //public float zoomY=1;
        protected readonly List<Handle> handles = new List<Handle>();

        protected AbstractSel(Element el)
        {
            Location0.X = el.getX();
            Location0.Y = el.getY();
            Location1.X = el.getX1();            
            Location1.Y = el.getY1();
            IsSelected = false;
            CanRotate = el.CanRotate;
            Rotation = el.GetRotation();
            GroupZoom.X = el.getGprZoomX();
            GroupZoom.Y = el.getGprZoomY();
            IsLine = el.IsLine;
            IsGroup = el.IsGroup;
            endMoveRedim();
        }

        public override void endMoveRedim()
        {
            base.endMoveRedim();
            foreach (var h in handles)
            {
                h.endMoveRedim();
            }
        }


        public void setZoom(float x, float y)
        {
            GroupZoom.X = x;
            GroupZoom.Y = y;
            foreach (var h in handles)
            {
                h.rePosition(this);
            }
        }

        public override void Rotate(float x, float y)
        {
            base.Rotate(x, y);
            foreach (var h in handles)
            {
                h.rePosition(this);
            }
        }

        public override void move(int x, int y)
        {
            base.move(x, y);
            //
            foreach (var h in handles)
            {
                //h.move(x, y);
                h.rePosition(this);
            }
        }

        public void showHandles(bool i)
        {
            IsGroup = i;
        }

        /// <summary>
        /// Su quale maniglia cade il punto x,y? 
        /// </summary>
        public RedimStatus isOver(int x, int y)
        {
            foreach (var h in handles)
            {
                RedimStatus ret = h.isOver(x, y);
                if (ret != RedimStatus.None)
                    return ret;
            }

            return Contains(x, y) ? RedimStatus.MoveSelected : RedimStatus.Idle;
        }

        public override void Select()
        {
            UndoElement = Copy();
        }

        public override void Redim(int x, int y, RedimStatus redimStatus)
        {
            base.Redim(x, y, redimStatus); //
            foreach (var h in handles)
                h.rePosition(this);
        }

        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            foreach (var h in handles)
                if (h.IsVisible)
                    h.Draw(gfx, dx, dy, zoom);
        }
    }


    /// <summary>
    /// Handle tool for redim/move/rotate shapes
    /// </summary>
    [Serializable]
    public class SelRect : AbstractSel
    {
        public SelRect(Element el)
            : base(el)
        {
            setup();
        }

        /// <summary>
        ///set ups handles
        /// </summary>
        public void setup()
        {
            if (!IsGroup)
            {
                //NW
                handles.Add(new RedimHandle(this, RedimStatus.NW));
                //SE
                handles.Add(new RedimHandle(this, RedimStatus.SE));
                if (!IsLine)
                {
                    //N
                    handles.Add(new RedimHandle(this, RedimStatus.N));
                    if (CanRotate) //ROT
                        handles.Add(new RotHandle(this, RedimStatus.Rotate));                    
                    //NE
                    handles.Add(new RedimHandle(this, RedimStatus.NE));
                    //E
                    handles.Add(new RedimHandle(this, RedimStatus.E));
                    //S
                    handles.Add(new RedimHandle(this, RedimStatus.S));
                    //SW
                    handles.Add(new RedimHandle(this, RedimStatus.SW));
                    //W
                    handles.Add(new RedimHandle(this, RedimStatus.W));
                }
            }
            else
            {
                //N
                handles.Add(new RedimHandle(this, RedimStatus.N));
                if (CanRotate)
                {
                    //ROT
                    handles.Add(new RotHandle(this, RedimStatus.Rotate));
                }
                //E
                handles.Add(new RedimHandle(this, RedimStatus.E));
                //S
                handles.Add(new RedimHandle(this, RedimStatus.S));
                //W
                handles.Add(new RedimHandle(this, RedimStatus.W));
                //ZOOM
                handles.Add(new ZoomHandle(this, RedimStatus.Zoom));
            }
        }

        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            //TEST
            //Matrix precMx = g.Transform.Clone();//store previous transformation
            //Matrix mx = g.Transform; // get previous transformation
            //PointF p = new PointF(zoom * (this.X + dx + (this.X1 - this.X) / 2), zoom * (this.Y + dy + (this.Y1 - this.Y) / 2));
            //mx.RotateAt(this.getRotation(), p, MatrixOrder.Append); //add a transformation
            //g.Transform = mx;

            base.Draw(gfx, dx, dy, zoom);
            using (var pen = new Pen(Color.Blue, 1.5f))
            {
                pen.DashStyle = DashStyle.Dash;
                if (IsLine)
                    gfx.DrawLine(pen, (Location0.X + dx)*zoom, (Location0.Y + dy)*zoom, (Location1.X + dx)*zoom, (Location1.Y + dy)*zoom);
                else
                    gfx.DrawRectangle(pen, (Location0.X + dx)*zoom, (Location0.Y + dy)*zoom, (Location1.X - Location0.X)*zoom, (Location1.Y - Location0.Y)*zoom);
            }

            //TEST
            //g.Transform = precMx;
            //precMx.Dispose();
            //mx.Dispose();
        }
    }


    /// <summary>
    /// Handle tool for redim/move/rotate Polygons
    /// </summary>
    [Serializable]
    public class SelPoly : AbstractSel
    {
        public SelPoly(Element el)
            : base(el)
        {
            setup((PointSet) el);
            /*this.X = ((PointSet)el).getMinX();
            this.Y = ((PointSet)el).getMinY();
            this.X1 = ((PointSet)el).getMaxX();
            this.Y1 = ((PointSet)el).getMaxY();*/
        }

        /// <summary>
        ///set ups handles
        /// </summary>
        public void setup(PointSet el)
        {
            if (CanRotate)
            {
                //ROT
                handles.Add(new RotHandle(this, RedimStatus.Rotate));
            }

            PointWr prec = null;
            int c = 0;
            int minx;
            int miny;
            int maxx;
            int maxy;
            foreach (var p in  el.points)
            {
                c++;
                handles.Add(new PointHandle(this, RedimStatus.Poly, p));
                if (prec != null)
                {
                    minx = Math.Min(p.X, prec.X);
                    miny = Math.Min(p.Y, prec.Y);
                    maxx = Math.Max(p.X, prec.X);
                    maxy = Math.Max(p.Y, prec.Y);
                    var newP = new PointWr(minx + ((maxx - minx)/2), miny + ((maxy - miny)/2));
                    handles.Add(new NewPointHandle(this, RedimStatus.NewPoint, newP, c));
                }
                prec = p;
            }

            if (c > 0)
            {
                var newP = new PointWr(prec.X + 7, prec.Y + 7);
                handles.Add(new NewPointHandle(this, RedimStatus.NewPoint, newP, c + 1));
            }

            //SE
            handles.Add(new RedimHandle(this, RedimStatus.SE));
            //S
            handles.Add(new RedimHandle(this, RedimStatus.S));
            //E
            handles.Add(new RedimHandle(this, RedimStatus.E));
            //W
            handles.Add(new RedimHandle(this, RedimStatus.W));
            //SW
            handles.Add(new RedimHandle(this, RedimStatus.SW));
            //NW
            handles.Add(new RedimHandle(this, RedimStatus.NW));
            //N
            handles.Add(new RedimHandle(this, RedimStatus.N));
            //NE
            handles.Add(new RedimHandle(this, RedimStatus.NE));
        }


        /// <summary>
        ///set ups handles
        /// </summary>
        public void reCreateCreationHandles(PointSet el)
        {
            // TODO: rewrite this back-to-front, instead of with a clumsy temp list
            var tmp = new List<Handle>();
            foreach (var h in handles)
                if (h is NewPointHandle)
                    tmp.Add(h);
            foreach (var h in tmp)
                handles.Remove(h);


            PointWr prec = null;
            int c = 0;
            int minx = 0;
            int miny = 0;
            int maxx = 0;
            int maxy = 0;
            foreach (var p in el.points)
            {
                c++;
                if (prec != null)
                {
                    minx = Math.Min(p.X, prec.X);
                    miny = Math.Min(p.Y, prec.Y);
                    maxx = Math.Max(p.X, prec.X);
                    maxy = Math.Max(p.Y, prec.Y);
                    var newP = new PointWr(minx + ((maxx - minx)/2), miny + ((maxy - miny)/2));
                    handles.Add(new NewPointHandle(this, RedimStatus.NewPoint, newP, c));
                }
                prec = p;
            }

            if (c > 0)
            {
                var newP = new PointWr(prec.X + 7, prec.Y + 7);
                handles.Add(new NewPointHandle(this, RedimStatus.NewPoint, newP, c + 1));
            }
        }

        public List<PointWr> getSelPoints()
        {
            var a = new List<PointWr>();
            foreach (var h in handles)
                if (h is PointHandle && h.IsSelected)
                    a.Add(((PointHandle) h).getPoint());
            return a;
        }

        public int getIndex()
        {
            foreach (var h in handles)
            {
                if (h is NewPointHandle)
                {
                    if (h.IsSelected)
                    {
                        return ((NewPointHandle) h).Index;
                    }
                }
            }
            return 0;
        }


        public PointWr getNewPoint()
        {
            foreach (var h in handles)
            {
                if (h is NewPointHandle)
                {
                    if (h.IsSelected)
                    {
                        return ((NewPointHandle) h).getPoint();
                    }
                }
            }
            return null;
        }

        public void movePoints(int dx, int dy)
        {
            //(SelPoly)(this.sRec).movePoints(dx, dy);
            foreach (var h in handles)
            {
                if (h is PointHandle)
                {
                    if (h.IsSelected)
                    {
                        h.move(dx, dy);
                    }
                }
            }
        }


        public override void Redim(int x, int y, RedimStatus redimStatus)
        {
            base.Redim(x, y, redimStatus);
            foreach (var h in handles)
                if (h is NewPointHandle)
                    h.IsVisible = false;
        }

        public override void Rotate(float x, float y)
        {
            base.Rotate(x, y);
            foreach (var h in handles)
            {
                if (h is PointHandle || h is NewPointHandle)
                {
                    h.IsVisible = false;
                }
            }
        }

        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            //TEST
            //Matrix precMx = g.Transform.Clone();//store previous transformation
            //Matrix mx = g.Transform; // get previous transformation
            //PointF p = new PointF(zoom * (this.X + dx + (this.X1 - this.X) / 2), zoom * (this.Y + dy + (this.Y1 - this.Y) / 2));
            //mx.RotateAt(this.getRotation(), p, MatrixOrder.Append); //add a transformation
            //g.Transform = mx;

            base.Draw(gfx, dx, dy, zoom);

            using (var pen = new Pen(Color.Blue, 1f) {DashStyle = DashStyle.Dash})
            {
                gfx.DrawRectangle(pen, (Location0.X + dx)*zoom, (Location0.Y + dy)*zoom, (Location1.X - Location0.X)*zoom, (Location1.Y - Location0.Y)*zoom);
            }

            //TEST
            //g.Transform = precMx;
            //precMx.Dispose();
            //mx.Dispose();
        }
    }
}