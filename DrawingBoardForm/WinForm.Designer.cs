using System.Drawing;

namespace DrawingBoard
{
    partial class WinForm
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
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.drawingBoard = new DrawingBoard();
            this.toolBox = new global::DrawingBoard.Toolbox.ToolBox();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.drawingBoard);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.toolBox);
            this.splitContainerMain.Size = new System.Drawing.Size(853, 553);
            this.splitContainerMain.SplitterDistance = 532;
            this.splitContainerMain.SplitterWidth = 3;
            this.splitContainerMain.TabIndex = 37;
            // 
            // userControl11
            // 
            this.drawingBoard.AllowDrop = true;
            this.drawingBoard.AutoScroll = true;
            this.drawingBoard.BackColor = System.Drawing.Color.White;
            this.drawingBoard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.drawingBoard.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            this.drawingBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawingBoard.CanvasOriginX = 0;
            this.drawingBoard.CanvasOriginY = 0;
            this.drawingBoard.GridSize = 0;
            this.drawingBoard.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            this.drawingBoard.IsBoundedCanvas = true;
            this.drawingBoard.ShowPaperOutside = true;
            this.drawingBoard.PaperOutsideColor = Color.FromArgb(230, 230, 255);
            this.drawingBoard.Location = new System.Drawing.Point(0, 0);
            this.drawingBoard.Name = "DrawingBoard";
            this.drawingBoard.Size = new System.Drawing.Size(532, 553);
            this.drawingBoard.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.drawingBoard.TabIndex = 3;
            this.drawingBoard.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            this.drawingBoard.Zoom = 1F;
            // 
            // toolBox1
            // 
            this.toolBox.AutoSize = true;
            this.toolBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolBox.Location = new System.Drawing.Point(0, 0);
            this.toolBox.Margin = new System.Windows.Forms.Padding(4);
            this.toolBox.Name = "ToolBox";
            this.toolBox.Size = new System.Drawing.Size(318, 553);
            this.toolBox.TabIndex = 1;
            // 
            // WinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(853, 553);
            this.Controls.Add(this.splitContainerMain);
            this.Name = "Main DrawingBoard Form";
            this.Text = "DrawingBoard";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.Panel2.PerformLayout();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private DrawingBoard drawingBoard;
        private global::DrawingBoard.Toolbox.ToolBox toolBox;

    }
}

