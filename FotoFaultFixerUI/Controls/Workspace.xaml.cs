using FotoFaultFixerUI.Services;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FotoFaultFixerUI.Controls
{
    /// <summary>
    /// Interaction logic for Workspace.xaml
    /// </summary>
    public partial class Workspace : UserControl
    {        

        public bool CropIsActive = false;
        public event Action<int, int, int, int> Workspace_TriggerCropEvent;

        public Workspace()
        {
            InitializeComponent();
            CropCanvas.CropCanvas_TriggerCropEvent += CropCanvas_CropCanvas_TriggerCropEvent;
        }

        private void CropCanvas_CropCanvas_TriggerCropEvent(double startXPrc, double startYPrc, double endXPrc, double endYPrc)
        {
            // We need to translate the % values to actual coordinates on the real image
            // Get the true pixel dimensions of the image
            var bitmapSource = (workspaceImage.Source as BitmapSource);
            double imgWidth = bitmapSource.PixelWidth;
            double imgHeight = bitmapSource.PixelHeight;
            
            // Top-left Point
            int startX = (int)(imgWidth * startXPrc);
            int startY = (int)(imgHeight * startYPrc);                        
            startX = Math.Max(0, startX);
            startY = Math.Max(0, startY);

            // Bottom-Right Point
            int endX = (int)(imgWidth * endXPrc);
            int endY = (int)(imgHeight * endYPrc);
            endX = (int)Math.Min(imgWidth, endX);
            endY = (int)Math.Min(imgHeight, endY);

            // Calc width/height based on start/end points
            int cropWidth = Math.Abs(endX - startX);
            int cropHeight = Math.Abs(endY - startY);

            // Perform the crop!
            if (Workspace_TriggerCropEvent != null)
            {
                Workspace_TriggerCropEvent(startX, startY, cropWidth, cropHeight);
            }
        }

        public void ActivateCrop()
        {                                              
            CropCanvas.Setup((
                int)Canvas.GetLeft(workspaceImage),
                (int)Canvas.GetTop(workspaceImage),
                (int)workspaceImage.ActualWidth,
                (int)workspaceImage.ActualHeight
            );
            CropCanvas.ActivateCrop();
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
            e.Handled = true;
            workspaceImageWrapper.ZoomIn();
        }

        private void zoomOutBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true;
            workspaceImageWrapper.ZoomOut();
        }

        private void zoomResetBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true;
            ZoomReset();
        }
        #endregion
    }
}
