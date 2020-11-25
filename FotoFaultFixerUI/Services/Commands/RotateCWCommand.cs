using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoFaultFixerUI.Services.Commands
{
    public class RotateCWCommand : ICommandBMP
    {
        public RotateCWCommand() { }

        public Bitmap Execute(Bitmap bmp)
        {
            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            return bmp;
        }

        public Bitmap UnExecute(Bitmap bmp)
        {
            bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            return bmp;
        }
    }
}
