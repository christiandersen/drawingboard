using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Represent a point in a pointset or in a polygon
    /// </summary>
    [Serializable]
    public class PointElement
    {
        #region Variables
        private Point current; //Current point
        private bool selected = false;
        private Point start; //Start(initial) point
        #endregion

        #region Properties
        /// <summary>
        /// Indicates whether point is selected or not
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Indicates whether point is selected or not")]
        public bool Selected
        {
            get { return this.selected; }
            set { this.selected = value; }
        }
        /// <summary>
        /// X position of the point element
        /// </summary>
         [CategoryAttribute("Layout"), Description("X position of the point element")]
        public int X
        {
            get { return this.current.X; }
            set { this.current.X = value; }
        }
        /// <summary>
        /// Y position of the point element
        /// </summary>
        [CategoryAttribute("Layout"), Description("Y position of the point element")]
        public int Y
        {
            get { return this.current.Y; }
            set { this.current.Y = value; }
        }
        /// <summary>
        /// Initial X position 
        /// </summary>
        [CategoryAttribute("Layout"), Description("Initial X position ")]
        public int StartX
        {
            get { return this.start.X; }
            set { this.start.X = value; }
        }
        /// <summary>
        /// Initial Y position 
        /// </summary>
        [CategoryAttribute("Layout"),Description("Initial Y position ")]
        public int StartY
        {
            get { return this.start.Y; }
            set { this.start.Y = value; }
        }
        /// <summary>
        /// Initial(start) position of the point element
        /// </summary>
        [YAXDontSerialize]
        [Browsable(false)]
        public Point StartPoint
        {
            get { return this.start; }
            set { this.start = value; }
        }
        /// <summary>
        /// Current position of the point element
        /// </summary>
        [YAXDontSerialize]
        [Browsable(false)]
        public Point Point
        {
            get { return this.current; }
            set { this.current = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// A point element
        /// </summary>
        public PointElement()
        {
        }
        /// <summary>
        /// A point element
        /// </summary>
        /// <param name="point">Position of the PointElement</param>
        public PointElement(PointF point)
        {
            this.current = this.start = Point.Round(point);
        }
        /// <summary>
        /// A point element
        /// </summary>
        /// <param name="point">Position of the PointElement</param>
        public PointElement(Point point)
        {
            this.current = this.start = point;
        }
        /// <summary>
        /// A point element
        /// </summary>
        /// <param name="x">X position of the point element</param>
        /// <param name="y">Y position of the point element</param>
        public PointElement(int x, int y)
        {
            this.current.X = x;
            this.current.Y = y;
            this.start = this.current;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Copies element
        /// </summary>
        /// <returns>Created instance for copy</returns>
        public PointElement Copy()
        {
            return new PointElement(this.X, this.Y);
        }
        /// <summary>
        /// Rotate point at point(x,y)
        /// </summary>
        /// <param name="x">x coordinate value of the rotation point</param>
        /// <param name="y">y coordinate value of the rotation point</param>
        /// <param name="rotationAngle">Rotation angle</param>
        public void RotateAt(float x, float y, int rotationAngle)
        {
            float tmpX = this.X - x;
            float tmpY = this.Y - y;
            PointF tmpPoint = DrawingUtils.RotatePoint(new PointF(tmpX, tmpY), rotationAngle);

            this.X = (int)(tmpPoint.X + x);
            this.Y = (int)(tmpPoint.X + y);
        }
        /// <summary>
        /// Invoked when zooming is started
        /// </summary>
        /// <param name="dx">Zoom coefficient value of X side</param>
        /// <param name="dy">Zoom coefficient value of Y side</param>
        public void Zoom(float dx, float dy)
        {
            this.X = (int)(start.X * dx);
            this.Y = (int)(start.Y * dy);
        }
        /// <summary>
        /// Invoked when zooming is ended
        /// </summary>
        public void EndZoom()
        {
            this.start = this.current;
        }
        /// <summary>
        /// Mirrors the shape at X axis
        /// </summary>
        /// <param name="width">Width to be added</param>
        public void XMirror(int width)
        {
            this.X = (-1) * current.X + width;
            start = current;
        }
        /// <summary>
        /// Mirrors the shape at Y coordinate
        /// </summary>
        /// <param name="height">Height to be added</param>
        public void YMirror(int height)
        {
            this.Y = (-1) * current.Y + height;
            start = current;
        }
        #endregion
    }
}
