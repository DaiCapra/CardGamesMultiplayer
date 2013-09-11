using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using MySql.Data.MySqlClient;
using Core;


namespace PokerServer
{
    class Server
    {
        private string DatabaseConnectionString = "Server=localhost;Database=blog;Uid=root;Pwd=;";

        private const int MAX_CONNECTIONS = 200;
        private const int KEEPALIVE_CAP = 30;

        private string ip;
        private int port;

        private TcpListener listener;
        private List<Handler> handlers;
        private List<Thread> readThreads;
        Thread listenThread = null;

        private int currentConnections;
        private int connectionIndex;

        private MySqlConnection databaseConnection;
        private MySqlDataReader databaseReader;
        private MySqlCommand databaseCommand;

        private List<User> users;

        public Server(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            handlers = new List<Handler>();
            readThreads = new List<Thread>();
            users = new List<User>();

            IPAddress address = IPAddress.Parse(ip);
            listener = new TcpListener(address, port);
        }
        public void Run()
        {
            try
            {
                Console.WriteLine("Server started on: " + ip + ":" + port.ToString());
                InitDatabase();
                ReadFromDatabase();

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

        private bool InitDatabase()
        {
            databaseConnection = new MySqlConnection(DatabaseConnectionString);
          
            try
            {
                databaseConnection.Open();
                Console.WriteLine("Connection to user database established");
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Error connecting to user database");
                return false;
            }
        }
        private void ReadFromDatabase()
        {
            databaseCommand = databaseConnection.CreateCommand();
            databaseCommand.CommandText = "SELECT * FROM users";

            try
            {
                databaseReader = databaseCommand.ExecuteReader();
            }
            catch (Exception e)
            {
                
            }
            if (databaseReader != null)
            {
                while (databaseReader.Read())
                {
                    string name = "";
                    string email = "";
                    string password = "";
                    string salt = "";


                    for (int i = 1; i < databaseReader.FieldCount; i++)
                    {
                        string s = databaseReader.GetValue(i).ToString();
                        Console.WriteLine("i "+i+": "+ s);
                        switch (i)
                        {
                            case 1:
                                name = s;
                                break;
                            case 2:
                                email = s;
                                break;
                            case 3:
                                password = s;
                                break;
                            case 4:
                                salt = s;
                                users.Add(new User(name, email, password, salt));
                                break;
                        }
                    }
                }
            }
        }
        public void Update()
        {
            while (true)
            {
                DateTime date = DateTime.Now;
                string s = string.Format("{0:HH:mm:ss tt}", date);
                //Console.WriteLine(s);

                Packet p = new Packet(PacketData.PacketType.ServerMessage.ToString(), new string[]{PacketData.ServerType.Login.ToString()});
        
                //TimeOutCheck();
                Thread.Sleep(10);
            }
        }
        public void IncomingPacket(string message, int handlerID)
        {
            string s = message.Remove(0, 1);
            Console.WriteLine(s);
            Packet p = new Packet(PacketData.PacketType.KeepAlive.ToString(), new string[]{"test"});
            List<object> objs = p.JSONToList(s);

            try
            {
                string field1 = objs[0].ToString();
                string type = PacketData.PacketType.Disconnect.ToString();
                string value;

                if (field1 == type)
                {
                    DisconnectbyIndex(handlerID);
                }

                type = PacketData.PacketType.GameMessage.ToString();

                type = PacketData.PacketType.KeepAlive.ToString();

                type = PacketData.PacketType.PlayerMessage.ToString();

                type = PacketData.PacketType.ServerMessage.ToString();

                //Console.WriteLine(type);
                //Console.WriteLine(field1);

                if (field1 == type)
                {
                    value = objs[1].ToString();
                    if (value == "Login")
                    {
                        Console.WriteLine("Handler " + handlerID + ", trying to log in");
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
        }


        public void TimeOutCheck()
        {
            foreach (Handler h in handlers)
            {
                h.TimeSinceKeepAlive++;
                if ((h.ActiveConnection) && (h.TimeSinceKeepAlive > KEEPALIVE_CAP))
                {
                    Packet p = new Packet(PacketData.PacketType.Disconnect.ToString(),new string[]{"disconnect"});
                    h.Write(p.ToString());
                    Console.WriteLine("Handler " + h.ID.ToString() + ": has timed out.");
                }
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

                    listenThread = null;
                    listenThread = new Thread(handlers[connectionIndex].ListenOnPort);
                    listenThread.Start();
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
            Console.WriteLine("Read thread with id: " + (ConnectionIndex-1));
            readThreads.Add(new Thread(handlers[ConnectionIndex-1].Read));
            readThreads[ConnectionIndex-1].Start();
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
