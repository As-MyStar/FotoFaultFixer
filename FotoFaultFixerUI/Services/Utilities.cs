﻿namespace FotoFaultFixerUI.Services
{
    using System.Drawing;
    using System.IO;
    using System.Windows.Media.Imaging;

    public static class Utilities
    {
        /// <summary>
        /// Converts a Bitmap image into the format required to be a source for a WPF Image
        /// </summary>
        /// <param name="bitmap">The bitmap to use as the image source</param>
        /// <returns>A bitmapImage that can be set as an Image's source</returns>
        public static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        // PG: Beware, this can be a 32bitARGB, which we can't reload!
        public static Bitmap ImageSourceToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();                
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);
                return new Bitmap(bitmap);
            }
        }
    }
}
