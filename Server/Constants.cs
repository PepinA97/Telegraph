namespace MyServer
{
    class Constants
    {
        public class Cryptographer
        {
            public static readonly string DecryptionKey = "";
            public static readonly string InitialVector = "";
        }

        public class Database
        {
            public static readonly string ConnectionString = "";
        }

        public class Log
        {
            public static readonly string RelativeLogDirectory = "logs\\";

            public static readonly bool ShouldPrintToScreen = true;
            public static readonly bool ShouldLogPersistently = true;
        }

        public class Server
        {
            public static readonly string IPAddress = "25.0.178.195";
            public static readonly int DefaultPort = 53101;
        }

        public class Session
        {
            public static readonly int UnauthenticatedUserID = -1;
            public static readonly int MaxStrikesAllowed = 5;
            public static readonly int TimeoutDuration = 15000;
        }
    }
}
