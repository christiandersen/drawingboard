using System;
using System.Windows.Forms;

namespace DrawingBoard
{
    public partial class PrintPreview : Form
    {
        public PrintPreview()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            docToPrint.Print();
        }

        private void Printpreview_Load(object sender, EventArgs e)
        {
        }

        private void Printpreview_Resize(object sender, EventArgs e)
        {
            PrintPreviewControl1.Width = Width - 10;
            PrintPreviewControl1.Height = Height - toolStrip1.Height - 37;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            PrintPreviewControl1.Zoom = (float) Convert.ToDouble(toolStripMenuItem2.Text)/100;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            PrintPreviewControl1.Zoom = (float) Convert.ToDouble(toolStripMenuItem3.Text)/100;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            PrintPreviewControl1.Zoom = (float) Convert.ToDouble(toolStripMenuItem4.Text)/100;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            PrintPreviewControl1.Zoom = (float) Convert.ToDouble(toolStripMenuItem5.Text)/100;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            PrintPreviewControl1.Zoom = (float) Convert.ToDouble(toolStripMenuItem6.Text)/100;
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            PrintPreviewControl1.Zoom = (float) Convert.ToDouble(toolStripMenuItem7.Text)/100;
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            PrintPreviewControl1.Zoom = (float) Convert.ToDouble(toolStripMenuItem8.Text)/100;
        }

        private void PrintPreviewControl1_MouseMove(object sender, MouseEventArgs e)
        {
        }
    }
}