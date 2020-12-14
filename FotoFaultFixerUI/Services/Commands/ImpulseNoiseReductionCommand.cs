using FotoFaultFixerLib;
using FotoFaultFixerLib.ImageProcessor;

namespace FotoFaultFixerUI.Services.Commands
{
    public class ImpulseNoiseReductionCommand : ICommandCImage
    {
        CImage _original = null;
        int _lightNoiseSuppression = 0;
        int _darkNoiseSuppression = 0;

        public ImpulseNoiseReductionCommand(int lightNoiseSupp, int darkNoiseSupp)
        {
            _lightNoiseSuppression = lightNoiseSupp;
            _darkNoiseSuppression = darkNoiseSupp;
        }

        public CImage Execute(CImage img)
        {
            _original = new CImage(img.Width, img.Height, img.NBits, img.PixelFormat, img.Grid);
            return ImageFunctions.ImpulseNoiseReduction_Universal(img, _lightNoiseSuppression, _darkNoiseSuppression);
        }

        public CImage UnExecute(CImage img)
        {
            return _original;
        }
    }
}
