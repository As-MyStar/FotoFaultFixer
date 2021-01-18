using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using System;

namespace FotoFaultFixerUI.Services.Commands
{
    public class RotateCWCommand : ICommandCImage
    {
        public RotateCWCommand() { }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {
            return Transformations.RotateCW(img);
        }

        public CImage UnExecute(CImage img)
        {
            return Transformations.RotateCCW(img);
        }
    }
}
