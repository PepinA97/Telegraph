using MyClient.Networking;
using MyClient.Networking.Requests;
using System;
using System.Windows.Input;

namespace MyClient.Windows.ChatWindow.Commands
{
    class MessageEntered : ICommand
    {
        ViewModel ViewModel;

        public MessageEntered(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {

            }

            remove
            {

            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        void ICommand.Execute(object parameter)
        {
            IRequest request = new SendMessage(ViewModel.SelectedChat.RecipientUsername, ViewModel.MessageBoxContent);

            Session.SendRequest(request);

            ViewModel.MessageBoxContent = "";

            ViewModel.OnPropertyChanged("MessageBoxContent");
        }
    }
}
