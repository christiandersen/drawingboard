using System;
using System.Windows.Forms;

namespace DrawingBoard
{
    public partial class WinForm : Form
    {
        public WinForm()
        {
            InitializeComponent();
            myInit();
        }

        private void myInit()
        {
            // Link our toolBox to the drawingBoard control
            toolBox.Drawingboard = drawingBoard;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if(DesignMode)
                return;

            // do OnLoad init here..
        }
    }
}