using System;
using System.Drawing;
using System.Xml.Serialization;
using System.ComponentModel;

using DrawingBoard2.Utils;


namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Point Color: used to store and update points in arraylist of gradient brush
    /// </summary>
    [Serializable]
    public class ColoredPoint : PointElement
    {
        #region Properties
        /// <summary>
        /// Color of the point element
        /// </summary>
        [YAXDontSerialize]
        [Category("Appearance"), Description("Color of the point element")]
        public Color Color { get; set; }

        /// <summary>
        /// Color of the point element in hexadecimal string format
        /// </summary>
        [Browsable(false)]
        public string ColorStr
        {
            get { return ConverterUtil.ToString(Color); }
            set { this.Color = ConverterUtil.ToColor(value); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Colored Point
        /// </summary>
        /// <param name="point">System.Drawing.Point representation of the point</param>
        /// <param name="color">Color of the point</param>
        public ColoredPoint(Point point, Color color)
            :base(point)
        {
            this.Color = color;
        }
        /// <summary>
        /// Colored Point
        /// </summary>
        /// <param name="point">System.Drawing.Point representation of the point</param>
        public ColoredPoint(Point point):
            base(point)
        {
        }
        /// <summary>
        /// Colored Point
        /// </summary>
        /// <param name="x">X position of the point</param>
        /// <param name="y">Y position of the point</param>
        public ColoredPoint(int x, int y) :
            base(x,y)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deep Copy of the ColoredPoint
        /// </summary>
        /// <returns>Created/Copied ColoredPoint element</returns>
        public new ColoredPoint Copy()
        {
            return new ColoredPoint(this.X, this.Y);
        }
        #endregion
    }
}
