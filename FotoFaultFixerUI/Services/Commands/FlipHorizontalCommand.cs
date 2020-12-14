using FotoFaultFixerLib;
using FotoFaultFixerLib.ImageProcessor;

namespace FotoFaultFixerUI.Services.Commands
{
    public class FlipHorizontalCommand : ICommandCImage
    {
        public FlipHorizontalCommand() { }

        public CImage Execute(CImage img)
        {
            return ImageFunctions.FlipHorizontal(img);
        }

        public CImage UnExecute(CImage img)
        {
            return ImageFunctions.FlipHorizontal(img);
        }
    }
}
