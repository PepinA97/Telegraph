namespace MyClient.Networking.Requests
{
    class TestUsername : IRequest
    {
        public string Username { get; set; }

        public TestUsername(string username)
        {
            Username = username;
        }
    }
}
