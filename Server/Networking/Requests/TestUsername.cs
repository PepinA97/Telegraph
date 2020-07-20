using MyServer.Networking.Posts;

namespace MyServer.Networking.Requests
{
    class TestUsername : IRequest
    {
        bool IRequest.RequireAuthentication => true;

        public string Username { get; set; }

        void IRequest.Execute(Session session)
        {
            // Find User ID in database
            TestUsernameResult testUsernameResult;

            if (Database.DoesUsernameExist(Username))
            {
                // Does exist
                testUsernameResult = new TestUsernameResult(true);
            }
            else
            {
                // Doesn't exist
                testUsernameResult = new TestUsernameResult(false);
            }

            session.SendPost(testUsernameResult);
        }
    }
}
