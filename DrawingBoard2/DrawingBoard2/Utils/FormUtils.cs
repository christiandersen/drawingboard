using System;
using System.Drawing;
using System.Windows.Forms;

namespace DrawingBoard2.Utils
{
    /// <summary>
    /// Contains static form functions
    /// </summary>
    public static class FormUtils
    {
        /// <summary>
        /// Function that displays <see cref="System.Windows.Forms.OpenFileDialog"/>
        /// and loads image as <see cref="System.Drawing.Bitmap"/>
        /// </summary>
        /// <returns>Loaded <see cref="System.Drawing.Bitmap"/></returns>
        public static Bitmap GetImageByFileDialog()
        {
            using (OpenFileDialog imageFileDialog = new OpenFileDialog())
            {
                imageFileDialog.Title = "Load Image";
                imageFileDialog.Filter = "jpg files (*.jpg)|*.jpg|bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
                imageFileDialog.DefaultExt = "jpg";

                if (imageFileDialog.ShowDialog() == DialogResult.OK)
                    return Image.FromFile(imageFileDialog.FileName) as Bitmap;
            }
            return null;
        }
        /// <summary>
        /// Returns Cursor according to handler operator value
        /// </summary>
        /// <param name="hOperator">HandlerOperator</param>
        /// <returns><see cref="System.Windows.Forms.Cursor"/></returns>
        public static Cursor GetCursor(HandlerOperator hOperator)
        {
            if (hOperator == HandlerOperator.NewPoint || hOperator == HandlerOperator.Polygon
                || hOperator == HandlerOperator.Rotation)
                return Cursors.SizeAll;
            else if (hOperator == HandlerOperator.Default)
                return Cursors.Hand;
            else if (hOperator == HandlerOperator.RedimNorthWest)
                return Cursors.SizeNWSE;
            else if (hOperator == HandlerOperator.RedimNorth)
                return Cursors.SizeNS;
            else if (hOperator == HandlerOperator.RedimNorthEast)
                return Cursors.SizeNESW;
            else if (hOperator == HandlerOperator.RedimEast)
                return Cursors.SizeWE;
            else if (hOperator == HandlerOperator.RedimSouthEast)
                return Cursors.SizeNWSE;
            else if (hOperator == HandlerOperator.RedimSouth)
                return Cursors.SizeNS;
            else if (hOperator == HandlerOperator.RedimSouthWest)
                return Cursors.SizeNESW;
            else if (hOperator == HandlerOperator.RedimWest)
                return Cursors.SizeWE;
            else if (hOperator == HandlerOperator.Zoom)
                return Cursors.SizeNWSE;
            return Cursors.Default;
        }
    }
}
