using System;
using System.Drawing;

using DrawingBoard2.Utils;

namespace DrawingBoard2
{
    /// <summary>
    /// Stucture that represents shape region between 2 points
    /// </summary>
    [Serializable]
    public struct Region
    {
        #region Variables
        private int x0;
        private int x1;
        private int y0;
        private int y1;
        #endregion

        #region Properties
        /// <summary>
        /// Middle Point(X,Y) of the shape
        /// </summary>
        [YAXDontSerialize]
        public Point MidPoint
        {
            get { return Point.Round(this.MidPointF); }
        }
        /// <summary>
        /// Middle Point(X,Y) of the shape
        /// </summary>
        [YAXDontSerialize]
        public PointF MidPointF
        {
            get { return new PointF(this.MidX, this.MidY); }
        }
        /// <summary>
        /// Indicates if region is empty ( Not Assigned ) or not
        /// </summary>
        [YAXDontSerialize]
        public bool IsEmpty
        {
            get { return x0 == -1 && x1 == -1 && y0 == -1 && y1 == -1; }
        }
        /// <summary>
        /// Middle X coordinate point of the region
        /// </summary>
        [YAXDontSerialize]
        public float MidX
        {
            get { return ((float)this.x1 + this.x0) / 2; }
        }
        /// <summary>
        /// Middle Y coordinate point of the region
        /// </summary>
        [YAXDontSerialize]
        public float MidY
        {
            get { return ((float)this.y1 + this.y0) / 2; }
        }
        /// <summary>
        /// Width of the region
        /// </summary>
        [YAXDontSerialize]
        public int Width
        {
            get { return Math.Abs(this.x1 - this.x0); }
        }
        /// <summary>
        /// Height of the region
        /// </summary>
        [YAXDontSerialize]
        public int Height
        {
            get { return Math.Abs(this.y1 - this.y0); }
        }
        /// <summary>
        /// Start point of region at x axis
        /// </summary>
        public int X0
        {
            get { return this.x0; }
            set { this.x0 = value; }
        }
        /// <summary>
        /// End point of region at x axis
        /// </summary>
        public int X1
        {
            get { return this.x1; }
            set { this.x1 = value; }
        }
        /// <summary>
        /// Start point of region at y axis
        /// </summary>
        public int Y0
        {
            get { return this.y0; }
            set { this.y0 = value; }
        }
        /// <summary>
        /// End point of region at y axis
        /// </summary>
        public int Y1
        {
            get { return this.y1; }
            set { this.y1 = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Stucture that represents shape region between 2 points
        /// </summary>
        /// <param name="x0">Start point of region at x axis</param>
        /// <param name="y0">Start point of region at y axis</param>
        /// <param name="x1">End point of region at x axis</param>
        /// <param name="y1">End point of region at y axis</param>
        public Region(int x0, int y0, int x1, int y1)
        {
            this.x0 = x0;
            this.y0 = y0;
            this.x1 = x1;
            this.y1 = y1;
        }
        /// <summary>
        /// Stucture that represents shape region between 2 points
        /// </summary>
        /// <param name="x0">Start point of region at x axis</param>
        /// <param name="y0">Start point of region at y axis</param>
        /// <param name="x1">End point of region at x axis</param>
        /// <param name="y1">End point of region at y axis</param>
        public Region(float x0, float y0, float x1, float y1)
        {
            this.x0 = (int)x0;
            this.y0 = (int)y0;
            this.x1 = (int)x1;
            this.y1 = (int)y1;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Calculates the actual region on board according to zoom and shift values
        /// </summary>
        /// <param name="dx">Shift value of x axis</param>
        /// <param name="dy">Shift value of y axis</param>
        /// <param name="zoom">Zoom value</param>
        /// <returns>Calculated point</returns>
        public PointF GetActualregion(int dx, int dy, float zoom)
        {
            return new PointF((this.x0 + dx + (int)(this.x1 - this.x0) / 2) * zoom,
                    (this.y0 + dy + (int)(this.y1 - this.y0) / 2) * zoom);
        }
        /// <summary>
        /// Copies region to itself(deep copy)
        /// </summary>
        /// <param name="region">Region to be copied</param>
        public void CopyFrom(Region region)
        {
            this.x0 = region.x0;
            this.y0 = region.y0;
            this.x1 = region.x1;
            this.y1 = region.y1;
        }
        /// <summary>
        /// Moves <paramref name="old"/> region by x,y
        /// </summary>
        /// <param name="old">Old region</param>
        /// <param name="x">value to move at x axis</param>
        /// <param name="y">value to move at y axis</param>
        public void Move(Region old, int x, int y)
        {
            this.x0 = old.x0 - x;
            this.y0 = old.y0 - y;
            this.x1 = old.x1 - x;
            this.y1 = old.y1 - y;
        }
        /// <summary>
        /// Creates new rectangle according to dx ,dy and zoom value
        /// </summary>
        /// <param name="dx">Shift value of x axis</param>
        /// <param name="dy">Shift value of y axis</param>
        /// <param name="zoom">Zoom value</param>
        /// <returns>Calculated Rectangle</returns>
        public RectangleF GetRectangleF(int dx, int dy, float zoom)
        {
            return new RectangleF((x0 + dx) * zoom, (y0 + dy) * zoom, (x1 - x0) * zoom, (y1 - y0) * zoom);
        }
        /// <summary>
        /// Fixes regions so that , difference between end and start points cant be less then
        /// minimum difference value
        /// </summary>
        /// <param name="minimumSizeDifference">Minimum Difference </param>
        public void FixSize(int minimumSizeDifference)
        {
            if (x1 - minimumSizeDifference <= x0)
                x1 = x0 + minimumSizeDifference;
            if (y1 - minimumSizeDifference <= y0)
                y1 = y0 + minimumSizeDifference;
        }
        #endregion

        #region Static
        /// <summary>
        /// Empty(unassigned) region
        /// </summary>
        public static Region Empty = new Region(-1, -1, -1, -1);
        #endregion
    }
}
