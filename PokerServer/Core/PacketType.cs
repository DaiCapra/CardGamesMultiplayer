using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class PacketData
    {

        public enum PacketType
        {
            Disconnect,
            KeepAlive,
            ServerMessage,
            GameMessage,
            PlayerMessage
        }

        public enum ServerType
        {
            Login,
            
        }

        public PacketData()
        {
            
        }
    }
}
