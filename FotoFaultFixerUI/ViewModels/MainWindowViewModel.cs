using FotoFaultFixerLib.ImageProcessing;
using System.ComponentModel;
using System.Drawing;

namespace FotoFaultFixerUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _workingImageName;
        private string _workingImagePath;
        private CImage _workingImage;
        private bool _hasUnsavedChanges;

        public void SetImage(string imagePath, Bitmap bmp)
        {
            _workingImagePath = imagePath;
            _workingImageName = _workingImagePath.Substring(_workingImagePath.LastIndexOf(@"\") + 1);
            //_workingImage = new CImage(bmp);
            _hasUnsavedChanges = false;
        }

        public string WorkingImagePath {
            get {
                return _workingImagePath;
            }
            private set
            {
                _workingImagePath = value;
                OnPropertyChanged("WorkingImagePath");
            }
        }

        public bool HasUnsavedChanges
        {
            get => _hasUnsavedChanges;
            set
            {
                _hasUnsavedChanges = value;
                OnPropertyChanged("HasUnsavedChanges");
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
