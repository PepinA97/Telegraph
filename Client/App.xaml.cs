#define DEBUG_CHATWINDOW_ONLY

using MyClient.Windows.ChatWindow;
using System.Windows;

namespace MyClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            while (true)
            {
                Windows.Startup startupWindow = new Windows.Startup();

                // Show "startup window" dialog, get result - if true then start main window
                bool? result = startupWindow.ShowDialog();

                if (result.GetValueOrDefault())
                {
                    // Initialize the view model
                    ViewModel viewModel = new ViewModel(startupWindow.Username);

                    bool isLogout = Session.Start(viewModel);

                    Session.Stop();

                    if (isLogout)
                    {
                        continue;
                    }
                }

                break;
            }

            Shutdown();
        }
    }
}
