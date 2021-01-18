using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using System;
using System.Drawing;

namespace FotoFaultFixerUI.Services.Commands
{
    public class FourPointStraightenCommand : ICommandCImage
    {
        CImage _original = null;
        Point[] _newCornerPoints = null;
        bool _shouldCrop;

        public FourPointStraightenCommand(Point[] newCornerPoints, bool shouldCrop)
        {
            _newCornerPoints = newCornerPoints;
            _shouldCrop = shouldCrop;
        }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {
            _original = new CImage(img.Width, img.Height, img.NBits, img.PixelFormat, img.Grid);
            return Transformations.FourPointStraighten(img, _newCornerPoints, _shouldCrop);
        }

        public CImage UnExecute(CImage img)
        {
            return _original;
        }
    }
}
