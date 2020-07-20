namespace MyClient.Networking.Requests
{
    class CreateAccount : IRequest
    {
        public string Username { get; set; }
        public byte[] Password { get; set; }

        public CreateAccount(string username, byte[] password)
        {
            Username = username;
            Password = password;
        }
    }
}
