using System;

namespace MyClient.Models
{
    public class Message
    {
        public string SenderUsername { get; set; }
        public string Content { get; set; }

        public DateTime WhenSent { get; set; }
    }
}
