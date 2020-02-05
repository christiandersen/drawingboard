using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawingBoard
{
    /// <summary>
    /// Rounded Rectangle
    /// </summary>
    /// 
    [Description("Rounded rectangle")]
    [Serializable]
    public class RoundedRect : Element
    {
        private int _arcsWidth;
        public RoundedRect(int x, int y, int x1, int y1)
        {
            Location0.X = x;
            Location0.Y = y;
            Location1.X = x1;
            Location1.Y = y1;
            IsSelected = true;
            ArcsWidth = 20;
            Rotation = 0;
            endMoveRedim();
            CanRotate = true; //can rotate?
        }

        [Category("1"), Description("Rounded Rectangle")]
        public string ObjectType
        {
            get { return "Rounded Rectangle"; }
        }


        [Category("Vertex Appearance"), Description("Dimension of vertex arcs.")]
        public int ArcsWidth
        {
            get { return _arcsWidth; }
            set { _arcsWidth = value <= 9 ? 10 : value; }
        }

        public override Element Copy()
        {
            var newE = new RoundedRect(Location0.X, Location0.Y, Location1.X, Location1.Y)
            {
                PenColor = PenColor,
                PenWidth = PenWidth,
                FillColor = FillColor,
                IsFilled = IsFilled,
                IsLine = IsLine,
                Alpha = Alpha,
                dashStyle = dashStyle,
                ShowBorder = ShowBorder,
                ArcsWidth = ArcsWidth,
                Rotation = Rotation,
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
            ArcsWidth = ((RoundedRect)element).ArcsWidth;
            Rotation = element.Rotation;
        }


        public override void Select()
        {
            UndoElement = Copy();
        }


        public override void AddGp(GraphicsPath gp, int dx, int dy, float zoom)
        {
            float n = ArcsWidth;
            gp.AddArc(new RectangleF((Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, n * zoom, n * zoom), 180, 90);
            gp.AddLine((Location0.X + dx + n / 2) * zoom, (Location0.Y + dy) * zoom, (Location1.X + dx - n / 2) * zoom, (Location0.Y + dy) * zoom);

            gp.AddArc(new RectangleF((Location1.X + dx - n) * zoom, (Location0.Y + dy) * zoom, n * zoom, n * zoom), 270, 90);
            gp.AddLine((Location1.X + dx) * zoom, (Location0.Y + dy + n / 2) * zoom, (Location1.X + dx) * zoom, (Location1.Y + dy - n / 2) * zoom);

            gp.AddArc(new RectangleF((Location1.X + dx - n) * zoom, (Location1.Y + dy - n) * zoom, n * zoom, n * zoom), 0, 90);
            gp.AddLine((Location0.X + dx + n / 2) * zoom, (Location1.Y + dy) * zoom, (Location1.X + dx - n / 2) * zoom, (Location1.Y + dy) * zoom);

            gp.AddArc(new RectangleF((Location0.X + dx) * zoom, (Location1.Y + dy - n) * zoom, n * zoom, n * zoom), 90, 90);
            gp.AddLine((Location0.X + dx) * zoom, (Location1.Y + dy - n / 2) * zoom, (Location0.X + dx) * zoom, (Location0.Y + dy + n / 2) * zoom);
        }


        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            float n = ArcsWidth;
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

                // Create a path and add the object.
                using (var myPath = new GraphicsPath())
                {
                    myPath.AddArc(new RectangleF((Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, n * zoom, n * zoom), 180, 90);
                    myPath.AddLine((Location0.X + dx + n / 2) * zoom, (Location0.Y + dy) * zoom, (Location1.X + dx - n / 2) * zoom, (Location0.Y + dy) * zoom);

                    myPath.AddArc(new RectangleF((Location1.X + dx - n) * zoom, (Location0.Y + dy) * zoom, n * zoom, n * zoom), 270, 90);
                    myPath.AddLine((Location1.X + dx) * zoom, (Location0.Y + dy + n / 2) * zoom, (Location1.X + dx) * zoom, (Location1.Y + dy - n / 2) * zoom);

                    myPath.AddArc(new RectangleF((Location1.X + dx - n) * zoom, (Location1.Y + dy - n) * zoom, n * zoom, n * zoom), 0, 90);
                    myPath.AddLine((Location0.X + dx + n / 2) * zoom, (Location1.Y + dy) * zoom, (Location1.X + dx - n / 2) * zoom, (Location1.Y + dy) * zoom);

                    myPath.AddArc(new RectangleF((Location0.X + dx) * zoom, (Location1.Y + dy - n) * zoom, n * zoom, n * zoom), 90, 90);
                    myPath.AddLine((Location0.X + dx) * zoom, (Location1.Y + dy - n / 2) * zoom, (Location0.X + dx) * zoom, (Location0.Y + dy + n / 2) * zoom);

                    using (var translateMatrix = new Matrix())
                    {
                        translateMatrix.RotateAt(Rotation,
                                                 new PointF((Location0.X + dx + (Location1.X - Location0.X) / 2) * zoom, (Location0.Y + dy + (Location1.Y - Location0.Y) / 2) * zoom));
                        myPath.Transform(translateMatrix);
                    }

                    // Draw the transformed ellipse to the screen.
                    if (IsFilled)
                    {
                        gfx.FillPath(brush, myPath);
                        if (ShowBorder)
                            gfx.DrawPath(pen, myPath);
                    }
                    else
                        gfx.DrawPath(pen, myPath);

                    //FillWithLines(gfx, dx, dy, zoom, myPath, 1, 0);
                }
            }
        }
    }
}
