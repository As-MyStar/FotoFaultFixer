using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using FotoFaultFixerUI.Services.Commands.Base;
using System;

namespace FotoFaultFixerUI.Services.Commands
{
    public class BrightenCommand : ICommand<CImage>
    {
        CImage _original;
        int _brightnessFactor = 0;

        public BrightenCommand(int brightnessFactor)
        {
            _brightnessFactor = brightnessFactor;
        }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {
            _original = new CImage(img.Width, img.Height, img.NBits, img.PixelFormat, img.Grid);
            return Coloring.AdjustBrightnesss(img, _brightnessFactor);
        }
    }
}
