using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using System;

namespace FotoFaultFixerUI.Services.Commands
{
    class FlipVerticalCommand : ICommandCImage
    {
        public FlipVerticalCommand() { }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {
            return Transformations.FlipVertical(img);
        }

        public CImage UnExecute(CImage img)
        {
            return Transformations.FlipVertical(img);
        }
    }
}
