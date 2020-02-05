using System;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;

using DrawingBoard2.Forms;

namespace DrawingBoard2
{
    /// <summary>
    /// Property editor for RTF(RichTextBox content)
    /// </summary>
    public class RTFTypeEditor : UITypeEditor
    {
        /// <summary>
        /// Return Edit Style of the RTF type
        /// </summary>
        /// <param name="context">context</param>
        /// <returns>Drop Down Type Editor Edit Style</returns>
        public override UITypeEditorEditStyle GetEditStyle(
            ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        /// <summary>
        /// Edits RTF value
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="provider">Provider</param>
        /// <param name="value">RTF value of the property</param>
        /// <returns>Changed value of the property</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService = provider.GetService(
                typeof(IWindowsFormsEditorService)) as
                IWindowsFormsEditorService;

            if (editorService != null)
            {
                RichTextBoxForm rtbForm = new RichTextBoxForm(false);
                rtbForm.RichTextBox.Rtf = value.ToString();
                rtbForm.editorService = editorService;
                editorService.DropDownControl(rtbForm);

                value = rtbForm.RichTextBox.Rtf;
                rtbForm.Close();
            }
            return value;
        }
    }
}
