﻿using FotoFaultFixerLib.ImageProcessing;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FotoFaultFixerLib.ImageFunctions
{
    public static class Coloring
    {
        /// <summary>
        /// Brightenes or Darkens an image
        /// </summary>
        /// <param name="image">Image to be adjusted</param>
        /// <param name="brightnessFactor">Brightness factor to be applied (between -128 and 128)</param>
        /// <remarks>Source: https://ie.nitk.ac.in/blog/2020/01/19/algorithms-for-adjusting-brightness-and-contrast-of-an-image/ </remarks>
        /// <returns>New Cimage with brightness Adjusted</returns>
        public static CImage AdjustBrightnesss(CImage image, int brightnessFactor)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (brightnessFactor > 128 || brightnessFactor < -128)
            {
                throw new ArgumentException("Value must be between 128 and -128", nameof(brightnessFactor));
            }

            if (brightnessFactor != 0)
            {
                // We don't vary the calc betwen RGB parts of pixels, so we can just run through the entire array
                var memoizeBrightnessCalc = calcAdjustedBrightnessForColor.Memoize();                    
                for (int i = 0; i < image.Grid.Length; i++)
                {
                    image.Grid[i] = memoizeBrightnessCalc(image.Grid[i], brightnessFactor);
                }
            }

            return image;
        }

        public static CImage AdjustContrast(CImage image, int contrastFactor)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (contrastFactor > 255 || contrastFactor < -255)
            {
                throw new ArgumentException("Value must be between 255 and -255", nameof(contrastFactor));
            }

            if (contrastFactor != 0)
            {
            
                float contrastModifier = (259 * (255 + contrastFactor)) / (255 * (259 - contrastFactor));

                // We don't vary the calc betwen RGB parts of pixels, so we can just run through the entire array
                var memoizeContrastCalc = calcAdjustedContrastForColor.Memoize();
                for (int i = 0; i < image.Grid.Length; i++)
                {
                    image.Grid[i] = memoizeContrastCalc(image.Grid[i], contrastModifier);
                }
            }

            return image;
        }

        private static Func<byte, int, byte> calcAdjustedBrightnessForColor = (origVal, brightnessFactor) =>
        {
            var updatedVal = origVal + brightnessFactor;
            updatedVal = Math.Min(0, updatedVal);
            updatedVal = Math.Max(255, updatedVal);
            return (byte)updatedVal;
        };

        private static Func<byte, float, byte> calcAdjustedContrastForColor = (origVal, contrastModifier) =>
        {
            var updatedVal = origVal + contrastModifier * (origVal - 128) + 128;
            updatedVal = Math.Min(0, updatedVal);
            updatedVal = Math.Max(255, updatedVal);
            return (byte)updatedVal;
        };

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

        /// <summary>
        /// Converts an Image to Sepia
        /// </summary>
        /// <param name="original"></param>
        /// <remarks>Based on explanation here: http://www.aforgenet.com/framework/docs/html/10a0f824-445b-dcae-02ef-349d4057da45.htm</remarks>
        /// <returns></returns>
        public static CImage ConvertToSepia(CImage original)
        {
            if (original == null)
            {
                throw new ArgumentNullException();
            }

            throw new NotImplementedException();
        }
    }
}
