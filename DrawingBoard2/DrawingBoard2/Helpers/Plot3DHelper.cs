using System;
using System.Drawing;
using System.Collections.Generic;
using DrawingBoard2.Plot3D;

namespace DrawingBoard2.Helpers
{
    /// <summary>
    /// Helper class for 3D drawing
    /// </summary>
    public static class Plot3DHelper
    {
        /// <summary>
        /// Calculates points of cube
        /// </summary>
        /// <param name="region">Region of the Cube shape</param>
        /// <returns>List of calculated <see cref="System.Drawing.PointF"/>s</returns>
        public static List<PointF> GetCubePoints(Region region)
        {
            Plotter3D plotter = new Plotter3D();
            List<PointF> cubePoints = new List<PointF>();

            plotter.Location = new Point3D(region.X0, region.Y0, 0);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    cubePoints.AddRange(plotter.Forward(region.Width));
                    plotter.TurnRight(90);
                }
                cubePoints.AddRange(plotter.Forward(region.Width));
                plotter.TurnDown(90);
            }
            return cubePoints;
        }
    }
}
