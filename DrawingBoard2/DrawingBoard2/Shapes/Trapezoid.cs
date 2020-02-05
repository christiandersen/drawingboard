using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;

using DrawingBoard2.Helpers;
using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Trapezoid shape
    /// </summary>
    [Serializable]
    public class Trapezoid : Polygon
    {
        #region Properties
        /// <summary>
        /// Region of the trapezoid
        /// <remarks>Note that,points of the trapezoid are generated here</remarks>
        /// </summary>         
        [Category("Appearance"), Description("Region of the trapezoid")]
        public override Region Region
        {
            set
            {
                base.Region = value;

                this.points.Add(new PointElement(region.X1 - region.Width / 4, region.Y0));
                this.points.Add(new PointElement(region.X0 + region.Width / 4, region.Y0));
                this.points.Add(new PointElement(region.X0, region.Y1));
                this.points.Add(new PointElement(region.X1, region.Y1));
            }
            get
            {
                return base.Region;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Create new Trapezoid shape
        /// </summary>
        public Trapezoid()
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
