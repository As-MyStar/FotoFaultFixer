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
            zoomBtnsBar.Visibility = System.Windows.Visibility.Visible;
        }

        public Bitmap GetImage()
        {
            return Utilities.ImageSourceToBitmap((BitmapImage)workspaceImage.Source);
        }

        #region Button Event Handlers

        private void zoomInBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            workspaceImageWrapper.ZoomIn();
        }

        private void zoomOutBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            workspaceImageWrapper.ZoomOut();
        }

        private void zoomResetBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ZoomReset();
        }
        #endregion
    }
}
