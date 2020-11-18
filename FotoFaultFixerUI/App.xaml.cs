using System.Windows;

namespace FotoFaultFixerUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainAppWindow = new MainWindow();
            
            if (e.Args.Length == 1)
            {
                mainAppWindow.SetSourceImage(e.Args[0]);
            }

            mainAppWindow.Show();            
        }
    }
}
