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
#if DEBUG_CHATWINDOW_ONLY
            bool shouldLogout = false;

            Windows.Startup startupWindow = new Windows.Startup();

            // Show "startup window" dialog, get result - if true then start main window
            bool? result = startupWindow.ShowDialog();

            if ((result != null) && ((bool)result))
            {
                // Initialize the view model
                ViewModel viewModel = new ViewModel(startupWindow.Username);

                // Start the session (also shows the view)
                Session.Start(viewModel);

                Session.Stop();

                shouldLogout = viewModel.ShouldLogout;
            }
            Shutdown();
#else

            // Initialize the view model
            ChatWindow.ViewModel viewModel = new ChatWindow.ViewModel("Anderson");

            // Start the session (also shows the view)
            Session.Start(null, viewModel);
#endif

        }
    }
}
