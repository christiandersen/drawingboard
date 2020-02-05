using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawingBoard2.Forms
{
    /// <summary>
    /// Contains simple textbox and font properties
    /// </summary>
    public partial class SimpleTextForm : Form
    {
        #region Variables
        private bool isCanceled = false;
        #endregion

        #region Properties
        /// <summary>
        /// Font Color of the textbox
        /// </summary>
        public Color FontColor
        {
            set { this.tscbFontColor.Color = value; }
        }
        /// <summary>
        /// Background Color of the textbox
        /// </summary>
        public new Color BackColor
        {
            set { this.tscbFillColor.Color = value; }
        }
        /// <summary>
        /// Indicates that form is closed by cancel button or ok button
        /// </summary>
        public bool IsCanceled
        {
            get { return this.isCanceled; }
        }
        /// <summary>
        /// TextBox on the form
        /// </summary>
        public TextBox TextBox
        {
            get { return this.txtInput; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Contains simple textbox and font properties
        /// </summary>
        public SimpleTextForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        private void tscbFontColor_SelectedColorChanged(object sender, EventArgs e)
        {
            this.txtInput.ForeColor = tscbFontColor.Color;
        }
        private void tscbFillColor_SelectedColorChanged(object sender, EventArgs e)
        {
            this.txtInput.BackColor = tscbFillColor.Color;
        }
        private void tscbFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFont();
        }
        /// <summary>
        /// Fired when ok button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOkButton_Click(object sender, EventArgs e)
        {
            this.isCanceled = false;
            this.Close();
        }
        private void SimpleTextForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.isCanceled = true;
        }
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.isCanceled = true;
            this.Close();
        }
        private void SimpleTextForm_Load(object sender, EventArgs e)
        {
            this.tscb_FontSize.SelectedIndex = 4;
            this.tscb_Style.SelectedIndex = 0;           
        }
        private void tscb_Fonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFont();
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

            FontFamily fontFamily = new FontFamily(fontName);

            if (fontFamily != null && !fontFamily.IsStyleAvailable(style))
            {
                if (fontFamily.IsStyleAvailable(FontStyle.Regular))
                    style = FontStyle.Regular;
                else if (fontFamily.IsStyleAvailable(FontStyle.Italic))
                    style = FontStyle.Italic;
            }
            txtInput.Font = new Font(fontName, fontSize, style);
            txtInput.Invalidate();
        }
        private void tscb_Style_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFont();
        }
        private void tscb_FontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFont();
        }
        private void tscb_Underline_CheckedChanged(object sender, EventArgs e)
        {
            SetFont();
        }
        private void tscb_StrikeOut_CheckedChanged(object sender, EventArgs e)
        {
            SetFont();
        }
        #endregion
    }
}
