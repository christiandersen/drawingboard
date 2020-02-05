using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Base class for all shape elements
    /// </summary>
    [Serializable]
    public abstract class ShapeElement 
    {
        #region Variables
        /// <summary>
        /// Is it a group or not
        /// </summary>
        protected bool isGroup = false; 
        /// <summary>
        /// Shape can rotate or not
        /// </summary>
        protected bool canRotate; 
        /// <summary>
        /// Is region empty or not
        /// </summary>
        protected Region region = Region.Empty;
        /// <summary>
        /// Rotation angle of shape
        /// </summary>
        protected int rotation; 

        /// <summary>
        /// Initial region before moving/resizing
        /// </summary>
        protected Region oldregion = Region.Empty;    

        //Some Visual variables
        private Color penColor; //Color of pen
        private float penWidth; //Width of pen
        private Color fillColor; //Fill color of shape
        private bool Isfilled; //Is shape is filled or not
        private bool showBorder; //Show border around shape or not
        private DashStyle dashStyle; //Dash style of shape
        private int alphaValue = 255; //Transparency(alpha) value [0,256)

        //Gradient variables
        private bool useGradientFillColor; //True if gradient fill color is used
        private Color gradientColor = Color.White;
        private int gradientAlpha = 255;
        private int gradientDimension = 0; 
        private int gradientAngle = 0;
        private float endColorPosition = 1; 

        /// <summary>
        /// Group Zoom Value on X axis
        /// </summary>
        protected float groupZoomX = 1.0f;
        /// <summary>
        ///  Group Zoom Value on Y axis
        /// </summary>
        protected float groupZoomY = 1.0f;
        /// <summary>
        /// Indicates Corner name generation is enabled or not
        /// </summary>
        protected bool generateCornerNames = false;
        /// <summary>
        /// Indicates whether shape is <see cref="DrawingBoard2.Shapes.Line">Line</see> shape or not
        /// </summary>
        protected bool IsLine; //Is shape is line
        /// <summary>
        /// Indicates whether shape is selected or not
        /// </summary>
        protected bool selected; 
        /// <summary>
        /// Storage area for undo/redo action
        /// </summary>
        protected ShapeElement undoShape;
 
        private OnGroupResize eastResize = OnGroupResize.Resize;
        private OnGroupResize westResize = OnGroupResize.Resize;
        private OnGroupResize southResize = OnGroupResize.Resize;
        private OnGroupResize northResize = OnGroupResize.Resize;
        #endregion

        #region Properties
        /// <summary>
        /// Indicates whether shape generates corner name for each corner or not
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Indicates whether shape generates corner name for each corner or not")]
        public bool EnableCornerNameGeneration
        {
            get { return this.generateCornerNames; }
            set { this.generateCornerNames = value; }
        }
        /// <summary>
        /// Action when group of shapes are resized from north side
        /// </summary>
        [CategoryAttribute("Group Behaviour"), Description("Action when group of shapes are resized from north side")]
        public OnGroupResize NorthResize
        {
            get { return northResize; }
            set { northResize = value; }
        }
        /// <summary>
        /// Action when group of shapes are resized from south side
        /// </summary>
        [CategoryAttribute("Group Behaviour"), Description("Action when group of shapes are resized from south side")]
        public OnGroupResize SouthResize
        {
            get { return southResize; }
            set { southResize = value; }
        }
        /// <summary>
        /// Action when group of shapes are resized from west side
        /// </summary>
        [CategoryAttribute("Group Behaviour"), Description("Action when group of shapes are resized from west side")]
        public OnGroupResize WestResize
        {
            get { return westResize; }
            set { westResize = value; }
        }
        /// <summary>
        /// Action when group of shapes are resized from east side
        /// </summary>
        [CategoryAttribute("Group Behaviour"), Description("Action when group of shapes are resized from east side")]
        public OnGroupResize EastResize
        {
            get { return this.eastResize; }
            set { this.eastResize = value; }
        }
        /// <summary>
        /// Region of the shape on drawing board
        /// </summary>
        [Browsable(false)]
        [CategoryAttribute("Layout"), Description("Region of the shape on drawing board")]
        public Region Region
        {
            get { return this.region; }
            set { this.region = value;

            if (this.oldregion.IsEmpty)
                this.oldregion = region;
            }
        }
        /// <summary>
        /// Previous state of the shape
        /// </summary>
        [YAXDontSerialize]
        [Browsable(false)]
        public ShapeElement UndoShape
        {
            get { return this.undoShape; }
            set { this.undoShape = value; }
        }
        /// <summary>
        /// Indicates whether shape element is line or not
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Indicates whether shape element is line or not")]
        public bool IsThisLine
        {
            get { return this.IsLine; }
            set { this.IsLine = value; }
        }
        /// <summary>
        /// Indicates whether shape element is selected or not
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Indicates whether shape element is selected or not")]
        public bool Selected
        {
            get { return this.selected; }
            set { this.selected = value; }
        }
        /// <summary>
        /// Indicates whether shape element can rotate or not
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Indicates whether shape element can rotate or not")]
        public bool CanRotate
        {
            set { this.canRotate = value; }
            get { return this.canRotate; }
        }
        /// <summary>
        /// Rotation angle value of the shape
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Rotation angle value of the shape")]
        public int Rotation
        {
            get
            {
                if (this.CanRotate)
                    return this.rotation;
                return 0;
            }
            set { this.rotation = value; }
        }
        /// <summary>
        /// Indicates whether shape element is group or not
        /// </summary>
        [Browsable(false)]
        public bool IsGroup
        {
            set { this.isGroup = value; }
            get { return this.isGroup; }
        }
        /// <summary>
        /// Start X position of the shape
        /// </summary>
        [YAXDontSerialize]
        [Browsable(false)]
        public int PosStartX
        {
            get { return this.region.X0; }
        }
        /// <summary>
        /// Start Y position of the shape
        /// </summary>
        [YAXDontSerialize]
        [Browsable(false)]
        public int PosStartY
        {
            get { return this.region.Y0; }
        }
        /// <summary>
        /// End X position of the shape
        /// </summary>
        [YAXDontSerialize]
        [Browsable(false)]
        public int PosEndX
        {
            get { return this.region.X1; }
        }
        /// <summary>
        /// End Y position of the shape
        /// </summary>
        [YAXDontSerialize]
        [Browsable(false)]
        public int PosEndY
        {
            get { return this.region.Y1; }
        }     
        /// <summary>
        /// Dash style of the shape
        /// </summary>
         [CategoryAttribute("Appearance"), Description("Dash style of the shape")]
        public virtual DashStyle DashStyle
        {
            get { return this.dashStyle; }
            set { this.dashStyle = value; }
        }
        /// <summary>
        /// Shape has border or not
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Shape has border or not")]
        public virtual bool ShowBorder
        {
            get { return this.showBorder; }
            set { this.showBorder = value; }
        }
        /// <summary>
        /// Color of pen that is used for drawing shape outline
        /// </summary>
        [YAXDontSerialize]
        [CategoryAttribute("Appearance"), Description("Color of pen that is used for drawing shape outline")]
        public virtual Color PenColor
        {
            get { return this.penColor; }
            set { this.penColor = value; }
        }
        /// <summary>
        /// Pen color string in hexadecimal format
        /// </summary>
        [Browsable(false)]
        public string PenColorStr
        {
            get { return ConverterUtil.ToString(this.penColor); }
            set { this.penColor = ConverterUtil.ToColor(value); }
        }
        /// <summary>
        /// Color that is used for filling the shape
        /// </summary>
        [YAXDontSerialize]
        [CategoryAttribute("Appearance"), Description("Color that is used for filling the shape")]
        public virtual Color FillColor
        {
            get { return this.fillColor; }
            set { this.fillColor = value; }
        }
        /// <summary>
        /// Fill color string in hexadecimal format
        /// </summary>
        [Browsable(false)]
        public string FillColorStr
        {
            get { return ConverterUtil.ToString(this.fillColor); }
            set { this.fillColor = ConverterUtil.ToColor(value); }
        }
        /// <summary>
        /// Width of the pen
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Width of the pen")]
        public virtual float PenWidth
        {
            get { return this.penWidth; }
            set { this.penWidth = value; }
        }
        /// <summary>
        /// Indicates whether shape is filled or not
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Indicates whether shape is filled or not")]
        public virtual bool FillEnabled
        {
            get { return this.Isfilled; }
            set { this.Isfilled = value; }
        }
        /// <summary>
        /// Alpha value of the color , only will be used when gradient brush is enabled
        /// </summary>
        [CategoryAttribute("Appearance"), Description("Alpha value of the color")]
        public virtual int ColorAlpha
        {
            get { return this.alphaValue; }
            set { this.alphaValue = value >= 255 ? 255 : (value < 0 ? 0 : value); }
        }
        /// <summary>
        /// Enable/Disable gradient brush
        /// </summary>
        [CategoryAttribute("Gradient"), Description("Enable/Disable gradient brush")]
        public virtual bool UseGradientColor
        {
            get { return this.useGradientFillColor; }
            set { this.useGradientFillColor = value; }
        }
        /// <summary>
        /// Center of gradient color to focus
        /// </summary>
        [CategoryAttribute("Gradient"), Description("Center of gradient color to focus")]
        public virtual float EndColorPosition
        {
            get { return this.endColorPosition; }
            set { this.endColorPosition = value < 0 ? 0 : (value > 1 ? 1 : value); }
        }
        /// <summary>
        /// Gradient color
        /// </summary>
        [YAXDontSerialize]
        [CategoryAttribute("Gradient"), Description("Gradient color")]
        public virtual Color GradientColor
        {
            get { return this.gradientColor; }
            set { this.gradientColor = value; }
        }
        /// <summary>
        /// Gradient color in hexadecimal format
        /// </summary>
        [Browsable(false)]
        public string GradientColorStr
        {
            get { return ConverterUtil.ToString(this.gradientColor); }
            set { this.gradientColor = ConverterUtil.ToColor(value); }
        }
        /// <summary>
        /// Gradient alpha value
        /// </summary>
        [CategoryAttribute("Gradient"), Description("Gradient alpha value")]
        public virtual int GradientAlpha
        {
            get { return this.gradientAlpha; }
            set { this.gradientAlpha = value >= 255 ? 255 : (value < 0 ? 0 : value); }
        }
        /// <summary>
        /// Gradient color dimension
        /// </summary>
        [CategoryAttribute("Gradient"), Description("Gradient color dimension")]
        public virtual int GradientDimension
        {
            get { return this.gradientDimension; }
            set { this.gradientDimension = value >= 0 ? value : 0; }
        }
        /// <summary>
        /// Gradient color angle
        /// </summary>
        [CategoryAttribute("Gradient"), Description("Gradient color angle")]
        public virtual int GradientAngle
        {
            get { return this.gradientAngle; }
            set { this.gradientAngle = value; }
        }
        /// <summary>
        /// Zoom value of the group at x coordinate
        /// </summary>
        [CategoryAttribute("Group Behaviour"), Description("Zoom value of the group at x coordinate")]
        public float GroupZoomX
        {
            get { return this.groupZoomX; }
            set { this.groupZoomX = value; }
        }
        /// <summary>
        /// Zoom value of the group at y coordinate
        /// </summary>
        [CategoryAttribute("Group Behaviour"), Description("Zoom value of the group at y coordinate")]
        public float GroupZoomY
        {
            get { return this.groupZoomY; }
            set { this.groupZoomY = value; }
        }      
        #endregion

        #region Constructors
        /// <summary>
        /// Default ShapeElement constructor , Create new shape element
        /// </summary>
        public ShapeElement()
        {
            this.penColor = Color.Black;//Default color is black
            this.penWidth = 1.0f; //Set default pen width
            this.fillColor = Color.Black; //Set default fill color
            this.Isfilled = false; //Default , shape is not filled
            this.showBorder = true; //Show borders
            this.dashStyle = DashStyle.Solid; //Set default dashstyle
            this.alphaValue = 255;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add value to rotation angle
        /// </summary>
        /// <param name="addValue">Value to be added to rotation angle</param>
        public void AddRotation(int addValue)
        {
            this.rotation += addValue;
        }
        /// <summary>
        /// Add shape to graphics path
        /// </summary>
        /// <param name="graphicsPath">Graphics path to be added</param>
        /// <param name="dx">X region on canvas?</param>
        /// <param name="dy">Y region on canvas?</param>
        /// <param name="zoom">Zoom value</param>
        public void BaseAddToGraphPath(GraphicsPath graphicsPath, int dx, int dy, float zoom)
        {
            GraphicsPath tempGraphicsPath = new GraphicsPath();
            AddToGraphPath(tempGraphicsPath, dx, dy, zoom);
            Matrix translateMatrix = new Matrix();
            translateMatrix.RotateAt(this.rotation, region.GetActualregion(dx,dy,zoom));

            tempGraphicsPath.Transform(translateMatrix);
            tempGraphicsPath.AddPath(tempGraphicsPath, true);
        }
        /// <summary>
        /// Copy the gradient properties from another element 
        /// </summary>
        protected void CopyGradientprop(ShapeElement element)
        {
            this.useGradientFillColor = element.useGradientFillColor;
            this.gradientColor = element.gradientColor;
            this.gradientAlpha = element.gradientAlpha;
            this.gradientDimension = element.gradientDimension;
            this.gradientAngle = element.gradientAngle;
            this.endColorPosition = element.endColorPosition;
        }
        /// <summary>
        /// Calculate Scaled pend width
        /// </summary>
        /// <param name="zoom">Zoom value</param>
        /// <returns>Pen width according to zoom value</returns>
        protected float ScaledPenWidth(float zoom)
        {
            return zoom < 0.1f ? this.penWidth * 0.1f : this.penWidth * zoom;
        }
        /// <summary>
        /// Calculates and returns X dimension of the shape
        /// </summary>
        /// <returns></returns>
        private float GetDimensionX()
        {
            return (float)(Math.Sqrt(Math.Pow(region.Width, 2) +
                Math.Pow(region.Height, 2)) - region.Width) / 2;
        }
        /// <summary>
        /// Calculates and returns Y dimension of the shape
        /// </summary>
        /// <returns></returns>
        private float GetDimensionY()
        {
            return (float)(Math.Sqrt(Math.Pow(region.Width, 2) +
                Math.Pow(region.Height, 2)) - region.Height) / 2;
        }
        /// <summary>
        /// Gets a brush from the properties of the shape
        /// </summary>
        protected Brush GetBrush(int dx, int dy, float zoom)
        {
            if (this.Isfilled)
            {
                if (this.UseGradientColor)
                {
                    float width;
                    float height;

                    if (this.GradientDimension > 0)
                        height = width = this.GradientDimension;
                    else
                    {
                        width = ((region.X1 - region.X0) + 2 * GetDimensionX()) * zoom;
                        height = ((region.Y1 - region.Y0) + 2 * GetDimensionY()) * zoom;
                    }
                    LinearGradientBrush br = new LinearGradientBrush(
                        new RectangleF((region.X0 - GetDimensionX() + dx) * zoom
                        , (region.Y0 - GetDimensionY() + dy) * zoom, width, height)
                        , DrawingUtils.SetTransparency(this.fillColor, this.alphaValue)
                        , DrawingUtils.SetTransparency(this.gradientColor, this.gradientAlpha)
                        , this.GradientAngle
                        , true);
                    br.SetBlendTriangularShape(this.EndColorPosition, 0.95f);
                    br.WrapMode = WrapMode.TileFlipXY;
                    return br;
                }
                else
                    return new SolidBrush(DrawingUtils.SetTransparency(this.fillColor,
                        this.alphaValue));
            }
            else
                return null;
        }
        /// <summary>
        /// Deep copies "this" object  to "to" object
        /// </summary>
        /// <param name="to">Object to copied</param>
        protected void CopyProperties(ShapeElement to)
        {
            this.CopyProperties(to);
        }
        /// <summary>
        /// Creates pen object
        /// </summary>
        /// <param name="zoom">Current zoom value</param>
        /// <returns>Pen object</returns>
        protected Pen CreatePen(float zoom)
        {
            Pen pen = new Pen(this.PenColor, ScaledPenWidth(zoom));
            pen.DashStyle = this.dashStyle;

            if (selected)
            {
                pen.Color = DrawingUtils.SetTransparency(Color.Red, 120);
                pen.Width += 1;
            }

            return pen;
        }
        #endregion

        #region Virtual Methods
        /// <summary>
        /// Changes dimensions of shape
        /// </summary>
        /// <param name="x">Redim X region value</param>
        /// <param name="y">Redim Y region value</param>
        /// <param name="direction">Direction where shape will be redim</param>
        public virtual void Redim(int x, int y, Direction direction)
        {
            switch (direction)
            {
                case Direction.NorthWest:
                    region.X0 = this.oldregion.X0 + x;
                    region.Y0 = this.oldregion.Y0 + y;
                    break;
                case Direction.North:
                    region.Y0 = this.oldregion.Y0 + y;
                    break;
                case Direction.NorthEast:
                    region.X1 = this.oldregion.X1 + x;
                    region.Y0 = this.oldregion.Y0 + y;
                    break;
                case Direction.East:
                    region.X1 = this.oldregion.X1 + x;
                    break;
                case Direction.SouthEast:
                    region.X1 = this.oldregion.X1 + x;
                    region.Y1 = this.oldregion.Y1 + y;
                    break;
                case Direction.South:
                    region.Y1 = this.oldregion.Y1 + y;
                    break;
                case Direction.SouthWest:
                    region.X0 = this.oldregion.X0 + x;
                    region.Y1 = this.oldregion.Y1 + y;
                    break;
                case Direction.West:
                    region.X0 = this.oldregion.X0 + x;
                    break;
                default:
                    break;
            }
            if (!this.IsLine)
            {   // manage redim limits
                if (region.X1 <= region.X0)
                    region.X1 = region.X0 + 10;
                if (region.Y1 <= region.Y0)
                    region.Y1 = region.Y0 + 10;
            }
        }
        /// <summary>
        /// Called at the end of move/redim of the shape. Stores startX|Y|X1|Y1 
        /// for a correct rendering during object move/redim
        /// </summary>
        public virtual void EndMoveRedim()
        { 
            this.oldregion.CopyFrom(region);
        }
        /// <summary>
        /// Returns true if shape contains point(x,y),
        /// Returns false if not
        /// </summary>
        /// <param name="x">X region of point</param>
        /// <param name="y">Y region of point</param>
        /// <returns></returns>
        public virtual bool Contains(int x, int y)
        {
            if (this.IsLine)
            {
                int dist1 = CalculationUtils.CalculateDistance(x, y, region.X0, region.Y0)
                    + CalculationUtils.CalculateDistance(x, y, region.X1, region.Y1);

                int dist2 = CalculationUtils.CalculateDistance(region.X1, region.Y1,
                    region.X0, region.Y0) + 7;

                return dist1 < dist2;
            }
            return new Rectangle(region.X0, region.Y0, region.X1 -
                region.X0, region.Y1 - region.Y0).Contains(x, y);
        }
        /// <summary>
        /// Moves the shape by x,y
        /// </summary>
        public virtual void Move(int x, int y)
        {
            region.Move(oldregion, x, y);
        }
        /// <summary>
        /// Moves shape element to left by 5 unit
        /// </summary>
        public virtual void MoveLeft()
        {
            region.Move(this.region, 5, 0);
        }
        /// <summary>
        /// Moves shape element to right by 5 unit
        /// </summary>
        public virtual void MoveRight()
        {
            region.Move(this.region, -5, 0);
        }
        /// <summary>
        /// Moves shape element to up by 5 unit
        /// </summary>
        public virtual void MoveUp()
        {
            region.Move(this.region, 0, 5);
        }
        /// <summary>
        /// Moves shape element to down by 5 unit
        /// </summary>
        public virtual void MoveDown()
        {
            region.Move(this.region, 0, -5);
        }
        /// <summary>
        /// Draw this shape to a graphic ogj. 
        /// </summary>
        public virtual void Draw(Graphics graphObj, int dx, int dy, float zoom)
        { 
        }
        /// <summary>
        /// Add this shape to a graphic path. 
        /// </summary>
        public virtual void AddToGraphPath(GraphicsPath graphicPath,
            int dx, int dy, float zoom)
        { 
        }
        /// <summary>
        /// Used to degroup a grouped shape. Returns a list of shapes.
        /// </summary>
        public virtual List<ShapeElement> DeGroup()
        {
            return null;
        }
        /// <summary>
        /// Select this shape.
        /// </summary>
        public virtual void Select()
        { 
        }
        /// <summary>
        /// Select this shape.
        /// </summary>
        public virtual void Select(RichTextBox richTextBox)
        {
        }
        /// <summary>
        /// Select this shape.
        /// </summary>
        public virtual void Select(Region region)
        {
        }
        /// <summary>
        /// Deselect this shape.
        /// </summary>
        public virtual void DeSelect()
        { 
        }
        /// <summary>
        /// Clone this shape
        /// </summary>
        public virtual ShapeElement Copy()
        {
            return null;
        }
        /// <summary>
        /// Adapt the shape at the gridsize 
        /// </summary>
        public virtual void FitTogrid(int gridsize)
        {
            this.oldregion.X0 = gridsize * (int)(this.oldregion.X0 / gridsize);
            this.oldregion.X1 = gridsize * (int)(this.oldregion.X1 / gridsize);
            this.oldregion.Y0 = gridsize * (int)(this.oldregion.Y0 / gridsize);
            this.oldregion.Y1 = gridsize * (int)(this.oldregion.Y1 / gridsize);
        }
        /// <summary>
        /// Rotate element, with a rotation angle from a vertical line from the center
        /// of the shape and a line from the center to the point (x,y)
        /// </summary>
        public virtual void Rotate(float x, float y)
        {
            Point center = new Point((int)(region.X0 + (region.X1- region.X0) / 2),
                (int)(region.Y0+(region.Y1 - region.Y0) / 2));

            float dx = x - center.X;
            float dy = y - center.Y;
			float addAngle = 0;
			float alpha= 0f;

            if ((dx>0)&(dy>0))
			{
				addAngle= 90;
				alpha= (float)Math.Abs((Math.Atan((double)(dy/dx)) * (180/Math.PI)));
			}
            else
            {
                if ((dx <= 0) & (dy >= 0))
                {
                    addAngle = 180;
                    if (dy > 0)
                        alpha = (float)Math.Abs((Math.Atan((double)(dx / dy)) *
                            (180 / Math.PI)));
                    else if (dy == 0)
                        addAngle = 270;
                }
				else
                {
					if ((dx<0)&(dy<0))
					{
						addAngle= 270;
						alpha= (float)Math.Abs((Math.Atan((double)(dy/dx)) * 
                            (180/Math.PI)));
					}
					else
					{
						addAngle= 0;
						alpha= (float)Math.Abs((Math.Atan((double)(dx/dy)) * 
                            (180/Math.PI)));
					}
                }
            }
            this.rotation = (int)(addAngle + alpha);
        }
        #endregion
    }
}
