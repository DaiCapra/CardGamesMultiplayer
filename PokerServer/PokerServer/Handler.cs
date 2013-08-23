using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
namespace PokerServer
{
    class Handler
    {
        private int handlerID;
        private Socket socket;
        private Server server;

        private int timeSinceKeepAlive;
        private int messageIndex = 1;
        private bool activeConnection;
        public string lastMsg = "";

        public Handler(int id, Server s)
        {
            handlerID = id;
            server = s;
        }
        public void Run()
        {
            Console.WriteLine("Handler " + ID + " has started.");
        }

        public void ListenOnPort()
        { 

            Console.WriteLine("Handler " + ID + " listens.");
            socket = server.Listener.AcceptSocket();
            Thread.Sleep(10);
            if (socket.Connected)
            {
                Console.WriteLine("Handler " + ID + " has connected.");

                activeConnection = true;

                server.CurrentConnections++;
                server.ConnectionIndex++;

                server.GetNewConnection();
                server.NewReadThread();
            }
        }
        public void Read()
        {
            while (true)
            {
                if (socket != null)
                {
                    if (socket.Connected)
                    {
                        byte[] b = new byte[100];
                        int k = socket.Receive(b);

                        char[] c = new char[k];

                        for (int i = 0; i < k; i++)
                        {
                            c[i] = Convert.ToChar(b[i]);
                        }
                        string s = new string(c);

                        if (s != lastMsg)
                        {
                            
                            lastMsg = s;
                            Console.WriteLine("Handler " + ID + ": " + s);
                            server.Broadcast(s, ID);
                        }
                        Thread.Sleep(10);
                    }
                }
            }
        }
        public void Write(string s)
        {
            if (messageIndex == 10)
                messageIndex = 1;

            string s2 = messageIndex.ToString() + s;
            messageIndex++;
            ASCIIEncoding asen = new ASCIIEncoding();
            socket.Send(asen.GetBytes(s2));
        }
        public void Disconnect()
        {
            socket.Close();
            activeConnection = false;
        }
        public Socket Socket
        {
            get { return socket; }
        }
        public int ID
        {
            get { return handlerID; }
        }
        public bool ActiveConnection
        {
            get { return activeConnection; }
        }
        public int TimeSinceKeepAlive
        {
            get { return timeSinceKeepAlive; }
            set { timeSinceKeepAlive = value; }
        }
    }
}
