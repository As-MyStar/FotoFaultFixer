using System.ComponentModel;

namespace FotoFaultFixerUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _originalImagePath;
        private bool _canModify;
        private bool _canSave;

        public string sourceImagePath
        {
            get => _originalImagePath;
            set {
                _originalImagePath = value;
                CanModify = true;
                CanSave = false;
            }
        }

        public bool CanModify {
            get => _canModify; 
            set
            {
                _canModify = value;
                OnPropertyChanged("CanModify");
            }
        }
        
        public bool CanSave
        {
            get => _canSave;
            set
            {
                _canSave = value;
                OnPropertyChanged("CanSave");
            }
        }
        public int LightNoiseSuppressionAmount { get; set; }
        public int DarkNoiseSupressionAmount { get; set; }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
