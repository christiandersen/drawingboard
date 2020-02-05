using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawingBoard
{
    [Serializable]
    public class PenWR
    {
        public PenWR(Color c)
        {
            color = c;

            var p = new Pen(c);

            alignment = p.Alignment;
            compoundArray = p.CompoundArray;
            //if (p.CustomEndCap!=null)
            // customEndCap = p.CustomEndCap;
            //customStartCap = p.CustomStartCap;
            dashCap = p.DashCap;
            dashOffset = p.DashOffset;
            //dashPattern = p.DashPattern;
            dashStyle = p.DashStyle;
            endCap = p.EndCap;
            lineJoin = p.LineJoin;
            miterLimit = p.MiterLimit;
            startCap = p.StartCap;
            width = p.Width;
        }

        public PenAlignment alignment { get; set; }

        public Color color { get; set; }

        public float[] compoundArray { get; set; }
        public CustomLineCap customEndCap { get; set; }
        public CustomLineCap customStartCap { get; set; }
        public DashCap dashCap { get; set; }
        public float dashOffset { get; set; }
        public float[] dashPattern { get; set; }
        public DashStyle dashStyle { get; set; }
        public LineCap endCap { get; set; }
        public LineJoin lineJoin { get; set; }
        public float miterLimit { get; set; }
        public LineCap startCap { get; set; }
        public float width { get; set; }

        public Pen getPen()
        {
            var p = new Pen(color) {Alignment = alignment};

            //set p properties
            //if (this.compoundArray!=null)
            //    p.CompoundArray = this.compoundArray;
            if (customEndCap != null)
                p.CustomEndCap = customEndCap;
            if (customStartCap != null)
                p.CustomStartCap = customStartCap;
            p.DashCap = dashCap;
            p.DashOffset = dashOffset;
            if (dashPattern != null)
                p.DashPattern = dashPattern;
            p.DashStyle = dashStyle;
            p.EndCap = endCap;
            p.LineJoin = lineJoin;
            p.MiterLimit = miterLimit;
            p.StartCap = startCap;
            p.Width = width;

            return p;
        }
    }
}