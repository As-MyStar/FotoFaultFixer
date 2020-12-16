using FotoFaultFixerLib.ImageProcessing;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FotoFaultFixerLib.ImageFunctions
{
    public static class Coloring
    {
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
