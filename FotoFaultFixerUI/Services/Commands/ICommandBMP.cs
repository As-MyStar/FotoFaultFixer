using System.Drawing;

namespace FotoFaultFixerUI.Services.Commands
{
    public interface ICommandBMP
    {
        public Bitmap Execute(Bitmap bmp);
        public Bitmap UnExecute(Bitmap bmp);
    }
}
