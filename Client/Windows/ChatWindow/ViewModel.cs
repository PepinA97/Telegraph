using MyClient.Models;
using MyClient.Networking;
using MyClient.Networking.Requests;
using MyClient.Windows.ChatWindow.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MyClient.Windows.ChatWindow
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region On Property Changed
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        public ViewModel(string username)
        {
            View = new View(this);

            // Set visible username
            CUsername = username;

            // Initialize empty chats
            UserChats = new ObservableCollection<Chat>();

            // Commands initialization
            MessageEntered = new MessageEntered(this);
            SearchUsername = new SearchUsername(this);

            ChatMessages = new ObservableCollection<string>();

            // Create locks for accessing across threads
            UserChatsLock = new object();
            BindingOperations.EnableCollectionSynchronization(UserChats, UserChatsLock);

            ChatMessagesLock = new object();
            BindingOperations.EnableCollectionSynchronization(ChatMessages, ChatMessagesLock);

            // Get existing chats from server
            IRequest request = new GetChats();
            Session.SendRequest(request);
        }

        public View View;

        public string CUsername { get; set; }

        Chat _SelectedChat;
        public Chat SelectedChat
        {
            get
            {
                return _SelectedChat;
            }
            set
            {
                ObservableCollection<string> observableMessages = new ObservableCollection<string>();

                if (value != null)
                {
                    foreach (Message msg in value.Messages)
                    {
                        observableMessages.Add(msg.SenderUsername + ": " + msg.Content);
                    }
                    _SelectedChat = value;
                }

                ChatMessages = observableMessages;
                OnPropertyChanged("ChatMessages");
            }
        }

        public ObservableCollection<Chat> UserChats { get; set; }

        public ObservableCollection<string> ChatMessages { get; set; }

        string _SearchContent;
        public string SearchContent
        {
            get
            {
                return _SearchContent;
            }
            set
            {
                _SearchContent = value;
                OnPropertyChanged("SearchBox");
            }
        }

        public string MessageBoxContent { get; set; }

        public bool? TestUsernameResult;

        public bool ShouldLogout { get; set; }

        object UserChatsLock;
        object ChatMessagesLock;

        public ICommand MessageEntered { get; set; }
        public ICommand SearchUsername { get; set; }

        public bool Show()
        {
            bool? result = View.ShowDialog();

            return result.GetValueOrDefault();
        }

        public void PopulateUserChats(List<Chat> chats)
        {
            foreach (Chat chat in chats)
            {
                lock (UserChatsLock)
                {
                    UserChats.Add(chat);
                }
            }
        }

        public void AddMessage(Message message, string receiverUsername)
        {
            string senderUsername = message.SenderUsername;

            foreach (Chat chat in UserChats)
            {
                if (ValidChat(chat, senderUsername, receiverUsername))
                {
                    chat.Messages.Add(message);
                }
            }

            if (_SelectedChat != null)
            {
                if (_SelectedChat.RecipientUsername == receiverUsername || CUsername == receiverUsername)
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        ChatMessages.Add(message.SenderUsername + ": " + message.Content);
                    });

                    OnPropertyChanged("SelectedChat");
                }
            }
        }

        bool ValidChat(Chat chat, string firstUsername, string secondUsername)
        {
            return chat.InitiatorUsername == firstUsername && chat.RecipientUsername == secondUsername ||
                chat.InitiatorUsername == secondUsername && chat.RecipientUsername == firstUsername;
        }
    }
}
