using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FotoFaultFixerUI.Controls
{
    /// <summary>
    /// Interaction logic for ProgressBar.xaml
    /// </summary>
    public partial class ProgressReportBar : UserControl
    {
        public ProgressReportBar()
        {
            InitializeComponent();
        }

        public void UpdateProgress(int progressPercent)
        {
            ProgressBar.Value = progressPercent;
            ProgressBarValue.Text = String.Format("{0}%", progressPercent);
        }

        public void Start()
        {
            UpdateProgress(0);
            this.Visibility = Visibility.Visible;            
        }

        public void ResetAndClose()
        {
            this.Visibility = Visibility.Collapsed;
            UpdateProgress(0);
        }
    }
}
