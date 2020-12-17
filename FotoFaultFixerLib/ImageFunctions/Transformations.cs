using FotoFaultFixerLib.ImageProcessing;
using System;

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

            throw new NotImplementedException();
        }

        public static CImage RotateCCW(CImage original)
        {
            if (original == null)
            {
                throw new ArgumentNullException();
            }

            throw new NotImplementedException();
        }
    }
}
