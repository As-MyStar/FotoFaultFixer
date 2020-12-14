using FotoFaultFixerLib.ImageProcessor;
using System.Drawing;

namespace FotoFaultFixerUI.Services.Commands
{
    public interface ICommandCImage
    {
        public CImage Execute(CImage img);
        public CImage UnExecute(CImage img);
    }
}
