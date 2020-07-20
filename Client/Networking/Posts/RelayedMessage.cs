using MyClient.Models;
using MyClient.Windows.ChatWindow;

namespace MyClient.Networking.Posts
{
    class RelayedMessage : IPost
    {
        public Message Message { get; set; }
        public string ReceiverUsername { get; set; }

        public void Execute(ViewModel viewModel)
        {
            viewModel.AddMessage(Message, ReceiverUsername);
        }
    }
}
