namespace FotoFaultFixerUI.Services.Commands
{
    using FotoFaultFixerLib.ImageFunctions;
    using FotoFaultFixerLib.ImageProcessing;
    using FotoFaultFixerUI.Services.Commands.Base;
    using System;
    public class ImpulseNoiseReductionCommand : ICommand<CImage>
    {
        int _lightNoiseSuppression = 0;
        int _darkNoiseSuppression = 0;

        public ImpulseNoiseReductionCommand(int lightNoiseSupp, int darkNoiseSupp)
        {
            _lightNoiseSuppression = lightNoiseSupp;
            _darkNoiseSuppression = darkNoiseSupp;
        }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {
            return Filters.ImpulseNoiseReduction_Universal(img, _darkNoiseSuppression, _lightNoiseSuppression, progressReporter);
        }
    }
}
