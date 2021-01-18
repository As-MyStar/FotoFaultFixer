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
        
        //private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        //{
        //    ToolbarItemClicked.Invoke(this, e);
        //}

        private void toolbarButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            ToolbarItemClicked.Invoke(sender, e);
        }
    }
}
