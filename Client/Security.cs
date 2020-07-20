using System.Text;

namespace MyClient
{
    class Security
    {
        public static byte[] EncryptPassword(string password)
        {
            return Encoding.ASCII.GetBytes(password);
        }
    }
}
