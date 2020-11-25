using FotoFaultFixerLib;
using System;
using System.Drawing;

namespace FotoFaultFixerUI.Services.Commands
{
    public class ImpulseNoiseReductionCommand : ICommandBMP
    {
        Bitmap _original = null;
        int _lightNoiseSuppression = 0;
        int _darkNoiseSuppression = 0;

        public ImpulseNoiseReductionCommand(int lightNoiseSupp, int darkNoiseSupp)
        {
            _lightNoiseSuppression = lightNoiseSupp;
            _darkNoiseSuppression = darkNoiseSupp;
        }

        ~ImpulseNoiseReductionCommand()
        {
            if (_original != null)
            {
                _original.Dispose();
            }
        }

        public Bitmap Execute(Bitmap bmp)
        {
            _original = (Bitmap)bmp.Clone();
            bmp = ImageFunctions.ImpulseNoiseReduction_Universal(bmp, _lightNoiseSuppression, _darkNoiseSuppression);
            return bmp;
        }

        public Bitmap UnExecute(Bitmap bmp)
        {
            return _original;
        }
    }
}
