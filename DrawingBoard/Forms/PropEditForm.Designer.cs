using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.Design;

namespace DrawingBoard
{
    partial class PropEditForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.drawingBoard = new DrawingBoard();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(97, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Validated += new System.EventHandler(this.textBox1_Validated);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(115, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 29);
            this.button1.TabIndex = 1;
            this.button1.Text = "ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(275, 7);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 29);
            this.button2.TabIndex = 3;
            this.button2.Text = "AddColorToBrush";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // vectShapes1
            // 
            this.drawingBoard.IsBoundedCanvas = false;
            this.drawingBoard.ShowPaperOutside = true;
            this.drawingBoard.PaperOutsideColor = Color.FromArgb(230, 230, 255);
            this.drawingBoard.AllowDrop = true;
            this.drawingBoard.BackColor = System.Drawing.Color.White;
            this.drawingBoard.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            this.drawingBoard.CanvasOriginX = 0;
            this.drawingBoard.CanvasOriginY = 0;
            this.drawingBoard.GridSize = 0;
            this.drawingBoard.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            this.drawingBoard.Location = new System.Drawing.Point(9, 40);
            this.drawingBoard.Name = "drawingBoard";
            this.drawingBoard.Size = new System.Drawing.Size(245, 166);
            this.drawingBoard.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.drawingBoard.TabIndex = 2;
            this.drawingBoard.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            this.drawingBoard.Zoom = 1F;
            // 
            // PropEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 266);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.drawingBoard);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropEditForm";
            this.ShowInTaskbar = false;
            this.Text = "PropEditForm";
            this.Load += new System.EventHandler(this.PropEditForm_Load);
            this.Closed += new System.EventHandler(this._Closed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion        
        
        //public string BarValue;
        public List<PointWr> BarValue;
        public System.Windows.Forms.TextBox textBox1;
        public IWindowsFormsEditorService _wfes;
        private System.Windows.Forms.Button button1;
        public DrawingBoard drawingBoard;
        private System.Windows.Forms.Button button2;
    }
}