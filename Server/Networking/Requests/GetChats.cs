using MyServer.Models;
using MyServer.Networking.Posts;
using System.Collections.Generic;

namespace MyServer.Networking.Requests
{
    class GetChats : IRequest
    {
        // Nothing
        bool IRequest.RequireAuthentication => true;

        void IRequest.Execute(Session session)
        {
            List<Chat> chats = new List<Chat>();

            int[] userIDs = Database.GetUserIDsFromExistingChats(session.GetUserID());

            foreach (int recipientUserID in userIDs)
            {
                int chatID = Database.GetChatID(session.GetUserID(), recipientUserID);

                Chat chat = Database.GetChat(chatID);

                string username = Database.GetUsernameFromUserID(session.GetUserID());
                if (chat.InitiatorUsername != username)
                {
                    chat.RecipientUsername = chat.InitiatorUsername;
                    chat.InitiatorUsername = username;
                }

                chats.Add(chat);
            }

            session.SendPost(new GetChatsResult(chats));
        }
    }
}
