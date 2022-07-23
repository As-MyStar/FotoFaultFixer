namespace FotoFaultFixerUI.Extensions
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using FotoFaultFixerLib.ImageProcessing;

    /// <summary>
    /// Extension methods for CImage for Loading to and from .Net BMPs
    /// </summary>
    internal static class CImageExtensions
    {
        /// <summary>
        /// Create a new CImage from a Bitmap
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        internal static CImage FromBitmap(Bitmap bmp)
        {
            if (bmp == null)
            {
                throw new ArgumentNullException(nameof(bmp), "Bmp need to have a value");
            }

            CImage image = null;

            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    image = new CImage(bmp.Width, bmp.Height, 1);
                    BitmapToGrid_8bppIndexed(bmp, ref image);
                    break;
                case PixelFormat.Format24bppRgb:
                    image = new CImage(bmp.Width, bmp.Height, 3);
                    BitmapToGrid_24bppRGB(bmp, ref image);
                    break;
            }

            return image;
        }


        internal static Bitmap ToBitmap(CImage image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image), "Image need to have a value");
            }

            PixelFormat pf = (image.nBytes == 1) ? PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb;

            Bitmap bmp = new Bitmap(image.Width, image.Height, pf);
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            int length = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[length];

            switch (pf)
            {
                case PixelFormat.Format24bppRgb:
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            rgbValues[0 + 3 * x + Math.Abs(bmpData.Stride) * y] = image.Grid[0 + 3 * (x + bmp.Width * y)]; // B
                            rgbValues[1 + 3 * x + Math.Abs(bmpData.Stride) * y] = image.Grid[1 + 3 * (x + bmp.Width * y)]; // G
                            rgbValues[2 + 3 * x + Math.Abs(bmpData.Stride) * y] = image.Grid[2 + 3 * (x + bmp.Width * y)]; // R
                        }
                    }
                    break;
                case PixelFormat.Format8bppIndexed:
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            Color color = bmp.Palette.Entries[image.Grid[3 * (x + bmp.Width * y)]];
                            rgbValues[3 * (x + Math.Abs(bmpData.Stride) * y) + 0] = color.B;
                            rgbValues[3 * (x + Math.Abs(bmpData.Stride) * y) + 1] = color.G;
                            rgbValues[3 * (x + Math.Abs(bmpData.Stride) * y) + 2] = color.R;
                        }
                    }
                    break;
                default:
                    throw new Exception("Unsuitable Pixel Format");
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, length);

            bmp.UnlockBits(bmpData);
            return bmp;
        }


        private static void BitmapToGrid_8bppIndexed(Bitmap bmp, ref CImage image)
        {
            Color color;
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    int i = (x + (bmp.Width * y));
                    color = bmp.GetPixel(x, y);

                    image.Grid[3 * i + 0] = color.B;
                    image.Grid[3 * i + 1] = color.G;
                    image.Grid[3 * i + 2] = color.R;
                }
            }
        }

        private static void BitmapToGrid_24bppRGB(Bitmap bmp, ref CImage image)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            int length = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[length];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, length);

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    image.Grid[0 + 3 * (x + (bmp.Width * y))] = rgbValues[0 + 3 * x + Math.Abs(bmpData.Stride) * y]; // B
                    image.Grid[1 + 3 * (x + (bmp.Width * y))] = rgbValues[1 + 3 * x + Math.Abs(bmpData.Stride) * y]; // G
                    image.Grid[2 + 3 * (x + (bmp.Width * y))] = rgbValues[2 + 3 * x + Math.Abs(bmpData.Stride) * y]; // R
                }
            }

            bmp.UnlockBits(bmpData);
        }
    }
}
