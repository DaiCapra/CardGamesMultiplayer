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
        private int messageIndex;

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
                stream = tcpClient.GetStream();
            }
            catch
            {
                connected = false;
                Thread.Sleep(500);
            }
        }
        public void Write(string s)
        {
            if (messageIndex == 10)
                messageIndex = 0;
            string s2 = messageIndex.ToString() + s;
            messageIndex++;

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(s2);

            stream.Write(ba, 0, ba.Length);
        }
        public void Read()
        {
            while (true)
            {
                try
                {
                    byte[] bb = new byte[100];
                    int k = stream.Read(bb, 0, 100);

                    char[] c = new char[k];

                    for (int i = 0; i < k; i++)
                        c[i] = Convert.ToChar(bb[i]);

                    string s = new string(c);
                    Thread.Sleep(10);
                }
                catch (Exception e)
                {
                }
            }
        }

        public bool Connected
        {
            get { return connected; }
            set { connected = value; }
        }
    }
}
