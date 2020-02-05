using System;
using System.Drawing;

namespace DrawingBoard2.Utils
{
    /// <summary>
    /// Contains drawing related utility functions
    /// </summary>
    public static class DrawingUtils
    {
        /// <summary>
        /// Calculates location of zoomed point
        /// </summary>
        /// <param name="x">X region of the point</param>
        /// <param name="y">Y region of the point</param>
        /// <param name="dx">Shift X value</param>
        /// <param name="dy">Shift Y value</param>
        /// <param name="zoom">Zoom Value</param>
        /// <returns>Calculated <see cref="System.Drawing.PointF"/></returns>
        public static PointF GetZoomPointF(float x, float y, int dx, int dy, float zoom)
        {
            return new PointF((x + dx) * zoom, (y + dy) * zoom);
        }
        /// <summary>
        /// Multiplies X and Y dimension value of point with a multiplier value
        /// </summary>
        /// <param name="point">Point to be multiplied</param>
        /// <param name="multiplier">Multiplier value</param>
        /// <returns>Multiplied value</returns>
        public static PointF Multiply(PointF point, float multiplier)
        {
            return new PointF(point.X * multiplier, point.Y * multiplier);
        }
        /// <summary>
        /// Rotates a point around center(0,0) by rotationAngle value
        /// </summary>
        /// <param name="point">Point to be rotated</param>
        /// <param name="rotationAngle">Rotation angle</param>
        /// <returns>Rotated point</returns>
        public static PointF RotatePoint(PointF point, int rotationAngle)
        {
            double RotAngF = rotationAngle * Math.PI / 180;
            double SinVal = Math.Sin(RotAngF);
            double CosVal = Math.Cos(RotAngF);

            float Nx = (float)(point.X * CosVal - point.Y * SinVal);
            float Ny = (float)(point.Y * CosVal + point.X * SinVal);

            return new PointF(Nx, Ny);
        }
        /// <summary>
        /// Sets Color transparency
        /// </summary>
        /// <param name="color">Color to set trasparency value</param>
        /// <param name="alpha">Transparency(alpha) value</param>
        /// <returns>Transparent Color</returns>
        public static Color SetTransparency(Color color, int alpha)
        {
            return Color.FromArgb(alpha < 0 ? 0 : (alpha > 255 ? 255 : alpha), color);
        }
    }
}
