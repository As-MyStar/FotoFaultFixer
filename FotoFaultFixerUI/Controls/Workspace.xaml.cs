using FotoFaultFixerUI.Services;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
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

        private void CropCanvas_CropCanvas_TriggerCropEvent(int screenX, int screenY, int width, int height)
        {
            // these coords are position on the screen.
            // We need to translate those to where they sit on the image (if at all)
            var newStartingPoint = workspaceImage.PointFromScreen(new System.Windows.Point(screenX, screenY));

            if (newStartingPoint.X < 0)
            {
                width = Math.Max(0, width - Math.Abs((int)newStartingPoint.X));
                newStartingPoint.X = 0;
            }

            if (newStartingPoint.Y < 0)
            {
                height = Math.Max(0, height - Math.Abs((int)newStartingPoint.Y));
                newStartingPoint.Y = 0;
            }

            if (Workspace_TriggerCropEvent != null)
            {
                Workspace_TriggerCropEvent((int)newStartingPoint.X, (int)newStartingPoint.Y, width, height);
            }
        }

        public void ActivateCrop()
        {                                  
            CropCanvas.ActivateCrop();
        }
        
        public void ZoomReset()
        {
            workspaceImageWrapper.Reset();
        }

        public void SetImage(BitmapImage bmpImg)
        {
            workspaceImage.Source = bmpImg;
            CropCanvas.Height = workspaceImage.Height;
            CropCanvas.Width = workspaceImage.Width;
            CropCanvas.Margin = workspaceImage.Margin;
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
