using System.Windows.Controls;
using System.Windows.Input;

namespace FotoFaultFixerUI.Controls
{
    /// <summary>
    /// Interaction logic for Toolbar.xaml
    /// </summary>
    public partial class Toolbar : UserControl
    {
        public static RoutedCommand MenuItemSelected { get; } = new RoutedCommand("MenuItemSelected", typeof(Toolbar));
        public event ExecutedRoutedEventHandler ToolbarItemClicked;

        public Toolbar()
        {
            InitializeComponent();
        }
        
        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ToolbarItemClicked.Invoke(this, e);
        }
    }
}
