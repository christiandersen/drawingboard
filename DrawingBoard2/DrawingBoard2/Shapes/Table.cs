using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Table shape that has columns and rows 
    /// </summary>
    [Serializable]
    public class Table : ShapeElement
    {
        #region Variables
        private int columnCount = 2;
        private int rowCount = 2;
        #endregion

        #region Properties
        /// <summary>
        /// Total row count of the shape
        /// </summary>
        [Description("Total row count of the shape")]
        public int RowCount
        {
            get { return this.rowCount; }
            set {
                if (value == 0)
                    this.rowCount = 1;
                this.rowCount = value; 
            }
        }
        /// <summary>
        /// Total column count of the shape
        /// </summary>
        [Description("Total column count of the shape")]
        public int ColumnCount
        {
            get { return this.columnCount; }
            set {
                if (value == 0)
                    this.columnCount = 1; 
                this.columnCount = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Create table shape that has columns and rows 
        /// </summary>
        public Table()
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
            //return this.CreateInstanceForCopy() as ShapeElement;
        }
        /// <summary>
        /// Select CoordinatePlane
        /// </summary>
        public override void Select()
        {
            this.undoShape = this.Copy();
        }
        /// <summary>
        /// Adds table to a graph path
        /// </summary>
        /// <param name="graphicPath">Graph path to be added</param>
        /// <param name="dx">X region</param>
        /// <param name="dy">Y region</param>
        /// <param name="zoom">Zoom value</param>
        public override void AddToGraphPath(GraphicsPath graphicPath, int dx, int dy, float zoom)
        {
            graphicPath.AddRectangle(region.GetRectangleF(dx, dy, zoom));

            float rowHeight = this.region.Height / this.rowCount;
            float colWidth = this.region.Width / this.columnCount;

            float x0 = region.X0;
            float y0 = region.Y0;
            float x1 = region.X1;
            float y1 = region.Y1;

            //Add rows
            for (int i = 0; i < this.rowCount; i++ , y0 += rowHeight)
                graphicPath.AddLine(x0, y0, x1, y0);

            //Add Columns
            y0 = region.Y0;
            for (int i = 0; i < this.columnCount; i++, x0 += colWidth)
                graphicPath.AddLine(x0, y0, x0, y1);
        }
        /// <summary>
        /// Draws table shape
        /// </summary>
        /// <param name="graphObj">Graphic object to be drawn on</param>
        /// <param name="dx">X shift value</param>
        /// <param name="dy">Y shift value</param>
        /// <param name="zoom">Zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            Pen myPen = this.CreatePen(zoom);
            Brush myBrush = GetBrush(dx, dy, zoom);

            graphObj.DrawRectangle(myPen, Rectangle.Round(region.GetRectangleF(dx, dy, zoom)));
            if (FillEnabled)
                graphObj.FillRectangle(myBrush, Rectangle.Round(region.GetRectangleF(dx, dy, zoom)));

            float rowHeight = this.region.Height / this.rowCount;
            float colWidth = this.region.Width / this.columnCount;

            float x0 = region.X0;
            float y0 = region.Y0;
            float x1 = region.X1;
            float y1 = region.Y1;

            //Add rows
            for (int i = 0; i < this.rowCount; i++, y0 += rowHeight)
                graphObj.DrawLine(myPen, DrawingUtils.GetZoomPointF(x0, y0, dx, dy, zoom),
                    DrawingUtils.GetZoomPointF(x1, y0, dx, dy, zoom));
            //Add Columns
            y0 = region.Y0;
            for (int i = 0; i < this.columnCount; i++, x0 += colWidth)
                graphObj.DrawLine(myPen, DrawingUtils.GetZoomPointF(x0, y0, dx, dy, zoom),
                    DrawingUtils.GetZoomPointF(x0, y1, dx, dy, zoom));

     
            myPen.Dispose();
        }
        #endregion
    }
}
