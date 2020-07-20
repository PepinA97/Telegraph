namespace MyClient.Networking.Requests
{
    class SendMessage : IRequest
    {
        public string ReceiverUsername { get; set; }
        public string Content { get; set; }

        public SendMessage(string receiverUsername, string content)
        {
            ReceiverUsername = receiverUsername;
            Content = content;
        }
    }
}
