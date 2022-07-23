namespace FotoFaultFixerUI.Services.Commands
{
    using FotoFaultFixerLib.ImageFunctions;
    using FotoFaultFixerLib.ImageProcessing;
    using FotoFaultFixerUI.Services.Commands.Base;
    using System;
    using System.Drawing;

    public class FourPointStraightenCommand : ICommand<CImage>
    {
        Point[] _newCornerPoints = null;

        public FourPointStraightenCommand(Point[] newCornerPoints)
        {
            _newCornerPoints = newCornerPoints;
        }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {
            return Transformations.FourPointStraighten(img, _newCornerPoints);
        }
    }
}
