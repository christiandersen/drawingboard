using System;
using System.Drawing;

namespace DrawingBoard2
{
    /// <summary>
    /// Since <see cref="System.Drawing.Font"/> is not XML serializable 
    /// This class is used for XML serialing/deserializng font objects
    /// </summary>
    [Serializable]
    public class SerializableFont
    {
        #region Properties
        /// <summary>
        /// Name of Font Family
        /// </summary>
        public string FontFamily { get; set; }
        /// <summary>
        /// Type of GraphicsUnit
        /// </summary>
        public GraphicsUnit GraphicsUnit { get; set; }
        /// <summary>
        /// Size of font
        /// </summary>
        public float Size { get; set; }
        /// <summary>
        /// Style of font
        /// </summary>
        public FontStyle Style { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Intended for xml serialization purposes only
        /// </summary>
        public SerializableFont() { }
        #endregion

        #region Methods
        /// <summary>
        /// Function that sets(converts) <see cref="System.Drawing.Font"/> object to 
        /// <see cref="DrawingBoard2.SerializableFont"/> object
        /// </summary>
        /// <param name="f">Font to be set</param>
        public void SetFont(Font f)
        {
            if (f != null)
            {
                FontFamily = f.FontFamily.Name;
                GraphicsUnit = f.Unit;
                Size = f.Size;
                Style = f.Style;
            }
            else //If null , then set default values
            {
                FontFamily = System.Drawing.FontFamily.GenericSansSerif.Name;
                GraphicsUnit = System.Drawing.GraphicsUnit.Pixel;
                Size = 12;
                Style = FontStyle.Regular;
            }
        }
        /// <summary>
        /// Converts  to <see cref="System.Drawing.Font"/>
        /// </summary>
        /// <returns><see cref="System.Drawing.Font"/></returns>
        public Font ToFont()
        {
            return new Font(FontFamily, Size, Style,
                GraphicsUnit);
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Creates <see cref="DrawingBoard2.SerializableFont"/> instance from
        /// <see cref="System.Drawing.Font"/> instance
        /// </summary>
        /// <param name="font">Font instance</param>
        /// <returns>Created SerializableFont object</returns>
        public static SerializableFont FromFont(Font font)
        {
            SerializableFont sFont = new SerializableFont();
            sFont.SetFont(font);

            return sFont;
        }
        #endregion
    }
}
