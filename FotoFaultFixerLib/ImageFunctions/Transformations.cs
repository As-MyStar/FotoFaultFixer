using FotoFaultFixerLib.ImageProcessing;
using System;
using System.Drawing;

namespace FotoFaultFixerLib.ImageFunctions
{
    public static class Transformations
    {
        public static CImage FlipVertical(CImage original)
        {
            if (original == null)
            {
                throw new ArgumentNullException();
            }

            CImage verticallyFlipped = new CImage(original.Width, original.Height, original.NBits);

            int i_orig;
            int i_flipped;
            int maxY = original.Height - 1;
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x< original.Width; x++)
                {
                    i_orig = (x + (original.Width * y));
                    i_flipped = (x + (original.Width * (maxY - y)));
                    verticallyFlipped.Grid[0 + 3 * i_flipped] = original.Grid[0 + 3 * i_orig];
                    verticallyFlipped.Grid[1 + 3 * i_flipped] = original.Grid[1 + 3 * i_orig];
                    verticallyFlipped.Grid[2 + 3 * i_flipped] = original.Grid[2 + 3 * i_orig];                    
                }
            }

            return verticallyFlipped;
        }

        public static CImage FlipHorizontal(CImage original)
        {
            if (original == null)
            {
                throw new ArgumentNullException();
            }

            CImage horizontallyFlipped = new CImage(original.Width, original.Height, original.NBits);

            int i_orig;
            int i_flipped;
            int maxX = original.Width - 1;
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    i_orig = (x + (original.Width * y));
                    i_flipped = ((maxX - x) + (original.Width * y));
                    horizontallyFlipped.Grid[0 + 3 * i_flipped] = original.Grid[0 + 3 * i_orig];
                    horizontallyFlipped.Grid[1 + 3 * i_flipped] = original.Grid[1 + 3 * i_orig];
                    horizontallyFlipped.Grid[2 + 3 * i_flipped] = original.Grid[2 + 3 * i_orig];
                }
            }

            return horizontallyFlipped;
        }

        public static CImage RotateCW(CImage original)
        {
            if (original == null)
            {
                throw new ArgumentNullException();
            }

            // Height and width are switched here
            CImage rotatedCW = new CImage(original.Height, original.Width, original.NBits);

            int i_orig;
            int i_rotated;
            int maxY = original.Height - 1;
            int yIdx = -1;
            for (int y = maxY; y >= 0; y--)
            {
                yIdx += 1;
                for (int x = 0; x < original.Width; x++)
                {
                    i_orig = (x + (original.Width * y));
                    i_rotated = yIdx + (rotatedCW.Width * x);
                    rotatedCW.Grid[0 + 3 * i_rotated] = original.Grid[0 + 3 * i_orig];
                    rotatedCW.Grid[1 + 3 * i_rotated] = original.Grid[1 + 3 * i_orig];
                    rotatedCW.Grid[2 + 3 * i_rotated] = original.Grid[2 + 3 * i_orig];
                }
            }

            return rotatedCW;
        }

        public static CImage RotateCCW(CImage original)
        {
            if (original == null)
            {
                throw new ArgumentNullException();
            }

            // Height and width are switched here
            CImage rotatedCCW = new CImage(original.Height, original.Width, original.NBits);

            int i_orig;
            int i_rotated;
            int maxX = original.Width - 1;

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = maxX; x >= 0; x--)
                {
                    i_orig = (x + (original.Width * y));
                    i_rotated = y + (rotatedCCW.Width * (maxX - x));

                    rotatedCCW.Grid[0 + 3 * i_rotated] = original.Grid[0 + 3 * i_orig];
                    rotatedCCW.Grid[1 + 3 * i_rotated] = original.Grid[1 + 3 * i_orig];
                    rotatedCCW.Grid[2 + 3 * i_rotated] = original.Grid[2 + 3 * i_orig];
                }
            }

            return rotatedCCW;
        }

        public static CImage Crop(CImage original, Point startPoint, int width, int height)
        {
            throw new NotImplementedException();
        }

        public static CImage FourPointStraighten(CImage original, Point[] newCornerPoints)
        {
            throw new NotImplementedException();
        }
    }
}
