namespace MyClient
{
    class Constants
    {
        public static class Connection
        {
            public static string ServerIPAddress = "25.0.178.195";
            public static int ServerPort = 53101;
        }

        public static class StartupWindow
        {
            public static string[] CreateAccountResultStrings =
            {
                "Account already exists",
                "Too many attempts (try again in a few minutes)",
                "Connection failed!",
                "Account created!",
                "Passwords don't match!"
            };

            public static string[] LoginResultStrings =
            {
                "Login failed!",
                "Too many attempts (try again in a few minutes)",
                "Connection failed!",
                "Login success!"
            };
        }
    }
}
