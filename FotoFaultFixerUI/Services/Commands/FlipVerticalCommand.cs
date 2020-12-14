using FotoFaultFixerLib;
using FotoFaultFixerLib.ImageProcessor;

namespace FotoFaultFixerUI.Services.Commands
{
    class FlipVerticalCommand : ICommandCImage
    {
        public FlipVerticalCommand() { }

        public CImage Execute(CImage img)
        {
            return ImageFunctions.FlipVertical(img);
        }

        public CImage UnExecute(CImage img)
        {
            return ImageFunctions.FlipVertical(img);
        }
    }
}
