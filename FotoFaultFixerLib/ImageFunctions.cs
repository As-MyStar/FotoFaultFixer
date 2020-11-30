using System;
using System.Drawing;
using System.Drawing.Imaging;
using FotoFaultFixerLib.ImageProcessor;

namespace FotoFaultFixerLib
{
    public static class ImageFunctions
    {
        public static Bitmap ImpulseNoiseReduction_Universal(Bitmap original, int maxSizeD, int maxSizeL, IProgress<int> progress = null)
        {
            if (original == null)
            {
                throw new ArgumentNullException();
            }

            if (original.Width < 100 || original.Height < 100)
            {
                throw new ArgumentOutOfRangeException("original", "Image needs to be greater than 100 x 100");
            }

            int[] histo = new int[256];
            int maxLight = 0, minLight = 0;

            CImage workingCopy = new CImage(original);
            workingCopy.DeleteBit0();

            GenerateLightHistogram(workingCopy, ref histo, ref maxLight, ref minLight);

            CPnoise PN = new CPnoise(histo, 1000, 4000);
            PN.NoiseFilter(ref workingCopy, histo, minLight, maxLight, maxSizeD, maxSizeL, progress);

            // Clean-up
            for (int i = 0; i < (3 * workingCopy.Width * workingCopy.Height); i++)
            {
                if (workingCopy.Grid[i] == 252 || workingCopy.Grid[i] == 254)
                {
                    workingCopy.Grid[i] = 255;
                }
            }

            return workingCopy.ToBitmap();
        }

        /// <summary>
        /// Converts an Image to Grayscale
        /// </summary>
        /// <param name="original"></param>
        /// <remarks>Modified from https://web.archive.org/web/20110827032809/http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale</remarks>
        /// <returns></returns>
        public static Bitmap ConvertToGrayscale(Bitmap original)
        {
            if (original == null)
            {
                throw new ArgumentNullException();
            }

            //create a new bitmap, same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height, original.PixelFormat);

            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                // create the grayscale ColorMatrix
                ColorMatrix gsMatrix = new ColorMatrix(new float[][]{
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(gsMatrix);

                //draw the original image on the new image
                //using the grayscale color matrix
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }

            return newBitmap;
        }

        public static int MaxC(int R, int G, int B)
        {
            int max;
            if (R * 0.713 > G)
            {
                max = (int)(R * 0.713);
            }
            else
            {
                max = G;
            }

            if (B * 0.527 > max)
            {
                max = (int)(B * 0.527);
            }

            return max;
        }

        private static void GenerateLightHistogram(CImage workingCopy, ref int[] histo, ref int maxLight, ref int minLight)
        {
            for (int i = 0; i < 256; i++)
            {
                histo[i] = 0;
            }

            int light, index;
            for (int y = 0; y < workingCopy.Height; y++)
            {
                for (int x = 0; x < workingCopy.Width; x++)
                {
                    index = x + y * workingCopy.Width; // Index of the pixel (x, y)
                    light = MaxC(workingCopy.Grid[3 * index + 2] & 254,
                                 workingCopy.Grid[3 * index + 1] & 254,
                                 workingCopy.Grid[3 * index + 0] & 254);

                    light = Math.Max(light, 0);
                    light = Math.Min(light, 255);

                    histo[light]++;
                }
            }

            // Populate maxLight and minLight values
            for (maxLight = 255; maxLight > 0; maxLight--)
            {
                if (histo[maxLight] != 0)
                {
                    break;
                }
            }
            for (minLight = 0; minLight < 256; minLight++)
            {
                if (histo[minLight] != 0)
                {
                    break;
                }
            }
        }
    }
}
