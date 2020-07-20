using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MyServer.Networking
{
    class Listener
    {
        TcpListener TCPListener;
        Thread Thread;

        public Listener(int port)
        {
            TCPListener = new TcpListener(Server.GetIPAddress(), port);
        }

        public void Run()
        {
            TCPListener.Start();

            Thread = new Thread(ThreadProc);

            Thread.Start();

            Log.ListenerRunning(((IPEndPoint)TCPListener.LocalEndpoint).Port);
        }

        public void End()
        {
            TCPListener.Stop();

            Thread = null;

            Log.ListenerEnded();
        }

        public int GetPort()
        {
            return ((IPEndPoint)TCPListener.LocalEndpoint).Port;
        }

        void ThreadProc()
        {
            while (true)
            {
                Session newSession = AcceptNewSession();

                if (newSession == null)
                {
                    // Listener ended
                    return;
                }

                Server.OnSessionConnected(newSession);
            }
        }

        Session AcceptNewSession()
        {
            TcpClient tcpClient;

            try
            {
                tcpClient = TCPListener.AcceptTcpClient();
            }
            catch (Exception)
            {
                return null;
            }

            return new Session(tcpClient);
        }
    }
}
