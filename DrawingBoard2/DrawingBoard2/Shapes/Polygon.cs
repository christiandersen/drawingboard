using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using DrawingBoard2.Helpers;
using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Polygon shape
    /// </summary>
    [Serializable]
    public class Polygon : ShapeElement
    {
        #region Variables
        /// <summary>
        /// Points of polygon
        /// </summary>
        public List<PointElement> points = new List<PointElement>();
        /// <summary>
        /// Indicates whether polygon is curved or not
        /// </summary>
        protected bool curved = false;
        /// <summary>
        /// Indicates whether polygon is closed or not
        /// </summary>
        protected bool closed = false;
        /// <summary>
        /// Indicates whether corners of polygon are moveable or not
        /// </summary>
        protected bool fixedCorners = false;
        #endregion

        #region Properties
        /// <summary>
        /// Region of the polygon
        /// </summary>
        [Category("Layout"), Description("Region of the polygon")]
        public new virtual Region Region
        {
            set
            {
                base.Region = value;
            }
            get
            {
                return base.Region;
            }
        }
        /// <summary>
        /// Point element list of the 
        /// </summary>
        [Browsable(false)]
        public List<PointElement> Points
        {
            get { return this.points; }
            set { this.points = value; }
        }
        /// <summary>
        /// Indicates whether is it a curved shape or not
        /// </summary>
         [Category("Appearance"), Description("Indicates whether is it a curved shape or not")]
        public bool Curved
        {
            get { return this.curved; }
            set { this.curved = value; }
        }
        /// <summary>
        /// Indicates whether is it a closed shape or not
        /// </summary>
        [Category("Appearance"), Description("Indicates whether is it a closed shape or not")]
        public bool Closed
        {
            get { return this.closed; }
            set { this.closed = value; }
        }
        /// <summary>
        /// Indicates, whether corners of the polgon are fixed(immovable) or not
        /// </summary>
        [Category("Appearance"), Description("Indicates, whether corners of the polgon are fixed(immovable) or not")]
        public bool FixedCorners
        {
            get { return this.fixedCorners; }
            set { this.fixedCorners = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a polygon
        /// </summary>
        public Polygon()
        {            
            this.selected = true;
            this.EndMoveRedim();
            this.rotation = 0;
            this.canRotate = true;
            this.SetupSize();
            this.EndMoveRedim();
            this.rotation = 0;
            this.canRotate = true;
        }       
        #endregion

        #region Methods

        /// <summary>
        /// Sets size
        /// </summary>
        public void SetupSize()
        {
            if (this.points != null)
            {
                int maxX = 0;
                int maxY = 0;
                foreach (PointElement point in this.points)
                {
                    if (point.X > maxX)
                        maxX = point.X;
                    if (point.Y > maxY)
                        maxY = point.Y;
                }
                //region.Y1 = region.Y0 + maxY;
                //region.X1 = region.X0 + maxX;
                this.RePosition();
            }
        }
        /// <summary>
        /// Re-positions the point set
        /// </summary>
        private void RePosition()
        {
            if (points != null)
            {
                int minNegativeX = 0;
                int minNegativeY = 0;

                foreach (PointElement point in points)
                {
                    minNegativeX = point.X;
                    minNegativeY = point.Y;
                    break;
                }

                foreach (PointElement point in points)
                {
                    if (point.X < minNegativeX)
                        minNegativeX = point.X;
                    if (point.Y < minNegativeY)
                        minNegativeY = point.Y;
                }
                foreach (PointElement point in points)
                {
                    point.X -= minNegativeX; ;
                    point.Y -= minNegativeY; ;
                }
             //   region.X0 += minNegativeX;
               // region.Y0 += minNegativeY;
            }
        }
        /// <summary>
        /// Calculates and returns list of points with real regions(regions on board)
        /// </summary>
        /// <returns>List of points with real regions</returns>
        public List<PointElement> GetRealregionPoints()
        {
            List<PointElement> tempList = new List<PointElement>();
            foreach (PointElement point in points)
                tempList.Add(new PointElement(point.X + region.X0, point.Y + region.Y0));
            return tempList;
        }
        /// <summary>
        /// Overridden EndMoveRedim function
        /// </summary>
        public override void EndMoveRedim()
        {
            base.EndMoveRedim();
            foreach (PointElement point in points)
                point.EndZoom();
        }
        /// <summary>
        /// Over-ridden redimension method
        /// </summary>
        /// <param name="x">X value of redimension</param>
        /// <param name="y">Y value of redimension</param>
        /// <param name="direction">Direction of the redimension</param>
        public override void Redim(int x, int y, Direction direction)
        {
            base.Redim(x, y, direction);

            float dx = (float)(region.X1 - region.X0) / 
                (float)(this.oldregion.X1  - this.oldregion.X0);
            float dy = (float)(region.Y1 - region.Y0) /
                (float)(this.oldregion.Y1 - this.oldregion.Y0);

            foreach (PointElement point in points)
                point.Zoom(dx, dy);
        }
        /// <summary>
        /// Decided whether point(x,y) is contained in the set
        /// </summary>
        /// <param name="x">X coordinate of the point to be checked</param>
        /// <param name="y">Y coordinate of the point to be checked</param>
        /// <returns>True if contains,false if not</returns>
        public override bool Contains(int x, int y)
        {
            int minX = region.X0;
            int minY = region.Y0;
            int maxX = region.X1;
            int maxY = region.Y1;

            foreach (PointElement point in points)
            {
                if (minX > region.X0 + point.X)
                    minX = region.X0 + point.X;
                if (minY > region.Y0 + point.Y)
                    minY = region.Y0 + point.Y;
                if (maxX < region.X0 + point.X)
                    maxX = region.X0 + point.X;
                if (maxY < region.Y0 + point.Y)
                    maxY = region.Y0 + point.Y;
            }
            return new Rectangle(minX, minY, maxX - minX, maxY - minY).Contains(x, y);
        }
        /// <summary>
        /// Over-ridden FitToGrid function
        /// </summary>
        /// <param name="gridsize">size of the grid</param>
        public override void FitTogrid(int gridsize)
        {
            base.FitTogrid(gridsize);

            foreach (PointElement point in this.points)
            {
                point.X = gridsize * (int)(point.X / gridsize);
                point.Y = gridsize * (int)(point.Y / gridsize);
            }
        }
        /// <summary>
        /// Over-riden commit rotate function. 
        /// Invoked when rotation is confirmed/Completed
        /// </summary>
        public void CommitRotate()
        {
            if (this.Rotation > 0)
            {
                //CENTER POINT
                foreach (PointElement point in points)
                    point.RotateAt(this.region.MidX, this.region.MidY, this.Rotation);
                this.rotation = 0;
            }
        }
        /// <summary>
        /// Apply mirror
        /// </summary>
        /// <param name="xmirror">Shape is mirrored at x axis or not</param>
        /// <param name="ymirror">Shape is mirrored at y coordinate or not</param>
        public void CommitMirror(bool xmirror, bool ymirror)
        {
            foreach (PointElement point in points)
            {
                if (xmirror)
                    point.XMirror(this.region.Width);
                if (ymirror)
                    point.YMirror(this.region.Height);
            }
            SetupSize();
        }
        /// <summary>
        /// Deselects each point
        /// </summary>
        public override void DeSelect()
        {
            foreach (PointElement point in points)
                point.Selected = false;
        }
        /// <summary>
        /// Selects element
        /// </summary>
        /// <param name="region">Selected region</param>
        public override void Select(Region region)
        {
            Rectangle rect = new Rectangle(region.X0, region.Y0, region.Width, region.Height);

            foreach (PointElement point in points)
            {
                point.Selected = false;
                if (rect.Contains(new Point(point.X + region.X0, point.Y + region.Y0)))
                   point.Selected = true;
            }
        }
        /// <summary>
        /// Creates a new instance for deep copy
        /// </summary>
        /// <returns></returns>
        public override ShapeElement Copy()
        {
            return CloneUtil<ShapeElement>.DeepClone<ShapeElement>(this);
        }
        /// <summary>
        /// Select shape, copies itself to undo shape
        /// </summary>
        public override void Select()
        {
            this.undoShape = this.Copy();
        }
        /// <summary>
        /// Adds points to a graph path
        /// </summary>
        /// <param name="graphPath">Graph path to be added</param>
        /// <param name="dx">X region</param>
        /// <param name="dy">Y region</param>
        /// <param name="zoom">Zoom value</param>
        public override void AddToGraphPath(GraphicsPath graphPath, int dx, int dy, float zoom)
        {
            // To ARRAY
            PointF[] temPointArr = new PointF[this.points.Count];
            int i = 0;

            foreach (PointElement point in this.points)
                temPointArr[i++] = new PointF((point.X + region.X0 + dx) * zoom, 
                    (point.Y + region.Y0 + dy) * zoom);// p.point;

            if (i < 2)
                graphPath.AddLines(temPointArr);
            else
                if (this.curved)
                    graphPath.AddCurve(temPointArr);
                else
                    graphPath.AddPolygon(temPointArr);
        }
        /// <summary>
        /// Draws point set on the board(graphObj)
        /// </summary>
        /// <param name="graphObj">Graph Object(board) to drawn on</param>
        /// <param name="dx">X region on board</param>
        /// <param name="dy">Y region on board</param>
        /// <param name="zoom">Zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            Brush myBrush = GetBrush(dx, dy, zoom);
            Pen myPen = this.CreatePen(zoom);

            // Create a path and add the object.
            GraphicsPath myPath = new GraphicsPath();

            // To ARRAY
            PointF[] myArr = new PointF[this.points.Count];
            int i = 0;

            foreach (PointElement p in this.points)
                myArr[i++] = new PointF((p.X + region.X0 + dx) * zoom, 
                    (p.Y + region.Y0 + dy) * zoom);

            if (myArr.Length < 3 | !this.curved)
            {
                if (closed && myArr.Length >= 3)
                    myPath.AddPolygon(myArr);
                else
                    myPath.AddLines(myArr);
            }
            else
            {
                if (closed)
                    myPath.AddClosedCurve(myArr);
                else
                    myPath.AddCurve(myArr);
            }

            if (this.generateCornerNames && this.closed)
            {
                char letter = 'A';
                foreach (PointF point in myArr)
                {
                    myPath.AddString(letter.ToString(), SystemFonts.CaptionFont.FontFamily,
                        (int)SystemFonts.CaptionFont.Style, 12,
                        PolygonHelper.CalculateCornerLetterPoint(point,myArr),
                        StringFormat.GenericDefault);
                    letter++;
                }
            }
            Matrix translateMatrix = new Matrix();
            translateMatrix.RotateAt(this.Rotation, region.GetActualregion(dx,dy,zoom));
            myPath.Transform(translateMatrix);

            if (this.FillEnabled)
            {
                if (!(this is Cube))
                    graphObj.FillPath(myBrush, myPath);
                if (this.ShowBorder)
                    graphObj.DrawPath(myPen, myPath);
            }
            else
                graphObj.DrawPath(myPen, myPath);

            myPath.Dispose();
            myPen.Dispose();
            if (myBrush != null)
                myBrush.Dispose();
        }
        #endregion 
    }
}
