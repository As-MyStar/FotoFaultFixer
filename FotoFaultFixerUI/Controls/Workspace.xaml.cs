using FotoFaultFixerUI.Services;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace FotoFaultFixerUI.Controls
{
    /// <summary>
    /// Interaction logic for Workspace.xaml
    /// </summary>
    public partial class Workspace : UserControl
    {
        public Workspace()
        {
            InitializeComponent();
        }

        public void ZoomReset()
        {
            workspaceImageWrapper.Reset();
        }

        public void SetImage(BitmapImage bmpImg)
        {
            workspaceImage.Source = bmpImg;
        }

        public Bitmap GetImage()
        {
            return Utilities.ImageSourceToBitmap((BitmapImage)workspaceImage.Source);
        }
    }
}
