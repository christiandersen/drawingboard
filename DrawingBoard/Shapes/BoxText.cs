using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DrawingBoard
{
    /// <summary>
    /// Text Box
    /// </summary>
    [Serializable]
    public class BoxText : Element
    {
        public string rtf; // save rtf text


        // placed here to not overload draw metod
        [NonSerialized]
        protected RichTextBoxPrintCtrl.RichTextBoxPrintCtrl tmpR = new RichTextBoxPrintCtrl.RichTextBoxPrintCtrl();

        public BoxText(int x, int y, int x1, int y1)
        {
            Location0.X = x;
            Location0.Y = y;
            Location1.X = x1;
            Location1.Y = y1;
            IsSelected = true;
            endMoveRedim();
            rtf = string.Empty;
            //tBox = new TxtBox();
        }

        [Category("1"), Description("RTF Box")]
        public string ObjectType
        {
            get { return "Text Rectangle"; }
        }

        public override void AfterLoad()
        {
            // tmpR is not serialized, I must recreate after Load
            if (tmpR == null)
                tmpR = new RichTextBoxPrintCtrl.RichTextBoxPrintCtrl();
        }

        public override void AddGp(GraphicsPath gp, int dx, int dy, float zoom)
        {
            gp.AddRectangle(new RectangleF((Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom));
        }


        public override Element Copy()
        {
            var newE = new BoxText(Location0.X, Location0.Y, Location1.X, Location1.Y)
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

            newE.rtf = rtf;

            return newE;
        }

        public override void CopyFrom(Element element)
        {
            copyStdProp(element, this);
            rtf = ((BoxText)element).rtf;
        }

        public override void Select()
        {
            UndoElement = Copy();
        }

        public override void Select(RichTextBox r)
        {
            r.Rtf = rtf;
        }

        public override void ShowEditor(RichtTextForm f)
        {
            f.richTextBox1.Rtf = rtf;
            f.ShowDialog();
            if (f.confermato)
                rtf = f.richTextBox1.Rtf;
        }

        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
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

                if (IsFilled)
                {
                    gfx.FillRectangle(brush, (Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom);
                }
                if (ShowBorder || IsSelected)
                    gfx.DrawRectangle(pen, (Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom);
            }

            tmpR.BorderStyle = BorderStyle.None;
            tmpR.ScrollBars = RichTextBoxScrollBars.None;

            tmpR.Rtf = rtf;

            /*
            // TO ENABLE TEST ROTATION / ZOOM
            //TEST START
                Bitmap curBitmap = new Bitmap((int)(this.Width * zoom),(int)( this.Height * zoom));
                curBitmap.SetResolution(this.Width , this.Height );
                Graphics curG = Graphics.FromImage(curBitmap);
                curG.PageUnit = GraphicsUnit.Point;
                if (this.Rotation > 0)
                {
                    // activate the rotation on the graphic obj
                    using(var X = new Matrix())
                    {
                        X.RotateAt(this.Rotation, new PointF(curBitmap.Width / 2, curBitmap.Height / 2));
                        curG.Transform = X;
                    }
                }
                // I draw img over the tmp bitmap 
                if (curG.DpiX < 600)
                {
                    //tmpR.Draw(0, tmpR.TextLength, curG, (int)((this.X + dx) * zoom), (int)((this.Y + dy) * zoom), (int)((dx + this.X1 - (int)((this.X1 - this.X) * .08)) * zoom), (int)((dy + this.Y1 - (int)((this.Y1 - this.Y) * .08)) * zoom), 15);
                    tmpR.Draw(0, tmpR.TextLength, curG, 0, 0, (int)((this.Width * 1) * zoom), (int)(( this.Height * 1) * zoom), 15);
                }
                else
                {
                    //tmpR.Draw(0, tmpR.TextLength, curG, (int)((this.X + dx) * zoom), (int)((this.Y + dy) * zoom), (int)((this.X1 + dx) * zoom), (int)((this.Y1 + dy) * zoom), 14.4);
                    tmpR.Draw(0, tmpR.TextLength, curG, 0, 0, (int)((this.Width * 1) * zoom), (int)((this.Width * 1) * zoom), 14.4);
                }


                //if (this._tra .Transparent)
                    //curBitmap.MakeTransparent(backColor); // here I perform transparency

                curG.Save();
                // I draw the tmp bitmap on canvas
                g.DrawImage(curBitmap, (this.X + dx) * zoom, (this.Y + dy) * zoom, (this.X1 - this.X) * zoom, (this.Y1 - this.Y) * zoom);

                curG.Dispose();
                curBitmap.Dispose();

            // END
            */

            //Console.WriteLine("OSVersion: {0}", Environment.OSVersion.ToString());
            //Console.WriteLine("OSVersion: {0}", Environment.OSVersion.Platform.ToString());
            if (gfx.DpiX < 600)
            {
                //    tmpR.Draw(0, tmpR.TextLength, g, (int)((this.X + dx) * zoom), (int)((this.Y + dy) * zoom), (int)((dx + this.X1 - (int)((this.X1 - this.X) * .08)) * zoom),(int)( (dy + this.Y1 - (int)((this.Y1 - this.Y) * .08)) * zoom), 15);
                tmpR.Draw(0, tmpR.TextLength, gfx, (int)((Location0.X + dx) * zoom), (int)((Location0.Y + dy) * zoom), (int)((Location1.X + dx) * zoom),
                          (int)((Location1.Y + dy) * zoom), 1440 / gfx.DpiX, 1440 / gfx.DpiY);
            }
            else
                tmpR.Draw(0, tmpR.TextLength, gfx, (int)((Location0.X + dx) * zoom), (int)((Location0.Y + dy) * zoom), (int)((Location1.X + dx) * zoom),
                          (int)((Location1.Y + dy) * zoom), 14.4, 14.4);


            //tmpR.Dispose();
        }
    }

}
