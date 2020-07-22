using MyClient.Models;
using MyClient.Networking;
using MyClient.Networking.Requests;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace MyClient.Windows.ChatWindow.Commands
{
    class SearchUsername : ICommand
    {
        ViewModel ViewModel;

        public SearchUsername(ViewModel viewModel)
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
            IRequest request = new TestUsername(ViewModel.SearchContent);

            Session.SendRequest(request);

            while (true)
            {
                // Hold until the result is posted
                if (ViewModel.TestUsernameResult == true)
                {
                    ViewModel.TestUsernameResult = null;

                    return true;
                }
                else if (ViewModel.TestUsernameResult == false)
                {
                    ViewModel.TestUsernameResult = null;

                    return false;
                }
                else if (ViewModel.TestUsernameResult == null)
                {
                    continue;
                }
            }
        }

        void ICommand.Execute(object parameter)
        {
            Chat newChat = new Chat(ViewModel.CUsername, ViewModel.SearchContent, new List<Message>());

            ViewModel.UserChats.Add(newChat);

            ViewModel.SearchContent = String.Empty;
            ViewModel.OnPropertyChanged("SearchContent");
        }
    }
}
