using FotoFaultFixerLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FotoFaultFixerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string originalImageUrl;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void selectImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Image";
            openFileDialog.Filter = "Image File|*.bmp; *.jpg; *.jpeg; *.png;";
            if (openFileDialog.ShowDialog() == true)
            {
                originalImageUrl = openFileDialog.FileName;
                Uri fileUri = new Uri(openFileDialog.FileName);                
                originalImage.Source = new BitmapImage(fileUri);
                modifiedImage.Source = null;
            }
        }

        private void modifyImageBtn_Click(object sender, RoutedEventArgs e)
        {
            using (Bitmap original = (Bitmap)System.Drawing.Image.FromFile(originalImageUrl)) {

                using (Bitmap modified = ImageUtils.ImpulseNoiseReduction_Universal(original, (Int32)lightNoiseSupressionSlider.Value, (Int32)darkNoiseSupressionSlider.Value))
                {
                    modifiedImage.Source = BitmapToImageSource(modified);
                }
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
