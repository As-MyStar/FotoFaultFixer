using FotoFaultFixerLib;
using FotoFaultFixerUI.Services;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
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

        public void SetSourceImage(string path)
        {
            mainWindowVM.sourceImagePath = path;
            if (File.Exists(path))
            {
                Uri fileUri = new Uri(path);
                modifiedImage.Source = null;
                sourceImage.Source = new BitmapImage(fileUri);
            }
            else
            {
                string msgBoxText = string.Format("No file exists at indicated path: {0}", path);
                MessageBox.Show(msgBoxText, "Unable to load File");
            }
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
            OpenFileDialog selectFileDialog = new OpenFileDialog()
            {
                Title = "Open Image",
                Filter = FILEFILTER
            };

            if (selectFileDialog.ShowDialog() == true)
            {
                SetSourceImage(selectFileDialog.FileName);
            }
        }

        private void modifyImageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(mainWindowVM.sourceImagePath))
            {
                modifiedImage.Source = null;

                using (Bitmap original = (Bitmap)Image.FromFile(mainWindowVM.sourceImagePath))
                {
                    using (Bitmap modified = ImageFunctions.ImpulseNoiseReduction_Universal(original, mainWindowVM.LightNoiseSuppressionAmount, mainWindowVM.DarkNoiseSupressionAmount))
                    {
                        modifiedImage.Source = Utilities.BitmapToImageSource(modified);
                        mainWindowVM.CanSave = true;
                    }
                }
            }
        }

        private void saveImageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (modifiedImage.Source != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog()
                {
                    Title = "Save Image",
                    Filter = FILEFILTER
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (Bitmap fileToSave = Utilities.ImageSourceToBitmap((BitmapImage)modifiedImage.Source))
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
