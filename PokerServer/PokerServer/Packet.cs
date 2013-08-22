using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using System.Web.Script.Serialization;
using System.Net.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PokerServer
{
    
    class Packet
    {
        private PacketData.PacketType packetType;
        private string message;

        public Packet(PacketData.PacketType packetType, string message = "")
        {
            this.packetType = packetType;
            this.message = message;
        }

        public string ListToJson(List<Object> list)
        {
            int index = 1;
            Dictionary<string, object> keyValues = new Dictionary<string, object>();

            //keyValues.Add("field0", packetType.ToString());
            foreach (object obj in list)
            {
                string s = "field"+index.ToString();
                keyValues.Add(s, obj);
                index++;
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            
            string json = js.Serialize(keyValues);

            return json;
        }

        public void JSONTest(string json)
        {
            JsonTextParser parser = new JsonTextParser();
            JsonObject obj = parser.Parse(json);
            Console.WriteLine(obj);
            
            foreach (JsonObject field in obj as JsonObjectCollection)
            {
                string name = field.Name;
                string value = string.Empty;
                string type = field.GetValue().GetType().Name;

                switch (type)
                {
                    case "String":
                        value = (string)field.GetValue();
                        break;

                    case "Double":
                        value = field.GetValue().ToString();
                        break;

                    case "Boolean":
                        value = field.GetValue().ToString();
                        break;

                    default:
                        throw new NotSupportedException();
                }

                Console.WriteLine("{0} {1} {2}",
                    name.PadLeft(15), type.PadLeft(10), value.PadLeft(15));
            }
            
        }
    
        public PacketData.PacketType PacketType
        {
            set { packetType = value; }
            get { return packetType; }
        }
        public string Message
        {
            get{return message;}
        }
    }
}
