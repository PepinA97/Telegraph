using MyServer.Models;
using MyServer.Networking.Posts;

namespace MyServer.Networking.Requests
{
    class SendMessage : IRequest
    {
        bool IRequest.RequireAuthentication => true;

        public string ReceiverUsername { get; set; }
        public string Content { get; set; }

        void IRequest.Execute(Session session)
        {
            // Make sure receiver username exists or an exception will be thrown!
            if (Database.DoesUsernameExist(ReceiverUsername))
            {
                // Get needed info
                string senderUsername = Database.GetUsernameFromUserID(session.GetUserID());
                int receiverUserID = Database.GetUserIDFromUsername(ReceiverUsername);

                // Create the chat if it doesn't exist
                if (!Database.DoesChatExist(session.GetUserID(), receiverUserID))
                {
                    Database.CreateChat(session.GetUserID(), receiverUserID);
                }

                // Get the chat ID
                int chatID = Database.GetChatID(session.GetUserID(), receiverUserID);

                // Create the message to be sent and stored
                Message message = new Message(senderUsername, Content);

                // Add to database
                Database.AddMessageToChat(message, chatID);

                // Broadcast to connected sessions
                RelayedMessage relayedMessage = new RelayedMessage(message, ReceiverUsername);

                foreach (Session activeSession in Server.GetActiveSessions())
                {
                    // If the session is logged in to the same user
                    if (session.GetUserID() == activeSession.GetUserID())
                    {
                        activeSession.SendPost(relayedMessage);
                    }
                    // If a chat exists, send the changed username to the session
                    else if (Database.DoesChatExist(session.GetUserID(), activeSession.GetUserID()))
                    {
                        activeSession.SendPost(relayedMessage);
                    }
                }
            }
        }
    }
}
