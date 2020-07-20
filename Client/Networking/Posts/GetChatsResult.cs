using MyClient.Models;
using MyClient.Windows.ChatWindow;
using System.Collections.Generic;

namespace MyClient.Networking.Posts
{
    class GetChatsResult : IPost
    {
        public List<Chat> Chats { get; set; }

        public void Execute(ViewModel viewModel)
        {
            viewModel.PopulateUserChats(Chats);
        }
    }
}
