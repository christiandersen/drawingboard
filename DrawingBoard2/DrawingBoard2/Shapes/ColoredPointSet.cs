using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Set of colored points 
    /// </summary>
    [Serializable]
    public class ColoredPointSet : Polygon
    {
        #region Constructors
        /// <summary>
        /// Set of colored points
        /// </summary>
        public ColoredPointSet() 
        {
            this.selected = true;
            this.SetupSize();
            this.EndMoveRedim();
            this.rotation = 0;
            this.canRotate = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Draws object on the board(graphObj)
        /// </summary>
        /// <param name="graphObj">Graph Object(board) to drawn on</param>
        /// <param name="dx">X region on board</param>
        /// <param name="dy">Y region on board</param>
        /// <param name="zoom">Zoom value</param>
        public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {

            //myBrush.Color = this.Trasparency(this.fillColor, this.alpha);
            Pen myPen = this.CreatePen(zoom);

            // Create a path and add the object.
            GraphicsPath myPath = new GraphicsPath();

            // To ARRAY
            PointF[] myArr = new PointF[this.points.Count];
            Color[] myColorArr = new Color[this.points.Count];
            int i = 0;

            foreach (PointElement point in this.points)
            {
                myArr[i] = new PointF((point.X + region.X0 + dx) * zoom,
                    (point.Y + region.Y0 + dy) * zoom);
                if (point is ColoredPoint)
                    myColorArr[i++] = ((ColoredPoint)point).Color;
            }
            if (myArr.Length < 3 | !this.curved)
            {
                if (closed & myArr.Length >= 3)
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
            PathGradientBrush myBrush = new PathGradientBrush(myPath);
            myBrush.SurroundColors = myColorArr;
            myBrush.CenterColor = this.FillColor;

            Matrix translateMatrix = new Matrix();
            translateMatrix.RotateAt(this.Rotation,region.GetActualregion(dx,dy,zoom));
            myPath.Transform(translateMatrix);

            // Draw the transformed obj to the screen.
            graphObj.FillPath(myBrush, myPath);
            if (this.ShowBorder)
                graphObj.DrawPath(myPen, myPath);

            myPath.Dispose();
            myPen.Dispose();
            if (myBrush != null)
                myBrush.Dispose();
        }
        #endregion
    }
}
