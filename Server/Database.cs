using MyServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MyServer
{
    class Database
    {
        public static bool DoesUsernameExist(string username)
        {
            int result;

            using (SqlConnection connection = new SqlConnection(Constants.Database.ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT COUNT(1) FROM Users WHERE Username = @Username;",
                    connection);

                command.Parameters.AddWithValue("@Username", username);

                try
                {
                    connection.Open();

                    result = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine(e.ToString());

                    return true;
                }
            }

            return (result != 0);
        }

        public static bool DoesUserIDExist(int userID)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(Constants.Database.ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT COUNT(1) FROM Users WHERE ID = @UserID;",
                    connection);

                command.Parameters.AddWithValue("@UserID", userID);

                try
                {
                    connection.Open();

                    result = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception)
                {
                    return true;
                }
            }

            return (result != 0);
        }

        public static bool DoesChatExist(int initiatorUserID, int recipientUserID)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(Constants.Database.ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT COUNT(1) FROM Chats WHERE ((InitiatorUserID = @InitiatorUserID) AND (RecipientUserID = @RecipientUserID)) " +
                    "OR ((InitiatorUserID = @RecipientUserID) AND (RecipientUserID = @InitiatorUserID));"
                    , connection);

                command.Parameters.AddWithValue("@InitiatorUserID", initiatorUserID);
                command.Parameters.AddWithValue("@RecipientUserID", recipientUserID);

                try
                {
                    connection.Open();

                    result = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception)
                {
                    return true;
                }
            }

            return (result != 0);
        }

        public static int GetUserIDFromUsername(string username)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(Constants.Database.ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT ID FROM Users WHERE Username = @Username;",
                    connection);

                command.Parameters.AddWithValue("@Username", username);

                try
                {
                    connection.Open();

                    result = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception)
                {
                    return Constants.Session.UnauthenticatedUserID;
                }
            }

            return result;
        }

        public static string GetUsernameFromUserID(int userID)
        {
            string result;

            using (SqlConnection connection = new SqlConnection(Constants.Database.ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT Username FROM Users WHERE ID = @UserID;",
                    connection);

                command.Parameters.AddWithValue("@UserID", userID);

                try
                {
                    connection.Open();

                    result = (command.ExecuteScalar()).ToString();
                }
                catch (Exception)
                {
                    return String.Empty;
                }
            }

            return result;
        }

        public static string GetPasswordHash(int userID)
        {
            string result;

            using (SqlConnection connection = new SqlConnection(Constants.Database.ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT PasswordHash FROM Users WHERE ID = @UserID;",
                    connection);

                command.Parameters.AddWithValue("@UserID", userID);

                try
                {
                    connection.Open();

                    result = (command.ExecuteScalar()).ToString();
                }
                catch (Exception)
                {
                    return String.Empty;
                }
            }

            return result;
        }

        public static int GetChatID(int initiatorUserID, int recipientUserID)
        {
            object result;

            using (SqlConnection connection = new SqlConnection(Constants.Database.ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT ID FROM Chats WHERE ((InitiatorUserID = @InitiatorUserID) AND (RecipientUserID = @RecipientUserID)) " +
                    "OR ((InitiatorUserID = @RecipientUserID) AND (RecipientUserID = @InitiatorUserID));",
                    connection);

                command.Parameters.AddWithValue("@InitiatorUserID", initiatorUserID);
                command.Parameters.AddWithValue("@RecipientUserID", recipientUserID);

                try
                {
                    connection.Open();

                    result = command.ExecuteScalar();
                }
                catch (Exception)
                {
                    return 0;
                }
            }

            if (result == null)
            {
                return 0;
            }

            return (int)result;
        }

        public static int[] GetUserIDsFromExistingChats(int userID)
        {
            List<int> IDs = new List<int>();

            using (SqlConnection connection = new SqlConnection(Constants.Database.ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT InitiatorUserID, RecipientUserID FROM Chats WHERE ((InitiatorUserID = @UserID) OR (RecipientUserID = @UserID));",
                    connection);

                command.Parameters.AddWithValue("@UserID", userID);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if ((Int32)reader[0] == userID)
                    {
                        IDs.Add((Int32)reader[1]);
                    }
                    else
                    {
                        IDs.Add((Int32)reader[0]);
                    }
                }
            }

            return IDs.ToArray();
        }

        public static Chat GetChat(int chatID)
        {
            int initiatorUserID = 0;
            int recipientUserID = 0;

            using (SqlConnection connection = new SqlConnection(Constants.Database.ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT InitiatorUserID, RecipientUserID FROM Chats WHERE ID = @ChatID",
                    connection);

                command.Parameters.AddWithValue("@ChatID", chatID);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    initiatorUserID = reader.GetInt32(0);
                    recipientUserID = reader.GetInt32(1);
                }
            }

            List<Message> messages = new List<Message>();

            using (SqlConnection connection = new SqlConnection(Constants.Database.ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT SenderUserID, Content, WhenSent FROM Messages WHERE ChatID = @ChatID",
                    connection);

                command.Parameters.AddWithValue("@ChatID", chatID);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string username = GetUsernameFromUserID(reader.GetInt32(0));

                    messages.Add(new Message(username, reader.GetString(1), reader.GetDateTime(2)));
                }
            }

            string initiatorUsername = GetUsernameFromUserID(initiatorUserID);
            string recipientUsername = GetUsernameFromUserID(recipientUserID);

            return new Chat(initiatorUsername, recipientUsername, messages);
        }

        public static void CreateAccount(string username, string passwordHash)
        {
            using SqlConnection connection = new SqlConnection(Constants.Database.ConnectionString);

            SqlCommand command = new SqlCommand(
                "INSERT INTO Users (Username,PasswordHash) VALUES (@Username,@PasswordHash)")
            {
                CommandType = CommandType.Text,
                Connection = connection
            };

            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@PasswordHash", passwordHash);

            connection.Open();

            command.ExecuteNonQuery();
        }

        public static void CreateChat(int initiatorUserID, int recipientUserID)
        {
            using SqlConnection connection = new SqlConnection(Constants.Database.ConnectionString);

            SqlCommand command = new SqlCommand("INSERT INTO Chats (InitiatorUserID,RecipientUserID) VALUES (@InitiatorUserID,@RecipientUserID)")
            {
                CommandType = CommandType.Text,
                Connection = connection
            };

            command.Parameters.AddWithValue("@InitiatorUserID", initiatorUserID);
            command.Parameters.AddWithValue("@RecipientUserID", recipientUserID);

            connection.Open();

            command.ExecuteNonQuery();
        }

        public static void AddMessageToChat(Message message, int chatID)
        {
            using SqlConnection connection = new SqlConnection(Constants.Database.ConnectionString);

            int senderUserID = GetUserIDFromUsername(message.SenderUsername);

            SqlCommand command = new SqlCommand(
                "INSERT INTO Messages (ChatID,SenderUserID,Content,WhenSent) VALUES (@ChatID,@SenderUserID,@Content,@WhenSent)")
            {
                CommandType = CommandType.Text,
                Connection = connection
            };

            command.Parameters.AddWithValue("@ChatID", chatID);
            command.Parameters.AddWithValue("@SenderUserID", senderUserID);
            command.Parameters.AddWithValue("@Content", message.Content);
            command.Parameters.AddWithValue("@WhenSent", message.WhenSent);

            connection.Open();

            command.ExecuteNonQuery();
        }
    }
}