using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FotoFaultFixerLib.ImageProcessor
{
    public class CImage
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int NBits { get; set; }
        public byte[] Grid { get; set; }
        public PixelFormat PixelFormat { get; set; }

        #region Input        

        public CImage(int w, int h, int nBits)
        {
            PixelFormat = PixelFormat.Format24bppRgb;
            Width = w;
            Height = h;
            NBits = nBits;
            Grid = new byte[Width * Height * nBits / 8];
        }

        public CImage(int w, int h, int nBits, PixelFormat pf, byte[] img) : this(w, h, nBits)
        {
            PixelFormat = pf;

            for (int i = 0; i < (Width * Height * NBits / 8); i++)
            {
                Grid[i] = img[i];
            }
        }

        public CImage(Bitmap bmp)
        {
            Width = bmp.Width;
            Height = bmp.Height;
            PixelFormat = bmp.PixelFormat;
            Grid = new byte[3 * bmp.Width * bmp.Height];

            if (PixelFormat == PixelFormat.Format8bppIndexed)
            {
                NBits = 8;
                BitmapToGrid_8bppIndexed(bmp);
            }
            else if (PixelFormat == PixelFormat.Format24bppRgb)
            {
                NBits = 24;
                BitmapToGrid_24bppRGB(bmp);
            }
            //else if (PixelFormat == PixelFormat.Format32bppArgb)
            //{
            //    NBits = 32;
            //    BitmapToGrid_32bppARGB(bmp);
            //}
        }

        public void CopyFrom(CImage inp)
        {
            Width = inp.Width;
            Height = inp.Height;
            NBits = inp.NBits;
            PixelFormat = inp.PixelFormat;

            for (int i = 0; i < Width * Height * NBits / 8; i++)
            {
                Grid[i] = inp.Grid[i];
            }
        }

        private void BitmapToGrid_8bppIndexed(Bitmap bmp)
        {
            Color color;
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {                    
                    int i = (x + (bmp.Width * y));
                    color = bmp.GetPixel(x, y);

                    Grid[3 * i + 0] = color.B;
                    Grid[3 * i + 1] = color.G;
                    Grid[3 * i + 2] = color.R;
                }
            }
        }

        private void BitmapToGrid_24bppRGB(Bitmap bmp)
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
                    Grid[0 + 3 * (x + (bmp.Width * y))] = rgbValues[0 + 3 * x + Math.Abs(bmpData.Stride) * y];
                    Grid[1 + 3 * (x + (bmp.Width * y))] = rgbValues[1 + 3 * x + Math.Abs(bmpData.Stride) * y];
                    Grid[2 + 3 * (x + (bmp.Width * y))] = rgbValues[2 + 3 * x + Math.Abs(bmpData.Stride) * y];
                }
            }

            bmp.UnlockBits(bmpData);
        }

        private void BitmapToGrid_32bppARGB(Bitmap bmp)
        {
            throw new NotImplementedException();

            //Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            //BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            //IntPtr ptr = bmpData.Scan0;
            //int length = Math.Abs(bmpData.Stride) * bmp.Height;
            //byte[] rgbaValues = new byte[length];
            //System.Runtime.InteropServices.Marshal.Copy(ptr, rgbaValues, 0, length);

            //for (int y = 0; y < bmp.Height; y++)
            //{
            //    for (int x = 0; x < bmp.Width; x++)
            //    {
            //        Grid[0 + 3 * (x + (bmp.Width * y))] = rgbaValues[0 + 3 * x + Math.Abs(bmpData.Stride) * y];
            //        Grid[1 + 3 * (x + (bmp.Width * y))] = rgbaValues[1 + 3 * x + Math.Abs(bmpData.Stride) * y];
            //        Grid[2 + 3 * (x + (bmp.Width * y))] = rgbaValues[2 + 3 * x + Math.Abs(bmpData.Stride) * y];
            //        Grid[3 + 3 * (x + (bmp.Width * y))] = rgbaValues[3 + 3 * x + Math.Abs(bmpData.Stride) * y];
            //    }
            //}

            //bmp.UnlockBits(bmpData);
        }
        #endregion

        #region Util
        /// <summary>
        /// If "this" is a 8 bit image, then sets the bits 0 and 1 of each pixel to 0.
        /// Else it is a 24 bit one, then sets the bit 0 of green and red chanels to 0.
        /// </summary>
        /// <param name="nbyte"></param>
        /// <returns></returns>
        public void DeleteBit0()
        {
            int nByte = GetNBytesFromPixelFormat(PixelFormat);
            if (nByte == 1)
            {
                for (int i = 0; i < Width * Height; i++)
                {
                    Grid[i] = (byte)(Grid[i] - (Grid[i] % 4));
                }
            }
            else
            {
                for (int i = 0; i < Width * Height; i++)
                {
                    Grid[nByte * i + 2] = (byte)(Grid[nByte * i + 2] & 254);
                    Grid[nByte * i + 1] = (byte)(Grid[nByte * i + 1] & 254);
                }
            }
        }

        private static int GetNBytesFromPixelFormat(PixelFormat pf)
        {
            switch (pf)
            {
                case PixelFormat.Format24bppRgb:
                    return 3;
                case PixelFormat.Format8bppIndexed:
                    return 1;
                default:
                    throw new Exception("Unsuitable Pixel Format");
            }
        }
        #endregion

        #region Output
        public Bitmap ToBitmap()
        {
            Bitmap bmp = new Bitmap(Width, Height, PixelFormat);
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            int length = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[length];

            switch (PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            rgbValues[0 + 3 * x + Math.Abs(bmpData.Stride) * y] = Grid[0 + 3 * (x + bmp.Width * y)];
                            rgbValues[1 + 3 * x + Math.Abs(bmpData.Stride) * y] = Grid[1 + 3 * (x + bmp.Width * y)];
                            rgbValues[2 + 3 * x + Math.Abs(bmpData.Stride) * y] = Grid[2 + 3 * (x + bmp.Width * y)];
                        }
                    }
                    break;
                case PixelFormat.Format8bppIndexed:
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            Color color = bmp.Palette.Entries[Grid[3 * (x + bmp.Width * y)]];
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
        #endregion
    }
}
