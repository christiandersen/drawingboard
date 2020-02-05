using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawingBoard
{
    /// <summary>
    /// Line 
    /// </summary>
    [Serializable]
    public class Line : Element
    {
        public Line(int x, int y, int x1, int y1)
        {
            Location0.X = x;
            Location0.Y = y;
            Location1.X = x1;
            Location1.Y = y1;
            IsLine = true;
            IsSelected = true;
            starCap = LineCap.Custom;
            endCap = LineCap.Custom;
            endMoveRedim();
            CanRotate = false; //can rotate?
        }

        [Category("Line Appearance"), Description("Line Start Cap")]
        public LineCap starCap { get; set; }

        [Category("Line Appearance"), Description("Line End Cap")]
        public LineCap endCap { get; set; }

        [Category("1"), Description("Line")]
        public string ObjectType
        {
            get { return "Line"; }
        }


        public override Element Copy()
        {
            var newE = new Line(Location0.X, Location0.Y, Location1.X, Location1.Y);
            newE.PenColor = PenColor;
            newE.PenWidth = PenWidth;
            newE.FillColor = FillColor;
            newE.IsFilled = IsFilled;
            newE.Dashstyle = Dashstyle;
            newE.IsLine = IsLine;
            newE.Alpha = Alpha;
            //
            newE.starCap = starCap;
            newE.endCap = endCap;

            newE.OnGrpXRes = OnGrpXRes;
            newE.OnGrpX1Res = OnGrpX1Res;
            newE.OnGrpYRes = OnGrpYRes;
            newE.OnGrpY1Res = OnGrpY1Res;

            newE.GroupZoom.X = GroupZoom.X;
            newE.GroupZoom.Y = GroupZoom.Y;


            return newE;
        }

        public override void CopyFrom(Element element)
        {
            copyStdProp(element, this);
            endCap = ((Line)element).endCap;
            starCap = ((Line)element).starCap;
        }

        public override void Select()
        {
            UndoElement = Copy();
        }


        public override void AddGp(GraphicsPath gp, int dx, int dy, float zoom)
        {
            gp.AddLine((getX() + dx) * zoom, (getY() + dy) * zoom, (getX1() + dx) * zoom, (getY1() + dy) * zoom);
        }

        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            using (var pen = new Pen(PenColor, scaledPenWidth(zoom))
            {
                DashStyle = dashStyle,
                StartCap = starCap,
                EndCap = endCap,
                Color = ToTransparentColor(PenColor, Alpha)
            })
            {
                if (IsSelected)
                {
                    pen.Color = Color.Red;
                    pen.Color = ToTransparentColor(pen.Color, 120);
                    pen.Width = pen.Width + 1;
                    gfx.DrawEllipse(pen, (Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, 3, 3);
                }

                if (Location0.X == Location1.X && Location0.Y == Location1.Y)
                    gfx.DrawEllipse(pen, (Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, 3, 3);
                else
                    gfx.DrawLine(pen, (Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X + dx) * zoom, (Location1.Y + dy) * zoom);
            }
        }
    }
}
