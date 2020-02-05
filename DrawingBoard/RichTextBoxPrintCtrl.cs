using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RichTextBoxPrintCtrl
{
    public class RichTextBoxPrintCtrl : RichTextBox
    {
        //START : make this control transparent

        // END


        //Convert the unit used by the .NET framework (1/100 inch) 
        //and the unit used by Win32 API calls (twips 1/1440 inch)
        private const double DEFAULT_ONE_INCH = 14.4;

        private const int EM_FORMATRANGE = WM_USER + 57;
        private const int WM_USER = 0x0400;

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

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("USER32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        // Render the contents of the RichTextBox for printing
        //	Return the last character printed + 1 (printing start from this point for next page)
        public int Print(int charFrom, int charTo, PrintPageEventArgs e)
        {
            IntPtr lparam = IntPtr.Zero;
            IntPtr hdc = IntPtr.Zero;
            try
            {               
                hdc = e.Graphics.GetHdc();
                if (hdc != IntPtr.Zero)
                {
                    //Calculate the area to render and print
                    RECT rectToPrint;
                    rectToPrint.Top = (int)(e.MarginBounds.Top * DEFAULT_ONE_INCH);
                    rectToPrint.Bottom = (int)(e.MarginBounds.Bottom * DEFAULT_ONE_INCH);
                    rectToPrint.Left = (int)(e.MarginBounds.Left * DEFAULT_ONE_INCH);
                    rectToPrint.Right = (int)(e.MarginBounds.Right * DEFAULT_ONE_INCH);

                    //Calculate the size of the page
                    RECT rectPage;
                    rectPage.Top = (int)(e.PageBounds.Top * DEFAULT_ONE_INCH);
                    rectPage.Bottom = (int)(e.PageBounds.Bottom * DEFAULT_ONE_INCH);
                    rectPage.Left = (int)(e.PageBounds.Left * DEFAULT_ONE_INCH);
                    rectPage.Right = (int)(e.PageBounds.Right * DEFAULT_ONE_INCH); 

                    FORMATRANGE fmtRange;
                    fmtRange.chrg.cpMax = charTo; //Indicate character from to character to 
                    fmtRange.chrg.cpMin = charFrom;
                    fmtRange.hdc = hdc; //Use the same DC for measuring and rendering
                    fmtRange.hdcTarget = hdc; //Point at printer hDC
                    fmtRange.rc = rectToPrint; //Indicate the area on page to print
                    fmtRange.rcPage = rectPage; //Indicate size of page

                    //Get the pointer to the FORMATRANGE structure in memory
                    lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fmtRange));
                    if (lparam != IntPtr.Zero)
                    {
                        Marshal.StructureToPtr(fmtRange, lparam, false);

                        //Send the rendered data for printing 
                        IntPtr result = SendMessage(Handle, EM_FORMATRANGE, new IntPtr(1), lparam);

                        //Return last + 1 character printer
                        return result.ToInt32();
                    }
                }
                return 0;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Something went wrong: " + ex.Message);
                return 0;
            }
            finally
            {
                //Free the block of memory allocated
                if (lparam != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(lparam);

                //Release the device context handle obtained by a previous call
                if (hdc != IntPtr.Zero)
                    e.Graphics.ReleaseHdc(hdc);
            }

        }

        public int Draw(int charFrom, int charTo, Graphics gfx, int x, int y, int x1, int y1, double conversion,
                        double conversionY)
        {
            IntPtr lparam = IntPtr.Zero;
            IntPtr hdc = IntPtr.Zero;
            try
            {
                hdc = gfx.GetHdc();
                if (hdc != IntPtr.Zero)
                {
                    //Calculate the area to render and print
                    RECT rectToPrint;
                    rectToPrint.Top = (int)(y * conversionY);
                    rectToPrint.Bottom = (int)(y1 * conversionY);
                    rectToPrint.Left = (int)(x * conversion);
                    rectToPrint.Right = (int)(decimal)(x1 * conversion);

                    //Calculate the size of the page
                    RECT rectPage;
                    rectPage.Top = (int)(y * conversionY);
                    rectPage.Bottom = (int)(y1 * conversionY);
                    rectPage.Left = (int)(x * conversion);
                    rectPage.Right = (int)(x1 * conversion); 
                    
                    FORMATRANGE fmtRange;
                    fmtRange.chrg.cpMax = charTo; //Indicate character from to character to 
                    fmtRange.chrg.cpMin = charFrom;
                    fmtRange.hdc = hdc; //Use the same DC for measuring and rendering
                    fmtRange.hdcTarget = hdc; //Point at printer hDC
                    fmtRange.rc = rectToPrint; //Indicate the area on page to print
                    fmtRange.rcPage = rectPage; //Indicate size of page

                    //Get the pointer to the FORMATRANGE structure in memory
                    lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fmtRange));
                    if (lparam != IntPtr.Zero)
                    {
                        Marshal.StructureToPtr(fmtRange, lparam, false);

                        //Send the rendered data for printing 
                        IntPtr result = SendMessage(Handle, EM_FORMATRANGE, new IntPtr(1), lparam);

                        //Return last + 1 character printer
                        return result.ToInt32();
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong: " + ex.Message);
                return 0;
            }
            finally
            {
                //Free the block of memory allocated
                if (lparam != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(lparam);

                //Release the device context handle obtained by a previous call
                if (hdc != IntPtr.Zero)
                    gfx.ReleaseHdc(hdc);
            }

        }

        #region Nested type: CHARRANGE

        [StructLayout(LayoutKind.Sequential)]
        private struct CHARRANGE
        {
            public int cpMin; //First character of range (0 for start of doc)
            public int cpMax; //Last character of range (-1 for end of doc)
        }

        #endregion

        #region Nested type: FORMATRANGE

        [StructLayout(LayoutKind.Sequential)]
        private struct FORMATRANGE
        {
            public IntPtr hdc; //Actual DC to draw on
            public IntPtr hdcTarget; //Target DC for determining text formatting
            public RECT rc; //Region of the DC to draw to (in twips)
            public RECT rcPage; //Region of the whole DC (page size) (in twips)
            public CHARRANGE chrg; //Range of text to draw (see earlier declaration)
        }

        #endregion

        #region Nested type: RECT

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion
    }
}