using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PokerServer
{
    class Manager
    {
        private string ip = "127.0.0.1";
        private int port = 9010;

        private Server server;

        public Manager()
        {
            server = new Server(ip, port);

        }

        public void Run()
        {
            server.Run();
        }
    }
    
}
