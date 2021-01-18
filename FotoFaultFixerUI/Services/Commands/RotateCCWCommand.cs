using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using System;

namespace FotoFaultFixerUI.Services.Commands
{
    public class RotateCCWCommand : ICommandCImage
    {
        public RotateCCWCommand() { }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {
            return Transformations.RotateCCW(img);
        }

        public CImage UnExecute(CImage img)
        {
            return Transformations.RotateCW(img);
        }
    }
}
