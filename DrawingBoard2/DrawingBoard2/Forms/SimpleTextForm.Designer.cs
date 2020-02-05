namespace DrawingBoard2.Forms
{
    partial class SimpleTextForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleTextForm));
            this.txtInput = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tscbFont = new DrawingBoard2.Controls.FontToolStripComboBox(this.components);
            this.tscb_Style = new System.Windows.Forms.ToolStripComboBox();
            this.tscb_FontSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tscb_Underline = new DrawingBoard2.Controls.ToolStripCheckedBox();
            this.tscb_StrikeOut = new DrawingBoard2.Controls.ToolStripCheckedBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbFillColor = new DrawingBoard2.Controls.ToolStripColorPicker();
            this.tsbOkButton = new System.Windows.Forms.ToolStripButton();
            this.tsb_Cancel = new System.Windows.Forms.ToolStripButton();
            this.tscbFontColor = new DrawingBoard2.Controls.ToolStripColorPicker();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtInput.Location = new System.Drawing.Point(0, 28);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(740, 196);
            this.txtInput.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscbFont,
            this.tscb_Style,
            this.tscb_FontSize,
            this.toolStripSeparator1,
            this.tscbFontColor,
            this.tscbFillColor,
            this.tscb_Underline,
            this.tscb_StrikeOut,
            this.toolStripSeparator2,
            this.tsbOkButton,
            this.tsb_Cancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(740, 30);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tscbFont
            // 
            this.tscbFont.AutoSize = false;
            this.tscbFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscbFont.FontName = "Microsoft Sans Serif";
            this.tscbFont.Name = "tscbFont";
            this.tscbFont.Size = new System.Drawing.Size(250, 30);
            this.tscbFont.Text = "Microsoft Sans Serif";
            this.tscbFont.SelectedIndexChanged += new System.EventHandler(this.tscbFont_SelectedIndexChanged);
            // 
            // tscb_Style
            // 
            this.tscb_Style.BackColor = System.Drawing.SystemColors.Window;
            this.tscb_Style.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscb_Style.Items.AddRange(new object[] {
            "Regular",
            "Oblique",
            "Bold",
            "Bold Oblique"});
            this.tscb_Style.Name = "tscb_Style";
            this.tscb_Style.Size = new System.Drawing.Size(75, 30);
            this.tscb_Style.SelectedIndexChanged += new System.EventHandler(this.tscb_Style_SelectedIndexChanged);
            // 
            // tscb_FontSize
            // 
            this.tscb_FontSize.BackColor = System.Drawing.SystemColors.Window;
            this.tscb_FontSize.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "14",
            "16",
            "18",
            "20",
            "22",
            "24",
            "26",
            "28",
            "36",
            "48",
            "72"});
            this.tscb_FontSize.Name = "tscb_FontSize";
            this.tscb_FontSize.Size = new System.Drawing.Size(75, 30);
            this.tscb_FontSize.SelectedIndexChanged += new System.EventHandler(this.tscb_FontSize_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
            // 
            // tscb_Underline
            // 
            this.tscb_Underline.Checked = false;
            this.tscb_Underline.Name = "tscb_Underline";
            this.tscb_Underline.Size = new System.Drawing.Size(77, 27);
            this.tscb_Underline.Text = "Underline";
            this.tscb_Underline.CheckedChanged += new System.EventHandler(this.tscb_Underline_CheckedChanged);
            // 
            // tscb_StrikeOut
            // 
            this.tscb_StrikeOut.Checked = false;
            this.tscb_StrikeOut.Name = "tscb_StrikeOut";
            this.tscb_StrikeOut.Size = new System.Drawing.Size(73, 27);
            this.tscb_StrikeOut.Text = "Strikeout";
            this.tscb_StrikeOut.CheckedChanged += new System.EventHandler(this.tscb_StrikeOut_CheckedChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 30);
            // 
            // tscbFillColor
            // 
            this.tscbFillColor.AutoSize = false;
            this.tscbFillColor.ButtonDisplayStyle = DrawingBoard2.Controls.ToolStripColorPickerDisplayType.UnderLineAndImage;
            this.tscbFillColor.Color = System.Drawing.Color.White;
            this.tscbFillColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tscbFillColor.Image = global::DrawingBoard2.Properties.Resources.BucketFill;
            this.tscbFillColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tscbFillColor.Name = "tscbFillColor";
            this.tscbFillColor.Size = new System.Drawing.Size(30, 23);
            this.tscbFillColor.Text = "Background Color";
            this.tscbFillColor.ToolTipText = "";
            this.tscbFillColor.SelectedColorChanged += new System.EventHandler(this.tscbFillColor_SelectedColorChanged);
            // 
            // tsbOkButton
            // 
            this.tsbOkButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tsbOkButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbOkButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tsbOkButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tsbOkButton.Image = ((System.Drawing.Image)(resources.GetObject("tsbOkButton.Image")));
            this.tsbOkButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOkButton.Name = "tsbOkButton";
            this.tsbOkButton.Size = new System.Drawing.Size(28, 27);
            this.tsbOkButton.Text = "OK";
            this.tsbOkButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.tsbOkButton.Click += new System.EventHandler(this.tsbOkButton_Click);
            // 
            // tsb_Cancel
            // 
            this.tsb_Cancel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tsb_Cancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_Cancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tsb_Cancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tsb_Cancel.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Cancel.Image")));
            this.tsb_Cancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Cancel.Name = "tsb_Cancel";
            this.tsb_Cancel.Size = new System.Drawing.Size(47, 27);
            this.tsb_Cancel.Text = "Cancel";
            this.tsb_Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.tsb_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // tscbFontColor
            // 
            this.tscbFontColor.AutoSize = false;
            this.tscbFontColor.ButtonDisplayStyle = DrawingBoard2.Controls.ToolStripColorPickerDisplayType.UnderLineAndImage;
            this.tscbFontColor.Color = System.Drawing.Color.Black;
            this.tscbFontColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tscbFontColor.Image = global::DrawingBoard2.Properties.Resources.PenDraw;
            this.tscbFontColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tscbFontColor.Name = "tscbFontColor";
            this.tscbFontColor.Size = new System.Drawing.Size(30, 23);
            this.tscbFontColor.Text = "Font Color";
            this.tscbFontColor.ToolTipText = "";
            this.tscbFontColor.SelectedColorChanged += new System.EventHandler(this.tscbFontColor_SelectedColorChanged);
            // 
            // SimpleTextForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 224);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.txtInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SimpleTextForm";
            this.Text = "SimpleTextForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SimpleTextForm_FormClosing);
            this.Load += new System.EventHandler(this.SimpleTextForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// Textbox in the form
        /// </summary>
        public System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox tscb_FontSize;
        private System.Windows.Forms.ToolStripComboBox tscb_Style;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private DrawingBoard2.Controls.ToolStripCheckedBox tscb_Underline;
        private DrawingBoard2.Controls.ToolStripCheckedBox tscb_StrikeOut;
        private System.Windows.Forms.ToolStripButton tsbOkButton;
        private System.Windows.Forms.ToolStripButton tsb_Cancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private DrawingBoard2.Controls.FontToolStripComboBox tscbFont;
        private Controls.ToolStripColorPicker tscbFillColor;
        private Controls.ToolStripColorPicker tscbFontColor;

    }
}