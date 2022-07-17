using FotoFaultFixerLib.ImageProcessing;
using FotoFaultFixerUI.Services.Commands;
using FotoFaultFixerUI.Services.Commands.Base;
using Microsoft.Win32;
using System;
using System.Drawing;

namespace FotoFaultFixerUI.Services
{

    class ApplicationService
    {
        const string FILE_EXTENSION_FILTER = "Image File|*.bmp; *.jpg; *.jpeg; *.png; *.tiff";

        StateHandlerService<CImage> _ism;
        //ImageStateManager _ism;
        internal event EventHandler<ImageUpdateEventArgs> ImageUpdated;

        public ApplicationService() { }

        public bool CanUndo => (_ism != null && _ism.CanUndo());
        public bool CanRedo => (_ism != null && _ism.CanRedo());

        private async void InvokeCmdAndUpdate(ICommand<CImage> cmd, IProgress<int> progressReporter)
        {
            try
            {
                _ism.SetNewState(cmd.Execute(_ism.GetCurrentState(), progressReporter));
                FireImageUpdate();
            } 
            catch(Exception exc)
            {
                // PG: Best way to handle this?
                // Message Box?
            }
        }

        private void FireImageUpdate()
        {
            EventHandler<ImageUpdateEventArgs> handler = ImageUpdated;
            handler?.Invoke(this, new ImageUpdateEventArgs(_ism.GetCurrentState()));
        }

        #region File Actions
        internal void Exit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        internal string OpenImage()
        {
            OpenFileDialog selectFileDialog = new OpenFileDialog()
            {
                Title = "Open Image",
                Filter = FILE_EXTENSION_FILTER
            };

            if (selectFileDialog.ShowDialog() == true)
            {
                using (Bitmap loadedFile = (Bitmap)Image.FromFile(selectFileDialog.FileName))
                {
                    CImage cImg = new CImage(loadedFile);
                    _ism = new StateHandlerService<CImage>(cImg);
                    FireImageUpdate();
                }
            }

            return selectFileDialog.SafeFileName;
        }

        internal void SaveImage(Bitmap fileToSave)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = "Save Image",
                Filter = FILE_EXTENSION_FILTER
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                fileToSave.Save(saveFileDialog.FileName);
            }
        }
        #endregion

        #region Edit Actions
        internal void Undo()
        {
            _ism.Undo();
            FireImageUpdate();
        }

        internal void Redo()
        {
            _ism.Redo();
            FireImageUpdate();
        }
        #endregion

        #region Transformations
        internal void RotateClockWise()
        {
            InvokeCmdAndUpdate(new RotateCWCommand(), null);
        }

        internal void RotateCounterClockWise()
        {
            InvokeCmdAndUpdate(new RotateCCWCommand(), null);
        }

        internal void FlipHorizontal()
        {
            InvokeCmdAndUpdate(new FlipHorizontalCommand(), null);
        }

        internal void FlipVertical()
        {
            InvokeCmdAndUpdate(new FlipVerticalCommand(), null);
        }

        internal void Crop(int x, int y, int newWidth, int newHeight, IProgress<int> progressReporter)
        {
            InvokeCmdAndUpdate(new CropCommand(x, y, newWidth, newHeight), progressReporter);
        }

        internal void FourPointStraighten(Point[] points, IProgress<int> progressReporter)
        {
            InvokeCmdAndUpdate(new FourPointStraightenCommand(points), progressReporter);
        }
        #endregion

        #region Coloring
        internal void ConvertToGreyScale()
        {
            throw new NotImplementedException();
        }

        internal void ConvertToSepia()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Filters
        internal void ImpulseNoiseReduction(int lightNoiseSuppression, int darkNoiseSuppression, IProgress<int> progressReporter)
        {
            InvokeCmdAndUpdate(new ImpulseNoiseReductionCommand(lightNoiseSuppression, darkNoiseSuppression), progressReporter);
        }
        #endregion
    }
}
