using System;
using System.Windows.Forms;

namespace DrawingBoard
{
    public partial class PropEditForm : Form
    {
        public PropEditForm()
        {
            InitializeComponent();
            TopLevel = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BarValue = drawingBoard.getPointSet();
            Close();
        }


        private void _Closed(object sender, EventArgs e)
        {
            _wfes.CloseDropDown();
        }

        private void PropEditForm_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            //BarValue = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            drawingBoard.EditOption = EditOption.Ellipse;
        }
    }
}