using FotoFaultFixerLib;
using FotoFaultFixerLib.ImageProcessor;

namespace FotoFaultFixerUI.Services.Commands
{
    public class RotateCWCommand : ICommandCImage
    {
        public RotateCWCommand() { }

        public CImage Execute(CImage img)
        {
            return ImageFunctions.RotateCW(img);
        }

        public CImage UnExecute(CImage img)
        {
            return ImageFunctions.RotateCCW(img);
        }
    }
}
