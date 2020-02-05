using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using System.ComponentModel;

using DrawingBoard2.Utils;

namespace DrawingBoard2.Shapes
{
    /// <summary>
    /// Image Box shape
    /// A rectangle that contains image inside
    /// </summary>
    [Serializable]
    public class ImageBox : ShapeElement
    {
        #region Variables
        private Bitmap image;
        private bool isTransparent = true;
        #endregion

        #region Properties
        /// <summary>
        /// Bitmap image that is contained by ImageBox
        /// </summary>
        [YAXDontSerialize]
        [CategoryAttribute("Image"), Description("Bitmap image that is contained by ImageBox")]
        public Bitmap Image
        {
            get { return this.image; }
            set { this.image = value; }
        }
        /// <summary>
        /// Binary representation of the image
        /// </summary>
        [Browsable(false)]
        public byte[] ImageByte
        {
            get{ return ConverterUtil.ConvertToByteArray(this.image);}
            set { this.image = ConverterUtil.ConvertToBitmap(value); }
        }
        /// <summary>
        /// Set/Unset image as transparent
        /// </summary>
        [CategoryAttribute("Image"), Description("Set/Unset image as transparent")]
        public bool IsTransparent
        {
            get { return this.isTransparent; }
            set { this.isTransparent = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Image Box shape.A rectangle that contains image inside
        /// </summary>
        public ImageBox()
        {
            this.selected = true;
            this.EndMoveRedim();
            this.canRotate = true;
        }       
        #endregion

        #region Methods
        /// <summary>
        /// Adds itself to graphic path as Rectangle
        /// </summary>
        /// <param name="graphicPath">Graphic path that will contain rectangle</param>
        /// <param name="dx">X region on path</param>
        /// <param name="dy">Y region on path</param>
        /// <param name="zoom"></param>
        public override void AddToGraphPath(GraphicsPath graphicPath, int dx, int dy, float zoom)
        {
            graphicPath.AddRectangle(region.GetRectangleF(dx,dy,zoom));
        }
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
        /// <summary>
        /// Draws imagebox object on the board(graphObj)
        /// </summary>
        /// <param name="graphObj">Graph Object(board) to drawn on</param>
        /// <param name="dx">X region on board</param>
        /// <param name="dy">Y region on board</param>
        /// <param name="zoom">Zoom value</param>
         public override void Draw(Graphics graphObj, int dx, int dy, float zoom)
        {
            Pen myPen = this.CreatePen(zoom);

            if (image != null)
            {
                if (this.rotation == 0)
                    graphObj.DrawImage(image, (region.X0 + dx) * zoom,
                    (region.Y0 + dy) * zoom, region.Width * zoom,
                    this.region.Height * zoom);
                else
                    graphObj.DrawImage(ImageUtil.RotateImage(image, this.rotation), (region.X0 + dx) * zoom,
                    (region.Y0 + dy) * zoom, region.Width * zoom,
                    this.region.Height * zoom);
            }
            if (this.ShowBorder)
                graphObj.DrawRectangle(myPen, Rectangle.Ceiling(region.GetRectangleF(dx, dy, zoom)));

            myPen.Dispose();
        }       
        /// <summary>
        /// Load image in image box via a file dialog
        /// </summary>
        public void LoadImage()
        {
            this.image = FormUtils.GetImageByFileDialog();
        }
        #endregion
    }
}
