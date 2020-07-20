using System.Windows;

namespace MyClient.Windows.ChatWindow
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    partial class View : Window
    {
        public View(ViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
