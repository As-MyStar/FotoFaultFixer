namespace FotoFaultFixerUI.Services.Commands
{
    using FotoFaultFixerLib.ImageFunctions;
    using FotoFaultFixerLib.ImageProcessing;
    using FotoFaultFixerUI.Services.Commands.Base;
    using System;

    public class BrightenCommand : ICommand<CImage>
    {        
        int _brightnessFactor = 0;

        public BrightenCommand(int brightnessFactor)
        {
            _brightnessFactor = brightnessFactor;
        }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {            
            return Coloring.AdjustBrightnesss(img, _brightnessFactor);
        }
    }
}
