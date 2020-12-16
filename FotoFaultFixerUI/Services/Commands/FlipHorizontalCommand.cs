using FotoFaultFixerLib.ImageProcessing;
using FotoFaultFixerLib.ImageFunctions;

namespace FotoFaultFixerUI.Services.Commands
{
    public class FlipHorizontalCommand : ICommandCImage
    {
        public FlipHorizontalCommand() { }

        public CImage Execute(CImage img)
        {
            return Transformations.FlipHorizontal(img);
        }

        public CImage UnExecute(CImage img)
        {
            return Transformations.FlipHorizontal(img);
        }
    }
}
