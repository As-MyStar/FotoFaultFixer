using FotoFaultFixerLib;
using FotoFaultFixerUI.Controls.ImageFunctions;
using FotoFaultFixerUI.Services;
using FotoFaultFixerUI.ViewModels;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace FotoFaultFixerUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationService _appService;
        MainWindowViewModel _mainWindowVM;
        object _toolBarOptionsContent = null; // We use this to save reference to the current option panel

        public MainWindow()
        {
            _mainWindowVM = new MainWindowViewModel();
            _appService = new ApplicationService();
            _appService.ImageUpdated += AppService_ImageUpdated;
            InitializeComponent();
            this.DataContext = _mainWindowVM;
            ToolBarControl.DataContext = _mainWindowVM;
        }

        #region Event Handlers
        private void AppService_ImageUpdated(object sender, ImageUpdateEventArgs e)
        {
            imageWorkspace.SetImage(
                Utilities.BitmapToImageSource(
                    e.Image.ToBitmap()
                )
            );
            
            // When an image updates it due to an ImageFunction
            // So progressReporter is no longer required.
            progressReporter.ResetAndClose();

            // Lets update CanRedo and CanUndo so their toolbar buttons update
            _mainWindowVM.CanRedo = _appService.CanRedo;
            _mainWindowVM.CanUndo = _appService.CanUndo;
        }
        #endregion

        #region Toolbar Handlers
        private void SaveImage()
        {
            using (Bitmap fileToSave = imageWorkspace.GetImage())
            {
                _appService.SaveImage(fileToSave);
            }
        }

        private void ExitApplication()
        {
            // if there are unsaved changes
            //if (!_mainWindowVM.HasUnsavedChanges)
            {
                _appService.Exit();
            }
        }
        private void Toolbar_ToolbarItemClicked(object sender, RoutedEventArgs e)
        {
            bool imageHasBeenModified = true;
            ClearToolOptionsPanel();
            WorkspaceZoomReset();

            // PG: bad practice! TODO: Fix in future refactoring
            switch (((Button)sender).Tag)
            {
                case "Save":
                    SaveImage();
                    imageHasBeenModified = false;
                    break;
                case "Exit":
                    ExitApplication();
                    break;
                case "Open":                    
                    _mainWindowVM.SetImage(_appService.OpenImage());
                    break;
                case "Undo":
                    _appService.Undo();
                    break;
                case "Redo":
                    var progressIndicator = new Progress<int>(ReportImageFunctionProgress);
                    progressReporter.Start();
                    _appService.Redo(progressIndicator);
                    break;
                case "Crop":
                    // open crop Panel
                    break;
                case "4-pt Straighten":
                    _toolBarOptionsContent = new FourPointStraightenPanel();
                    // TODO: Assign handler to TriggerEvent                    
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
                //case "Convert To Greyscale":
                //    _appService.ConvertToGreyScale();
                //    break;
                //case "Convert to Sepia":
                //    _appService.ConvertToSepia();
                //    break;
                //case "Colorize with Reference":
                //    break;
                //case "Colorize with Annotations":
                //    break;
                case "Impulse Noise Reduction":
                    imageHasBeenModified = false;
                    _toolBarOptionsContent = new ImpulseNoiseReductionPanel();
                    ((ImpulseNoiseReductionPanel)_toolBarOptionsContent).ImpulseNoiseReductionTriggerEvent += Inr_ImpulseNoiseReductionTriggerEvent;
                    break;
                default:
                    // Do nothing
                    break;
            }

            if (_toolBarOptionsContent != null)
            {
                OpenToolOptionsPanel();
            }

            if (imageHasBeenModified)
            {
                _mainWindowVM.HasUnsavedChanges = true;
            }
        }

        private void Inr_ImpulseNoiseReductionTriggerEvent(int arg1, int arg2)
        {
            ClearToolOptionsPanel();
            var progressIndicator = new Progress<int>(ReportImageFunctionProgress);
            progressReporter.Start();
            _appService.ImpulseNoiseReduction(arg1, arg2, progressIndicator);
            _mainWindowVM.HasUnsavedChanges = true;
        }

        private void ReportImageFunctionProgress(int value)
        {
            progressReporter.UpdateProgress(value);
        }
        #endregion

        // TODO: Refactor
        public void SetSourceImage(string path)
        {
            _mainWindowVM.SetImage(path);
            if (File.Exists(path))
            {
                Uri fileUri = new Uri(path);
                imageName.Text = path.Substring(path.LastIndexOf(@"\"));
                imageWorkspace.SetImage(new BitmapImage(fileUri));
            }
            else
            {
                string msgBoxText = string.Format("No file exists at indicated path: {0}", path);
                MessageBox.Show(msgBoxText, "Unable to load File");
            }
        }

        public void WorkspaceZoomReset()
        {
            imageWorkspace.ZoomReset();
        }

        private void OpenToolOptionsPanel()
        {
            toolOptions.Content = _toolBarOptionsContent;
            toolOptions.Visibility = Visibility.Visible;
        }

        private void ClearToolOptionsPanel()
        {
            toolOptions.Content = _toolBarOptionsContent = null;
            toolOptions.Visibility = Visibility.Collapsed;
        }
    }
}
