using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using System.Drawing;

namespace FotoFaultFixerUI.Services.Commands
{
    public class FourPointStraightenCommand : ICommandCImage
    {
        CImage _original = null;
        Point[] _newCornerPoints = null;

        public FourPointStraightenCommand(Point[] newCornerPoints)
        {
            _newCornerPoints = newCornerPoints;
        }

        public CImage Execute(CImage img)
        {
            _original = new CImage(img.Width, img.Height, img.NBits, img.PixelFormat, img.Grid);
            return Transformations.FourPointStraighten(img, _newCornerPoints);
        }

        public CImage UnExecute(CImage img)
        {
            return _original;
        }
    }
}
