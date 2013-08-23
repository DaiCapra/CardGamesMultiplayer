using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PokerClient
{
    public class Listener
    {
        private string ip;
        private int port;
        private Socket socket;

        private TcpClient tcpClient;
        private Stream stream;

        private bool connected;

        public Listener(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            tcpClient = new TcpClient();
        }
        public void BeginConnect()
        {
            
            try
            {
                tcpClient.Connect(ip, port);
                connected = true;
            }
            catch
            {
                connected = false;
                Thread.Sleep(500);
            }
        }

        public bool Connected
        {
            get { return connected; }
            set { connected = value; }
        }
    }
}
