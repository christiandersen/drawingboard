using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;

using DrawingBoard2.Helpers;
using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Pentagon shape
    /// </summary>
    [Serializable]
    public class Pentagon : Polygon
    {
        #region Properties
        /// <summary>
        /// Region of the pentagon
        /// <remarks>Also creates points of pentagon when it's set</remarks>
        /// </summary>
        [Category("Layout"), Description("Region of the pentagon")]
        public override Region Region
        {
            set
            {
                base.Region = value;
                List<PointF> pentagonPoints = PolygonHelper.CalculatePolygonPoints(5,
                    value.Width / 2, 18, value.MidPointF);

                foreach (PointF point in pentagonPoints)
                    this.points.Add(new PointElement(point));
            }
            get
            {
                return base.Region;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Pentagon(5 Corner Polyon) shape
        /// </summary>
        public Pentagon()
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
