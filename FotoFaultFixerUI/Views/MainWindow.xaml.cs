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
        const string FILEFILTER = "Image File|*.bmp; *.jpg; *.jpeg; *.png;";
        ApplicationService _appService;
        MainWindowViewModel _mainWindowVM;
                
        public MainWindow()
        {
            _mainWindowVM = new MainWindowViewModel();
            _appService = new ApplicationService();
            _appService.ImageUpdated += _appService_ImageUpdated;            
            InitializeComponent();
            this.DataContext = _mainWindowVM;            
        }

        #region Event Handlers
        private void _appService_ImageUpdated(object sender, ImageUpdateEventArgs e)
        {
            workspaceImage.Source = Utilities.BitmapToImageSource(e.Image.ToBitmap());
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            ExitApplication();
        }
        #endregion

        #region Toolbar Handlers
        private void SaveImage()
        {
            using (Bitmap fileToSave = Utilities.ImageSourceToBitmap((BitmapImage)workspaceImage.Source))
            {
                _appService.SaveImage(fileToSave);                    
            }           
        }

        private void ExitApplication()
        {
            // if there are undaved changes
            if (!_mainWindowVM.HasUnsavedChanges)
            {
                _appService.Exit();
            }
        }

        private void Toolbar_ToolbarItemClicked(object sender, ExecutedRoutedEventArgs e)
        {            
            // PG: bad practice! TODO: FIx in future refactor
            switch (e.Parameter) {
                case "Save":
                    SaveImage();
                    break;
                case "Exit":
                    ExitApplication();
                    break;
                case "Open":
                    _appService.OpenImage();
                    break;
                case "Undo":
                    _appService.Undo();
                    break;
                case "Redo":
                    _appService.Redo();
                    break;
                case "Crop":
                    // open crop Panel
                    break;
                case "4-pt Straighten":
                    // open 4pt-Straighten Panel
                    break;
                case "Rotate Left":
                    _appService.RotateCounterClockWise();
                    break;
                case "Rotate Right":
                    _appService.RotateClockWise();
                    break;
                case "Flip Horizontal":
                    _appService.FlipHorizontal();
                    break;
                case "Flip Vertical":
                    _appService.FlipVertical();
                    break;
                case "Convert To Greyscale":
                    _appService.ConvertToGreyScale();
                    break;
                case "Convert to Sepia":
                    _appService.ConvertToSepia();
                    break;
                case "Colorize with Reference":
                    break;
                case "Colorize with Annotations":
                    break;
                case "Impulse Noise Reduction":
                    // open INR Panel
                    break;
            }

        }
        #endregion

        public void SetSourceImage(string path)
        {
            _mainWindowVM.SetImage(path, null);
            if (File.Exists(path))
            {
                Uri fileUri = new Uri(path);
                imageName.Text = path.Substring(path.LastIndexOf(@"\"));
                workspaceImage.Source = new BitmapImage(fileUri);
            }
            else
            {
                string msgBoxText = string.Format("No file exists at indicated path: {0}", path);
                MessageBox.Show(msgBoxText, "Unable to load File");
            }
        }
    }
}
