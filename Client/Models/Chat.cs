using System.Collections.Generic;

namespace MyClient.Models
{
    public class Chat
    {
        public string InitiatorUsername { get; set; }
        public string RecipientUsername { get; set; }

        public List<Message> Messages { get; set; }

        public Chat()
        {
            // For JSON
        }

        public Chat(string initiatorUsername, string recipientUsername, List<Message> messages)
        {
            InitiatorUsername = initiatorUsername;
            RecipientUsername = recipientUsername;

            Messages = messages;
        }
    }
}
