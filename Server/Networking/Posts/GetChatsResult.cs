using MyServer.Models;
using System.Collections.Generic;

namespace MyServer.Networking.Posts
{
    class GetChatsResult : IPost
    {
        public List<Chat> Chats { get; }

        public GetChatsResult(List<Chat> chats)
        {
            Chats = chats;
        }
    }
}
