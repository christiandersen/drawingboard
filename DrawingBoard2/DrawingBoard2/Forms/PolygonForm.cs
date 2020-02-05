using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DrawingBoard2.Helpers;

namespace DrawingBoard2.Forms
{
    /// <summary>
    /// Form that gets edge number of custom polygon
    /// </summary>
    public partial class PolygonForm : Form
    {
        private Region region = DrawingBoard2.Region.Empty;
        private List<PointF> pointList = null;

        /// <summary>
        /// Region of polygon
        /// </summary>
        public Region PolgonRegion
        {
            set { this.region = value; }
        }
        /// <summary>
        /// List of <see cref="System.Drawing.PointF">Points</see>
        /// </summary>
        public List<PointF> PointsList
        {
            get { return pointList; }
        }
        /// <summary>
        /// Form that gets edge number of custom polygon
        /// </summary>
        public PolygonForm()
        {
            InitializeComponent();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.pointList = PolygonHelper.CalculatePolygonPoints(Convert.ToInt32(cbEdgeCount.SelectedItem),
                region.Width / 2, 0, region.MidPointF);
            this.Close();
        }
    }
}
