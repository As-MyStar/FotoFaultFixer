namespace FotoFaultFixerUI.Services.Commands
{
    using FotoFaultFixerLib.ImageFunctions;
    using FotoFaultFixerLib.ImageProcessing;
    using FotoFaultFixerUI.Services.Commands.Base;
    using System;
    public class CropCommand : ICommand<CImage>
    {
        int _x = 0;
        int _y = 0;
        int _width = 0;
        int _height = 0;

        public CropCommand(int x, int y, int width, int height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public CImage Execute(CImage img, IProgress<int> progressReporter)
        {
            return Transformations.Crop(img, _x, _y, _width, _height);
        }
    }
}
