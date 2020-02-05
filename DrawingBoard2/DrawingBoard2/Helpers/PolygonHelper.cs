using System;
using System.Collections.Generic;
using System.Drawing;

namespace DrawingBoard2.Helpers
{
    /// <summary>
    /// Helper class that has polygon related functions
    /// </summary>
    internal static class PolygonHelper
    {
        /// <summary>
        /// Function that calculates points of polygon according to provided parameters
        /// </summary>
        /// <param name="sides">Number of polygon sides</param>
        /// <param name="radius">Radius of polygon</param>
        /// <param name="startingAngle">Angle of the polygon</param>
        /// <param name="center">Center point of the polygon</param>
        /// <returns>List of calculated polygon points</returns>
        public static List<PointF> CalculatePolygonPoints(int sides, int radius, int startingAngle, PointF center)
        {
            List<PointF> points = new List<PointF>();
            float step = 360.0f / sides;

            float angle = startingAngle; //starting angle
            for (double i = startingAngle; i < startingAngle + 360.0; i += step) //go in a circle
            {
                points.Add(DegreesToXY(angle, radius, center));
                angle += step;
            }

            return points;
        }
        /// <summary>
        /// Calculates point according to <paramref name="degrees"/> ,<paramref name="origin"/> and <paramref name="radius"/>
        /// </summary>
        /// <param name="degrees">Degree of point according to the origin</param>
        /// <param name="radius">Radius of the polygon</param>
        /// <param name="origin">Center point of the polygon</param>
        /// <returns>Calculated Point</returns>
        private static PointF DegreesToXY(float degrees, float radius, PointF origin)
        {
            PointF xy = new PointF();
            double radians = degrees * Math.PI / 180.0;

            xy.X = (float)(Math.Cos(radians) * radius + origin.X);
            xy.Y = (float)(Math.Sin(-radians) * radius + origin.Y);

            return xy;
        }
        /// <summary>
        /// Calculates region of the corner letter on the board
        /// </summary>
        /// <param name="corner">Corner point that will be named</param>
        /// <param name="array">All points in the polygon</param>
        /// <returns>Calculated region</returns>
        public static PointF CalculateCornerLetterPoint(PointF corner, PointF[] array)
        {
            if (array.Length == 0)
                return new PointF(corner.X, corner.Y);

            float minx = array[0].X;
            float maxx = array[0].X;
            float maxy = array[0].Y;
            float miny = array[0].Y;

            //Find bounds of the polygon(min - max coordinate values)
            for (int i = 1; i < array.Length; i++)
            {
                if (minx < array[i].X)
                    minx = array[i].X;
                if (maxx > array[i].X)
                    maxx = array[i].X;
                if (miny < array[i].Y)
                    miny = array[i].Y;
                if (maxy > array[i].Y)
                    maxy = array[i].Y;
            }
            float x = corner.X;
            float y = corner.Y;

            if (Math.Abs(x - maxx) > Math.Abs(x - minx))
                x += 5;
            else
                x -= 15;

            if (Math.Abs(y - maxy) > Math.Abs(y - miny))
                y += 5;
            else
                y -= 15;

            return new PointF(x, y);
        }
        /// <summary>
        /// Calculates point positions of 5 edged star shape
        /// </summary>
        /// <param name="Orig"> The origin is the middle of the star.</param>
        /// <param name="outerradius">Radius of the surrounding circle.</param>
        /// <param name="innerradius">Radius of the circle for the "inner" points</param>
        /// <returns>Array of 10 PointF structures</returns>
        public static PointF[] Calculate5StarPoints(PointF Orig, float outerradius, float innerradius)
        {
            // Define some variables to avoid as much calculations as possible
            // conversions to radians
            double Ang36 = Math.PI / 5.0;   // 36° x PI/180
            double Ang72 = 2.0 * Ang36;     // 72° x PI/180
            // some sine and cosine values we need
            float Sin36 = (float)Math.Sin(Ang36);
            float Sin72 = (float)Math.Sin(Ang72);
            float Cos36 = (float)Math.Cos(Ang36);
            float Cos72 = (float)Math.Cos(Ang72);
            // Fill array with 10 origin points
            PointF[] pnts = { Orig, Orig, Orig, Orig, Orig, Orig, Orig, Orig, Orig, Orig };
            pnts[0].Y -= outerradius;  // top off the star, or on a clock this is 12:00 or 0:00 hours
            pnts[1].X += innerradius * Sin36; pnts[1].Y -= innerradius * Cos36; // 0:06 hours
            pnts[2].X += outerradius * Sin72; pnts[2].Y -= outerradius * Cos72; // 0:12 hours
            pnts[3].X += innerradius * Sin72; pnts[3].Y += innerradius * Cos72; // 0:18
            pnts[4].X += outerradius * Sin36; pnts[4].Y += outerradius * Cos36; // 0:24 
            // Phew! Glad I got that trig working.
            pnts[5].Y += innerradius;
            // I use the symmetry of the star figure here
            pnts[6].X += pnts[6].X - pnts[4].X; pnts[6].Y = pnts[4].Y;  // mirror point
            pnts[7].X += pnts[7].X - pnts[3].X; pnts[7].Y = pnts[3].Y;  // mirror point
            pnts[8].X += pnts[8].X - pnts[2].X; pnts[8].Y = pnts[2].Y;  // mirror point
            pnts[9].X += pnts[9].X - pnts[1].X; pnts[9].Y = pnts[1].Y;  // mirror point

            return pnts;
        }
    }
}
