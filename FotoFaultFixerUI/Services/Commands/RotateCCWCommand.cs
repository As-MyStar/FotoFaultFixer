using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;

namespace FotoFaultFixerUI.Services.Commands
{
    public class RotateCCWCommand : ICommandCImage
    {
        public RotateCCWCommand() { }

        public CImage Execute(CImage img)
        {
            return Transformations.RotateCCW(img);
        }

        public CImage UnExecute(CImage img)
        {
            return Transformations.RotateCW(img);
        }
    }
}
