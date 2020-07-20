using MyClient.Networking.Posts;
using MyClient.Windows.ChatWindow;
using System.Text;
using System.Text.Json;

namespace MyClient.Networking
{
    interface IPost
    {
        public enum Type
        {
            Invalid,
            AttemptLoginResult,
            CreateAccountResult,
            GetChatsResult,
            RelayedMessage,
            TestUsernameResult
        }

        public void Execute(ViewModel viewModel);

        public static IPost Unpackage(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            // Get type from the first byte
            Type type = (Type)data[0];

            // Rest of data is object of request
            string content = Encoding.ASCII.GetString(data, 1, data.Length - 1);

            switch (type)
            {
                case Type.AttemptLoginResult:
                    return JsonSerializer.Deserialize<AttemptLoginResult>(content);

                case Type.CreateAccountResult:
                    return JsonSerializer.Deserialize<CreateAccountResult>(content);

                case Type.GetChatsResult:
                    return JsonSerializer.Deserialize<GetChatsResult>(content);

                case Type.RelayedMessage:
                    return JsonSerializer.Deserialize<RelayedMessage>(content);

                case Type.TestUsernameResult:
                    return JsonSerializer.Deserialize<TestUsernameResult>(content);

                default:
                    return null;
            }
        }
    }
}
