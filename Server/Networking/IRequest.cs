using MyServer.Networking.Requests;
using System.Text;
using System.Text.Json;

namespace MyServer.Networking
{
    interface IRequest
    {
        public enum Type
        {
            Disconnect,
            AttemptLogin,
            CreateAccount,
            GetChats,
            SendMessage,
            TestUsername,
            KeepAlive = byte.MaxValue
        }

        bool RequireAuthentication { get; }

        void Execute(Session session);

        public static void ExecuteRequest(IRequest request, Session session)
        {
            if (request.RequireAuthentication == true)
            {
                if (session.IsAuthenticated())
                {
                    request.Execute(session);
                }
                else
                {
                    return;
                }
            }
            else
            {
                request.Execute(session);
            }
        }

        public static IRequest Unpackage(byte[] data)
        {
            // Get type from the first byte
            Type type = (Type)data[0];

            // Rest of data is object of request
            string content = Encoding.ASCII.GetString(data, 1, data.Length - 1);

            Log.RequestUnpacked(type, content);

            switch (type)
            {
                case Type.AttemptLogin:
                    return JsonSerializer.Deserialize<AttemptLogin>(content);

                case Type.CreateAccount:
                    return JsonSerializer.Deserialize<CreateAccount>(content);

                case Type.GetChats:
                    return JsonSerializer.Deserialize<GetChats>(content);

                case Type.SendMessage:
                    return JsonSerializer.Deserialize<SendMessage>(content);

                case Type.TestUsername:
                    return JsonSerializer.Deserialize<TestUsername>(content);

                // Keep alive
                default:
                    return null;
            }
        }
    }
}
