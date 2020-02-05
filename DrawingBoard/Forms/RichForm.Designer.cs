namespace DrawingBoard
{
    partial class RichtTextForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RichtTextForm));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.DimensioneCbo = new System.Windows.Forms.ToolStripComboBox();
            this.SizeCbo = new System.Windows.Forms.ToolStripComboBox();
            this.GrassettoBtn = new System.Windows.Forms.ToolStripButton();
            this.SottolineatoBtn = new System.Windows.Forms.ToolStripButton();
            this.CorsivoBtn = new System.Windows.Forms.ToolStripButton();
            this.ColorBtn = new System.Windows.Forms.ToolStripButton();
            this.Sx = new System.Windows.Forms.ToolStripButton();
            this.Ce = new System.Windows.Forms.ToolStripButton();
            this.Dx = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(581, 249);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = string.Empty;
            this.richTextBox1.SelectionChanged += new System.EventHandler(this.richTextBox1_SelectionChanged);
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.richTextBox1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(581, 249);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(581, 274);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip2);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DimensioneCbo,
            this.SizeCbo,
            this.GrassettoBtn,
            this.SottolineatoBtn,
            this.CorsivoBtn,
            this.ColorBtn,
            this.Sx,
            this.Ce,
            this.Dx});
            this.toolStrip1.Location = new System.Drawing.Point(6, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(400, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // DimensioneCbo
            // 
            this.DimensioneCbo.DropDownWidth = 150;
            this.DimensioneCbo.MaxDropDownItems = 16;
            this.DimensioneCbo.Name = "DimensioneCbo";
            this.DimensioneCbo.Size = new System.Drawing.Size(150, 25);
            this.DimensioneCbo.ToolTipText = "Font";
            this.DimensioneCbo.SelectedIndexChanged += new System.EventHandler(this.DimensioneCbo_SelectedIndexChanged);
            this.DimensioneCbo.Click += new System.EventHandler(this.toolStripComboBox2_Click);
            // 
            // SizeCbo
            // 
            this.SizeCbo.DropDownWidth = 50;
            this.SizeCbo.Items.AddRange(new object[] {
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
            this.SizeCbo.MaxDropDownItems = 14;
            this.SizeCbo.MaxLength = 3;
            this.SizeCbo.Name = "SizeCbo";
            this.SizeCbo.Size = new System.Drawing.Size(75, 25);
            this.SizeCbo.ToolTipText = "Size";
            this.SizeCbo.Leave += new System.EventHandler(this.SizeCbo_Leave);
            this.SizeCbo.SelectedIndexChanged += new System.EventHandler(this.SizeCbo_SelectedIndexChanged);
            // 
            // GrassettoBtn
            // 
            this.GrassettoBtn.CheckOnClick = true;
            this.GrassettoBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.GrassettoBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrassettoBtn.Image = ((System.Drawing.Image)(resources.GetObject("GrassettoBtn.Image")));
            this.GrassettoBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GrassettoBtn.Name = "GrassettoBtn";
            this.GrassettoBtn.Size = new System.Drawing.Size(23, 22);
            this.GrassettoBtn.Text = "G";
            this.GrassettoBtn.Click += new System.EventHandler(this.GrassettoBtn_Click);
            // 
            // SottolineatoBtn
            // 
            this.SottolineatoBtn.CheckOnClick = true;
            this.SottolineatoBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SottolineatoBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SottolineatoBtn.Image = ((System.Drawing.Image)(resources.GetObject("SottolineatoBtn.Image")));
            this.SottolineatoBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SottolineatoBtn.Name = "SottolineatoBtn";
            this.SottolineatoBtn.Size = new System.Drawing.Size(23, 22);
            this.SottolineatoBtn.Text = RedimStatus.S.ToString();
            this.SottolineatoBtn.Click += new System.EventHandler(this.SottolineatoBtn_Click);
            // 
            // CorsivoBtn
            // 
            this.CorsivoBtn.CheckOnClick = true;
            this.CorsivoBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.CorsivoBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CorsivoBtn.Image = ((System.Drawing.Image)(resources.GetObject("CorsivoBtn.Image")));
            this.CorsivoBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CorsivoBtn.Name = "CorsivoBtn";
            this.CorsivoBtn.Size = new System.Drawing.Size(23, 22);
            this.CorsivoBtn.Text = "C";
            this.CorsivoBtn.Click += new System.EventHandler(this.CorsivoBtn_Click);
            // 
            // ColorBtn
            // 
            this.ColorBtn.BackColor = System.Drawing.Color.Black;
            this.ColorBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ColorBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ColorBtn.Name = "ColorBtn";
            this.ColorBtn.Size = new System.Drawing.Size(23, 22);
            this.ColorBtn.Text = "toolStripButton1";
            this.ColorBtn.ToolTipText = "Color";
            this.ColorBtn.Click += new System.EventHandler(this.ColorBtn_Click);
            // 
            // Sx
            // 
            this.Sx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Sx.Checked = true;
            this.Sx.CheckOnClick = true;
            this.Sx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Sx.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Sx.Image = ((System.Drawing.Image)(resources.GetObject("Sx.Image")));
            this.Sx.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Sx.Name = "Sx";
            this.Sx.Size = new System.Drawing.Size(23, 22);
            this.Sx.Text = "toolStripButton1";
            this.Sx.ToolTipText = "Allign Sx.";
            this.Sx.CheckedChanged += new System.EventHandler(this.Sx_CheckedChanged);
            this.Sx.Click += new System.EventHandler(this.Sx_Click);
            // 
            // Ce
            // 
            this.Ce.CheckOnClick = true;
            this.Ce.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Ce.Image = ((System.Drawing.Image)(resources.GetObject("Ce.Image")));
            this.Ce.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Ce.Name = "Ce";
            this.Ce.Size = new System.Drawing.Size(23, 22);
            this.Ce.Text = "toolStripButton2";
            this.Ce.ToolTipText = "Allign Cen.";
            this.Ce.Click += new System.EventHandler(this.Ce_Click);
            // 
            // Dx
            // 
            this.Dx.CheckOnClick = true;
            this.Dx.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Dx.Image = ((System.Drawing.Image)(resources.GetObject("Dx.Image")));
            this.Dx.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Dx.Name = "Dx";
            this.Dx.Size = new System.Drawing.Size(23, 22);
            this.Dx.Text = "toolStripButton3";
            this.Dx.ToolTipText = "Align Dx.";
            this.Dx.Click += new System.EventHandler(this.Dx_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.toolStripButton1});
            this.toolStrip2.Location = new System.Drawing.Point(461, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip2.Size = new System.Drawing.Size(115, 25);
            this.toolStrip2.TabIndex = 1;
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(26, 22);
            this.toolStripButton2.Text = "OK";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton1.ForeColor = System.Drawing.Color.Red;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(48, 22);
            this.toolStripButton1.Text = "Cancel";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // richForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 274);
            this.ControlBox = false;
            this.Controls.Add(this.toolStripContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "richForm2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Editor (RTF)";
            this.Load += new System.EventHandler(this.richForm2_Load);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox SizeCbo;
        private System.Windows.Forms.ToolStripComboBox DimensioneCbo;
        private System.Windows.Forms.ToolStripButton GrassettoBtn;
        private System.Windows.Forms.ToolStripButton CorsivoBtn;
        private System.Windows.Forms.ToolStripButton SottolineatoBtn;
        private System.Windows.Forms.ToolStripButton ColorBtn;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolStripButton Sx;
        private System.Windows.Forms.ToolStripButton Ce;
        private System.Windows.Forms.ToolStripButton Dx;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
    }
}