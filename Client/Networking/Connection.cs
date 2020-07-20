using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace MyClient.Networking
{
    static class Connection
    {
        public static TcpClient Open()
        {
            TcpClient tcpClient = new TcpClient();

            try
            {
                tcpClient.Connect(IPAddress.Parse(Constants.Connection.ServerIPAddress), Constants.Connection.ServerPort);
            }
            catch (Exception)
            {
                return null;
            }

            return tcpClient;
        }

        public static void Close(TcpClient tcpClient)
        {
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
        }

        public static bool SendData(TcpClient tcpClient, byte[] data)
        {
            try
            {
                tcpClient.GetStream().Write(data);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static byte[] ReceiveData(TcpClient tcpClient)
        {
            List<byte> byteList = new List<byte>();

            byte[] buffer = new byte[1];

            do
            {
                try
                {
                    tcpClient.GetStream().Read(buffer);

                    byteList.Add(buffer[0]);

                    if (!tcpClient.GetStream().DataAvailable)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    // If unexpected disconnection, return null
                    return null;
                }
            } while (true);

            return byteList.ToArray(); // If there is an /expected/ disconnection, returns 0
        }
    }
}
