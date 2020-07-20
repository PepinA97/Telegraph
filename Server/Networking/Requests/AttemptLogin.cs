using MyServer.Networking.Posts;
using System.Text;

namespace MyServer.Networking.Requests
{
    class AttemptLogin : IRequest
    {
        bool IRequest.RequireAuthentication => false;

        public string Username { get; set; }
        public byte[] Password { get; set; }

        void IRequest.Execute(Session session)
        {
            AttemptLoginResult attemptLoginResult;

            if (!Security.IsStruckOut(session.GetIPAddress()))
            {
                if (Database.DoesUsernameExist(Username))
                {
                    int userID = Database.GetUserIDFromUsername(Username);

                    // Decrypt password
                    string decryptedPassword = Security.DecryptPassword(Password);

                    // Get the real password hash
                    string realPasswordHash = Database.GetPasswordHash(userID);

                    // Check if hash matches stored hash for user
                    if (realPasswordHash == Security.CreatePasswordHash(Encoding.ASCII.GetBytes(decryptedPassword)))
                    {
                        session.Authenticate(userID);

                        attemptLoginResult = new AttemptLoginResult(AttemptLoginResult.ResultType.Success);
                    }
                    else
                    {
                        // Wrong password
                        attemptLoginResult = new AttemptLoginResult(AttemptLoginResult.ResultType.Failure);
                    }
                }
                else
                {
                    // Account not found
                    attemptLoginResult = new AttemptLoginResult(AttemptLoginResult.ResultType.Failure);
                }

                Security.Strike(session.GetIPAddress());
            }
            else
            {
                attemptLoginResult = new AttemptLoginResult(AttemptLoginResult.ResultType.TooManyAttempts);
            }

            session.SendPost(attemptLoginResult);
        }
    }
}
