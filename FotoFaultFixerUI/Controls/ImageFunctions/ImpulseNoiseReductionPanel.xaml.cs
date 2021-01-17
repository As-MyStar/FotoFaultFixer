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
    /// Interaction logic for ImpulseNoiseReduction.xaml
    /// </summary>
    public partial class ImpulseNoiseReductionPanel : UserControl
    {
        public event Action<int, int> ImpulseNoiseReductionTriggerEvent;
        private int _lightNoiseSupressionAmount = 20;
        private int _darkNoiseSupressionAmount = 20;

        public ImpulseNoiseReductionPanel()
        {
            InitializeComponent();
        }

        // TODO: COmeback to this...
        public void RemoveEventHandlers()
        {
            // Remove any attached handlers in deconstruction to avoid memory leaks
            foreach (Delegate d in ImpulseNoiseReductionTriggerEvent.GetInvocationList())
            {
                ImpulseNoiseReductionTriggerEvent -= (System.Action<int,int>)d;
            }
        }

        #region sliderHandlers
        private void lightNoiseSupressionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _lightNoiseSupressionAmount = (Int32)e.NewValue;
        }

        private void darkNoiseSupressionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _darkNoiseSupressionAmount = (Int32)e.NewValue;
        }
        #endregion

        private void runINRBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ImpulseNoiseReductionTriggerEvent != null)
            {
                ImpulseNoiseReductionTriggerEvent(_lightNoiseSupressionAmount, _darkNoiseSupressionAmount);
            }
        }
    }
}
