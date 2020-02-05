using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DrawingBoard2.Controls
{
    /// <summary>
    /// Tool Strip CheckBox control.
    /// Since, .NET doesnt have ToolStrip CheckBox control here we make our own ToolStripg CheckBox
    /// control
    /// </summary>
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | 
        ToolStripItemDesignerAvailability.StatusStrip)]
    internal class ToolStripCheckedBox : ToolStripControlHost
    {
        /// <summary>
        /// Event that is fired when check state of the checkbox is changed
        /// </summary>
        public event EventHandler CheckedChanged;

        /// <summary>
        /// Tool Strip CheckBox control.
        /// Since, .NET doesnt have ToolStrip CheckBox control here we make our own ToolStripg CheckBox
        /// control
        /// </summary>
        public ToolStripCheckedBox()
            : base(new CheckBox())
        {
            (this.Control as CheckBox).CheckedChanged += new EventHandler(ToolStripCheckedBox_CheckedChanged);
        }
        /// <summary>
        /// CheckedChanged event
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event args</param>
        public void ToolStripCheckedBox_CheckedChanged(object sender, EventArgs e)
        {
            if(this.CheckedChanged != null)
                this.CheckedChanged(sender, e);
        }
        /// <summary>
        /// Indicates whether CheckBox is checked or not
        /// </summary>
        public bool Checked
        {
            get{ return  (this.Control as CheckBox).Checked;}
            set { (this.Control as CheckBox).Checked = value; }
        }
    }
}
