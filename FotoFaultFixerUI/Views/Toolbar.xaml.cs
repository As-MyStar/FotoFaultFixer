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

namespace FotoFaultFixerUI.Views
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
