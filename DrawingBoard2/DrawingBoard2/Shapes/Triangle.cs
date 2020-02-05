using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Triangle shape
    /// </summary>
    [Serializable]
    public class Triangle : Polygon
    {
        #region Variables
        private TriangleType triangleType = TriangleType.Equilateral;
        #endregion

        #region Properties
        /// <summary>
        /// Type of Triangle(Right or Equilateral)
        /// </summary>
        [Description("Type of Triangle(Right or Equilateral)")]
        public TriangleType TriangleType
        {
            get { return this.triangleType; }
            set { this.triangleType = value; }
        }
        /// <summary>
        /// Region of the triangle
        /// <remarks>Note that,points of the triangles are generated here</remarks>
        /// </summary>
        [Category("Appearance"), Description("Region of the triangle")]
        public override Region Region
        {
            set
            {
                base.Region = value;

                if (this.triangleType == TriangleType.Equilateral)
                {
                    points.Add(new PointElement(region.X1, region.Y1));
                    points.Add(new PointElement(region.X1 - region.Width/2, region.Y0));
                    points.Add(new PointElement(region.X0, region.Y1));
                }
                else if (triangleType == TriangleType.Right)
                {
                    points.Add(new PointElement(region.X1, region.Y1));
                    points.Add(new PointElement(region.X0, region.Y0));
                    points.Add(new PointElement(region.X0, region.Y1));
                }
            }
            get
            {
                return base.Region;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// New triangle shape
        /// </summary>
        public Triangle()
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
