using FotoFaultFixerLib.ImageProcessing;
using FotoFaultFixerLib.ImageFunctions;
using System;
using FotoFaultFixerUI.Services.Commands.Base;

namespace FotoFaultFixerUI.Services.Commands
{
    public class FlipHorizontalCommand : ICommand<CImage>
    {
        public FlipHorizontalCommand() { }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {
            return Transformations.FlipHorizontal(img);
        }
    }
}
