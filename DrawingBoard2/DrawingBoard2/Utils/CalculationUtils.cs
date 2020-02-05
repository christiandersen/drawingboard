using System;

namespace DrawingBoard2.Utils
{
    /// <summary>
    /// Utility class that contains calculation utiliy functions
    /// </summary>
    public static class CalculationUtils
    {
        /// <summary>
        /// Calculates distance between two points by applying Euclidean formula
        /// Point-1 : x1,y1
        /// Point-2 : x2,y2
        /// </summary>
        /// <param name="x1">X region of point-1</param>
        /// <param name="y1">Y region of point-1</param>
        /// <param name="x2">X region of point-2</param>
        /// <param name="y2">Y region of point-2</param>
        /// <returns>Distance between points</returns>
        public static int CalculateDistance(int x1, int y1, int x2, int y2)
        {
            return (int)Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
        }
    }
}
