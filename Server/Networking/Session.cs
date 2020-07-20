using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MyServer.Networking
{
    class Session
    {
        TcpClient TCPClient;
        NetworkStream Stream;
        Thread Thread;

        int UserID;

        public Session(TcpClient tcpClient)
        {
            TCPClient = tcpClient;

            UserID = Constants.Session.UnauthenticatedUserID;

            Security.AddNewStrikee(GetIPAddress());
        }

        public void Run()
        {
            try
            {
                Stream = TCPClient.GetStream();

                Stream.ReadTimeout = Constants.Session.TimeoutDuration;
                Stream.WriteTimeout = Constants.Session.TimeoutDuration;
            }
            catch (Exception)
            {
                return;
            }

            Thread = new Thread(ThreadProc);

            Thread.Start();
        }

        public void End()
        {
            if (UserID != Constants.Session.UnauthenticatedUserID)
            {
                Log.UserLoggedOut(this);
            }

            TCPClient.Close();

            Thread = null;
        }

        public IPAddress GetIPAddress()
        {
            return ((IPEndPoint)TCPClient.Client.RemoteEndPoint).Address;
        }

        public int GetPort()
        {
            return ((IPEndPoint)TCPClient.Client.RemoteEndPoint).Port;
        }

        public int GetUserID()
        {
            return UserID;
        }

        public void Authenticate(int userID)
        {
            UserID = userID;

            Log.UserLoggedIn(this);
        }

        public bool IsAuthenticated()
        {
            if (UserID == Constants.Session.UnauthenticatedUserID)
            {
                return false;
            }

            return true;
        }

        public void SendPost(IPost post)
        {
            byte[] data = IPost.Package(post);

            SendData(data);
        }

        void ThreadProc()
        {
            while (true)
            {
                // Receive data
                byte[] receivedData = ReceiveData();

                if (receivedData == null)
                {
                    // Unexpected disconnection, kill thread
                    Server.OnSessionDisconnected(this);
                    return;
                }
                else if (receivedData[0] == (byte)IRequest.Type.Disconnect)
                {
                    // Disconnected
                    Server.OnSessionDisconnected(this);
                    return;
                }

                IRequest request = IRequest.Unpackage(receivedData);

                if (request == null)
                {
                    // Keep alive
                    continue;
                }

                // Execute request
                IRequest.ExecuteRequest(request, this);
            }
        }

        byte[] ReceiveData()
        {
            List<byte> byteList = new List<byte>();

            byte[] buffer = new byte[1];

            do
            {
                try
                {
                    Stream.Read(buffer);

                    byteList.Add(buffer[0]);

                    if (!Stream.DataAvailable)
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

        void SendData(byte[] data)
        {
            try
            {
                Stream.Write(data);
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
