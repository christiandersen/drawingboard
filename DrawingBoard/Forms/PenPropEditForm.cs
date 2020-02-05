using System;
using System.Windows.Forms;

namespace DrawingBoard
{
    public partial class PenPropEditForm : Form
    {
        public PenPropEditForm()
        {
            InitializeComponent();
            TopLevel = false;
        }


        private void _Closed(object sender, EventArgs e)
        {
            _wfes.CloseDropDown();
        }

        private void PropEditForm_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}