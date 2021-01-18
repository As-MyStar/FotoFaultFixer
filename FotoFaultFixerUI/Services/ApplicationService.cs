using FotoFaultFixerLib.ImageProcessing;
using FotoFaultFixerUI.Services.Commands;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace FotoFaultFixerUI.Services
{

    class ApplicationService
    {
        const string FILE_EXTENSION_FILTER = "Image File|*.bmp; *.jpg; *.jpeg; *.png; *.tiff";

        ImageStateManager _ism;
        internal event EventHandler<ImageUpdateEventArgs> ImageUpdated;

        public ApplicationService() { }

        public bool CanUndo => (_ism != null && _ism.CanUndo());
        public bool CanRedo => (_ism != null && _ism.CanRedo());

        private async void InvokeCmdAndUpdate(ICommandCImage cmd, IProgress<int> progressReporter)
        {
            await _ism.Invoke(cmd, progressReporter);
            FireImageUpdate();
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
                    _ism = new ImageStateManager(cImg);
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
        internal async void Undo()
        {
            await _ism.Undo();
            FireImageUpdate();
        }

        internal async void Redo(IProgress<int> progressReporter)
        {
            await _ism.Redo(progressReporter);
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

        internal void Crop(Point startingPoint, int newWidth, int newHeight, IProgress<int> progressReporter)
        {
            InvokeCmdAndUpdate(new CropCommand(startingPoint, newWidth, newHeight), progressReporter);
        }

        internal void FourPointStraighten(Point[] points, bool shouldCrop, IProgress<int> progressReporter)
        {
            InvokeCmdAndUpdate(new FourPointStraightenCommand(points, shouldCrop), progressReporter);
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
