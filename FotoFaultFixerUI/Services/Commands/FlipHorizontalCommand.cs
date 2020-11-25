using System.Drawing;

namespace FotoFaultFixerUI.Services.Commands
{
    public class FlipHorizontalCommand : ICommandBMP
    {
        public FlipHorizontalCommand() { }

        public Bitmap Execute(Bitmap bmp)
        {
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }

        public Bitmap UnExecute(Bitmap bmp)
        {
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }
    }
}
