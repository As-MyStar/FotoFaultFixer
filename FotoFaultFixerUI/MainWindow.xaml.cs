using FotoFaultFixerLib;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FotoFaultFixerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel mainWindowVM;
        const string FILEFILTER = "Image File|*.bmp; *.jpg; *.jpeg; *.png;";

        public MainWindow()
        {
            mainWindowVM = new MainWindowViewModel();            
            InitializeComponent();
            this.DataContext = mainWindowVM;
        }

        #region sliderHandlers
        private void lightNoiseSupressionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mainWindowVM.LightNoiseSuppressionAmount = (Int32)e.NewValue;
        }

        private void darkNoiseSupressionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mainWindowVM.DarkNoiseSupressionAmount = (Int32)e.NewValue;
        }
        #endregion

        #region Button Handlers
        private void selectImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog selectFileDialog = new OpenFileDialog() {
                Title = "Open Image",
                Filter = FILEFILTER
            };
            
            if (selectFileDialog.ShowDialog() == true)
            {
                mainWindowVM.OriginalImagePath = selectFileDialog.FileName;
                Uri fileUri = new Uri(selectFileDialog.FileName);
                modifiedImage.Source = null;
                originalImage.Source = new BitmapImage(fileUri);                
            }
        }

        private void modifyImageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(mainWindowVM.OriginalImagePath))
            {
                modifiedImage.Source = null;

                using (Bitmap original = (Bitmap)Image.FromFile(mainWindowVM.OriginalImagePath))
                {
                    using (Bitmap modified = ImageUtils.ImpulseNoiseReduction_Universal(original, mainWindowVM.LightNoiseSuppressionAmount, mainWindowVM.DarkNoiseSupressionAmount))
                    {
                        modifiedImage.Source = Utils.BitmapToImageSource(modified);
                        mainWindowVM.CanSave = true;
                    }
                }
            }
        }

        private void saveImageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (modifiedImage.Source != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() { 
                    Title= "Save Image",
                    Filter = FILEFILTER
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (Bitmap fileToSave = Utils.ImageSourceToBitmap((BitmapImage)modifiedImage.Source))
                    {
                        fileToSave.Save(saveFileDialog.FileName);
                    }
                }
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}
