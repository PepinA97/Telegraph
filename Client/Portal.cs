using MyClient.Networking;
using MyClient.Networking.Posts;
using MyClient.Networking.Requests;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MyClient
{
    static class Portal
    {
        public enum Result
        {
            Failure,
            TooManyAttempts,
            ConnectionFailed,
            Success,
            PasswordsNotMatching
        }

        public static Result CreateAccount(TcpClient tcpClient, string username, string password)
        {
            IRequest request = new CreateAccount(username, Encoding.ASCII.GetBytes(password));

            // Get Json from sending request
            string content = JsonRequestRoundtrip(tcpClient, request);

            // Get post
            CreateAccountResult post = JsonSerializer.Deserialize<CreateAccountResult>(content);

            if (post.Result == CreateAccountResult.ResultType.Success)
            {
                return Result.Success;
            }
            else if (post.Result == CreateAccountResult.ResultType.TooManyAttempts)
            {
                return Result.TooManyAttempts;
            }
            else
            {
                return Result.Failure;
            }
        }

        public static Result Login(TcpClient tcpClient, string username, string password)
        {
            IRequest request = new AttemptLogin(username, Encoding.ASCII.GetBytes(password));

            // Get Json from sending request
            string content = JsonRequestRoundtrip(tcpClient, request);

            // Get post
            AttemptLoginResult post = JsonSerializer.Deserialize<AttemptLoginResult>(content);

            if (post.Result == AttemptLoginResult.ResultType.Success)
            {
                return Result.Success;
            }
            else if (post.Result == AttemptLoginResult.ResultType.TooManyAttempts)
            {
                return Result.TooManyAttempts;
            }
            else
            {
                return Result.Failure;
            }
        }

        static string JsonRequestRoundtrip(TcpClient tcpClient, IRequest request)
        {
            // Package the request
            byte[] package = IRequest.Package(request);

            // Send the package
            Connection.SendData(tcpClient, package);

            // Wait for and receive data
            byte[] data = Connection.ReceiveData(tcpClient);

            // Rest of data is object of request
            return Encoding.ASCII.GetString(data, 1, (data.Length - 1));
        }
    }
}
