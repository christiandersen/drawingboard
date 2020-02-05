using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;

using DrawingBoard2.Helpers;
using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// <see href="http://en.wikipedia.org/wiki/Hexagon">Hexagon</see> shape
    /// </summary>
    [Serializable]
    public class Hexagon : Polygon
    {
        #region Properties
        /// <summary>
        /// Region of the Hexagon
        /// </summary>
        [CategoryAttribute("Layout"), Description("Region of the Hexagon")]
        public override Region Region
        {
            set
            {
                base.Region = value;
                List<PointF> hexagonPoints = PolygonHelper.CalculatePolygonPoints(6,
                    value.Width / 2, 0, value.MidPointF);

                foreach (PointF point in hexagonPoints)
                    this.points.Add(new PointElement(point));
            }
            get
            {
                return base.region;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Hexagon shape
        /// </summary>
        public Hexagon()
        {
            this.curved = false;
            this.closed = true;
            this.fixedCorners = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates an instance and returns it for deep copy
        /// </summary>
        /// <returns>Created instance for deep copy</returns>
        public override ShapeElement Copy()
        {
            return CloneUtil<ShapeElement>.DeepClone<ShapeElement>(this);
        }
        /// <summary>
        /// Sets itself as undo shape 
        /// </summary>
        public override void Select()
        {
            this.undoShape = this.Copy();
        }
        #endregion
    }
}
