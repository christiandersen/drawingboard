using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DrawingBoard.Toolbox
{
    public partial class ToolBox : UserControl
    {
        #region Data Members
            private DrawingBoard _drawingBoard;
        #endregion

        //////////////////////////////////////////////////////////////////////////////
        #region Properties
            public DrawingBoard Drawingboard
            {
                get { return _drawingBoard; }
                set
                {
                    if (_drawingBoard == value)                                 // setting a drawingboard that's already set has no effect
                        return;
                    if(_drawingBoard != null)                                   // if there's an already-existing drawingboard.. uncouple it
                    {
                        _drawingBoard.OnOptionChanged -= OnOptionChanged;
                        _drawingBoard.OnShapeSelected -= OnShapeSelected;
                        _drawingBoard.OnKeyOperation -= OnKeyOperation;
                    }
                    _drawingBoard = value;                                      // adopt the fresh drawingboard
                    if (_drawingBoard != null)                                  // .. and if it's a valid object, re-couple our events to it.
                    {
                        _drawingBoard.OnOptionChanged += OnOptionChanged;
                        _drawingBoard.OnShapeSelected += OnShapeSelected;
                        _drawingBoard.OnKeyOperation += OnKeyOperation;
                    }
                }
            }

        #endregion

        //////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor.
        /// Note: to connect to a drawingboard, use the Drawingboard property setter
        /// </summary>
        public ToolBox()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Constructor to connect to an existing drawingBoard
        /// </summary>
        /// <param name="drawingBoard"></param>
        public ToolBox(DrawingBoard drawingBoard)
        {
            InitializeComponent();
            Drawingboard = drawingBoard;
        }

        //////////////////////////////////////////////////////////////////////////////
        #region _drawingBoard's Event Handling

            /// <summary>
            /// when a shape gets selected on the drawingboard, we get notified
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void OnShapeSelected(object sender, PropertyEventArgs e)
            {
                //this.propertyGrid1.SelectedObject = e.ele;
                if (e.element.Length == 0)
                    shapePropertyGrid.SelectedObject = sender;
                else
                    shapePropertyGrid.SelectedObjects = e.element;

                RedoBtn.Enabled = e.Redoable;
                UndoBtn.Enabled = e.Undoable;

                // managmet of 2Front,2back,Delete and Copy buttons
                if (e.element.Length > 0)
                {
                    ToFrontBtn.Enabled = true;
                    ToBackBtn.Enabled = true;
                    DeleteBtn.Enabled = true;
                    CopyBtn.Enabled = true;
                    ObjBtn.Enabled = true;
                }
                else
                {
                    ToFrontBtn.Enabled = false;
                    ToBackBtn.Enabled = false;
                    DeleteBtn.Enabled = false;
                    CopyBtn.Enabled = false;
                    ObjBtn.Enabled = false;
                }
                GroupBtn.Enabled = e.element.Length > 1;
            }

            /// <summary>
            /// When a shape's option gets changed, we get notified
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void OnOptionChanged(object sender, OptionEventArgs e)
            {
                if (e.option != EditOption.Select) 
                    return;
                deselectAll();
                SelectBtn.Checked = true;
            }

            /// <summary>
            /// When the drawingboard issues a key operation, we update our state too
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="keyOperation"></param>
            void OnKeyOperation(object sender, KeyOperation keyOperation)
            {
                switch (keyOperation)
                {
                    case KeyOperation.None:
                        break;
                    case KeyOperation.SelectMode:
                        deselectAll();
                        SelectBtn.Checked = true;
                        break;
                    case KeyOperation.DeSelect:
                        break;
                    case KeyOperation.RectMode:
                        deselectAll();
                        RectBtn.Checked = true;
                        break;
                    case KeyOperation.LineMode:
                        deselectAll();
                        LineBtn.Checked = true;
                        break;
                    case KeyOperation.EllipseMode:
                        deselectAll();
                        EllipseBtn.Checked = true;
                        break;
                    case KeyOperation.RoundRectangleMode:
                        deselectAll();
                        RRectBtn.Checked = true;
                        break;
                    case KeyOperation.SimpleTextMode:
                        deselectAll();
                        break;
                    case KeyOperation.RichTextMode:
                        deselectAll();
                        break;
                    case KeyOperation.ImageMode:
                        deselectAll();
                        ImageBtn.Checked = true;
                        break;
                    case KeyOperation.AcquireImageMode:
                        deselectAll();
                        AcquireImageBtn.Checked = true;
                        break;
                    case KeyOperation.ArcMode:
                        deselectAll();
                        ArcBtn.Checked = true;
                        break;
                    case KeyOperation.PolyMode:
                        deselectAll();
                        PolyBtn.Checked = true;
                        break;
                    case KeyOperation.PenMode:
                        deselectAll();
                        CurveBtn.Checked = true;
                        break;
                    case KeyOperation.Load:
                        break;
                    case KeyOperation.Save:
                        break;
                    case KeyOperation.LoadObjects:
                        break;
                    case KeyOperation.SaveSelectedObjects:
                        break;
                    case KeyOperation.PrintPreview:
                        break;
                    case KeyOperation.Print:
                        break;
                    case KeyOperation.GridOff:
                        break;
                    case KeyOperation.Grid3:
                        break;
                    case KeyOperation.Grid5:
                        break;
                    case KeyOperation.Grid8:
                        break;
                    case KeyOperation.Grid10:
                        break;
                    case KeyOperation.Grid12:
                        break;
                    case KeyOperation.Grid15:
                        break;
                    case KeyOperation.Grid20:
                        break;
                    case KeyOperation.GridCustom:
                        break;
                    case KeyOperation.Undo:
                        break;
                    case KeyOperation.Redo:
                        break;
                    case KeyOperation.Group:
                        deselectAll();
                        GroupBtn.Checked = true;
                        break;
                    case KeyOperation.UnGroup:
                        deselectAll();
                        UnGroupBtn.Checked = true;
                        break;
                    case KeyOperation.PolyMerge:
                        break;
                    case KeyOperation.PolyDeletePoints:
                        break;
                    case KeyOperation.PolyExtendPoints:
                        break;
                    case KeyOperation.PolyMirrorX:
                        break;
                    case KeyOperation.PolyMirrorY:
                        break;
                    case KeyOperation.PolyMirrorXY:
                        break;
                    case KeyOperation.ToFront:
                        deselectAll();
                        ToFrontBtn.Checked = true;
                        break;
                    case KeyOperation.ToBack:
                        deselectAll();
                        ToBackBtn.Checked = true;
                        break;
                    case KeyOperation.DeleteSelected:
                        break;
                    case KeyOperation.CopySelected:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("keyOperation");
                }
            }
        #endregion

        //////////////////////////////////////////////////////////////////////////////
        #region our own GUI event handling

            private void deselectAll()
            {
                SelectBtn.Checked = false;
                RectBtn.Checked = false;
                RRectBtn.Checked = false;
                EllipseBtn.Checked = false;
                LineBtn.Checked = false;
                AcquireImageBtn.Checked = false;
                ArcBtn.Checked = false;
                PolyBtn.Checked = false;
                CurveBtn.Checked = false;

                textBtn.BackColor = RRectBtn.BackColor;
                RTFBtn.Checked = false;
                SimpleTextBtn.Checked = false;
            }

            private void SelectBtn_Click(object sender, EventArgs e)
            {
                deselectAll();
                SelectBtn.Checked = true;
                _drawingBoard.EditOption = EditOption.Select;
            }

            private void RectBtn_Click(object sender, EventArgs e)
            {
                deselectAll();
                RectBtn.Checked = true;
                _drawingBoard.EditOption = EditOption.Rect;
            }

            private void LineBtn_Click(object sender, EventArgs e)
            {
                deselectAll();
                LineBtn.Checked = true;
                _drawingBoard.EditOption = EditOption.Line;
            }

            private void EllipseBtn_Click(object sender, EventArgs e)
            {
                deselectAll();
                EllipseBtn.Checked = true;
                _drawingBoard.EditOption = EditOption.Ellipse;
            }

            private void RRectBtn_Click(object sender, EventArgs e)
            {
                deselectAll();
                RRectBtn.Checked = true;
                _drawingBoard.EditOption = EditOption.RoundRect;
            }

            private void ImageBtn_Click(object sender, EventArgs e)
            {
                deselectAll();
                ImageBtn.Checked = true;
                _drawingBoard.EditOption = EditOption.Image;
            }

            private void AcquireImageBtn_Click(object sender, EventArgs e)
            {
                deselectAll();
                AcquireImageBtn.Checked = true;
                _drawingBoard.EditOption = EditOption.AcquireImage;
            }

            private void toolStripMenuItem1_Click(object sender, EventArgs e)
            {
                _drawingBoard.GridSize = 0;
                //this.s.redraw(true);         
            }

            private void toolStripMenuItem3_Click(object sender, EventArgs e)
            {
                _drawingBoard.GridSize = Convert.ToInt16(toolStripMenuItem3.Text);
                //this.s.redraw(true);
            }

            private void toolStripMenuItem4_Click(object sender, EventArgs e)
            {
                _drawingBoard.GridSize = Convert.ToInt16(toolStripMenuItem4.Text);
                //this.s.redraw(true);
            }

            private void toolStripMenuItem5_Click(object sender, EventArgs e)
            {
                _drawingBoard.GridSize = Convert.ToInt16(toolStripMenuItem5.Text);
                //this.s.redraw(true);
            }

            private void toolStripMenuItem6_Click(object sender, EventArgs e)
            {
                _drawingBoard.GridSize = Convert.ToInt16(toolStripMenuItem6.Text);
                //this.s.redraw(true);
            }

            private void toolStripMenuItem7_Click(object sender, EventArgs e)
            {
                _drawingBoard.GridSize = Convert.ToInt16(toolStripMenuItem7.Text);
                //this.s.redraw(true);
            }

            private void toolStripMenuItem8_Click(object sender, EventArgs e)
            {
                _drawingBoard.GridSize = Convert.ToInt16(toolStripMenuItem8.Text);
                //this.s.redraw(true);
            }

            private void toolStripMenuItem9_Click(object sender, EventArgs e)
            {
                _drawingBoard.GridSize = Convert.ToInt16(toolStripMenuItem9.Text);
                //this.s.redraw(true);
            }

            private void toolStripTextBox1_Click(object sender, EventArgs e)
            {
            }

            private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(toolStripTextBox1.Text))
                    return;
                foreach (var c in toolStripTextBox1.Text)
                    if (!Char.IsNumber(c))
                        return;
                _drawingBoard.GridSize = Convert.ToInt16(toolStripTextBox1.Text);
                _drawingBoard.Refresh();
            }

            private void PenColor_Click(object sender, EventArgs e)
            {
                colorDialog1.Color = PenColor.BackColor;
                colorDialog1.ShowDialog(this);
                PenColor.BackColor = colorDialog1.Color;

                // add code
                _drawingBoard.setPenColor(PenColor.BackColor);

                _drawingBoard.Refresh();
            }

            private void FillColor_Click(object sender, EventArgs e)
            {
                colorDialog1.Color = FillColor.BackColor;
                colorDialog1.ShowDialog(this);
                FillColor.BackColor = colorDialog1.Color;

                // add code
                _drawingBoard.setFillColor(FillColor.BackColor);


                _drawingBoard.Refresh();
            }

            private void PenWidth0Dot5_Click(object sender, EventArgs e)
            {
                _drawingBoard.setPenWidth((float) Convert.ToDouble(PenWidth0Dot5.Text));
            }

            private void PenWidth1_Click(object sender, EventArgs e)
            {
                _drawingBoard.setPenWidth((float) Convert.ToDouble(PenWidth1.Text));
            }

            private void PenWidth1Dot5_Click(object sender, EventArgs e)
            {
                _drawingBoard.setPenWidth((float) Convert.ToDouble(PenWidth1Dot5.Text));
            }

            private void PenWidth2_Click(object sender, EventArgs e)
            {
                _drawingBoard.setPenWidth((float) Convert.ToDouble(PenWidth2.Text));
            }

            private void PenWidth3_Click(object sender, EventArgs e)
            {
                _drawingBoard.setPenWidth((float) Convert.ToDouble(PenWidth3.Text));
            }

            private void PenWidth4_Click(object sender, EventArgs e)
            {
                _drawingBoard.setPenWidth((float) Convert.ToDouble(PenWidth4.Text));
            }

            private void PenWidth5_Click(object sender, EventArgs e)
            {
                _drawingBoard.setPenWidth((float) Convert.ToDouble(PenWidth5.Text));
            }

            private void FillCheckMenuItem_Click(object sender, EventArgs e)
            {
                _drawingBoard.setFilled(FillCheckMenuItem.Checked);
            }

            private void PenWidthTextMenuItem_TextChanged(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(PenWidthTextMenuItem.Text))
                    return;
                foreach (var c in PenWidthTextMenuItem.Text)
                    if (!Char.IsNumber(c))
                        return;
                _drawingBoard.setPenWidth((float) Convert.ToDouble(PenWidthTextMenuItem.Text));
                _drawingBoard.Refresh();
            }

            private void ToFrontBtn_Click(object sender, EventArgs e)
            {
                _drawingBoard.ToFront();
            }

            private void ToBackBtn_Click(object sender, EventArgs e)
            {
                _drawingBoard.ToBack();
            }

            private void toolStripButton3_Click(object sender, EventArgs e)
            {
                _drawingBoard.RemoveSelected();
            }

            private void propertyGrid1_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
            {
                _drawingBoard.propertyChanged();
                _drawingBoard.Refresh();
            }

            private void LoadDrawingBoardBtn_Click(object sender, EventArgs e)
            {
                _drawingBoard.LoadFromFile();
            }

            private void SaveDrawingBoardBtn_Click(object sender, EventArgs e)
            {
                _drawingBoard.SaveToFile();
            }

            private void PrintPreviewBtn_Click(object sender, EventArgs e)
            {
                _drawingBoard.PrintPreview(1f);
            }

            private void printToolStripMenuItem_Click(object sender, EventArgs e)
            {
                _drawingBoard.Print();
            }

            private void CopyBtn_Click(object sender, EventArgs e)
            {
                _drawingBoard.CopyMultiSelected();
            }

            private void UndoBtn_Click(object sender, EventArgs e)
            {
                _drawingBoard.Undo();
                RedoBtn.Enabled = _drawingBoard.RedoEnabled();
                UndoBtn.Enabled = _drawingBoard.UndoEnabled();
            }

            private void RedoBtn_Click(object sender, EventArgs e)
            {
                _drawingBoard.Redo();
                RedoBtn.Enabled = _drawingBoard.RedoEnabled();
                UndoBtn.Enabled = _drawingBoard.UndoEnabled();
            }

            private void ArcBtn_Click(object sender, EventArgs e)
            {
                deselectAll();
                ArcBtn.Checked = true;
                _drawingBoard.EditOption = EditOption.Arc;
            }

            private void ZoomInToolStripMenuItem_Click(object sender, EventArgs e)
            {
                _drawingBoard.ZoomIn();
            }

            private void ZoomOutToolStripMenuItem_Click(object sender, EventArgs e)
            {
                _drawingBoard.ZoomOut();
            }

            private void ZoomDot4ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                var provider = new NumberFormatInfo {NumberDecimalSeparator = "."};
                _drawingBoard.Zoom = (float) Convert.ToDouble(ZoomDot4ToolStripMenuItem.Text, provider);
            }

            private void ZoomDot6ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                var provider = new NumberFormatInfo {NumberDecimalSeparator = "."};
                _drawingBoard.Zoom = (float) Convert.ToDouble(ZoomDot6ToolStripMenuItem.Text, provider);
            }

            private void ZoomDot8ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                var provider = new NumberFormatInfo {NumberDecimalSeparator = "."};
                _drawingBoard.Zoom = (float) Convert.ToDouble(ZoomDot8ToolStripMenuItem.Text, provider);
            }

            private void Zoom1ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                var provider = new NumberFormatInfo {NumberDecimalSeparator = "."};
                _drawingBoard.Zoom = (float) Convert.ToDouble(Zoom1ToolStripMenuItem.Text, provider);
            }

            private void Zoom1Dot2ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                var provider = new NumberFormatInfo {NumberDecimalSeparator = "."};
                _drawingBoard.Zoom = (float) Convert.ToDouble(Zoom1Dot2ToolStripMenuItem.Text, provider);
            }

            private void Zoom1Dot4ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                var provider = new NumberFormatInfo {NumberDecimalSeparator = "."};
                _drawingBoard.Zoom = (float) Convert.ToDouble(Zoom1Dot4ToolStripMenuItem.Text, provider);
            }

            private void Zoom1Dot6ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                var provider = new NumberFormatInfo {NumberDecimalSeparator = "."};
                _drawingBoard.Zoom = (float) Convert.ToDouble(Zoom1Dot6ToolStripMenuItem.Text, provider);
            }

            private void Zoom2ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                var provider = new NumberFormatInfo {NumberDecimalSeparator = "."};
                _drawingBoard.Zoom = (float) Convert.ToDouble(Zoom2ToolStripMenuItem.Text, provider);
            }

            private void Zoom3ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                var provider = new NumberFormatInfo {NumberDecimalSeparator = "."};
                _drawingBoard.Zoom = (float) Convert.ToDouble(Zoom3ToolStripMenuItem.Text, provider);
            }

            private void Zoom4ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                var provider = new NumberFormatInfo {NumberDecimalSeparator = "."};
                _drawingBoard.Zoom = (float) Convert.ToDouble(Zoom4ToolStripMenuItem.Text, provider);
            }


            private void toolStripMenuItemLoadObj_Click(object sender, EventArgs e)
            {
                _drawingBoard.LoadObj();
            }

            private void toolStripMenuItemSaveObj_Click(object sender, EventArgs e)
            {
                _drawingBoard.SaveSelected();
            }

            private void toolStripMenuItemGroup_Click(object sender, EventArgs e)
            {
                _drawingBoard.GroupSelected();
            }

            private void UnGroupBtn_Click(object sender, EventArgs e)
            {
                _drawingBoard.UnGroupSelected();
            }

            private void ZoomBtn_Click(object sender, EventArgs e)
            {
            }

            private void propertyGrid1_Click(object sender, EventArgs e)
            {
            }

            private void PolyBtn_Click(object sender, EventArgs e)
            {
                deselectAll();
                PolyBtn.Checked = true;
                _drawingBoard.EditOption = EditOption.Poly;
            }

            private void toolStripMenuItem33_Click(object sender, EventArgs e)
            {
                _drawingBoard.MergePolygons();
            }

            private void delPointsToolStripMenuItem_Click(object sender, EventArgs e)
            {
                _drawingBoard.DeletePoints();
            }

            private void extPointsToolStripMenuItem_Click(object sender, EventArgs e)
            {
                _drawingBoard.ExtPoints();
            }

            private void xToolStripMenuItem_Click(object sender, EventArgs e)
            {
                _drawingBoard.XMirror();
            }

            private void yToolStripMenuItem_Click(object sender, EventArgs e)
            {
                _drawingBoard.YMirror();
            }

            private void xYToolStripMenuItem_Click(object sender, EventArgs e)
            {
                _drawingBoard.Mirror();
            }

            private void SimpleTextBtn_Click(object sender, EventArgs e)
            {
                deselectAll();
                textBtn.BackColor = Color.LightBlue;
                _drawingBoard.EditOption = EditOption.SimpleText;
            }

            private void RTFBtn_Click(object sender, EventArgs e)
            {
                deselectAll();
                textBtn.BackColor = Color.LightBlue;
                _drawingBoard.EditOption = EditOption.RichText;
            }

            private void CurveBtn_Click(object sender, EventArgs e)
            {
                deselectAll();
                CurveBtn.Checked = true;
                _drawingBoard.EditOption = EditOption.Pen;
            }
        #endregion


    }
}