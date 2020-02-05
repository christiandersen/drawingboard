using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms.Design;
using System.Text;
using System.Windows.Forms;

namespace DrawingBoard2.Forms
{
    /// <summary>
    /// A form that has a richtextbox.
    /// Reads user input 
    /// </summary>
    public partial class RichTextBoxForm : Form
    {
        #region Variables
        /// <summary>
        /// Editor Service, that is used editing RTF in property grid
        /// </summary>
        public IWindowsFormsEditorService editorService;
        private bool isConfirmed = false;
        #endregion

        #region Properties
        /// <summary>
        /// Richtextbox in the form
        /// </summary>
        public RichTextBox RichTextBox
        {
            get { return this.richTB; }
        }
        /// <summary>
        /// Are last changes confirmed or canceled
        /// </summary>
        public bool Confirmed
        {
            get { return this.isConfirmed; }
        }
        /// <summary>
        /// Back color of richtextbox
        /// </summary>
        public new Color BackColor
        {
            set { this.tscbBackColor.Color = value; }
        }
        /// <summary>
        /// Forecolor(Font color) of richtextbox
        /// </summary>
        public Color FontColor
        {
            set { this.tscpFontColorPicker.Color = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// A form that has a richtextbox.Reads user input 
        /// </summary>
        /// <param name="topLevel">Shows that form is top-level or not</param>
        public RichTextBoxForm(bool topLevel)
        {
            InitializeComponent();
            this.TopLevel = topLevel;
        }
        #endregion

        #region Functions
        private void tscpFontColorPicker_SelectedColorChanged(object sender, EventArgs e)
        {
            this.richTB.ForeColor = tscpFontColorPicker.Color;
        }
        private void tscbBackColor_SelectedColorChanged(object sender, EventArgs e)
        {
            this.richTB.BackColor = tscbBackColor.Color;
        }
        /// <summary>
        /// Cancel click event. Closes form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsb_Cancel_Click(object sender, EventArgs e)
        {
            this.isConfirmed = false;
            this.Close();
        }
        /// <summary>
        /// Form load event. Loads fonts to the combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RichTextBoxForm_Load(object sender, EventArgs e)
        {
            this.LoadFonts();
        }
        /// <summary>
        /// Loads installed fonts to font combobox
        /// Writes name of each font with its own font
        /// </summary>
        private void LoadFonts()
        {
            this.tscb_FontSize.SelectedIndex = 4;
            this.tscb_Style.SelectedIndex = 0;           
        }
        private void tscb_Fonts_Click(object sender, EventArgs e)
        {
            this.SetFont();
        }
        private void tscb_Style_Click(object sender, EventArgs e)
        {
            this.SetFont();
        }
        private void tscb_FontSize_Click(object sender, EventArgs e)
        {
            this.SetFont();
        }
        private void tscb_CheckedChanged(object sender, EventArgs e)
        {
            this.SetFont();
        }
        private void SetFont()
        {
            string fontName = tscbFont.FontName;
            int fontSize = 12;
            try
            {
                fontSize = Int32.Parse(tscb_FontSize.Text);
            }
            catch { }
            FontStyle style = FontStyle.Regular;
            int index = tscb_Style.SelectedIndex;

            if (index == 1)
                style = FontStyle.Italic;
            else if (index == 2)
                style = FontStyle.Bold;
            else if (index == 3)
                style = FontStyle.Bold | FontStyle.Italic;

            if (tscb_Underline.Checked)
                style |= FontStyle.Underline;
            if (tscb_StrikeOut.Checked)
                style |= FontStyle.Strikeout;

            richTB.SelectionFont = new Font(fontName, fontSize, style);
        }
        private void tsbLeftAlign_Click(object sender, EventArgs e)
        {
            this.richTB.SelectionAlignment = HorizontalAlignment.Left;
        }
        private void tsbCenterAlignment_Click(object sender, EventArgs e)
        {
            this.richTB.SelectionAlignment = HorizontalAlignment.Center;
        }
        private void tscbFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFont();
        }
        private void tsbRightAlign_Click(object sender, EventArgs e)
        {
            this.richTB.SelectionAlignment = HorizontalAlignment.Right;
        }
        private void tsbOkButton_Click(object sender, EventArgs e)
        {
            this.isConfirmed = true;
            this.Close();
        }
        private void tscpFontColorPicker_Click(object sender, EventArgs e)
        {
            this.richTB.SelectionColor = tscpFontColorPicker.Color;
        }
        private void tsbBullet_Click(object sender, EventArgs e)
        {
            this.richTB.SelectionBullet = !this.richTB.SelectionBullet;
        }
        #endregion           
    }
}
