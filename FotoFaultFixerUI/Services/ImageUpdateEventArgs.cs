using FotoFaultFixerLib.ImageProcessing;
using System;

namespace FotoFaultFixerUI.Services
{
    class ImageUpdateEventArgs : EventArgs
    {
        public ImageUpdateEventArgs(CImage img)
        {
            Image = img;
        }

        public CImage Image { get; }
    }
}
