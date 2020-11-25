using System.Drawing;

namespace FotoFaultFixerUI.Services.Commands
{
    class FlipVerticalCommand : ICommandBMP
    {
        public FlipVerticalCommand() { }

        public Bitmap Execute(Bitmap bmp)
        {
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return bmp;
        }

        public Bitmap UnExecute(Bitmap bmp)
        {
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return bmp;
        }
    }
}
