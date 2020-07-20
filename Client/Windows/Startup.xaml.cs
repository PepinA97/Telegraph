using MyClient.Networking;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace MyClient.Windows
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class Startup : Window
    {
        public string Username { get; set; }

        public Startup()
        {
            InitializeComponent();

            SetButtonsIsEnabled();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateAccount createAccountWindow = new CreateAccount();

            bool? result = createAccountWindow.ShowDialog();

            if (result == null)
            {
                result = false;
            }

            if (result == true)
            {
                // Show result to user
                PromptBlock.Text = Constants.StartupWindow.CreateAccountResultStrings[(int)Portal.Result.Success];
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Portal.Result result;

            // Try connection here
            TcpClient TCPClient = Connection.Open();

            if (TCPClient == null)
            {
                // Connection failed
                result = Portal.Result.ConnectionFailed;
            }
            else
            {
                // Connection success
                result = Portal.Login(TCPClient, UsernameBox.Text, PasswordBox.Password);

                if (result == Portal.Result.Success)
                {
                    // Give the TcpClient to the session
                    Session.SetTCPClient(TCPClient);

                    // Set result for dialog true
                    DialogResult = true;

                    // Update client username
                    Username = UsernameBox.Text;

                    // Close the window
                    Close();
                }
            }

            // Show result to user
            PromptBlock.Text = Constants.StartupWindow.LoginResultStrings[(int)result];

            // Clear password box
            PasswordBox.Password = string.Empty;
        }

        private void UsernameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetButtonsIsEnabled();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SetButtonsIsEnabled();
        }

        private void SetButtonsIsEnabled()
        {
            LoginButton.IsEnabled = true;

            if (UsernameBox.Text.Length < 1 || PasswordBox.Password.Length < 1)
            {
                LoginButton.IsEnabled = false;
            }
        }
    }
}
