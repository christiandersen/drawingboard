using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using WIA;

namespace DrawingBoard.WIALib
{
    public static class WiaLib
    {
        /// <summary>
        /// Acquire an image through the WIA interface
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static Bitmap AcquireImage(out string errMsg)
        {
            errMsg = "All OK!";
            try
            {
                const string wiaFormatJPEG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";
                var wiaDiag = new CommonDialogClass();
                var wiaImage = wiaDiag.ShowAcquireImage(WiaDeviceType.UnspecifiedDeviceType,
                                                        WiaImageIntent.GrayscaleIntent,
                                                        WiaImageBias.MaximizeQuality,
                                                        wiaFormatJPEG, true, true, false);
                if (wiaImage == null)
                {
                    errMsg = "Did not acquire data";
                    return null;
                }
                var vector = wiaImage.FileData;
                var img = Image.FromStream(new MemoryStream((byte[])vector.get_BinaryData()));
                if (img is Bitmap)
                    return img as Bitmap;
                var bmp = new Bitmap(img);
                return bmp;
            }
            catch (Exception ex)
            {
                errMsg = "ScanImage failed : " + ex.Message;
                Debug.WriteLine(errMsg);
                return null;
            }
        }

    }
}
