using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DrawingBoard
{
    public partial class RichtTextForm : Form
    {
        //private RichTextBox OuterRichText;

        public bool confermato;

        public RichtTextForm()
        {
            InitializeComponent();
        }

        public RichtTextForm(string rtfIn)
        {
            InitializeComponent();
            richTextBox1.Rtf = rtfIn;
        }

        public String SelectedFontFamily
        {
            get
            {
                // Sanity check the combobox before attempting to use it.
                if (DimensioneCbo == null)
                    return null;

                // Get the index of the currently selected item.
                int index = DimensioneCbo.SelectedIndex;

                // Sanity check the index before attempting to use it.
                if (index == -1)
                    return null;

                // Return the corresponding font family.
                return ((String) DimensioneCbo.SelectedItem);
            } // End get

            set
            {
                // Sanity check the combobox before attempting to use it.
                if (DimensioneCbo == null)
                    return;

                int index = -1;

                // Should we look for a matching item?
                if (value != null)
                    index = DimensioneCbo.FindString(value, -1);

                // Select the item.
                DimensioneCbo.SelectedIndex = index;
            } // End set
        } // End SelectedFontFamily


        public int SelectedSize
        {
            get
            {
                if (SizeCbo.Text == string.Empty)
                    return 8;
                // Return the corresponding font family.
                return (Convert.ToInt16(SizeCbo.Text));
            } // End get
            set
            {
                // Sanity check the combobox before attempting to use it.
                if (SizeCbo == null)
                    return;

                SizeCbo.Text = value.ToString();
            } // End set
        }

        private void toolStripComboBox2_Click(object sender, EventArgs e)
        {
        }

        public string getRtf()
        {
            return richTextBox1.Rtf;
        }

        public void setRtf(string rtfIn)
        {
            richTextBox1.Rtf = rtfIn;
        }


        private void richForm2_Load(object sender, EventArgs e)
        {
            //this.DimensioneCbo.


            FontFamily[] ff = FontFamily.Families;
            // Loop and create a sample of each font.
            for (int x = 0; x < ff.Length; x++)
            {
                Font font = null;

                // Create the font - based on the styles available.
                if (ff[x].IsStyleAvailable(FontStyle.Regular))
                    font = new Font(
                        ff[x].Name,
                        DimensioneCbo.Font.Size
                        );
                else if (ff[x].IsStyleAvailable(FontStyle.Bold))
                    font = new Font(
                        ff[x].Name,
                        DimensioneCbo.Font.Size,
                        FontStyle.Bold
                        );
                else if (ff[x].IsStyleAvailable(FontStyle.Italic))
                    font = new Font(
                        ff[x].Name,
                        DimensioneCbo.Font.Size,
                        FontStyle.Italic
                        );
                else if (ff[x].IsStyleAvailable(FontStyle.Strikeout))
                    font = new Font(
                        ff[x].Name,
                        DimensioneCbo.Font.Size,
                        FontStyle.Strikeout
                        );
                else if (ff[x].IsStyleAvailable(FontStyle.Underline))
                    font = new Font(
                        ff[x].Name,
                        DimensioneCbo.Font.Size,
                        FontStyle.Underline
                        );

                // Should we add the item?
                if (font != null)
                    DimensioneCbo.Items.Add(font.FontFamily.Name);
            } // End for all the fonts.
            DimensioneCbo.SelectedIndex = 0;
        }


        // End SelectedFontFamily


        private void DimensioneCbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(this,"You selected: " + this.SelectedFontFamily);
            setFont();
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionColor.Name != "0")
                ColorBtn.BackColor = richTextBox1.SelectionColor;
            if (richTextBox1.SelectionFont != null)
            {
                SelectedFontFamily = richTextBox1.SelectionFont.FontFamily.Name;
                SelectedSize = (int) richTextBox1.SelectionFont.Size;
                GrassettoBtn.Checked = richTextBox1.SelectionFont.Bold;
                CorsivoBtn.Checked = richTextBox1.SelectionFont.Italic;
                SottolineatoBtn.Checked = richTextBox1.SelectionFont.Underline;
            }


            Ce.Checked = (richTextBox1.SelectionAlignment == HorizontalAlignment.Center);
            Sx.Checked = (richTextBox1.SelectionAlignment == HorizontalAlignment.Left);
            Dx.Checked = (richTextBox1.SelectionAlignment == HorizontalAlignment.Right);

/*            if (this.richTextBox1.SelectionAlignment == HorizontalAlignment.Center)
            {
                Sx.Checked = false;
                Dx.Checked = false;
                Ce.Checked = true;
            }
            if (this.richTextBox1.SelectionAlignment == HorizontalAlignment.)
            {
                Sx.Checked = false;
                Dx.Checked = false;
                Ce.Checked = true;
            }
            if (this.richTextBox1.SelectionAlignment == HorizontalAlignment.Center)
            {
                Sx.Checked = false;
                Dx.Checked = false;
                Ce.Checked = true;
            }*/
        }

        private void GrassettoBtn_Click(object sender, EventArgs e)
        {
            setFont();
        }


        private void setFont()
        {
            FontStyle fs = 0;
            
            if (GrassettoBtn.Checked)
                fs |= FontStyle.Bold;
            if (CorsivoBtn.Checked)
                fs |= FontStyle.Italic;
            if (SottolineatoBtn.Checked)
                fs |= FontStyle.Underline;
            //FontStyle fs = FontStyle.Bold | FontStyle.Underline | FontStyle.Italic;
            try
            {
                var f = new Font(SelectedFontFamily, SelectedSize, fs);
                richTextBox1.SelectionFont = f;
                richTextBox1.SelectionColor = ColorBtn.BackColor;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            richTextBox1.Focus();
        }

        private void SottolineatoBtn_Click(object sender, EventArgs e)
        {
            setFont();
        }

        private void CorsivoBtn_Click(object sender, EventArgs e)
        {
            setFont();
        }

        private void SizeCbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFont();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void SizeCbo_TextUpdate(object sender, EventArgs e)
        {
            setFont();
        }

        private void SizeCbo_Leave(object sender, EventArgs e)
        {
            setFont();
        }

        private void ColorBtn_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = ColorBtn.BackColor;
            colorDialog1.ShowDialog(this);
            ColorBtn.BackColor = colorDialog1.Color;

            setFont();
        }

        private void Sx_Click(object sender, EventArgs e)
        {
            Sx.Checked = true;
            Dx.Checked = false;
            Ce.Checked = false;

            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
            richTextBox1.Focus();
        }

        private void Sx_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Ce_Click(object sender, EventArgs e)
        {
            Sx.Checked = false;
            Dx.Checked = false;
            Ce.Checked = true;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.Focus();
        }

        private void Dx_Click(object sender, EventArgs e)
        {
            Sx.Checked = false;
            Dx.Checked = true;
            Ce.Checked = false;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
            richTextBox1.Focus();
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            confermato = true;
            Visible = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            confermato = false;
            Visible = false;
        }
    }
}