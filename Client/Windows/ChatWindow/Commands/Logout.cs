using MyClient.Models;
using MyClient.Networking;
using MyClient.Networking.Requests;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace MyClient.Windows.ChatWindow.Commands
{
    class Logout : ICommand
    {
        ViewModel ViewModel;

        public Logout(ViewModel viewModel)
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
            ViewModel.Close(true);
        }
    }
}
