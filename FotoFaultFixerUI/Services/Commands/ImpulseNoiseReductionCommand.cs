using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using System;

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

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {
            _original = new CImage(img.Width, img.Height, img.NBits, img.PixelFormat, img.Grid);
            return Filters.ImpulseNoiseReduction_Universal(img, _lightNoiseSuppression, _darkNoiseSuppression, progressReporter);
        }

        public CImage UnExecute(CImage img)
        {
            return _original;
        }
    }
}
