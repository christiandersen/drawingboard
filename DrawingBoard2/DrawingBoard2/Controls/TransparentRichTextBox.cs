using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DrawingBoard2.Controls
{
    /// <summary>
    /// RichTextBox control that has trasparent background
    /// </summary>
    internal partial class TransparentRichTextBox : RichTextBox
    {
        #region Variables
        private const float pxToInch = 14.4f; //Pixel to inch coefficient
        private const int WM_USER = 0x0400;
        private const int EM_FORMATRANGE = WM_USER + 57;
        #endregion

        #region DLL Import
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr LoadLibrary(string lpFileName);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CHARRANGE
        {
            public int cpMin;         //First character of range (0 for start of doc)
            public int cpMax;           //Last character of range (-1 for end of doc)
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct FORMATRANGE
        {
            public IntPtr hdc;             //Actual DC to draw on
            public IntPtr hdcTarget;       //Target DC for determining text formatting
            public RECT rc;                //Region of the DC to draw to (in twips)
            public RECT rcPage;            //Region of the whole DC (page size) (in twips)
            public CHARRANGE chrg;         //Range of text to draw (see earlier declaration)
        }

        [DllImport("USER32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        #endregion

        #region Properties
        /// <summary>
        /// RichTextBox CreateParams , overriden for transparency
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams prams = base.CreateParams;
                if (LoadLibrary("msftedit.dll") != IntPtr.Zero)
                {
                    prams.ExStyle |= 0x020; // transparent
                    prams.ClassName = "RICHEDIT50W";
                }
                return prams;
            }
        }
        #endregion 

        #region Methods
        /// <summary>
        /// Draws richtextbox on graphics object
        /// </summary>
        /// <param name="graphObj">Graphics object to draw on</param>
        /// <param name="region">Region on the graphics object that richtextbox will be drawn on</param>
        /// <param name="conversionX">Value to be used convertion inch to pixel on X coordinate</param>
        /// <param name="conversionY">Value to be used convertion inch to pixel on Y coordinate</param>
        public void Draw(Graphics graphObj, Region region, double conversionX, double conversionY)
        {
            //Calculate the area to render and print
            RECT rectToPrint;
            rectToPrint.Top = (int)(region.Y0 * conversionY);
            rectToPrint.Bottom = (int)(region.Y1 * conversionY);
            rectToPrint.Left = (int)(region.X0 * conversionX);
            rectToPrint.Right = (int)(decimal)(region.X1 * conversionX);

            //Calculate the size of the page
            RECT rectPage;
            rectPage.Top = (int)(region.Y0 * conversionY);
            rectPage.Bottom = (int)(region.Y1 * conversionY);
            rectPage.Left = (int)(region.X0 * conversionX);
            rectPage.Right = (int)(region.X1 * conversionX);

            IntPtr hdc = graphObj.GetHdc();

            FORMATRANGE fmtRange;
            fmtRange.chrg.cpMax = this.TextLength;		//Indicate character from to character to 
            fmtRange.chrg.cpMin = 0;
            fmtRange.hdc = hdc;                    //Use the same DC for measuring and rendering
            fmtRange.hdcTarget = hdc;              //Point at printer hDC
            fmtRange.rc = rectToPrint;             //Indicate the area on page to print
            fmtRange.rcPage = rectPage;            //Indicate size of page

            IntPtr wparam = IntPtr.Zero;
            wparam = new IntPtr(1);

            //Get the pointer to the FORMATRANGE structure in memory
            IntPtr lparam = IntPtr.Zero;
            lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fmtRange));
            Marshal.StructureToPtr(fmtRange, lparam, false);
            //Send the rendered data for printing 
            SendMessage(Handle, EM_FORMATRANGE, wparam, lparam);
            //Free the block of memory allocated
            Marshal.FreeCoTaskMem(lparam);
            //Release the device context handle obtained by a previous call
            graphObj.ReleaseHdc(hdc);
        }
        #endregion
    }
}
