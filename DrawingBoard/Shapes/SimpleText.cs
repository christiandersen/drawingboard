using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawingBoard
{
    /// <summary>
    /// Simple Text 
    /// </summary>
    [Serializable]
    public class SimpleText : Element
    {
        //TEST
        private StringAlignment sa;

        public SimpleText(int x, int y, int x1, int y1)
        {
            Location0.X = x;
            Location0.Y = y;
            Location1.X = x1;
            Location1.Y = y1;
            IsSelected = true;
            endMoveRedim();

            Rotation = 0;
            //this.CharFont = new Font(FontFamily.GenericMonospace, 8); ;
            CanRotate = true; //can rotate?
        }

        public string Text { get; set; }

        public StringAlignment StrAllin
        {
            get { return sa; }
            set { sa = value; }
        }

        public Font CharFont { get; set; }

        [Category("1"), Description("Simple Text")]
        public string ObjectType
        {
            get { return "Text Rectangle"; }
        }

        public override void AddGp(GraphicsPath gp, int dx, int dy, float zoom)
        {
            gp.AddRectangle(new RectangleF((Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom));

            /*
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = this.sa;
            stringFormat.LineAlignment = StringAlignment.Near;
            FontFamily family = new FontFamily(this.CharFont.FontFamily.Name);
            //int fontStyle = (int)FontStyle.Bold;
            gp.AddString(this.Text, family, fontStyle, this.CharFont.Size * zoom, new RectangleF((this.X + dx) * zoom, (this.Y + dy) * zoom, (this.X1 - this.X) * zoom, (this.Y1 - this.Y) * zoom), stringFormat);
            */
        }


        public override Element Copy()
        {
            var newE = new SimpleText(Location0.X, Location0.Y, Location1.X, Location1.Y)
            {
                PenColor = PenColor,
                PenWidth = PenWidth,
                FillColor = FillColor,
                IsFilled = IsFilled,
                IsLine = IsLine,
                Alpha = Alpha,
                dashStyle = dashStyle,
                ShowBorder = ShowBorder,
                OnGrpXRes = OnGrpXRes,
                OnGrpX1Res = OnGrpX1Res,
                OnGrpYRes = OnGrpYRes,
                OnGrpY1Res = OnGrpY1Res
            };

            newE.copyGradprop(this);

            newE.Text = Text;
            newE.CharFont = CharFont;
            newE.StrAllin = StrAllin;
            //newE.rtf = this.rtf;

            return newE;
        }


        public override void CopyFrom(Element element)
        {
            copyStdProp(element, this);
        }

        public override void Select()
        {
            UndoElement = Copy();
        }


        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            var gs = gfx.Save(); //store previous transformation
            using (var matrix = gfx.Transform) // get previous transformation
            {
                var p = new PointF(zoom * (Location0.X + dx + (Location1.X - Location0.X) / 2), zoom * (Location0.Y + dy + (Location1.Y - Location0.Y) / 2));
                if (Rotation > 0)
                    matrix.RotateAt(Rotation, p, MatrixOrder.Append); //add a transformation

                gfx.Transform = matrix;
            }

            //System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(this.fillColor);
            using (var brush = GetBrush(dx, dy, zoom))
            using (var pen = new Pen(PenColor, scaledPenWidth(zoom)) { DashStyle = dashStyle })
            {
                if (IsSelected)
                {
                    pen.Color = Color.Red;
                    pen.Color = ToTransparentColor(pen.Color, 120);
                    pen.Width = pen.Width + 1;
                }

                if (IsFilled)
                    gfx.FillRectangle(brush, (Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom);
                if (ShowBorder || IsSelected)
                    gfx.DrawRectangle(pen, (Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom);
            }

            using (var stringFormat = new StringFormat { Alignment = sa, LineAlignment = StringAlignment.Near })
            using (var tmpf = new Font(CharFont.FontFamily, CharFont.Size * zoom, CharFont.Style))
            using (var brush = new SolidBrush(PenColor))
            {
                gfx.DrawString(Text, tmpf, brush, new RectangleF((Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom), stringFormat);
            }

            gfx.Restore(gs); //restore previous transformation
        }
    }
}
