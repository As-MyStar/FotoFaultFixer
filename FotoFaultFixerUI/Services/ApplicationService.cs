using FotoFaultFixerLib.ImageProcessing;
using FotoFaultFixerUI.Services.Commands;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Windows;

namespace FotoFaultFixerUI.Services
{

    class ApplicationService
    {
        const string FILE_EXTENSION_FILTER = "Image File|*.bmp; *.jpg; *.jpeg; *.png;";

        ImageStateManager _ism;
        public event EventHandler<ImageUpdateEventArgs> ImageUpdated;

        public ApplicationService() { }

        private void FireImageUpdate()
        {
            EventHandler<ImageUpdateEventArgs> handler = ImageUpdated;
            handler?.Invoke(this, new ImageUpdateEventArgs(_ism.GetCurrentState()));
        }

        public void Exit()
        {
            Application.Current.Shutdown();
        }

        public void OpenImage()
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

        public void SaveImage(Bitmap fileToSave)
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

        public void Undo()
        {
            _ism.Undo();
            FireImageUpdate();
        }

        public void Redo()
        {
            _ism.Redo();
            FireImageUpdate();
        }

        public void RotateClockWise()
        {
            _ism.Invoke(new RotateCWCommand());
            FireImageUpdate();
        }

        public void RotateCounterClockWise()
        {
            _ism.Invoke(new RotateCCWCommand());
            FireImageUpdate();
        }

        public void FlipHorizontal()
        {
            _ism.Invoke(new FlipHorizontalCommand());
            FireImageUpdate();
        }

        public void FlipVertical()
        {
            _ism.Invoke(new FlipVerticalCommand());
            FireImageUpdate();
        }

        internal void ConvertToGreyScale()
        {
            throw new NotImplementedException();
        }

        internal void ConvertToSepia()
        {
            throw new NotImplementedException();
        }
    }
}
