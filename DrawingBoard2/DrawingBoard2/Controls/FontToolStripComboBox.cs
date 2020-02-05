using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DrawingBoard2.Controls
{
    /// <summary>
    /// ToolStripComboBox Control that draws font names with their own font style
    /// </summary>
    [ToolboxItem(true),DefaultProperty("FontName"),
     Description("FontToolStripComboBox,lists system fonts with corresponding font style"),
     DefaultEvent("SelectedIndexChanged")]
    internal partial class FontToolStripComboBox : ToolStripComboBox
    {
        #region FontCbo Definition
        /// <summary>
        /// ComboBox item that represents font item in font combo box
        /// </summary>
        private class FontCbo
        {
            public Font FCFont; //Font object

            public FontCbo(Font FCCurrFont)
            {
                FCFont = FCCurrFont;  //Set This Font Equal To Font Supplied
            }
            public FontCbo(string name)
            {
                FCFont = new Font(name, 12);
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
            /// <summary>
            /// Over-ride Equals method
            /// </summary>
            /// <param name="obj">Object to be checked if it equals this</param>
            /// <returns>True if it equals,unless false</returns>
            public override bool Equals(object obj)
            {
                return this.ToString().Equals(obj.ToString());
            }
            /// <summary>
            /// Override ToString Method To Display Current Font's Name
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return FCFont.Name; //Display Font Name
            }
        }
        #endregion

        #region Private Members
        private SolidBrush FontForeColour; //Font's Colour
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #endregion

        #region Properties
        /// <summary>
        /// Get/Set FontName of the combobox
        /// </summary>
        public string FontName
        {
            get
            {
                if (this.SelectedItem != null)
                    return this.SelectedItem.ToString();
                return string.Empty;
            }
            set { this.SelectedItem = this.GetFontIndex(value); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// ToolStripComboBox Control that draws font names with their own fonts
        /// </summary>
        public FontToolStripComboBox()
        {
            InitializeComponent();
            this.InitializeComboBox();
            this.AutoSize = false;
            this.Size = new Size(250, 30);
        }
        /// <summary>
        /// ToolStripComboBox Control that draws font names with their own fonts
        /// </summary>
        /// <param name="container">Container that will contain FontToolStripComboBox</param>
        public FontToolStripComboBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            this.InitializeComboBox();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        /// <summary>
        /// Returns index of font
        /// </summary>
        /// <param name="fontName">Font name to be searched</param>
        /// <returns>Index of the font name, if not found returns 0</returns>
        private int GetFontIndex(string fontName)
        {
            int index = this.Items.IndexOf(new FontCbo(fontName));

            if (index < 0 || index > this.Items.Count)
                return 0;
            return index;
        }
        /// <summary>
        /// Set events, and load system fonts to the combobbox
        /// </summary>
        private void InitializeComboBox()
        {
            this.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.ComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            this.ComboBox.DrawItem += new DrawItemEventHandler(ComboBox_DrawItem);
            this.ComboBox.MeasureItem += new MeasureItemEventHandler(ComboBox_MeasureItem);

            //Fill Combobox
            FontFamily[] families = FontFamily.Families; //Obtain & Store System fonts Into Array

            foreach (FontFamily family in families) //Loop Through System Fonts
            {
                FontStyle style = FontStyle.Bold; //Set Current Font's Style To bold

                //These Are Only Available In Italic, Not In "Regular", So Test For Them, Else, Exception!!
                if (family.Name == "Monotype Corsiva" || family.Name == "Brush Script MT"
                    || family.Name == "Harlow Solid Italic" ||
                    family.Name == "Palace Script MT" || family.Name == "Vivaldi")
                {
                    style = style | FontStyle.Italic; //Set Style To Italic, To Overt "Regular" & Exception
                }
                this.Items.Add(new FontCbo(new Font(family.Name, 12, style, GraphicsUnit.Point)));
            }
            this.SelectedIndex = this.GetFontIndex("Microsoft Sans Serif");
        }
        /// <summary>
        /// Item Height Measurements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            Font font = ((FontCbo)this.Items[e.Index]).FCFont; //Get Current Font In ComboBox

            SizeF stringSize = e.Graphics.MeasureString(font.Name, font); //determine Its Size

            e.ItemHeight = (int)stringSize.Height; //Set Appropriate Height
            e.ItemWidth = (int)stringSize.Width; //Set Appropriate Width

        }
        /// <summary>
        /// Drawing Of Fonts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            Brush FontBrush; //Brush To Be used

            //If No Current Colour
            if (FontForeColour == null)
                FontForeColour = new SolidBrush(e.ForeColor); //Set ForeColour
            else
            {
                if (!FontForeColour.Color.Equals(e.ForeColor)) //Fore Colour Changed, Create New Brush
                {
                    FontForeColour.Dispose(); //Dispose Old Brush
                    FontForeColour = new SolidBrush(e.ForeColor); //Create New Brush
                }
            }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) //Set Appropriate Brush
                FontBrush = SystemBrushes.HighlightText;
            else
                FontBrush = FontForeColour;
            Font font = ((FontCbo)this.Items[e.Index]).FCFont; //Current item's Font
            e.DrawBackground(); //Redraw Item Background
            e.Graphics.DrawString(font.Name, font, FontBrush, e.Bounds.X, e.Bounds.Y); //Draw Current Font
            e.DrawFocusRectangle(); //Draw Focus Rectangle Around It

        }
        #endregion

        #region Dispose
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}