using System;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;

namespace DrawingBoard2.Controls
{
    /// <summary>
    /// Control designer for drawing board which will handle actions like "Dock In Parent"
    /// </summary>
    public class DrawingBoardDesigner : ControlDesigner
    {
        private DesignerActionListCollection _actionLists;

        /// <summary>
        /// Action list of the designer
        /// </summary>
        public override System.ComponentModel.Design.DesignerActionListCollection ActionLists
        {
            get
            {
                if (_actionLists == null)
                {
                    _actionLists = new DesignerActionListCollection();
                    _actionLists.Add(new DrawingBoardActionList(this));
                }
                return _actionLists;
            }
        }
    }
}
