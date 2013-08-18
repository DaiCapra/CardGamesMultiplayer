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
    class Server
    {
        private const int MAX_CONNECTIONS = 200;

        private TcpListener listener;
        private List<Handler> handlers;
        private List<Thread> readThreads;
        private string ip;
        private int port;
        private int currentConnections;
        private int connectionIndex;

        public Server(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            handlers = new List<Handler>();
            readThreads = new List<Thread>();

            IPAddress address = IPAddress.Parse(ip);
            listener = new TcpListener(address, port);
        }
        public void Run()
        {
            try
            {
                Console.WriteLine("Server started on: " + ip + ":" + port.ToString());
                listener.Start();
                GetNewConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldnt start server!");
                Console.WriteLine(e.ToString());
                throw;
            }
        }
        public void GetNewConnection()
        {
            if (currentConnections < MAX_CONNECTIONS)
            {
                try
                {
                    handlers.Add(new Handler(connectionIndex, this));
                    handlers[connectionIndex].Run();
                    handlers[connectionIndex].ListenOnPort();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    throw;
                }
            }
        }
        public void DisconnectbyIndex(int index)
        {
            handlers.RemoveAt(index);
            readThreads.RemoveAt(index);
        }
        public void NewReadThread()
        {
            readThreads.Add(new Thread(handlers[ConnectionIndex].Read));
            readThreads[ConnectionIndex].Start();

        }
        public void Broadcast(string msg, int senderID)
        {
            foreach (Handler h in handlers)
            {
                if (h.ActiveConnection)
                {
                    if (senderID != h.ID)
                        h.Write("Client " + senderID + ": " + msg);
                }
            }
        }
        public int ConnectionIndex
        {
            get { return connectionIndex; }
            set { connectionIndex = value; }
        }
        public int CurrentConnections
        {
            get { return currentConnections; }
            set { currentConnections = value; }
        }
        public TcpListener Listener
        {
            get { return listener; }
            set { listener = value; }
        }
    }
}
