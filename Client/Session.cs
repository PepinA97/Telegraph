using MyClient.Networking;
using MyClient.Windows.ChatWindow;
using System;
using System.Net.Sockets;
using System.Threading;

namespace MyClient
{
    static class Session
    {
        static TcpClient TCPClient { get; set; }

        static ViewModel ViewModel { get; set; }

        static Thread KeepAliveThread;
        static Thread ReceiveThread;

        public static bool Start(ViewModel viewModel)
        {
            ViewModel = viewModel;

            KeepAliveThread = new Thread(new ThreadStart(KeepAliveThreadProc));
            KeepAliveThread.Start();

            ReceiveThread = new Thread(new ThreadStart(ReceiveThreadProc));
            ReceiveThread.Start();

            // Show the view
            return ViewModel.Show(new View());
        }

        public static void Stop()
        {
            Connection.Close(TCPClient);

            ReceiveThread.Join();
            ReceiveThread = null;

            KeepAliveThread.Join();
            KeepAliveThread = null;

            ViewModel = null;
        }

        public static void SetTCPClient(TcpClient tcpClient)
        {
            TCPClient = tcpClient;
        }

        public static void SendRequest(IRequest request)
        {
            byte[] data = IRequest.Package(request);

            Connection.SendData(TCPClient, data);
        }

        static IPost ReceivePost()
        {
            // Receive posts from the server, execute them
            byte[] receivedData = Connection.ReceiveData(TCPClient);

            return IPost.Unpackage(receivedData);
        }

        static void KeepAliveThreadProc()
        {
            DateTime whenSendNextPacket = DateTime.Now.AddSeconds(10);
            
            while (true)
            {
                if (!TCPClient.Connected)
                {
                    return;
                }

                if(DateTime.Compare(DateTime.Now, whenSendNextPacket) > 0)
                {
                    // Send keep alive packets every such and such interval
                    byte[] keepAlivePacket = new byte[1];

                    keepAlivePacket[0] = Byte.MaxValue;

                    if (!Connection.SendData(TCPClient, keepAlivePacket))
                    {
                        // If the TCP Client closes, then end the thread
                        return;
                    }

                    whenSendNextPacket = DateTime.Now.AddSeconds(10);
                }
            }
        }

        static void ReceiveThreadProc()
        {
            while (true)
            {
                IPost post = ReceivePost();

                if (post == null)
                {
                    return;
                }

                post.Execute(ViewModel);
            }
        }
    }
}
