using System;
using System.Collections.Concurrent;

namespace FotoFaultFixerLib.ImageFunctions
{
    public static class Utilities
    {
        public static Func<int, int, int, int> MaxC = (R, G, B) =>
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
        };

        public static void SetProgress(IProgress<int> progressReporter, int progressPercent)
        {
            if (progressReporter != null)
            {
                progressReporter.Report(progressPercent);
            }
        }
    }
}
