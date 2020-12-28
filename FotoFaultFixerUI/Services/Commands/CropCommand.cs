using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using System.Drawing;

namespace FotoFaultFixerUI.Services.Commands
{
    public class CropCommand : ICommandCImage
    {
        CImage _original = null;
        Point _startingPoint;
        int _width = 0;
        int _height = 0;

        public CropCommand(Point startingPoint, int width, int height)
        {
            _startingPoint = startingPoint;
            _width = width;
            _height = height;
        }

        public CImage Execute(CImage img)
        {
            _original = new CImage(img.Width, img.Height, img.NBits, img.PixelFormat, img.Grid);
            return Transformations.Crop(img, _startingPoint, _width, _height);
        }

        public CImage UnExecute(CImage img)
        {
            return _original;
        }
    }
}
