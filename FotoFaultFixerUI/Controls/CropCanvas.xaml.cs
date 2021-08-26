using System;
using System.Windows.Controls;

namespace FotoFaultFixerUI.Controls
{
    /// <summary>
    /// Interaction logic for CropCanvas.xaml
    /// </summary>
    public partial class CropCanvas : UserControl
    {
        public bool CropIsActive = false;
        public event Action<double, double, double, double> CropCanvas_TriggerCropEvent;

        public CropCanvas()
        {
            InitializeComponent();
        }

        public void Setup(int x, int y, int width, int height)
        {
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
            this.Width = width;
            this.Height = height;            
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
                // Convert absolute position of Crop rectangle start/EndInit to % values
                
                // Transform the topleft Point (Rectangle start point)
                double startXPrc = (Canvas.GetLeft(CropRectangle) / this.Width) ;
                double startYPrc = (Canvas.GetTop(CropRectangle) / this.Height);

                // Transform the BottomRight Point (Current Mouse Position)
                double endXPrc = ((Canvas.GetLeft(CropRectangle) + CropRectangle.Width) / this.Width);
                double endYPrc = ((Canvas.GetTop(CropRectangle) +CropRectangle.Height) / this.Height);

                CropCanvas_TriggerCropEvent(
                    startXPrc,
                    startYPrc,
                    endXPrc,
                    endYPrc
                );
            }
        }
    }
}
