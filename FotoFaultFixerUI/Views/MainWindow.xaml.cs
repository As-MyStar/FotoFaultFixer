using FotoFaultFixerLib;
using FotoFaultFixerUI.Services;
using FotoFaultFixerUI.ViewModels;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace FotoFaultFixerUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel _mainWindowVM;
        const string FILEFILTER = "Image File|*.bmp; *.jpg; *.jpeg; *.png;";

        public MainWindow()
        {
            _mainWindowVM = new MainWindowViewModel();
            InitializeComponent();
            this.DataContext = _mainWindowVM;
        }

        public void SetSourceImage(string path)
        {
            _mainWindowVM.SourceImagePath = path;
            if (File.Exists(path))
            {
                Uri fileUri = new Uri(path);
                imageName.Text = path.Substring(path.LastIndexOf(@"\"));
                workspaceImage.Source = new BitmapImage(fileUri);
                _mainWindowVM.CanSave = true;
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
            _mainWindowVM.LightNoiseSuppressionAmount = (Int32)e.NewValue;
        }

        private void darkNoiseSupressionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _mainWindowVM.DarkNoiseSupressionAmount = (Int32)e.NewValue;
        }
        #endregion

        #region Button Handlers

        private void OpenMI_Click(object sender, RoutedEventArgs e)
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

        private void SaveMI_Click(object sender, RoutedEventArgs e)
        {
            //if (_mainWindowVM.CanSave)
            //{
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = "Save Image",
                Filter = FILEFILTER
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (Bitmap fileToSave = Utilities.ImageSourceToBitmap((BitmapImage)workspaceImage.Source))
                {
                    fileToSave.Save(saveFileDialog.FileName);
                }
            }
            //}
        }

        private void ExitMI_Click(object sender, RoutedEventArgs e)
        {
            // if there are undaved changes
            // Prompt about saving 
            // then exit
            // else 
            // just exit
            Application.Current.Shutdown();
        }

        #endregion
    }
}
