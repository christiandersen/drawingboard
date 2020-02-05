using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawingBoard
{
    /// <summary>
    /// Group
    /// </summary>
    [Serializable]
    public class Group : Element
    {
        // sub objects contained in group
        public static int Ngrp; // used to generate names
        private readonly List<Element> objs;

        // Manage the group like a Graphic path 
        private bool _grapPath;

        private GroupDisplay _GroupDisplay = GroupDisplay.Default;

        // the name of the group
        private string _name = string.Empty;
        private bool _xMirror;
        private bool _yMirror;

        /// <summary>
        /// .Ctor) 
        /// </summary>
        public Group(List<Element> a)
        {
            IsGroup = true;
            objs = a;

            int minX = +32000;
            int maxX = -32000;
            int minY = +32000;
            int maxY = -32000;

            foreach (var e in objs)
            {
                if (e.getX() < minX)
                    minX = e.getX();
                if (e.getY() < minY)
                    minY = e.getY();
                if (e.getX1() > maxX)
                    maxX = e.getX1();
                if (e.getY1() > maxY)
                    maxY = e.getY1();
                e.IsSelected = false;
            }

            Location0.X = minX;
            Location0.Y = minY;
            Location1.X = maxX;
            Location1.Y = maxY;
            IsSelected = true;
            endMoveRedim();
            Rotation = 0;
            CanRotate = true; //can rotate?
            Name = "Itm" + Ngrp;
            Ngrp++;
        }

        #region OVERRIDEN PROPERIES

        #region GET/SET

        [Category("Group"), Description("Shape List")]
        public Element[] Objs
        {
            get
            {
                var aar = new Element[objs.Count];
                int i = 0;
                foreach (var e in objs)
                    aar[i++] = e;
                return aar;
            }
        }

        [Category("Group"), Description("Grp.Display")]
        public GroupDisplay GroupDisplay
        {
            get { return _GroupDisplay; }
            set { _GroupDisplay = value; }
        }

        [Category("Group"), Description("X Mirror ON/OFF ")]
        public bool XMirror
        {
            get { return _xMirror; }
            set { _xMirror = value; }
        }

        [Category("Group"), Description("Y Mirror ON/OFF ")]
        public bool YMirror
        {
            get { return _yMirror; }
            set { _yMirror = value; }
        }


        [Category("Group"), Description("X Scale Zoom ")]
        public float GrpZoomX
        {
            get { return GroupZoom.X; }
            set
            {
                if (value > 0)
                    GroupZoom.X = value;
            }
        }

        [Category("Group"), Description("Y Scale Zoom ")]
        public float GrpZoomY
        {
            get { return GroupZoom.Y; }
            set
            {
                if (value > 0)
                    GroupZoom.Y = value;
            }
        }


        [Category("GradientBrush"), Description("True: use gradient fill color")]
        public override bool UseGradientLineColor
        {
            get { return base.UseGradientLineColor; }
            set
            {
                base.UseGradientLineColor = value;
                if (objs != null)
                    foreach (var e in objs)
                        e.UseGradientLineColor = value;
            }
        }

        [Category("GradientBrush"), Description("End Color Position [0..1])")]
        public override float EndColorPosition
        {
            get { return base.EndColorPosition; }
            set
            {
                base.EndColorPosition = value;
                if (objs != null)
                    foreach (var e in objs)
                        e.EndColorPosition = value;
            }
        }


        [Category("GradientBrush"), Description("Gradient End Color")]
        public override Color EndColor
        {
            get { return base.EndColor; }
            set
            {
                base.EndColor = value;
                if (objs != null)
                    foreach (var e in objs)
                        e.EndColor = value;
            }
        }


        [Category("GradientBrush"), Description("End Color Opacity")]
        public override int EndAlpha
        {
            get { return base.EndAlpha; }
            set
            {
                base.EndAlpha = value;
                if (objs != null)
                    foreach (var e in objs)
                        e.EndAlpha = value;
            }
        }

        [Category("GradientBrush"), Description("Gradient Dimension")]
        public override int GradientLength
        {
            get { return base.GradientLength; }
            set
            {
                base.GradientLength = value;
                if (objs != null)
                    foreach (var e in objs)
                        e.GradientLength = value;
            }
        }

        [Category("GradientBrush"), Description("Gradient Angle")]
        public override int GradientAngle
        {
            get { return base.GradientAngle; }
            set
            {
                base.GradientAngle = value;
                if (objs != null)
                    foreach (var e in objs)
                        e.GradientAngle = value;
            }
        }

        public override int Alpha
        {
            get { return base.Alpha; }
            set
            {
                base.Alpha = value;
                if (objs != null)
                    foreach (var e in objs)
                        e.Alpha = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != string.Empty)
                    _name = value;
            }
        }


        [Description("Manage group as a graphic path.")]
        public bool graphPath
        {
            get { return _grapPath; }
            set { _grapPath = value; }
        }


        public override Color FillColor
        {
            get { return base.FillColor; }
            set
            {
                base.FillColor = value;
                if (objs != null)
                    foreach (var e in objs)
                        e.FillColor = value;
            }
        }

        public override bool IsFilled
        {
            get { return base.IsFilled; }
            set
            {
                base.IsFilled = value;
                if (objs != null)
                    foreach (var e in objs)
                        e.IsFilled = value;
            }
        }

        public override Color PenColor
        {
            get { return base.PenColor; }
            set
            {
                base.PenColor = value;
                if (objs != null)
                    foreach (var e in objs)
                        e.PenColor = value;
            }
        }

        public override float PenWidth
        {
            get { return base.PenWidth; }
            set
            {
                base.PenWidth = value;
                if (objs != null)
                    foreach (var e in objs)
                        e.PenWidth = value;
            }
        }

        public override bool ShowBorder
        {
            get { return base.ShowBorder; }
            set
            {
                base.ShowBorder = value;
                if (objs != null)
                    foreach (var e in objs)
                        e.ShowBorder = value;
            }
        }

        public override DashStyle dashStyle
        {
            get { return base.dashStyle; }
            set
            {
                base.dashStyle = value;
                if (objs != null)
                    foreach (var e in objs)
                        e.dashStyle = value;
            }
        }

        #endregion

        #endregion

        [Category("1"), Description("GroupObj")]
        public string ObjectType
        {
            get { return "Group"; }
        }

        public override void AfterLoad()
        {
            base.AfterLoad();
            foreach (var e in objs)
                e.AfterLoad();
        }

        public override void endMoveRedim()
        {
            base.endMoveRedim();
            foreach (var e in objs)
                e.endMoveRedim();
        }

        public override List<Element> UnGroup()
        {
            return objs;
        }

        public override void move(int x, int y)
        {
            foreach (var e in objs)
                e.move(x, y);
            Location0.X = Start0.X - x;
            Location0.Y = Start0.Y - y;
            Location1.X = Start1.X - x;
            Location1.Y = Start1.Y - y;
        }

        public override void ShowEditor(RichtTextForm f)
        {
            foreach (var e in objs)
                if (e.OnGrpDClick)
                    e.ShowEditor(f);
        }

        public void Load_IMG()
        {
            foreach (var e in objs)
            {
                if (!e.OnGrpDClick)
                    continue;
                if (e is ImageBox)
                    ((ImageBox)e).Load_IMG();
                if (e is Group)
                    ((Group)e).Load_IMG();
            }
        }

        public void setZoom(int x, int y)
        {
            float dx = (Location1.X - x) * 2;
            float dy = (Location1.Y - y) * 2;

            GrpZoomX = (Width - dx) / Width;
            GrpZoomY = (Height - dy) / Height;
        }

        public override Element Copy()
        {
            //Copy chils
            var l1 = new List<Element>();
            foreach (var e in objs)
                l1.Add(e.Copy());

            var newE = new Group(l1)
            {
                Rotation = Rotation,
                _grapPath = _grapPath,
                GroupZoom = GroupZoom,
                IsGroup = IsGroup,
                _name = (Name + "_" + Ngrp),
                OnGrpDClick = OnGrpDClick,
                OnGrpXRes = OnGrpXRes,
                OnGrpX1Res = OnGrpX1Res,
                OnGrpYRes = OnGrpYRes,
                OnGrpY1Res = OnGrpY1Res,
                GroupDisplay = GroupDisplay
            };
            /*
            newE.penColor = this.penColor;
            newE.penWidth = this.penWidth;
            newE.fillColor = this.fillColor;
            newE.filled = this.filled;
            newE.dashStyle = this.dashStyle;
            newE.alpha = this.alpha;
            newE.IsLine = this.IsLine;
            //newE.Rotation = this.Rotation;
            newE.showBorder = this.showBorder;
            */

            if (newE._grapPath)
            {
                newE.PenColor = PenColor;
                newE.PenWidth = PenWidth;
                newE.FillColor = FillColor;
                newE.IsFilled = IsFilled;
                newE.dashStyle = dashStyle;
                newE.Alpha = Alpha;
                newE.IsLine = IsLine;
                newE.Rotation = Rotation;
                newE.ShowBorder = ShowBorder;

                newE.UseGradientLineColor = UseGradientLineColor;
                newE.GradientAngle = GradientAngle;
                newE.GradientLength = GradientLength;
                newE.EndAlpha = EndAlpha;
                newE.EndColor = EndColor;
                newE.EndColorPosition = EndColorPosition;
            }

            return newE;
        }

        public override void CopyFrom(Element element)
        {
            copyStdProp(element, this);
            //
            //this._grapPath = ele._grapPath;
        }

        public override void Select()
        {
            UndoElement = Copy();
        }

        public override void Redim(int x, int y, RedimStatus redimStatus)
        {
            foreach (var e in objs)
            {
                switch (redimStatus)
                {
                    case RedimStatus.N:
                        base.Redim(x, y, redimStatus);
                        if (e.OnGrpYRes != OnGroupResize.Nothing)
                        {
                            if (e.OnGrpYRes == OnGroupResize.Move)
                                e.move(0, -y);
                            else
                                e.Redim(0, y, redimStatus);
                        }
                        break;
                    case RedimStatus.E:
                        base.Redim(x, y, redimStatus);
                        if (e.OnGrpX1Res != OnGroupResize.Nothing)
                        {
                            if (e.OnGrpX1Res == OnGroupResize.Move)
                                e.move(-x, 0);
                            else
                                e.Redim(x, 0, redimStatus);
                        }
                        break;
                    case RedimStatus.S:
                        base.Redim(x, y, redimStatus);
                        if (e.OnGrpY1Res != OnGroupResize.Nothing)
                        {
                            if (e.OnGrpY1Res == OnGroupResize.Move)
                                e.move(0, -y);
                            else
                                e.Redim(0, y, redimStatus);
                        }
                        break;
                    case RedimStatus.W:
                        base.Redim(x, y, redimStatus);
                        if (e.OnGrpXRes != OnGroupResize.Nothing)
                        {
                            if (e.OnGrpXRes == OnGroupResize.Move)
                                e.move(-x, 0);
                            else
                                e.Redim(x, 0, redimStatus);
                        }
                        break;
                    default:
                        break;
                }
            }
        }


        public override void AddGp(GraphicsPath gp, int dx, int dy, float zoom)
        {
            foreach (var e in objs)
            {
                //e.AddGp(gp, dx, dy, zoom);
                e.AddGraphPath(gp, dx, dy, zoom);
            }
        }


        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            //Matrix precMx = g.Transform.Clone();//store previous transformation
            GraphicsState gs = gfx.Save(); //store previous transformation
            using (var matrix = gfx.Transform) // get previous transformation
            {
                var p = new PointF(zoom * (Location0.X + dx + (Location1.X - Location0.X) / 2), zoom * (Location0.Y + dy + (Location1.Y - Location0.Y) / 2));
                if (Rotation > 0)
                    matrix.RotateAt(Rotation, p, MatrixOrder.Append); //add a transformation

                //X MIRROR  //Y MIRROR
                if (_xMirror || _yMirror)
                {
                    matrix.Translate(-(Location0.X + Width / 2 + dx) * zoom, -(Location0.Y + Height / 2 + dy) * zoom, MatrixOrder.Append);
                    if (_xMirror)
                        matrix.Multiply(new Matrix(-1, 0, 0, 1, 0, 0), MatrixOrder.Append);
                    if (_yMirror)
                        matrix.Multiply(new Matrix(1, 0, 0, -1, 0, 0), MatrixOrder.Append);
                    matrix.Translate((Location0.X + Width / 2 + dx) * zoom, (Location0.Y + Height / 2 + dy) * zoom, MatrixOrder.Append);
                }

                if (GrpZoomX > 0 && GrpZoomY > 0)
                {
                    matrix.Translate((-1) * zoom * (Location0.X + dx + (Location1.X - Location0.X) / 2), (-1) * zoom * (Location0.Y + dy + (Location1.Y - Location0.Y) / 2), MatrixOrder.Append);
                    matrix.Scale(GrpZoomX, GrpZoomY, MatrixOrder.Append);
                    matrix.Translate(zoom * (Location0.X + dx + (Location1.X - Location0.X) / 2), zoom * (Location0.Y + dy + (Location1.Y - Location0.Y) / 2), MatrixOrder.Append);
                }
                gfx.Transform = matrix;

                //g.ResetTransform();
                //The next drawn objs are translated over origin , rotated and then traslated again.
                //g.TranslateTransform((-1) * zoom * (this.X + dx + (this.X1 - this.X) / 2), (-1) * zoom * (this.Y + dy + (this.Y1 - this.Y) / 2), MatrixOrder.Append);
                //g.RotateTransform(this.Rotation, MatrixOrder.Append);
                //g.TranslateTransform(zoom * (this.X + dx + (this.X1 - this.X) / 2), zoom * (this.Y + dy + (this.Y1 - this.Y) / 2), MatrixOrder.Append);

                if (_grapPath)

                #region path

                {
                    //System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(this.fillColor);
                    using (var brush = GetBrush(dx, dy, zoom))
                    using (var pen = new Pen(PenColor, scaledPenWidth(zoom)) { DashStyle = dashStyle })
                    {
                        if (IsSelected)
                        {
                            //brush.Color = this.dark(this.fillColor, 5, this.alpha);
                            pen.Color = Color.Red;
                            pen.Color = ToTransparentColor(pen.Color, 120);
                            pen.Width = pen.Width + 1;
                        }

                        using (var gp = new GraphicsPath())
                        {
                            foreach (var e in objs)
                            {
                                //e.AddGp(gp, dx, dy, zoom);
                                e.AddGraphPath(gp, dx, dy, zoom);
                                //rr.Intersect(gp);
                                //rr.Xor(gp);
                            }
                            //g.SetClip(rr,CombineMode.Intersect);
                            if (IsFilled)
                            {
                                gfx.FillPath(brush, gp);
                                if (ShowBorder)
                                    gfx.DrawPath(pen, gp);
                            }
                            else
                                gfx.DrawPath(pen, gp);
                        }
                    }
                    //g.ResetClip();
                    //TEST START     

                    //FillWithLines(g, dx, dy, zoom, gp, gridSize, gridRot);
                    //TEST END

                }
                #endregion

                else
                {
                    //MANAGE This.GroupDisplay
                    if (GroupDisplay != GroupDisplay.Default)
                    {
                        using (var rr = new Region())
                        {
                            bool first = true;
                            foreach (var e in objs)
                            {
                                using (var gp = new GraphicsPath(FillMode.Winding))
                                {
                                    e.AddGraphPath(gp, dx, dy, zoom);
                                    if (first)
                                        rr.Intersect(gp);
                                    else
                                    {
                                        switch (GroupDisplay)
                                        {
                                            case GroupDisplay.Intersect:
                                                rr.Intersect(gp);
                                                break;
                                            case GroupDisplay.Xor:
                                                rr.Xor(gp);
                                                break;
                                            case GroupDisplay.Exclude:
                                                rr.Exclude(gp);
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                                first = false;
                            }
                            gfx.SetClip(rr, CombineMode.Intersect);
                        }
                    }

                    foreach (var e in objs)
                        e.Draw(gfx, dx, dy, zoom);
                    if (GroupDisplay != GroupDisplay.Default)
                        gfx.ResetClip();
                }

                gfx.Restore(gs); //restore previous transformation
                if (IsSelected)
                {
                    using (var pen = new Pen(PenColor, scaledPenWidth(zoom)) { DashStyle = dashStyle, Color = Color.Red })
                    {
                        pen.Color = ToTransparentColor(pen.Color, 120);
                        pen.Width = pen.Width + 1;
                        gfx.DrawRectangle(pen, (Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom);
                    }
                }
            }
            //precMx.Dispose();
        }
    }
}
