using MyServer.Networking.Posts;
using System.Text;

namespace MyServer.Networking.Requests
{
    class CreateAccount : IRequest
    {
        bool IRequest.RequireAuthentication => false;

        public string Username { get; set; }
        public byte[] Password { get; set; }

        void IRequest.Execute(Session session)
        {
            CreateAccountResult createAccountResult;

            if (!Security.IsStruckOut(session.GetIPAddress()))
            {
                // Check if the username is available
                if (!Database.DoesUsernameExist(Username))
                {
                    if (Security.IsUsernameWithinRules(Username))
                    {
                        createAccountResult = new CreateAccountResult(CreateAccountResult.ResultType.Success);

                        // Decrypt the password
                        string decryptedPassword = Security.DecryptPassword(Password);

                        // Hash it
                        string passwordHash = Security.CreatePasswordHash(Encoding.ASCII.GetBytes(decryptedPassword));

                        // Store user/password hash
                        Database.CreateAccount(Username, passwordHash);

                        Log.AccountCreationAttempted(session, Username);
                    }
                    else
                    {
                        Log.AccountCreationAttempted(session);

                        return;
                    }
                }
                else
                {
                    createAccountResult = new CreateAccountResult(CreateAccountResult.ResultType.AlreadyExists);

                    Log.AccountCreationAttempted(session);
                }

                Security.Strike(session.GetIPAddress());
            }
            else
            {
                createAccountResult = new CreateAccountResult(CreateAccountResult.ResultType.TooManyAttempts);
            }

            session.SendPost(createAccountResult);
        }
    }
}
