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
        internal event EventHandler<ImageUpdateEventArgs> _imageUpdated;

        public ApplicationService() { }

        private async void InvokeCmdAndUpdate(ICommandCImage cmd)
        {
            await _ism.Invoke(cmd);
            FireImageUpdate();
        }

        private void FireImageUpdate()
        {
            EventHandler<ImageUpdateEventArgs> handler = _imageUpdated;
            handler?.Invoke(this, new ImageUpdateEventArgs(_ism.GetCurrentState()));
        }

        #region File Actions
        internal void Exit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        internal void OpenImage()
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

        internal async void Redo()
        {
            await _ism.Redo();
            FireImageUpdate();
        }
        #endregion

        #region Transformations
        internal void RotateClockWise()
        {
            InvokeCmdAndUpdate(new RotateCWCommand());
        }

        internal void RotateCounterClockWise()
        {
            InvokeCmdAndUpdate(new RotateCCWCommand());
        }

        internal void FlipHorizontal()
        {
            InvokeCmdAndUpdate(new FlipHorizontalCommand());
        }

        internal void FlipVertical()
        {
            InvokeCmdAndUpdate(new FlipVerticalCommand());
        }

        internal void Crop(Point startingPoint, int newWidth, int newHeight)
        {
            InvokeCmdAndUpdate(new CropCommand(startingPoint, newWidth, newHeight));
        }

        internal void FourPointStraighten(Point[] points, bool shouldCrop)
        {
            InvokeCmdAndUpdate(new FourPointStraightenCommand(points, shouldCrop));
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
        internal void ImpulseNoiseReduction(int lightNoiseSuppression, int darkNoiseSuppression)
        {
            InvokeCmdAndUpdate(new ImpulseNoiseReductionCommand(lightNoiseSuppression, darkNoiseSuppression));
        }
        #endregion
    }
}
