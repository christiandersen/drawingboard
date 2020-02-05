using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace DrawingBoard2.Controls
{
    /// <summary>
    /// <see cref="System.ComponentModel.Design.DesignerActionList">DesignerActionList</see> that contains
    /// Dock In Parent action
    /// </summary>
    public class DrawingBoardActionList : DesignerActionList
    {
        /// <summary>
        /// <see cref="System.ComponentModel.Design.DesignerActionList">DesignerActionList</see> that contains
        /// Dock In Parent action
        /// </summary>
        /// <param name="designer">User control designeer</param>
        public DrawingBoardActionList(DrawingBoardDesigner designer) : base(designer.Component) { }

        /// <summary>
        /// Sorts action items
        /// </summary>
        /// <returns>Return Sorted action list </returns>
        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionPropertyItem("DockInParent", "Dock in parent"));

            return items;
        }
        /// <summary>
        /// Indicates whether drawing board is set as "Dock In Parent" or not
        /// </summary>
        public bool DockInParent
        {
            get
            {
                return ((DrawingBoard)base.Component).Dock == DockStyle.Fill;
            }
            set
            {
                TypeDescriptor.GetProperties(base.Component)["Dock"].SetValue(base.Component, value ? DockStyle.Fill : DockStyle.None);
            }
        }
    }
}
