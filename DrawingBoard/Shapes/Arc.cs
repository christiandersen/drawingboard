using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawingBoard
{
    /// <summary>
    /// Arc 
    /// </summary>
    [Serializable]
    public class Arc : Element
    {
        public Arc(int x, int y, int x1, int y1)
        {
            Location0.X = x;
            Location0.Y = y;
            Location1.X = x1;
            Location1.Y = y1;
            IsSelected = true;
            endMoveRedim();
            StartAng = 0;
            LenAng = 90;
            startCap = LineCap.Custom;
            endCap = LineCap.Custom;
        }

        [Category("1"), Description("Rectangle")]
        public string ObjectType
        {
            get { return "Arc"; }
        }

        [Category("Line Appearance"), Description("Line Start Cap")]
        public LineCap startCap { get; set; }

        [Category("Line Appearance"), Description("Line End Cap")]
        public LineCap endCap { get; set; }


        [Description("Start angle")]
        public int StartAng { get; set; }

        [Description("Angle length")]
        public int LenAng { get; set; }


        public override Element Copy()
        {
            var newE = new Arc(Location0.X, Location0.Y, Location1.X, Location1.Y)
            {
                PenColor = PenColor,
                PenWidth = PenWidth,
                FillColor = FillColor,
                IsFilled = IsFilled,
                dashStyle = dashStyle,
                Alpha = Alpha,
                IsLine = IsLine,
                StartAng = StartAng,
                LenAng = LenAng,
                ShowBorder = ShowBorder,
                endCap = endCap,
                startCap = startCap,
                OnGrpXRes = OnGrpXRes,
                OnGrpX1Res = OnGrpX1Res,
                OnGrpYRes = OnGrpYRes,
                OnGrpY1Res = OnGrpY1Res
            };

            newE.copyGradprop(this);

            return newE;
        }


        public override void CopyFrom(Element element)
        {
            copyStdProp(element, this);
            StartAng = ((Arc)element).StartAng;
            LenAng = ((Arc)element).LenAng;
            startCap = ((Arc)element).startCap;
            endCap = ((Arc)element).endCap;
        }


        public override void Select()
        {
            UndoElement = Copy();
        }


        public override void AddGp(GraphicsPath gp, int dx, int dy, float zoom)
        {
            gp.AddArc((Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom, StartAng, LenAng);
        }


        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            using (var brush = GetBrush(dx, dy, zoom))
            using (var pen = new Pen(PenColor, scaledPenWidth(zoom))
            {
                DashStyle = dashStyle,
                EndCap = endCap,
                StartCap = startCap
            })
            {
                if (IsSelected)
                {
                    using (var myPen1 = new Pen(PenColor, scaledPenWidth(zoom)) { Width = 0.5f, DashStyle = DashStyle.Dot })
                    {
                        gfx.DrawEllipse(myPen1, (Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom);
                    }
                    //brush.Color = this.dark(this.fillColor, 5, this.alpha);
                    pen.Color = Color.Red;
                    pen.Color = ToTransparentColor(pen.Color, 120);
                    pen.Width = pen.Width + 1;
                }

                // Create a path and add the object.
                using (var myPath = new GraphicsPath())
                {
                    myPath.AddArc((Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom, StartAng, LenAng);
                    //using (var translateMatrix = new Matrix())
                    //{
                    //    translateMatrix.RotateAt(this.Rotation, new Point(this.X + (int)(this.X1 - this.X) / 2, this.Y + (int)(this.Y1 - this.Y) / 2));
                    //    myPath.Transform(translateMatrix);
                    //}

                    // Draw the transformed ellipse to the screen.
                    if (IsFilled)
                    {
                        gfx.FillPath(brush, myPath);
                        if (ShowBorder)
                            gfx.DrawPath(pen, myPath);
                    }
                    else
                        gfx.DrawPath(pen, myPath);
                }
            }
        }
    }
}
