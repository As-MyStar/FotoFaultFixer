using FotoFaultFixerUI.Views;
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
            
            // If a image path is passed in as a console param
            if (e.Args.Length == 1)
            {
                // apply it and load its image
                mainAppWindow.SetSourceImage(e.Args[0]);
            }

            mainAppWindow.Show();            
        }
    }
}
