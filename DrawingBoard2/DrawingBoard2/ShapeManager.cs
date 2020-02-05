using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using DrawingBoard2.Utils;
using DrawingBoard2.Shapes;
using DrawingBoard2.Handlers;

namespace DrawingBoard2
{
    /// <summary>
    /// Shape manager that manages shapes and handlers
    /// </summary>
    public sealed class ShapeManager : IDisposable
    {
        #region Variables
        private List<ShapeElement> shapes = new List<ShapeElement>();
        private BaseHandlerCollection handlerCol;
        private ShapeElement selectedElement;
        private UndoManager<UndoElement> undoManager = new UndoManager<UndoElement>(30); //Create undo/redo buffer with 30 object size
        private const int MinimumSize = 10;
        #endregion

        #region Properties
        /// <summary>
        /// Total count of shapes in the chape list 
        /// </summary>
        public int ShapeCount
        {
            get { return this.shapes.Count; }
        }
        /// <summary>
        /// Minimum X coordinate value of the shapes
        /// </summary>
        public int MinX
        {
            get
            {
                return (from s in this.shapes
                        select s.Region.X0).Min();
            }
        }
        /// <summary>
        /// Maximum X coordinate value of the shapes
        /// </summary>
        public int MaxX
        {
            get
            {
                 return   (from s in this.shapes
                           select s.PosEndX).Max();
            }
        }
        /// <summary>
        /// Minimum Y coordinate value of the shapes
        /// </summary>
        public int MinY
        {
            get
            {
                return (from s in this.shapes
                        select s.Region.Y0).Min();
            }
        }
        /// <summary>
        /// Maximum Y coordinate value of the shapes
        /// </summary>
        public int MaxY
        {
            get
            {
                return  (from s in this.shapes
                         select s.PosEndY).Max();
            }
        }
        /// <summary>
        /// Collection of current handlers
        /// </summary>
        public BaseHandlerCollection HanderCollection
        {
            set { this.handlerCol = value; }
            get { return this.handlerCol; }
        }
        /// <summary>
        /// Selected shape
        /// </summary>
        public ShapeElement SelectedElement
        {
            get { return this.selectedElement; }
            set { this.selectedElement = value; }
        }
        /// <summary>
        /// Can be undoable
        /// </summary>
        public bool UndoEnabled
        {
            get { return this.undoManager.IsUndoable; }
        }
        /// <summary>
        /// Can be redoable
        /// </summary>
        public bool RedoEnabled
        {
            get { return this.undoManager.IsRedoable; }
        }
        #endregion

        #region Move Methods
        /// <summary>
        /// Moves selected shapes by dx,dy
        /// </summary>
        /// <param name="dx">Value to move at x coordinate</param>
        /// <param name="dy">Value to mvoe at y coordinate</param>
        public void Move(int dx, int dy)
        {
            foreach (ShapeElement shape in this.shapes)
                if (shape.Selected)
                    shape.Move(dx, dy);
        }
        /// <summary>
        /// Moves selected element to left
        /// </summary>
        public void MoveLeft()
        {
            if (this.handlerCol != null)
                handlerCol.MoveLeft();
            if (this.selectedElement != null)
            {
                this.StoreInBuffer(BufferOperation.Update, this.selectedElement);
                selectedElement.MoveLeft();
            }
        }
        /// <summary>
        /// Moves seleceted element to right
        /// </summary>
        public void MoveRight()
        {
            if (this.handlerCol != null)
                handlerCol.MoveRight();
            if (this.selectedElement != null)
            {
                this.StoreInBuffer(BufferOperation.Update, this.selectedElement);
                selectedElement.MoveRight();
            }
        }
        /// <summary>
        /// Moves selected element to up
        /// </summary>
        public void MoveUp()
        {
            if (this.handlerCol != null)
                handlerCol.MoveUp();
            if (this.selectedElement != null)
            {
                this.StoreInBuffer(BufferOperation.Update, this.selectedElement);
                selectedElement.MoveUp();
            }
        }
        /// <summary>
        /// Moves selected element to down
        /// </summary>
        public void MoveDown()
        {
            if (this.handlerCol != null)
                handlerCol.MoveDown();
            if (this.selectedElement != null)
            {
                this.StoreInBuffer(BufferOperation.Update, this.selectedElement);
                selectedElement.MoveDown();
            }
        }
        #endregion

        #region Add Shape Methods
        /// <summary>
        /// Adds trapezoid shape in shape list
        /// </summary>
        /// <param name="region">region of the trapezoid on the board</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is trapezoid filled</param>
        public void AddTrapezoid(Region region, Color penColor, Color fillColor, float penWidth, bool isFilled)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();

            Trapezoid trapezoid = new Trapezoid();
            trapezoid.Region = region;
            trapezoid.PenColor = penColor;
            trapezoid.PenWidth = penWidth;
            trapezoid.FillColor = fillColor;
            trapezoid.FillEnabled = isFilled;

            this.shapes.Add(trapezoid);
            StoreInBuffer(BufferOperation.Insert, trapezoid);
            handlerCol = new ShapeHandlerCollection(trapezoid);
            this.selectedElement = trapezoid;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds star to shape collection
        /// </summary>
        /// <param name="region">region of the star on the board</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is star filled</param>
        public void AddStar(Region region, Color penColor, Color fillColor, float penWidth, bool isFilled)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            Star star = new Star();
            star.Region = region;
            star.PenColor = penColor;
            star.PenWidth = penWidth;
            star.FillColor = fillColor;
            star.FillEnabled = isFilled;

            this.shapes.Add(star);
            StoreInBuffer(BufferOperation.Insert, star);
            handlerCol = new ShapeHandlerCollection(star);
            this.selectedElement = star;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds hexagon to shape collection
        /// </summary>
        /// <param name="region">region of the hexagon on the board</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is hexagon filled</param>
        public void AddHexagon(Region region, Color penColor, Color fillColor, float penWidth, bool isFilled)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            Hexagon hexagon = new Hexagon();
            hexagon.Region = region;
            hexagon.PenColor = penColor;
            hexagon.PenWidth = penWidth;
            hexagon.FillColor = fillColor;
            hexagon.FillEnabled = isFilled;

            this.shapes.Add(hexagon);
            StoreInBuffer(BufferOperation.Insert, hexagon);
            handlerCol = new ShapeHandlerCollection(hexagon);
            this.selectedElement = hexagon;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds a regular multi-edged polygon on board
        /// </summary>
        /// <param name="region">region of the polygon</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is pentagon filled</param>
        /// <param name="points">Points of the polygon</param>
        public void AddRegularPolygon(Region region, Color penColor, Color fillColor,
            float penWidth, bool isFilled, List<PointF> points)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            Polygon regularPolygon = new Polygon();
            regularPolygon.Region = region;
            regularPolygon.PenColor = penColor;
            regularPolygon.PenWidth = penWidth;
            regularPolygon.FillColor = fillColor;
            regularPolygon.FillEnabled = isFilled;
            regularPolygon.Curved = false;
            regularPolygon.Closed = true;
            regularPolygon.FixedCorners = true;

            List<PointElement> pointElementList = new List<PointElement>();

            foreach (PointF point in points)
                pointElementList.Add(new PointElement(point));
            regularPolygon.points = pointElementList;
            this.shapes.Add(regularPolygon);
            StoreInBuffer(BufferOperation.Insert, regularPolygon);
            handlerCol = new ShapeHandlerCollection(regularPolygon);
            this.selectedElement = regularPolygon;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds a cube to shape collection
        /// </summary>
        /// <param name="region">region of the Cube</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is Cube filled</param>
        public void AddCube(Region region, Color penColor, Color fillColor, float penWidth, bool isFilled)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            Cube cube = new Cube();
            cube.Region = region;
            cube.PenColor = penColor;
            cube.PenWidth = penWidth;
            cube.FillColor = fillColor;
            cube.FillEnabled = isFilled;

            this.shapes.Add(cube);
            StoreInBuffer(BufferOperation.Insert, cube);
            handlerCol = new ShapeHandlerCollection(cube);
            this.selectedElement = cube;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds pentagon to shape collection
        /// </summary>
        /// <param name="region">region of the pentagon on the board</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is pentagon filled</param>
        public void AddPentagon(Region region, Color penColor, Color fillColor, float penWidth, bool isFilled)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            Pentagon pentagon = new Pentagon();
            pentagon.Region = region;
            pentagon.PenColor = penColor;
            pentagon.PenWidth = penWidth;
            pentagon.FillColor = fillColor;
            pentagon.FillEnabled = isFilled;

            this.shapes.Add(pentagon);
            StoreInBuffer(BufferOperation.Insert, pentagon);
            handlerCol = new ShapeHandlerCollection(pentagon);
            this.selectedElement = pentagon;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds triangle to shape collection
        /// </summary>
        /// <param name="region">region of the triangle on the board</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is triangle filled</param>
        /// <param name="triangleType">Type of triangle( right or Equilateral)</param>
        public void AddTriangle(Region region, Color penColor, Color fillColor, float penWidth,
            bool isFilled, TriangleType triangleType)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            Triangle triangle = new Triangle();
            triangle.TriangleType = triangleType;
            triangle.Region = region;
            triangle.PenColor = penColor;
            triangle.PenWidth = penWidth;
            triangle.FillColor = fillColor;
            triangle.FillEnabled = isFilled;

            this.shapes.Add(triangle);
            StoreInBuffer(BufferOperation.Insert, triangle);
            handlerCol = new ShapeHandlerCollection(triangle);
            this.selectedElement = triangle;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds polygon to shape collection
        /// </summary>
        /// <param name="region">region of the polygon</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="filled">Is polygon filled</param>
        /// <param name="points">List of points of the polygon</param>
        /// <param name="isCurved">Is polygon curved</param>
        public void AddPointSet(Region region, Color penColor, Color fillColor, float penWidth, bool filled,
            List<PointElement> points, bool isCurved)
        {
            this.DeSelect();
            Polygon pointSet = new Polygon();
            pointSet.Region = region;
            pointSet.points = points;
            pointSet.PenColor = penColor;
            pointSet.PenWidth = penWidth;
            pointSet.FillColor = fillColor;
            pointSet.FillEnabled = filled;
            pointSet.Curved = isCurved;

            this.shapes.Add(pointSet);
            StoreInBuffer(BufferOperation.Insert, pointSet);
            handlerCol = new PolygonHandlerCollection(pointSet);
            this.selectedElement = pointSet;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds colored point set to shape collection
        /// </summary>
        /// <param name="region">region of the colored point set</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="filled">Is polygon filled</param>
        /// <param name="points">List of points of the polygon</param>
        /// <param name="isCurved">Is polygon curved</param>
        public void AddColoredPointSet(Region region, Color penColor, Color fillColor, float penWidth, bool filled,
            List<PointElement> points, bool isCurved)
        {
            this.DeSelect();
            ColoredPointSet colPointSet = new ColoredPointSet();
            colPointSet.Region = region;
            colPointSet.points = points;
            colPointSet.PenColor = penColor;
            colPointSet.PenWidth = penWidth;
            colPointSet.FillColor = fillColor;
            colPointSet.FillEnabled = filled;
            colPointSet.Curved = isCurved;

            this.shapes.Add(colPointSet);
            StoreInBuffer(BufferOperation.Insert, colPointSet);
            handlerCol = new PolygonHandlerCollection(colPointSet);
            this.selectedElement = colPointSet;
            this.selectedElement.Select();

        }
        /// <summary>
        /// Adds table to shape collection
        /// </summary>
        /// <param name="region">region of the table</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is table filled</param>
        public void AddTable(Region region, Color penColor, Color fillColor, float penWidth, bool isFilled)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            Table table = new Table();
            table.Region = region;
            table.PenColor = penColor;
            table.PenWidth = penWidth;
            table.FillColor = fillColor;
            table.FillEnabled = isFilled;

            this.shapes.Add(table);
            StoreInBuffer(BufferOperation.Insert, table);
            handlerCol = new ShapeHandlerCollection(table);
            this.selectedElement = table;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds rectangle to shape collection
        /// </summary>
        /// <param name="region">region of the rectangle</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is rectangle filled</param>
        public void AddRectangle(Region region, Color penColor, Color fillColor, float penWidth, bool isFilled)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            Rect rectangle = new Rect();
            rectangle.Region = region;
            rectangle.PenColor = penColor;
            rectangle.PenWidth = penWidth;
            rectangle.FillColor = fillColor;
            rectangle.FillEnabled = isFilled;

            this.shapes.Add(rectangle);
            StoreInBuffer(BufferOperation.Insert, rectangle);
            handlerCol = new ShapeHandlerCollection(rectangle);
            this.selectedElement = rectangle;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds pie to shape collection
        /// </summary>
        /// <param name="region">region of the pie</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is pie filled</param>
        public void AddPie(Region region, Color penColor, Color fillColor, float penWidth, bool isFilled)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            Pie pie = new Pie();
            pie.Region = region;
            pie.PenColor = penColor;
            pie.PenWidth = penWidth;
            pie.FillColor = fillColor;
            pie.FillEnabled = isFilled;

            this.shapes.Add(pie);
            StoreInBuffer(BufferOperation.Insert, pie);
            handlerCol = new ShapeHandlerCollection(pie);
            this.selectedElement = pie;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds arc to shape collection
        /// </summary>
        /// <param name="region">region of the arc</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is arc filled</param>
        public void AddArc(Region region, Color penColor, Color fillColor, float penWidth, bool isFilled)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            Arc arc = new Arc();
            arc.Region = region;
            arc.PenColor = penColor;
            arc.PenWidth = penWidth;
            arc.FillColor = fillColor;
            arc.FillEnabled = isFilled;

            this.shapes.Add(arc);
            StoreInBuffer(BufferOperation.Insert, arc);
            handlerCol = new ShapeHandlerCollection(arc);
            this.selectedElement = arc;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds <see href="http://en.wikipedia.org/wiki/Cartesian_coordinate_system">
        /// Cartesian Plane</see> to shape collection
        /// </summary>
        /// <param name="region">region of the arc</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        public void AddCartesianPlane(Region region, Color penColor, Color fillColor, float penWidth)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();

            CartesianPlane corPlane = new CartesianPlane();
            corPlane.Region = region;
            corPlane.PenColor = penColor;
            corPlane.PenWidth = penWidth;
            corPlane.FillColor = fillColor;

            this.shapes.Add(corPlane);
            StoreInBuffer(BufferOperation.Insert, corPlane);
            handlerCol = new ShapeHandlerCollection(corPlane);
            this.selectedElement = corPlane;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds line to shape collection
        /// </summary>
        /// <param name="region">region of the line</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        public void AddLine(Region region, Color penColor, Color fillColor, float penWidth)
        {
            this.DeSelect();
            Line line = new Line();
            line.Region = region;
            line.PenColor = penColor;
            line.PenWidth = penWidth;
            line.FillColor = fillColor;

            this.shapes.Add(line);
            StoreInBuffer(BufferOperation.Insert, line);
            handlerCol = new ShapeHandlerCollection(line);
            this.selectedElement = line;
            this.selectedElement.Select();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="region">region of the  text box </param>
        /// <param name="richTb">RichTextbox to be added</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is  text box  filled</param>
        public void AddRichTextBox(Region region, RichTextBox richTb, Color penColor,
            Color fillColor, float penWidth, bool isFilled)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            RichTextBoxShape rtbShape = new RichTextBoxShape();
            rtbShape.Region = region;
            rtbShape.PenWidth = penWidth;
            rtbShape.FillColor = fillColor;
            rtbShape.PenColor = penColor;
            rtbShape.FillEnabled = isFilled;
            rtbShape.RTF = richTb.Rtf;
            this.shapes.Add(rtbShape);

            // StoreInBuffer(BufferOperation.Insert, rtbShape);
            handlerCol = new ShapeHandlerCollection(rtbShape);
            this.selectedElement = rtbShape;
            //this.selectedElement.Select();
        }

        /// <summary>
        /// Adds simple text box to shape collection
        /// </summary>
        /// <param name="region">region of the  text box </param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is  text box  filled</param>
        /// <param name="textBox">TextBox that contains data of textbox</param>
        public void AddSimpleTextBox(Region region, TextBox textBox, Color penColor,
            Color fillColor, float penWidth, bool isFilled)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            SimpleTextBox stextbox = new SimpleTextBox();
            stextbox.Region = region;
            stextbox.Text = textBox.Text;
            stextbox.Font = textBox.Font;
            stextbox.PenColor = penColor;
            stextbox.PenWidth = penWidth;
            stextbox.FillEnabled = isFilled;
            stextbox.FillColor = fillColor;

            this.shapes.Add(stextbox);
            StoreInBuffer(BufferOperation.Insert, stextbox);
            handlerCol = new ShapeHandlerCollection(stextbox);
            this.selectedElement = stextbox;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds rounded rectangle to shape collection
        /// </summary>
        /// <param name="region">region of the rectangle</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is rectangle filled</param>
        public void AddRoundedRectangle(Region region, Color penColor, Color fillColor, float penWidth, bool isFilled)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            RoundedRect rectangle = new RoundedRect();
            rectangle.Region = region;
            rectangle.PenColor = penColor;
            rectangle.PenWidth = penWidth;
            rectangle.FillColor = fillColor;
            rectangle.FillEnabled = isFilled;

            this.shapes.Add(rectangle);
            StoreInBuffer(BufferOperation.Insert, rectangle);
            handlerCol = new ShapeHandlerCollection(rectangle);
            this.selectedElement = rectangle;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds ellipse to shape collection
        /// </summary>
        /// <param name="region">region of the ellipse</param>
        /// <param name="penColor">Color of the pen</param>
        /// <param name="fillColor">Fill color</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="isFilled">Is ellipse filled</param>
        public void AddEllipse(Region region, Color penColor, Color fillColor, float penWidth, bool isFilled)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            Ellipse ellipse = new Ellipse();
            ellipse.Region = region;//new Ellipse(region);
            ellipse.PenColor = penColor;
            ellipse.PenWidth = penWidth;
            ellipse.FillColor = fillColor;
            ellipse.FillEnabled = isFilled;

            this.shapes.Add(ellipse);
            StoreInBuffer(BufferOperation.Insert, ellipse);
            handlerCol = new ShapeHandlerCollection(ellipse);
            this.selectedElement = ellipse;
            this.selectedElement.Select();
        }
        /// <summary>
        /// Adds image box to shape collection .
        /// Loads image from file
        /// </summary>
        /// <param name="region">region of the imagebox</param>
        /// <param name="image">Image of the image box</param>
        /// <param name="penWidth">Width of the pen</param>
        /// <param name="penColor">Color of the pen</param>
        public void AddImageBox(Region region, Bitmap image, Color penColor, float penWidth)
        {
            region.FixSize(MinimumSize);
            this.DeSelect();
            ImageBox imageBox = new ImageBox();
            imageBox.Region = region;
            imageBox.PenColor = penColor;
            imageBox.PenWidth = penWidth;

            this.shapes.Add(imageBox);
            StoreInBuffer(BufferOperation.Insert, imageBox);
            imageBox.Image = image;
            handlerCol = new ShapeHandlerCollection(imageBox);
            this.selectedElement = imageBox;
            this.selectedElement.Select();
        }
        #endregion

        #region Other Methods
        /// <summary>
        /// Copies all selected elements
        /// </summary>
        /// <param name="dx">Moves selected element by dx</param>
        /// <param name="dy">Moves selected element by dy</param>
        public void CopyMultiSelected(int dx, int dy)
        {
            List<ShapeElement> tempList = new List<ShapeElement>();

            foreach (ShapeElement shape in this.shapes)
            {
                if (shape.Selected)
                {
                    ShapeElement temp = shape.Copy();
                    temp.Selected = false;
                    temp.Move(dx, dy);
                    tempList.Add(temp);

                    this.handlerCol = new BaseHandlerCollection(temp);
                    selectedElement = temp;
                    selectedElement.EndMoveRedim();
                }
            }
            foreach (ShapeElement temp in tempList)
            {
                this.shapes.Add(temp);
                StoreInBuffer(BufferOperation.Insert, temp);
            }
        }
        /// <summary>
        /// Removes selected elements
        /// </summary>
        public void RemoveSelected()
        {
            ShapeElement[] selectedShapes = (from ShapeElement element in this.shapes
                                             where element.Selected
                                             select element).ToArray();

            if (this.selectedElement != null)
            {
                this.selectedElement = null;
                this.handlerCol = null;
            }

            foreach (ShapeElement shape in selectedShapes)
            {
                StoreInBuffer(BufferOperation.Delete, shape);
                shapes.Remove(shape);
            }
        }
        /// <summary>
        /// Moves points
        /// </summary>
        /// <param name="dx">Movement amount at x coordinate</param>
        /// <param name="dy">Movement amount at y coordinate</param>
        public void MovePoint(int dx, int dy)
        {

            Polygon polygon = this.selectedElement as Polygon;

            if (!polygon.FixedCorners)
            {
                (this.handlerCol as PolygonHandlerCollection).MovePoints(dx, dy);
                (this.handlerCol as PolygonHandlerCollection).ReCreateCreationHandlers(polygon);
            }
        }
        /// <summary>
        /// Adds new point element in polygon
        /// </summary>
        public void AddPoint()
        {
            PolygonHandlerCollection phCol = (this.handlerCol as PolygonHandlerCollection);

            if (phCol != null)
            {
                PointElement point = phCol.GetNewPoint();
                int index = phCol.GetNewPointIndex();

                if (index > 0)
                {
                    (this.selectedElement as Polygon).points.Insert(index - 1, point);
                    this.handlerCol = new PolygonHandlerCollection(this.selectedElement);
                }
            }
        }
         /// <summary>
        /// Adapts shape at grid  
        /// </summary>
        /// <param name="gridSize">Size of the grid to adapt</param>
        public void FitToGrid(int gridSize)
        {
            foreach (ShapeElement shape in this.shapes)
                if (shape.Selected)
                    shape.FitTogrid(gridSize);
        }
        /// <summary>
        /// Invoked when movement is ended.Moves all selected shape element at correct region
        /// </summary>
        public void EndMove()
        {
            foreach (ShapeElement element in this.shapes)
                if (element.Selected)
                    element.EndMoveRedim();
        }
        /// <summary>
        /// Stores element in the undo/redo buffer 
        /// </summary>
        /// <param name="operation">Operation that will be save for the element</param>
        /// <param name="element">Element to be buffered</param>
        public void StoreInBuffer(BufferOperation operation, ShapeElement element)
        {
            UndoElement UndoElement = new UndoElement(element, operation);
            this.undoManager.AddItem(UndoElement);
            element.UndoShape = element.Copy();
        }
        /// <summary>
        /// Undo last action
        /// </summary>
        public void Undo()
        {
            UndoElement element = this.undoManager.Undo();

            if (element == null)
                return;

            switch (element.BufferOperation)
            {
                case BufferOperation.Update:
                    element.CurrentShape = element.OldShape.Copy();
                    break;
                case BufferOperation.Insert:
                    this.shapes.Remove(element.CurrentShape);
                    break;
                case BufferOperation.Delete:
                    this.shapes.Add(element.CurrentShape);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Redos the action
        /// </summary>
        public void Redo()
        {
            UndoElement element = this.undoManager.Redo();

            if (element == null)
                return;

            switch (element.BufferOperation)
            {
                case BufferOperation.Update:
                    element.CurrentShape = element.NewShape.Copy();
                    break;
                case BufferOperation.Insert:
                    this.shapes.Add(element.CurrentShape);
                    break;
                case BufferOperation.Delete:
                    this.shapes.Remove(element.CurrentShape);
                    break;
            }
        }
        /// <summary>
        /// Returns list of selected shapes
        /// </summary>
        /// <returns></returns>
        public ShapeElement[] GetSelectedShapes()
        {
            return (from shape in shapes
                    where shape.Selected
                    select shape).ToArray();
        }
        /// <summary>
        /// Sets shape element list 
        /// </summary>
        /// <param name="shapeList">List to be set</param>
        public void SetList(List<ShapeElement> shapeList)
        {
            this.shapes.Clear();
            this.shapes.AddRange(shapeList.ToArray());
        }
        /// <summary>
        /// Brings selected shape to front
        /// </summary>
        public void BringToFront()
        {
            if (this.selectedElement != null)
            {
                this.shapes.Remove(selectedElement);
                this.shapes.Add(selectedElement);
            }
        }
        /// <summary>
        /// Sends selected shape element to back
        /// </summary>
        public void SendToBack()
        {
            if (this.selectedElement != null)
            {
                this.shapes.Remove(selectedElement);
                this.shapes.Insert(0, selectedElement);
                this.DeSelect();
            }
        }
        /// <summary>
        /// Deselects element
        /// </summary>
        public void DeSelect()
        {
            shapes.ForEach(shape => shape.Selected = false);
            this.selectedElement = null;
            this.handlerCol = null;
        }
        /// <summary>
        /// Draws all unselected shapes on board(graphic object)
        /// </summary>
        /// <param name="graphObj">Graphic object to draw on</param>
        /// <param name="dx">X location</param>
        /// <param name="dy">Y location</param>
        /// <param name="zoom">Zoom value</param>
        /// /// <param name="export">Shows that drawing graphic object is drawn on board, or will be exported as Bitmap</param>
        public void DrawUnselectedShapes(Graphics graphObj, int dx, int dy, float zoom, bool export)
        {
            graphObj.PageScale = 10;

            foreach (ShapeElement shape in this.shapes)
                if (!shape.Selected)
                    shape.Draw(graphObj, dx, dy, zoom);
        }
        /// <summary>
        /// Draws selected shapes on board(graphic object)
        /// </summary>
        /// <param name="graphObj">Graphic object to draw on</param>
        /// <param name="dx">X location</param>
        /// <param name="dy">Y location</param>
        /// <param name="zoom">Zoom value</param>
        /// <param name="export">Shows that drawing graphic object is drawn on board, or will be exported as Bitmap</param>
        public void DrawSelectedShapes(Graphics graphObj, int dx, int dy, float zoom, bool export)
        {
            bool anySelected = false;

            foreach (ShapeElement shape in this.shapes)
            {
                if (shape.Selected)
                {
                    shape.Draw(graphObj, dx, dy, zoom);
                    anySelected = true;
                }
            }
            if (anySelected && handlerCol != null)
                handlerCol.Draw(graphObj, dx, dy, zoom);
        }
        /// <summary>
        /// Selects last shape containing point(x,y)
        /// </summary>
        /// <param name="x">X region on coordinate system</param>
        /// <param name="y">Y region on coordinate system</param>
        public void Click(int x, int y)
        {
            handlerCol = null;
            selectedElement = null;

            foreach (ShapeElement element in this.shapes)
            {
                element.Selected = false;
                element.DeSelect();

                if (element.Contains(x, y))
                    selectedElement = element;
            }
            if (selectedElement != null)
            {
                selectedElement.Selected = true;
                selectedElement.Select();

                if (selectedElement is Polygon)
                    handlerCol = new PolygonHandlerCollection(selectedElement);
                else
                    handlerCol = new ShapeHandlerCollection(selectedElement);
            }
        }
        /// <summary>
        /// Merges points of all pointsets(polygons)
        /// </summary>
        public void MergePolygons()
        {
            bool first = true;
            List<PointElement> tempPointList = new List<PointElement>();
            List<Polygon> tempPolygonList = new List<Polygon>();
            Polygon tempPointSet = null;

            foreach (ShapeElement element in this.shapes)
            {
                if (element.Selected & element is Polygon)
                {
                    Polygon pointSet = element as Polygon;

                    if (first)
                    {
                        first = false;
                        tempPointSet = pointSet;
                    }
                    tempPolygonList.Add(pointSet);
                    tempPointList.AddRange(pointSet.GetRealregionPoints().ToArray());
                }
            }
            if (tempPolygonList.Count > 1)
            {
                foreach (ShapeElement element in tempPolygonList)
                {
                    this.shapes.Remove(element);
                }
                this.AddPointSet(new Region(0, 0, tempPointSet.PosEndX, tempPointSet.PosEndY), tempPointSet.PenColor,
                    tempPointSet.FillColor, tempPointSet.PenWidth, tempPointSet.FillEnabled,
                    tempPointList, false);
            }
        }
        /// <summary>
        /// Select all shapes in an input rectangle
        /// </summary>
        /// <param name="region">region of the rectangle</param>
        public void MultiSelect(Region region)
        {
            handlerCol = null;
            selectedElement = null;

            foreach (ShapeElement element in this.shapes)
            {
                element.Selected = false; //Set not selected (if its in region region, it'll be selected)
                element.DeSelect(); // Deselect

                int x0 = Math.Min(element.PosStartX, element.PosEndX);
                int x1 = Math.Max(element.PosStartX, element.PosEndX);
                int y0 = Math.Min(element.PosStartY, element.PosEndY);
                int y1 = Math.Max(element.PosStartY, element.PosEndY);

                if (x0 <= region.X1 & x1 >= region.X0 &
                    y0 <= region.Y1 & y1 > region.Y0) //If in region
                {
                    selectedElement = element; //Set it as selected
                    element.Selected = true;
                    element.Select();
                    //    element.Select(richTxtBox);
                    element.Select(region);
                }

                if (selectedElement != null)
                {
                    if (selectedElement is Polygon)
                        handlerCol = new PolygonHandlerCollection(selectedElement);
                    else
                        handlerCol = new ShapeHandlerCollection(selectedElement);
                }
            }
        }
        /// <summary>
        /// Clears(Removes) all shapes and format
        /// </summary>
        public void Clear()
        {
            this.shapes.Clear();
            undoManager.ResetBuffer();

            if (this.handlerCol != null)
                this.handlerCol.Clear();
        }
        /// <summary>
        /// Unselects selected elements
        /// </summary>
        public void UnselectElements()
        {
            foreach (ShapeElement shape in this.shapes)
                if (shape.Selected)
                    shape.Selected = false;

            if (this.handlerCol != null)
                this.handlerCol.Clear();
        }
        #endregion

        #region Serialization Methods
        /// <summary>
        /// Serializes selected shape in binary format
        /// </summary>
        /// <returns>Binary serialized shape element</returns>
        public byte[] SerializeBinarySelected()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, this.selectedElement);

            return stream.ToArray();
        }
        /// <summary>
        /// Deserializes single shape element from binary format
        /// </summary>
        /// <param name="binaryData">Binary data to be deserialized</param>
        public void DeSerializeBinarySelected(byte[] binaryData)
        {
            using (MemoryStream stream = new MemoryStream(binaryData))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                this.selectedElement = binaryFormatter.Deserialize(stream) as ShapeElement;
                this.shapes.Add(this.selectedElement);
            }
        }
        /// <summary>
        /// Deserializes binary data into shape list.
        /// </summary>
        /// <param name="binaryData">Binary data to be deserialized</param>
        public void DeSerializeBinary(byte[] binaryData)
        {
            using (MemoryStream stream = new MemoryStream(binaryData))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                this.shapes = binaryFormatter.Deserialize(stream) as List<ShapeElement>;
            }
        }
        /// <summary>
        /// Serializes shapes in binary format
        /// </summary>
        /// <returns>Byte array that contains serialized shapes</returns>
        public byte[] SerializeBinary()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, this.shapes);

            return stream.ToArray();
        }
        /// <summary>
        /// DeSerializes single shape element
        /// </summary>
        /// <param name="xmlContent">XML content to be deserialized</param>
        public void DeSerializeXMLSelected(string xmlContent)
        {
            YAXSerializer serializer = new YAXSerializer(typeof(ShapeElement));
            this.selectedElement = serializer.Deserialize(xmlContent) as ShapeElement;
            this.shapes.Add(this.selectedElement);
        } 
        /// <summary>
        /// Serializes selected shape in XML format
        /// </summary>
        /// <returns>Selected shape in XML format</returns>
        public string SerializeXMLSelected()
        {
            YAXSerializer serializer = new YAXSerializer(typeof(ShapeElement));

            return serializer.Serialize(this.selectedElement);
        }
        /// <summary>
        /// Serializes all shapes in XML format
        /// </summary>
        /// <returns>XML content of board after serialization</returns>
        public string SerializeXML()
        {
            YAXSerializer serializer = new YAXSerializer(typeof(List<ShapeElement>));

            return serializer.Serialize(this.shapes);
        }
        /// <summary>
        /// Imports xml content and deserialize it as list of shapes
        /// </summary>
        /// <param name="xmlContent">XML content to deserialize</param>
        public void DeSerializeXML(string xmlContent)
        {
            YAXSerializer serializer = new YAXSerializer(typeof(List<ShapeElement>));
            this.shapes = serializer.Deserialize(xmlContent) as List<ShapeElement>;
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Disposes object
        /// </summary>
        public void Dispose()
        {
            this.shapes = null;
            this.undoManager = null;
            this.handlerCol = null;
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
