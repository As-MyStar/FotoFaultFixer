namespace FotoFaultFixerUI.Services.Commands
{
    using FotoFaultFixerLib.ImageFunctions;
    using FotoFaultFixerLib.ImageProcessing;
    using FotoFaultFixerUI.Services.Commands.Base;
    using System;
    public class ContrastCommand : ICommand<CImage>
    {
        int _contrastFactor = 0;

        public ContrastCommand(int contrastFactor)
        {
            _contrastFactor = contrastFactor;
        }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {            
            return Coloring.AdjustContrast(img, _contrastFactor);
        }
    }
}
