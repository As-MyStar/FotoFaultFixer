using FotoFaultFixerLib.ImageProcessing;
using System.ComponentModel;
using System.Drawing;

namespace FotoFaultFixerUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _workingImageName = null;
        private string _workingImagePath = null;
        private bool _hasUnsavedChanges = false;

        public void SetImage(string imagePath)
        {
            WorkingImagePath = imagePath;
            WorkingImageName = _workingImagePath.Substring(_workingImagePath.LastIndexOf(@"\") + 1);
            HasUnsavedChanges = false;
        }

        public bool ImageHasBeenLoaded {
            get
            {
                return !(string.IsNullOrEmpty(_workingImagePath));
            }
        }

        public string WorkingImageName
        {
            get
            {
                return _workingImageName;
            }
            private set
            {
                _workingImageName = value;
                OnPropertyChanged("WorkingImageName");
            }
        }

        public string WorkingImagePath {
            get {
                return _workingImagePath;
            }
            set
            {
                _workingImagePath = value;
                OnPropertyChanged("WorkingImagePath");
                OnPropertyChanged("ImageHasBeenLoaded");
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
