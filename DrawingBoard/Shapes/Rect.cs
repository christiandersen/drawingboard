using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawingBoard
{
    /// <summary>
    /// Rectangle
    /// </summary>
    [Serializable]
    public class Rect : Element
    {
        public Rect(int x, int y, int x1, int y1)
        {
            Location0.X = x;
            Location0.Y = y;
            Location1.X = x1;
            Location1.Y = y1;
            IsSelected = true;
            endMoveRedim();
            Rotation = 0;
            CanRotate = true; //can rotate?
        }

        [Category("1"), Description("Rectangle")]
        public string ObjectType
        {
            get { return "Rectangle"; }
        }


        public override Element Copy()
        {
            var newE = new Rect(Location0.X, Location0.Y, Location1.X, Location1.Y)
            {
                PenColor = PenColor,
                PenWidth = PenWidth,
                FillColor = FillColor,
                IsFilled = IsFilled,
                dashStyle = dashStyle,
                Alpha = Alpha,
                IsLine = IsLine,
                Rotation = Rotation,
                ShowBorder = ShowBorder,
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
            Rotation = element.Rotation;
        }


        public override void Select()
        {
            UndoElement = Copy();
        }

        public override void AddGp(GraphicsPath gp, int dx, int dy, float zoom)
        {
            gp.AddRectangle(new RectangleF((Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom));
        }


        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            //System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(this.fillColor);
            using (var brush = GetBrush(dx, dy, zoom))
            using (var pen = new Pen(PenColor, scaledPenWidth(zoom)) { DashStyle = dashStyle })
            {
                if (IsSelected)
                {
                    //brush.Color = this.dark(this.fillColor, 5,this.alpha);
                    pen.Color = Color.Red;
                    pen.Color = ToTransparentColor(pen.Color, 120);
                    pen.Width = pen.Width + 1;
                }

                // Create a path and add the object.
                using (var myPath = new GraphicsPath())
                {
                    myPath.AddRectangle(new RectangleF((Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom));
                    using (var translateMatrix = new Matrix())
                    {
                        translateMatrix.RotateAt(Rotation, new PointF((Location0.X + dx + (Location1.X - Location0.X) / 2) * zoom, (Location0.Y + dy + (Location1.Y - Location0.Y) / 2) * zoom));
                        myPath.Transform(translateMatrix);
                    }

                    // Draw the transformed ellipse to the screen.
                    if (IsFilled)
                    {
                        gfx.FillPath(brush, myPath);
                        //g.FillPath(br, myPath);
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
