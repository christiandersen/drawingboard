using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;

using DrawingBoard2;
using DrawingBoard2.Utils;
using DrawingBoard2.Helpers;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// 3-D Cube shape 
    /// </summary>
    [Serializable]
    public class Cube : Polygon
    {
        #region Properties
        /// <summary>
        /// Region of the cube
        /// </summary>
        /// <remarks>Cube points are generated when cube region is set</remarks>
        [Category("Layout"), Description("Region of the cube")]
        public override Region Region
        {
            set
            {
                base.Region = value;
                List<PointF> cubePoints = Plot3DHelper.GetCubePoints(value);

                foreach (PointF point in cubePoints)
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
        /// 3-D Cube shape 
        /// </summary>
        public Cube()
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
