using System;
using System.Drawing;
using System.ComponentModel;
using DrawingBoard2.Helpers;

using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// 5-Star shape
    /// </summary>
    [Serializable]
    public class Star : Polygon
    {
        #region Properties
        /// <summary>
        /// Region of the star shape
        /// <remarks>Note : Position of each point of star is calculated in this property</remarks>
        /// </summary>
        [Category("Layout"), Description("Region of the star shape")]
        public override Region Region
        {
            set
            {
                base.Region = value;

                PointF[] starPoints = PolygonHelper.Calculate5StarPoints(this.region.MidPointF,
                    this.region.Width/2, this.region.Width / 6);

                foreach (PointF point in starPoints)
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
        /// 5-Star shape
        /// </summary>
        public Star()
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
            //return this.CreateInstanceForCopy() as ShapeElement;
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
