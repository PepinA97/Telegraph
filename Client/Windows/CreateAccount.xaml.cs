using MyClient.Networking;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace MyClient.Windows
{
    /// <summary>
    /// Interaction logic for CreateAccountWindow.xaml
    /// </summary>
    public partial class CreateAccount : Window
    {
        public string Password { get; set; }

        public CreateAccount()
        {
            InitializeComponent();

            CheckIfButtonShouldBeEnabled();
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Portal.Result result;

            if (PasswordBox.Password == ConfirmPasswordBox.Password)
            {
                // Try connection here
                TcpClient TCPClient = Connection.Open();

                if (TCPClient != null)
                {
                    // Connection success
                    result = Portal.CreateAccount(TCPClient, UsernameBox.Text, PasswordBox.Password);
                }
                else
                {
                    // Connection failed
                    result = Portal.Result.ConnectionFailed;
                }

                // Close and clear the client
                Connection.Close(TCPClient);
            }
            else
            {
                // Passwords don't match
                result = Portal.Result.PasswordsNotMatching;
            }

            if (result == Portal.Result.Success)
            {
                DialogResult = true;

                Close();
            }
            else
            {
                // Show result to user
                MessageBox.Show(Constants.StartupWindow.CreateAccountResultStrings[(int)result]);
            }

            // Clear password box
            PasswordBox.Password = string.Empty;
            ConfirmPasswordBox.Password = string.Empty;
        }

        private void UsernameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfButtonShouldBeEnabled();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckIfButtonShouldBeEnabled();
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckIfButtonShouldBeEnabled();
        }

        private void CheckIfButtonShouldBeEnabled()
        {
            CreateAccountButton.IsEnabled = true;

            if (UsernameBox.Text.Length < 1 || PasswordBox.Password.Length < 1 || ConfirmPasswordBox.Password.Length < 1)
            {
                CreateAccountButton.IsEnabled = false;
            }
        }
    }
}
