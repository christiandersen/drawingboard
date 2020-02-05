using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// <see href="http://en.wikipedia.org/wiki/Cartesian_coordinate_system">Cartesian Plane</see>Shape 
    /// </summary>
    [Serializable]
    public class CartesianPlane : ShapeElement
    {
        #region Variables
        private int verticalUnitCount = 4;
        private int horizontalUnitCount = 4;
        #endregion

        #region Properties
        /// <summary>
        /// Total number of units on Y axis
        /// </summary>
        [Category("Appearance"), Description("Total number of units on Y axis")]
        public int VerticalUnitCount
        {
            get { return this.verticalUnitCount; }
            set { this.verticalUnitCount = value; }
        }
        /// <summary>
        /// Total number of units on X axis
        /// </summary>
        [Category("Appearance"), Description("Total number of units on X axis")]
        public int HorizontalUnitCount
        {
            get { return this.horizontalUnitCount; }
            set { this.horizontalUnitCount = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Cartesian Plane Shape
        /// </summary>
        public CartesianPlane()
        {
            this.IsLine = false;
            this.selected = true;
            this.EndMoveRedim();
            this.canRotate = false;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deep copies CoordinatePlane
        /// </summary>
        /// <returns>Copied(created) element</returns>
        public override ShapeElement Copy()
        {
            return CloneUtil<ShapeElement>.DeepClone<ShapeElement>(this);
        }
        /// <summary>
        /// Select CoordinatePlane
        /// </summary>
        public override void Select()
        {
            this.undoShape = this.Copy();
        }
        /// <summary>
        /// Adss CoordinatePlane to graphic path
        /// </summary>
        /// <param name="graphicPath">Graphic path that CoordinatePlane will be added to</param>
        /// <param name="dx">X region </param>
        /// <param name="dy">Y region</param>
        /// <param name="zoom">Zoom value</param>
        public override void AddToGraphPath(GraphicsPath graphicPath, int dx, int dy, float zoom)
        {
            //Add vertical line
            graphicPath.AddLine((region.MidX + dx) * zoom, (region.Y0 + dy) * zoom,
                (region.MidX + dx) * zoom, (region.Y1 + dy) * zoom);
            //Add Horizontal line
            graphicPath.AddLine((region.X0 + dx) * zoom, (region.MidY + dy) * zoom,
                (region.X1 + dx) * zoom, (region.MidY + dy) * zoom);
        }
        /// <summary>
        /// Draw shape on the graphic object
        /// </summary>
        /// <param name="graphObj">Graphic object that will be drawn on</param>
        /// <param name="dx">X region on the graphic object</param>
        /// <param name="dy">Y region on the graphic object</param>
        /// <param name="zoom">Zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            Pen myPen = this.CreatePen(zoom);
            SolidBrush brush = new SolidBrush(this.PenColor);
            myPen.StartCap = LineCap.ArrowAnchor;
            myPen.EndCap = LineCap.ArrowAnchor;

            graphObj.DrawLine(myPen,(region.MidX + dx) * zoom, (region.Y0 + dy) * zoom,
                (region.MidX + dx) * zoom, (region.Y1 + dy) * zoom);
            graphObj.DrawLine(myPen,(region.X0 + dx) * zoom, (region.MidY + dy) * zoom,
                (region.X1 + dx) * zoom, (region.MidY + dy) * zoom);

            float midx = region.MidX;
            float midy = region.MidY;

            float xwidth = this.region.Width / (2 * this.verticalUnitCount);
            float ywidth = this.region.Height / (2 * this.horizontalUnitCount);

            //Draw Vertical Units
            for (int i = 0; i < this.verticalUnitCount; i++,  midx += xwidth)
                graphObj.DrawLine(myPen, (midx + dx)*zoom, (midy - 1 + dy) * zoom, 
                    (midx + dx)*zoom, (midy + 1 + dy) * zoom);

            midx = region.MidX;
            for (int i = 0; i < this.verticalUnitCount; i++, midx -= xwidth)
                graphObj.DrawLine(myPen, (midx + dx)*zoom,( midy - 1 +dy) * zoom, 
                    (midx + dx ) *zoom,( midy + 1 +dy) * zoom);

            //Draw Horizontal Units
            midx = region.MidX;
            for (int i = 0; i < this.horizontalUnitCount; i++, midy += ywidth)
                graphObj.DrawLine(myPen,( midx - 1 +dx)* zoom , (midy + dy)*zoom,
                    (midx + 1 + dx) * zoom, (dy + midy) * zoom);         
            midy = region.MidY;
            for (int i = 0; i < this.horizontalUnitCount; i++, midy -= ywidth)
                graphObj.DrawLine(myPen, (midx - 1 + dx)* zoom, (midy + dy) * zoom, 
                    (midx + 1 + dx) * zoom,( dy + midy) * zoom);


            if (generateCornerNames)
            {
                graphObj.DrawString("X", SystemFonts.CaptionFont, brush, (region.X1 + dx) * zoom + 5,
                    (region.MidY + dy) * zoom);
                graphObj.DrawString("Y", SystemFonts.CaptionFont, brush, (region.MidX + dx) * zoom,
                        (region.Y0 + dy) * zoom - 5);
                graphObj.DrawString("0", SystemFonts.CaptionFont, brush, (region.MidX + dx) * zoom + 5,
                     (region.MidY + dy) * zoom + 5);
            }
            myPen.Dispose(); //Release 
            brush.Dispose();
        }
        #endregion

    }
}
