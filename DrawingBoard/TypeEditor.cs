using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace DrawingBoard
{

    /* REF:
     http://www.codeproject.com/KB/cpp/dropdownproperties.aspx
    */


    public class myTypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(
            ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            var wfes = provider.GetService(
                typeof(IWindowsFormsEditorService)) as
                IWindowsFormsEditorService;

            if (wfes != null)
            {
                var frm = new PropEditForm();

                /*
                frm.textBox1.Text = (string)value;
                frm._wfes = wfes;
                wfes.DropDownControl(frm);                
                value = frm.BarValue;
                frm.Close();
                */

                //frm.textBox1.Text = (string)value;
                frm.drawingBoard.addPointSet(value as List<PointWr>);
                
                frm._wfes = wfes;
                wfes.DropDownControl(frm);
                if (frm.BarValue!=null)
                    value = frm.BarValue;
                frm.Close();


            }
            return value;
        }
    }

    public class PenTypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(
            ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            var wfes = provider.GetService(
                typeof(IWindowsFormsEditorService)) as
                IWindowsFormsEditorService;

            if (wfes != null)
            {
                var frm = new PenPropEditForm {penWr = ((PenWR) value)};

                /*
                frm.textBox1.Text = (string)value;
                frm._wfes = wfes;
                wfes.DropDownControl(frm);                
                value = frm.BarValue;
                frm.Close();
                */


                //frm.vectShapes1.addPointSet(value as List<PointWr>);
                frm.propertyGrid1.SelectedObject = frm.penWr;

                frm._wfes = wfes;
                wfes.DropDownControl(frm);
                if (frm.penWr != null)
                    value = frm.penWr;
                frm.Close();
            }
            return value;
        }
    }


}
