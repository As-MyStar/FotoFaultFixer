namespace FotoFaultFixerLib.ImageFunctions
{
    public static class Utilities
    {
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
