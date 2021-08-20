using FotoFaultFixerLib.ImageProcessing;
using System;

namespace FotoFaultFixerLib.ImageFunctions
{
    public static class Filters
    {
        public static CImage ImpulseNoiseReduction_Universal(CImage original, int maxSizeD, int maxSizeL, IProgress<int> progressReporter = null)
        {
            Utilities.SetProgress(progressReporter, 0);

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

            CImage workingCopy = new CImage(original.Width, original.Height, original.NBits, original.PixelFormat, original.Grid); //new CImage(original);
            workingCopy.DeleteBit0();

            GenerateLightHistogram(workingCopy, ref histo, ref maxLight, ref minLight);

            CPnoise PN = new CPnoise(histo, 1000, 4000);
            PN.NoiseFilter(ref workingCopy, histo, minLight, maxLight, maxSizeD, maxSizeL, progressReporter);

            // Clean-up
            for (int i = 0; i < (3 * workingCopy.Width * workingCopy.Height); i++)
            {
                if (workingCopy.Grid[i] == 252 || workingCopy.Grid[i] == 254)
                {
                    workingCopy.Grid[i] = 255;
                }
            }

            Utilities.SetProgress(progressReporter, 100);
            return workingCopy;
        }

        private static void GenerateLightHistogram(CImage workingCopy, ref int[] histo, ref int maxLight, ref int minLight)
        {
            for (int i = 0; i < 256; i++)
            {
                histo[i] = 0;
            }

            int light, index;
            var memoizeMaxC = Utilities.MaxC.Memoize();
            for (int y = 0; y < workingCopy.Height; y++)
            {
                for (int x = 0; x < workingCopy.Width; x++)
                {
                    index = x + y * workingCopy.Width; // Index of the pixel (x, y)
                    light = memoizeMaxC(
                        workingCopy.Grid[3 * index + 2] & 254,
                        workingCopy.Grid[3 * index + 1] & 254,
                        workingCopy.Grid[3 * index + 0] & 254
                    );

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
