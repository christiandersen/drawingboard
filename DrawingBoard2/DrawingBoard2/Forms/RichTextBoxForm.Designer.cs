namespace DrawingBoard2.Forms
{
    partial class RichTextBoxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RichTextBoxForm));
            this.toolbarStrip = new System.Windows.Forms.ToolStrip();
            this.tscbFont = new DrawingBoard2.Controls.FontToolStripComboBox(this.components);
            this.tscb_Style = new System.Windows.Forms.ToolStripComboBox();
            this.tscb_FontSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tscb_Underline = new DrawingBoard2.Controls.ToolStripCheckedBox();
            this.tscb_StrikeOut = new DrawingBoard2.Controls.ToolStripCheckedBox();
            this.tsbBullet = new System.Windows.Forms.ToolStripButton();
            this.tscpFontColorPicker = new DrawingBoard2.Controls.ToolStripColorPicker();
            this.tscbBackColor = new DrawingBoard2.Controls.ToolStripColorPicker();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbLeftAlign = new System.Windows.Forms.ToolStripButton();
            this.tsbCenterAlignment = new System.Windows.Forms.ToolStripButton();
            this.tsbRightAlign = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbOkButton = new System.Windows.Forms.ToolStripButton();
            this.tsb_Cancel = new System.Windows.Forms.ToolStripButton();
            this.richTB = new System.Windows.Forms.RichTextBox();
            this.toolbarStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolbarStrip
            // 
            this.toolbarStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolbarStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscbFont,
            this.tscb_Style,
            this.tscb_FontSize,
            this.toolStripSeparator1,
            this.tscb_Underline,
            this.tscb_StrikeOut,
            this.tsbBullet,
            this.tscpFontColorPicker,
            this.tscbBackColor,
            this.toolStripSeparator3,
            this.tsbLeftAlign,
            this.tsbCenterAlignment,
            this.tsbRightAlign,
            this.toolStripSeparator2,
            this.tsbOkButton,
            this.tsb_Cancel});
            this.toolbarStrip.Location = new System.Drawing.Point(0, 0);
            this.toolbarStrip.Name = "toolbarStrip";
            this.toolbarStrip.Size = new System.Drawing.Size(777, 30);
            this.toolbarStrip.TabIndex = 0;
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
            this.tscb_Style.SelectedIndexChanged += new System.EventHandler(this.tscb_Style_Click);
            // 
            // tscb_FontSize
            // 
            this.tscb_FontSize.AutoSize = false;
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
            this.tscb_FontSize.Size = new System.Drawing.Size(35, 23);
            this.tscb_FontSize.SelectedIndexChanged += new System.EventHandler(this.tscb_FontSize_Click);
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
            this.tscb_Underline.CheckedChanged += new System.EventHandler(this.tscb_CheckedChanged);
            // 
            // tscb_StrikeOut
            // 
            this.tscb_StrikeOut.Checked = false;
            this.tscb_StrikeOut.Name = "tscb_StrikeOut";
            this.tscb_StrikeOut.Size = new System.Drawing.Size(73, 27);
            this.tscb_StrikeOut.Text = "Strikeout";
            this.tscb_StrikeOut.CheckedChanged += new System.EventHandler(this.tscb_CheckedChanged);
            // 
            // tsbBullet
            // 
            this.tsbBullet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbBullet.Image = global::DrawingBoard2.Properties.Resources.List_bullets_icon;
            this.tsbBullet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBullet.Name = "tsbBullet";
            this.tsbBullet.Size = new System.Drawing.Size(23, 27);
            this.tsbBullet.Text = "Bullet";
            this.tsbBullet.Click += new System.EventHandler(this.tsbBullet_Click);
            // 
            // tscpFontColorPicker
            // 
            this.tscpFontColorPicker.AutoSize = false;
            this.tscpFontColorPicker.ButtonDisplayStyle = DrawingBoard2.Controls.ToolStripColorPickerDisplayType.UnderLineAndImage;
            this.tscpFontColorPicker.Color = System.Drawing.Color.Black;
            this.tscpFontColorPicker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tscpFontColorPicker.Image = global::DrawingBoard2.Properties.Resources.PenDraw;
            this.tscpFontColorPicker.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tscpFontColorPicker.Name = "tscpFontColorPicker";
            this.tscpFontColorPicker.Size = new System.Drawing.Size(30, 23);
            this.tscpFontColorPicker.Text = "Font Color";
            this.tscpFontColorPicker.ToolTipText = "Font Color";
            this.tscpFontColorPicker.SelectedColorChanged += new System.EventHandler(this.tscpFontColorPicker_SelectedColorChanged);
            this.tscpFontColorPicker.Click += new System.EventHandler(this.tscpFontColorPicker_Click);
            // 
            // tscbBackColor
            // 
            this.tscbBackColor.AutoSize = false;
            this.tscbBackColor.ButtonDisplayStyle = DrawingBoard2.Controls.ToolStripColorPickerDisplayType.UnderLineAndImage;
            this.tscbBackColor.Color = System.Drawing.Color.Black;
            this.tscbBackColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tscbBackColor.Image = global::DrawingBoard2.Properties.Resources.BucketFill;
            this.tscbBackColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tscbBackColor.Name = "tscbBackColor";
            this.tscbBackColor.Size = new System.Drawing.Size(30, 23);
            this.tscbBackColor.Text = "Background Color";
            this.tscbBackColor.ToolTipText = "";
            this.tscbBackColor.SelectedColorChanged += new System.EventHandler(this.tscbBackColor_SelectedColorChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 30);
            // 
            // tsbLeftAlign
            // 
            this.tsbLeftAlign.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLeftAlign.Image = global::DrawingBoard2.Properties.Resources.align_left;
            this.tsbLeftAlign.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLeftAlign.Name = "tsbLeftAlign";
            this.tsbLeftAlign.Size = new System.Drawing.Size(23, 27);
            this.tsbLeftAlign.Text = "Left Alignment";
            this.tsbLeftAlign.Click += new System.EventHandler(this.tsbLeftAlign_Click);
            // 
            // tsbCenterAlignment
            // 
            this.tsbCenterAlignment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCenterAlignment.Image = global::DrawingBoard2.Properties.Resources.align_center;
            this.tsbCenterAlignment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCenterAlignment.Name = "tsbCenterAlignment";
            this.tsbCenterAlignment.Size = new System.Drawing.Size(23, 27);
            this.tsbCenterAlignment.Text = "Center Alignment";
            this.tsbCenterAlignment.Click += new System.EventHandler(this.tsbCenterAlignment_Click);
            // 
            // tsbRightAlign
            // 
            this.tsbRightAlign.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRightAlign.Image = global::DrawingBoard2.Properties.Resources.align_right;
            this.tsbRightAlign.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRightAlign.Name = "tsbRightAlign";
            this.tsbRightAlign.Size = new System.Drawing.Size(23, 27);
            this.tsbRightAlign.Text = "Right Alignment";
            this.tsbRightAlign.Click += new System.EventHandler(this.tsbRightAlign_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 30);
            // 
            // tsbOkButton
            // 
            this.tsbOkButton.BackColor = System.Drawing.Color.Transparent;
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
            this.tsb_Cancel.BackColor = System.Drawing.Color.Transparent;
            this.tsb_Cancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_Cancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tsb_Cancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tsb_Cancel.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Cancel.Image")));
            this.tsb_Cancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Cancel.Name = "tsb_Cancel";
            this.tsb_Cancel.Size = new System.Drawing.Size(47, 27);
            this.tsb_Cancel.Text = "Cancel";
            this.tsb_Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.tsb_Cancel.Click += new System.EventHandler(this.tsb_Cancel_Click);
            // 
            // richTB
            // 
            this.richTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTB.Location = new System.Drawing.Point(0, 30);
            this.richTB.Name = "richTB";
            this.richTB.Size = new System.Drawing.Size(777, 196);
            this.richTB.TabIndex = 1;
            this.richTB.Text = "";
            // 
            // RichTextBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 226);
            this.Controls.Add(this.richTB);
            this.Controls.Add(this.toolbarStrip);
            this.Name = "RichTextBoxForm";
            this.Text = "Rich TextBox Form";
            this.Load += new System.EventHandler(this.RichTextBoxForm_Load);
            this.toolbarStrip.ResumeLayout(false);
            this.toolbarStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolbarStrip;
        private System.Windows.Forms.ToolStripComboBox tscb_Style;
        private System.Windows.Forms.ToolStripComboBox tscb_FontSize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private DrawingBoard2.Controls.ToolStripCheckedBox tscb_Underline;
        private DrawingBoard2.Controls.ToolStripCheckedBox tscb_StrikeOut;
        private DrawingBoard2.Controls.ToolStripColorPicker tscpFontColorPicker;
        private System.Windows.Forms.ToolStripButton tsbLeftAlign;
        private System.Windows.Forms.ToolStripButton tsbCenterAlignment;
        private System.Windows.Forms.ToolStripButton tsbRightAlign;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbOkButton;
        private System.Windows.Forms.RichTextBox richTB;
        private System.Windows.Forms.ToolStripButton tsb_Cancel;
        private System.Windows.Forms.ToolStripButton tsbBullet;
        private DrawingBoard2.Controls.FontToolStripComboBox tscbFont;
        private Controls.ToolStripColorPicker tscbBackColor;
    }
}