using FotoFaultFixerLib.ImageProcessing;
using FotoFaultFixerUI.Services.Commands;
using Microsoft.Win32;
using System;
using System.Drawing;

namespace FotoFaultFixerUI.Services
{

    class ApplicationService
    {
        const string FILE_EXTENSION_FILTER = "Image File|*.bmp; *.jpg; *.jpeg; *.png; *.tiff";

        ImageStateManager _ism;
        internal event EventHandler<ImageUpdateEventArgs> _imageUpdated;

        public ApplicationService() { }

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
            _ism.Invoke(new RotateCWCommand());
            FireImageUpdate();
        }

        internal void RotateCounterClockWise()
        {
            _ism.Invoke(new RotateCCWCommand());
            FireImageUpdate();
        }

        internal void FlipHorizontal()
        {
            _ism.Invoke(new FlipHorizontalCommand());
            FireImageUpdate();
        }

        internal void FlipVertical()
        {
            _ism.Invoke(new FlipVerticalCommand());
            FireImageUpdate();
        }

        internal void Crop(Point startingPoint, int newWidth, int newHeight)
        {
            _ism.Invoke(new CropCommand(startingPoint, newWidth, newHeight));
            FireImageUpdate();
        }

        internal void FourPointStraighten(Point[] points, bool shouldCrop)
        {
            _ism.Invoke(new FourPointStraightenCommand(points, shouldCrop));
            FireImageUpdate();
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
            _ism.Invoke(new ImpulseNoiseReductionCommand(lightNoiseSuppression, darkNoiseSuppression));
            FireImageUpdate();
        }
        #endregion
    }
}
