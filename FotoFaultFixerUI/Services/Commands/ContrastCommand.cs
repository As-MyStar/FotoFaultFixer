using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using FotoFaultFixerUI.Services.Commands.Base;
using System;

namespace FotoFaultFixerUI.Services.Commands
{
    public class ContrastCommand : ICommand<CImage>
    {
        CImage _original = null;
        int _contrastFactor = 0;

        public ContrastCommand(int contrastFactor)
        {
            _contrastFactor = contrastFactor;
        }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {
            _original = new CImage(img.Width, img.Height, img.NBits, img.PixelFormat, img.Grid);
            return Coloring.AdjustContrast(img, _contrastFactor);
        }
    }
}
