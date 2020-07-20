namespace MyClient.Networking.Requests
{
    class AttemptLogin : IRequest
    {
        public string Username { get; set; }
        public byte[] Password { get; set; }

        public AttemptLogin(string username, byte[] password)
        {
            Username = username;
            Password = password;
        }
    }
}
