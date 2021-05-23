using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FotoFaultFixerUI.Controls
{
    /// <summary>
    /// Interaction logic for CropCanvas.xaml
    /// </summary>
    public partial class CropCanvas : UserControl
    {
        public bool CropIsActive = false;
        public event Action<int, int, int, int> CropCanvas_TriggerCropEvent;

        public CropCanvas()
        {
            InitializeComponent();
        }

        public void ActivateCrop()
        {
            if (!CropIsActive)
            {
                CropIsActive = true;

                this.Visibility = System.Windows.Visibility.Visible;
                this.MouseDown += TransformationSurface_MouseDown;                
            }
        }

        public void DisactivateCrop()
        {
            if (CropIsActive)
            {
                CropIsActive = false;

                // Reset Crop Rectangle
                CropRectangle.Width = 0;
                CropRectangle.Height = 0;
                Canvas.SetLeft(CropRectangle, 0);
                Canvas.SetTop(CropRectangle, 0);

                // Remove Event Handlers
                this.MouseDown -= TransformationSurface_MouseDown;
                this.MouseMove -= TransformationSurface_MouseMove;
                this.MouseUp -= TransformationSurface_MouseUp;

                this.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void TransformationSurface_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;

            try
            {
                var mousePos = e.GetPosition(this);
                Canvas.SetLeft(CropRectangle, (int)mousePos.X);
                Canvas.SetTop(CropRectangle, (int)mousePos.Y);

                this.MouseMove += TransformationSurface_MouseMove;
                this.MouseUp += TransformationSurface_MouseUp;
            }
            catch (Exception)
            {
                DisactivateCrop();
            }
        }

        private void TransformationSurface_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            e.Handled = true;

            // Update Rectangle width and height            
            var mousePos = e.GetPosition(this);
            int newWidth = (int)(mousePos.X - Canvas.GetLeft(CropRectangle));
            int newHeight = (int)(mousePos.Y - Canvas.GetTop(CropRectangle));

            if (newWidth >= 0 && newHeight >= 0)
            {
                CropRectangle.Width = newWidth;
                CropRectangle.Height = newHeight;
            }
        }

        private void TransformationSurface_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;

            TriggerCropEvent(sender, e);
            DisactivateCrop();
        }

        private void TriggerCropEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CropCanvas_TriggerCropEvent != null)
            {
                var cropRectanglePoint = new System.Windows.Point(
                        (int)Canvas.GetLeft(CropRectangle),
                        (int)Canvas.GetTop(CropRectangle)
                    );

                var ToScreenPoint2 = PresentationSource.FromVisual(CropRectangle).CompositionTarget.TransformToDevice.Transform(cropRectanglePoint);
                var ToScreenPoint = this.PointToScreen(ToScreenPoint2);                

                CropCanvas_TriggerCropEvent(
                    (int)ToScreenPoint.X,
                    (int)ToScreenPoint.Y,
                    (int)CropRectangle.Width,
                    (int)CropRectangle.Height
                );
            }
        }
    }
}
