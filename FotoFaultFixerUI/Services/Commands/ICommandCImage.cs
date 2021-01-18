using FotoFaultFixerLib.ImageProcessing;
using System;

namespace FotoFaultFixerUI.Services.Commands
{
    public interface ICommandCImage
    {
        public CImage Execute(CImage img, IProgress<int> progressReporter);
        public CImage UnExecute(CImage img);
    }
}
