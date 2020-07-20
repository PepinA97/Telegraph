using MyServer.Models;

namespace MyServer.Networking.Posts
{
    class RelayedMessage : IPost
    {
        public Message Message { get; }
        public string ReceiverUsername { get; }

        public RelayedMessage(Message message, string receiverUsername)
        {
            Message = message;
            ReceiverUsername = receiverUsername;
        }
    }
}
