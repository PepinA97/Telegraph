namespace MyServer.Networking.Posts
{
    class TestUsernameResult : IPost
    {
        public bool Success { get; }

        public TestUsernameResult(bool success)
        {
            Success = success;
        }
    }
}
