using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;

namespace FotoFaultFixerUI.Services.Commands
{
    public class RotateCWCommand : ICommandCImage
    {
        public RotateCWCommand() { }

        public CImage Execute(CImage img)
        {
            return Transformations.RotateCW(img);
        }

        public CImage UnExecute(CImage img)
        {
            return Transformations.RotateCCW(img);
        }
    }
}
