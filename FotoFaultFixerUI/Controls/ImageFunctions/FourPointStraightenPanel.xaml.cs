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

namespace FotoFaultFixerUI.Controls.ImageFunctions
{
    /// <summary>
    /// Interaction logic for FourPointStraightenPanel.xaml
    /// </summary>
    public partial class FourPointStraightenPanel : UserControl
    {
        public event Action FourPointStraightenTriggerEvent;

        public FourPointStraightenPanel()
        {
            InitializeComponent();            
        }

        public void RemoveEventHandlers()
        {
            // Remove any attached handlers in deconstruction to avoid memory leaks
            foreach (Delegate d in FourPointStraightenTriggerEvent.GetInvocationList())
            {
                FourPointStraightenTriggerEvent -= (System.Action)d;
            }
        }

        private void RunFourPointTransformButton_Click(object sender, RoutedEventArgs e)
        {
            if (FourPointStraightenTriggerEvent != null)
            {
                FourPointStraightenTriggerEvent();
            }
        }
    }
}
