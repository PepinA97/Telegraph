using System.Collections.Generic;
using System.Net;

namespace MyServer.Networking
{
    class Server
    {
        static Listener MainListener;
        static List<Session> ActiveSessions;
        static IPAddress IPAddress;
        static int? AssignablePort;

        public static void Start()
        {
            IPAddress = IPAddress.Parse(Constants.Server.IPAddress);

            if(AssignablePort == null)
            {
                AssignablePort = Constants.Server.DefaultPort;
            }

            Security.Initialize();

            MainListener = new Listener(AssignablePort.GetValueOrDefault());

            MainListener.Run();

            ActiveSessions = new List<Session>();

            Log.ServerStarted();
        }

        public static void Stop()
        {
            MainListener.End();

            MainListener = null;

            foreach (Session session in ActiveSessions)
            {
                OnSessionDisconnected(session);
            }

            ActiveSessions = null;

            Log.ServerStopped();
        }

        public static IPAddress GetIPAddress()
        {
            return IPAddress;
        }

        public static void AssignPort(int port)
        {
            AssignablePort = port;
        }

        public static void OnSessionConnected(Session session)
        {
            ActiveSessions.Add(session);

            session.Run();

            Log.SessionConnected(session);
        }

        public static void OnSessionDisconnected(Session session)
        {
            Log.SessionDisconnected(session);

            session.End();

            ActiveSessions.Remove(session);
        }

        public static List<Session> GetActiveSessions()
        {
            return ActiveSessions;
        }
    }
}
