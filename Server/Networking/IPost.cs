using MyServer.Networking.Posts;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MyServer.Networking
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

        public static byte[] Package(IPost post)
        {
            List<byte> byteList = new List<byte>();

            Type type;
            string content;

            switch (post)
            {
                case AttemptLoginResult obj:
                    content = JsonSerializer.Serialize(obj);
                    type = Type.AttemptLoginResult;
                    break;
                case CreateAccountResult obj:
                    content = JsonSerializer.Serialize(obj);
                    type = Type.CreateAccountResult;
                    break;
                case GetChatsResult obj:
                    content = JsonSerializer.Serialize(obj);
                    type = Type.GetChatsResult;
                    break;
                case RelayedMessage obj:
                    content = JsonSerializer.Serialize(obj);
                    type = Type.RelayedMessage;
                    break;
                case TestUsernameResult obj:
                    content = JsonSerializer.Serialize(obj);
                    type = Type.TestUsernameResult;
                    break;
                default:
                    return null;
            }

            Log.PostPacked(type, content);

            // Make the first byte the type
            byteList.Add((byte)type);

            byte[] data = Encoding.ASCII.GetBytes(content);

            // Append content to the list of bytes
            byteList.AddRange(data);

            return byteList.ToArray();
        }
    }
}
