using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;

namespace FotoFaultFixerUI.Services.Commands
{
    class FlipVerticalCommand : ICommandCImage
    {
        public FlipVerticalCommand() { }

        public CImage Execute(CImage img)
        {
            return Transformations.FlipVertical(img);
        }

        public CImage UnExecute(CImage img)
        {
            return Transformations.FlipVertical(img);
        }
    }
}
