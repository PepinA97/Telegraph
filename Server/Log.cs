#undef DEBUG

using MyServer.Networking;
using System;
using System.IO;
using System.Net;

namespace MyServer
{
    class Log
    {
        static DirectoryInfo Directory;
        static StreamWriter Stream;

        public static void Initialize()
        {
            CreateLogFile();
        }

        public static void ServerStarted()
        {
            AddLog("Server started");
        }

        public static void ServerStopped()
        {
            AddLog("Server stopped");
        }
        public static void ListenerRunning(int port)
        {
            AddLog("Listener running on port " + port.ToString());
        }

        public static void ListenerEnded()
        {
            AddLog("Listener ended");
        }
        
        public static void IPAddressStruckOut(IPAddress ipAddress)
        {
            AddLog(ipAddress.ToString() + "$ struck out!");
        }

        public static void UserLoggedIn(Session session)
        {
            string username = Database.GetUsernameFromUserID(session.GetUserID());

            AddLog(SessionDetails(session) + "$ " + username + " logged in");
        }

        public static void UserLoggedOut(Session session)
        {
            string username = Database.GetUsernameFromUserID(session.GetUserID());

            AddLog(SessionDetails(session) + "$ " + username + " logged out");
        }

        public static void AccountCreationAttempted(Session session)
        {
            AddLog(SessionDetails(session) + "$ Account creation failed");
        }

        public static void AccountCreationAttempted(Session session, string Username)
        {
            AddLog(SessionDetails(session) + "$ created new account with username: " + Username);
        }

        public static void SessionConnected(Session session)
        {
            AddLog(SessionDetails(session) + "$ connected");
        }

        public static void SessionDisconnected(Session session)
        {
            AddLog(SessionDetails(session) + "$ disconnected");
        }

#if DEBUG
        public static void RequestUnpacked(IRequest.Type type, string content)
        {
            AddLog("Request unpacked: " + type.ToString() + " " + content);
        }

        public static void PostPacked(IPost.Type type, string content)
        {
            AddLog("Post packed: " + type.ToString() + " " + content);
        }
#endif

        static void AddLog(string line)
        {
            string logRecord = DateTime.Now.ToString() + " - " + line;

            if (Constants.Log.ShouldPrintToScreen)
            {
                Console.Out.WriteLine(logRecord);
            }

            if (Constants.Log.ShouldLogPersistently)
            {
                if(Directory != null)
                {
                    Stream.WriteLine(line);
                }
            }
        }

        static void CreateLogFile()
        {
            try
            {
                Directory = new DirectoryInfo(Constants.Log.RelativeLogDirectory);

                if (!Directory.Exists)
                {
                    Directory.Create();
                }

                Stream = new StreamWriter(Constants.Log.RelativeLogDirectory + DateTime.Now.ToShortTimeString() + ".txt");
            }
            catch (Exception)
            {
                Console.WriteLine("Could not create directory for logs!");
            }
        }

        static string SessionDetails(Session session)
        {
            return session.GetIPAddress() + ":" + session.GetPort();
        }
    }
}
