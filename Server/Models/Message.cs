using System;

namespace MyServer.Models
{
    class Message
    {
        public string SenderUsername { get; set; }
        public string Content { get; set; }

        public DateTime WhenSent { get; set; }

        public Message(string senderUsername, string content)
        {
            SenderUsername = senderUsername;
            Content = content;

            WhenSent = DateTime.Now;
        }

        public Message(string senderUsername, string content, DateTime whenSent)
        {
            SenderUsername = senderUsername;
            Content = content;

            WhenSent = whenSent;
        }
    }
}
