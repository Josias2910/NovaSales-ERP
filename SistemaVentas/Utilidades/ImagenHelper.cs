using System;
using System.Collections.Generic;
using System.Text;

namespace CapaPresentacion.Utilidades
{
    public static class ImagenHelper
    {
        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            if (imageIn == null) return null;
            using (var ms = new System.IO.MemoryStream())
            {
                using (var bmp = new System.Drawing.Bitmap(imageIn))
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                }
                return ms.ToArray();
            }
        }

        public static System.Drawing.Image ByteArrayToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            try
            {
                using (var ms = new System.IO.MemoryStream(bytes))
                {
                    return System.Drawing.Image.FromStream(ms);
                }
            }
            catch { return null; }
        }
    }
}
