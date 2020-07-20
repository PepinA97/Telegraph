using MyClient.Networking.Requests;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MyClient.Networking
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
        }

        public static byte[] Package(IRequest request)
        {
            List<byte> byteList = new List<byte>();

            Type type;
            string content;

            switch (request)
            {
                case AttemptLogin obj:
                    content = JsonSerializer.Serialize(obj);
                    type = Type.AttemptLogin;
                    break;
                case CreateAccount obj:
                    content = JsonSerializer.Serialize(obj);
                    type = Type.CreateAccount;
                    break;
                case GetChats obj:
                    content = JsonSerializer.Serialize(obj);
                    type = Type.GetChats;
                    break;
                case SendMessage obj:
                    content = JsonSerializer.Serialize(obj);
                    type = Type.SendMessage;
                    break;
                case TestUsername obj:
                    content = JsonSerializer.Serialize(obj);
                    type = Type.TestUsername;
                    break;
                default:
                    return null;
            }

            // Make the first byte the type
            byteList.Add((byte)type);

            byte[] data = Encoding.ASCII.GetBytes(content);

            // Append content to the list of bytes
            byteList.AddRange(data);

            return byteList.ToArray();
        }
    }
}
