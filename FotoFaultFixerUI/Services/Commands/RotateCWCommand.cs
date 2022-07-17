using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using FotoFaultFixerUI.Services.Commands.Base;
using System;

namespace FotoFaultFixerUI.Services.Commands
{
    public class RotateCWCommand : ICommand<CImage>
    {
        public RotateCWCommand() { }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {
            return Transformations.RotateCW(img);
        }
    }
}
