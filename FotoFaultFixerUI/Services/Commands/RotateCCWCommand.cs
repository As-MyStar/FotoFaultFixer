using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using FotoFaultFixerUI.Services.Commands.Base;
using System;

namespace FotoFaultFixerUI.Services.Commands
{
    public class RotateCCWCommand : ICommand<CImage>
    {
        public RotateCCWCommand() { }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {
            return Transformations.RotateCCW(img);
        }
    }
}
