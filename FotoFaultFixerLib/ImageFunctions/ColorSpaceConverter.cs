using FotoFaultFixerLib.ImageProcessing;
using System;

namespace FotoFaultFixerLib.ImageFunctions
{
    public static class ColorSpaceConverter
    {
        public static CImage ConvertRGBToYIQ(CImage rgbImg)
        {
            CImage yiqImg = new CImage(rgbImg.Width, rgbImg.Height, rgbImg.nBytes);
            var yFunc = RGB2YIQCalculateY.Memoize();
            var iFunc = RGB2YIQCalculateI.Memoize();
            var qFunc = RGB2YIQCalculateQ.Memoize();

            byte r, g, b;

            for (int y = 0; y < rgbImg.Height; y++)
            {
                for (int x = 0; x < rgbImg.Width; x++)
                {
                    int pxIdx = (x + (rgbImg.Width * y));
                    r = rgbImg.Grid[3 * pxIdx + 2];
                    g = rgbImg.Grid[3 * pxIdx + 1];
                    b = rgbImg.Grid[3 * pxIdx + 0];

                    rgbImg.Grid[3 * pxIdx + 0] = qFunc(r, g, b); // Q
                    rgbImg.Grid[3 * pxIdx + 1] = iFunc(r, g, b); // I
                    rgbImg.Grid[3 * pxIdx + 2] = yFunc(r, g, b); // Y                     
                }
            }

            return rgbImg;
        }

        public static CImage ConvertYIQToRGB(CImage yiqImg)
        {
            CImage rgbImg = new CImage(yiqImg.Width, yiqImg.Height, yiqImg.nBytes);
            var rFunc = YIQ2RGBCalculateR.Memoize();
            var gFunc = YIQ2RGBCalculateG.Memoize();
            var bFunc = YIQ2RGBCalculateB.Memoize();

            byte lum, i, q;
            for (int y = 0; y < rgbImg.Height; y++)
            {
                for (int x = 0; x < rgbImg.Width; x++)
                {
                    int pxIdx = (x + (rgbImg.Width * y));
                    lum = yiqImg.Grid[3 * pxIdx + 2];
                    i = yiqImg.Grid[3 * pxIdx + 1];
                    q = yiqImg.Grid[3 * pxIdx + 0];

                    rgbImg.Grid[3 * pxIdx + 0] = bFunc(lum, i, q); // B
                    rgbImg.Grid[3 * pxIdx + 1] = gFunc(lum, i, q); // G
                    rgbImg.Grid[3 * pxIdx + 2] = rFunc(lum, i, q); // R                        
                }
            }

            return rgbImg;
        }

        #region Individual Channel Calculations

        // YIQ2RGB
        private static Func<byte, byte, byte, byte> YIQ2RGBCalculateR = (Y,I,Q) =>
        {
            double val = (Y + 0.948262 * I + 0.624013 * Q);
            return (byte)Math.Min(255, Math.Max(val, 0));
        };

        private static Func<byte, byte, byte, byte> YIQ2RGBCalculateG = (Y, I, Q) =>
        {
            double val = (Y - 0.276066 * I - 0.639810 * Q);
            return (byte)Math.Min(255, Math.Max(val, 0));
        };

        private static Func<byte, byte, byte, byte> YIQ2RGBCalculateB = (Y, I, Q) =>
        {
            double val = (Y - 1.105450 * I + 1.729860 * Q);
            return (byte)Math.Min(255, Math.Max(val, 0));
        };

        // RGB2YIQ
        private static Func<byte, byte, byte, byte> RGB2YIQCalculateY = (R, G, B) =>
        {
            double val = (R * .299000 + G * .587000 + B * .114000);
            return (byte)Math.Min(1, Math.Max(val, 0));
        };

        private static Func<byte, byte, byte, byte> RGB2YIQCalculateI = (R, G, B) =>
        {
            double val = (R * -.168736 + G * -.331264 + B * .500000 + 128);
            return (byte)Math.Min(1, Math.Max(val, 0));
        };

        private static Func<byte, byte, byte, byte> RGB2YIQCalculateQ = (R, G, B) =>
        {
            double val = (R * .500000 + G * -.418688 + B * -.081312 + 128);
            return (byte)Math.Min(1, Math.Max(val, 0));
        };
        #endregion
    }
}
