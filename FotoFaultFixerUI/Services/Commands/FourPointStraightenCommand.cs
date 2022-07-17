using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using FotoFaultFixerUI.Services.Commands.Base;
using System;
using System.Drawing;

namespace FotoFaultFixerUI.Services.Commands
{
    public class FourPointStraightenCommand : ICommand<CImage>
    {
        CImage _original = null;
        Point[] _newCornerPoints = null;

        public FourPointStraightenCommand(Point[] newCornerPoints)
        {
            _newCornerPoints = newCornerPoints;
        }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {
            _original = new CImage(img.Width, img.Height, img.NBits, img.PixelFormat, img.Grid);
            return Transformations.FourPointStraighten(img, _newCornerPoints);
        }
    }
}
