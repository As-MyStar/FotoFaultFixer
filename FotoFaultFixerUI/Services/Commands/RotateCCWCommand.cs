using FotoFaultFixerLib;
using FotoFaultFixerLib.ImageProcessor;

namespace FotoFaultFixerUI.Services.Commands
{
    public class RotateCCWCommand : ICommandCImage
    {
        public RotateCCWCommand() { }

        public CImage Execute(CImage img)
        {
            return ImageFunctions.RotateCCW(img);
        }

        public CImage UnExecute(CImage img)
        {
            return ImageFunctions.RotateCW(img);
        }
    }
}
