using System;
using System.Drawing;
using System.Reflection;
using FotoFaultFixerLib.ImageProcessor;

namespace FotoFaultFixerLib
{
    public static class ImageUtils
    {
        public static Bitmap ImpulseNoiseReduction_Universal(Bitmap original, int maxSizeD, int maxSizeL, IProgress<int> progress = null)
        {
            CImage workingCopy = new CImage(original);
            workingCopy.DeleteBit0();

            int maxLight, minLight;
            int[] histo = new int[256];
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
    }
}
