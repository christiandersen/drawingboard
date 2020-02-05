using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DrawingBoard
{
    /// <summary>
    /// Box Immagine
    /// </summary>
    [Serializable]
    public class ImageBox : Element
    {
        public ImageBox(int x, int y, int x1, int y1)
        {
            Location0.X = x;
            Location0.Y = y;
            Location1.X = x1;
            Location1.Y = y1;
            IsSelected = true;
            endMoveRedim();
            ShowBorder = false;
            CanRotate = true; //can rotate
        }

        [Category("Image"), Description("File image")]
        public Bitmap img { get; set; }

        [Category("Image"), Description("Transparency")]
        public bool Transparent { get; set; }

        [Category("1"), Description("Image Box")]
        public string ObjectType
        {
            get { return "Image Box"; }
        }

        public override void AddGp(GraphicsPath gp, int dx, int dy, float zoom)
        {
            gp.AddRectangle(new RectangleF((Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom));
        }


        public override Element Copy()
        {
            var newE = new ImageBox(Location0.X, Location0.Y, Location1.X, Location1.Y)
            {
                PenColor = PenColor,
                PenWidth = PenWidth,
                FillColor = FillColor,
                IsFilled = IsFilled,
                IsLine = IsLine,
                Alpha = Alpha,
                dashStyle = dashStyle,
                ShowBorder = ShowBorder,
                Transparent = Transparent,
                Rotation = Rotation,
                OnGrpXRes = OnGrpXRes,
                OnGrpX1Res = OnGrpX1Res,
                OnGrpYRes = OnGrpYRes,
                OnGrpY1Res = OnGrpY1Res,
                img = img
            };


            newE.copyGradprop(this);

            return newE;
        }

        public override void CopyFrom(Element element)
        {
            copyStdProp(element, this);
            img = ((ImageBox)element).img;
        }

        public override void Select()
        {
            UndoElement = Copy();
        }

        public void Load_IMG()
        {
            string f_name = imgLoader();
            if (f_name != null)
            {
                try
                {
                    var loadTexture = new Bitmap(f_name);
                    img = loadTexture;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private static string imgLoader()
        {
            try
            {
                using (var dlg = new OpenFileDialog
                {
                    Title = "Load background image",
                    Filter =
                        "jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|tif files (*.tif)|*.tif|bmp files (*.bmp)|*.bmp|All files (*.*)|*.*"
                })
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                        return (dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }


        public override void Draw(Graphics gfx, int dx, int dy, float zoom)
        {
            using (var pen = new Pen(PenColor, scaledPenWidth(zoom)) { DashStyle = dashStyle })
            {
                if (IsSelected)
                {
                    pen.Color = Color.Red;
                    pen.Color = ToTransparentColor(pen.Color, 120);
                    pen.Width = pen.Width + 1;
                }

                if (img != null)
                {
                    var backColor = img.GetPixel(0, 0); //get the back color from the first pixel (UP-LEFT)
                    // Create a temp Bitmap and a graphic object
                    // the dimension of the tmp bitmap must permit the rotation of img
                    var dim = (int)Math.Sqrt(img.Width * img.Width + img.Height * img.Height);
                    using (var curBitmap = new Bitmap(dim, dim))
                    {
                        using (var curGfx = Graphics.FromImage(curBitmap))
                        {
                            if (Rotation > 0)
                            {
                                // activate the rotation on the graphic obj
                                using (var matrix = new Matrix())
                                {
                                    matrix.RotateAt(Rotation, new PointF(curBitmap.Width >> 1, curBitmap.Height >> 1));
                                    curGfx.Transform = matrix;
                                }
                            }
                            // draw img over the tmp bitmap 
                            curGfx.DrawImage(img, (dim - img.Width) / 2, (dim - img.Height) / 2, img.Width, img.Height);

                            if (Transparent)
                                curBitmap.MakeTransparent(backColor); // perform color keying  with the background color

                            curGfx.Save();
                            // draw the tmp bitmap on canvas
                            gfx.DrawImage(curBitmap, (Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom);
                        }
                    }
                }

                if (ShowBorder)
                    gfx.DrawRectangle(pen, (Location0.X + dx) * zoom, (Location0.Y + dy) * zoom, (Location1.X - Location0.X) * zoom, (Location1.Y - Location0.Y) * zoom);
            }
        }
    }
}
