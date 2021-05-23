using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FotoFaultFixerUI.Controls
{
    /// <summary>
    /// Interaction logic for Toolbar.xaml
    /// </summary>
    public partial class Toolbar : UserControl
    {
        public event RoutedEventHandler ToolbarItemClicked;

        public Toolbar()
        {
            InitializeComponent();
        }
        
        private void ToolbarButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true;
            ToolbarItemClicked.Invoke(sender, e);
        }
    }
}
