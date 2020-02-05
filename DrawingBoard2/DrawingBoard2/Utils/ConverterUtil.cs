using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;

namespace DrawingBoard2.Utils
{
    /// <summary>
    /// Contains convertion functions between data types
    /// </summary>
    public static class ConverterUtil
    {
        /// <summary>
        /// Converts color to string in Argb format
        /// </summary>
        /// <param name="color">Color to be converted</param>
        /// <returns>String value of the color in hexadecimal format</returns>
        public static string ToString(Color color)
        {
            return String.Format("#{0:X}", color.ToArgb());
        }
        /// <summary>
        /// Parses string value and returns it as Color
        /// </summary>
        /// <param name="colorStr">String to be parsed</param>
        /// <returns><see cref="System.Drawing.Color"/></returns>
        public static Color ToColor(string colorStr)
        {
            Color color = Color.White;

            colorStr = colorStr.Trim();
            if (colorStr.StartsWith("#")) // remove leading # if any
                colorStr = colorStr.Substring(1);

            int n;
            if (Int32.TryParse(colorStr,
                NumberStyles.HexNumber, null, out n))
                color = Color.FromArgb(n);
            return color;
        }
        /// <summary>
        /// Function that converts bitmap to byte Array
        /// </summary>
        /// <param name="bitmap">Bitmap image to be converted</param>
        /// <returns>Converted byte array</returns>
        public static byte[] ConvertToByteArray(Bitmap bitmap)
        {
            if (bitmap == null)
                return null;

            byte[] byteArr = null;

            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);
                byteArr = stream.ToArray();
            }
            return byteArr;
        }
        /// <summary>
        /// Converts byte array to bitmap
        /// </summary>
        /// <param name="byteArr">Byte array that contains bitmap data</param>
        /// <returns>Converted <see cref="System.Drawing.Bitmap"/></returns>
        public static Bitmap ConvertToBitmap(byte[] byteArr)
        {
            Bitmap bitmap = null;

            if (byteArr == null)
                return null;
            using (MemoryStream stream = new MemoryStream(byteArr))
            {
                bitmap = new Bitmap(stream);
            }
            return bitmap;
        }
        /// <summary>
        /// Converts string to byte array
        /// </summary>
        /// <param name="str">String to be converted</param>
        /// <returns>Returns converted byte array</returns>
        public static byte[] StringToByteArray(string str)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            return encoding.GetBytes(str);
        }
        /// <summary>
        /// Converts byte array to string
        /// </summary>
        /// <param name="byteArr">Byte array to be converted</param>
        /// <returns>Converted string</returns>
        public static string ByteArrayToString(byte[] byteArr)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            return encoding.GetString(byteArr);
        }
        /// <summary>
        /// Convert <see cref="DrawingBoard2.HandlerOperator"/> enum value to
        /// <see cref="DrawingBoard2.Direction"/>enum value
        /// </summary>
        /// <param name="hOperator">Handler operation value</param>
        /// <returns>Direction enum value</returns>
        public static Direction ToDirection(HandlerOperator hOperator)
        {
            if (hOperator == HandlerOperator.RedimEast)
                return Direction.East;
            else if (hOperator == HandlerOperator.RedimNorth)
                return Direction.North;
            else if (hOperator == HandlerOperator.RedimNorthEast)
                return Direction.NorthEast;
            else if (hOperator == HandlerOperator.RedimNorthWest)
                return Direction.NorthWest;
            else if (hOperator == HandlerOperator.RedimSouth)
                return Direction.South;
            else if (hOperator == HandlerOperator.RedimSouthEast)
                return Direction.SouthEast;
            else if (hOperator == HandlerOperator.RedimSouthWest)
                return Direction.SouthWest;
            else if (hOperator == HandlerOperator.RedimWest)
                return Direction.West;
            return Direction.West;
        }
    }
}
