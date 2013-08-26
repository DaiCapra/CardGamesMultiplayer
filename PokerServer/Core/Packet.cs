using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using System.Web.Script.Serialization;
using System.Net.Json;

namespace Core
{
    public class Packet
    {
        private string packetType;
        private string message;
        private List<Object> packetInfo;

        public Packet(string packetType, string message = "")
        {
            this.packetType = packetType;
            this.message = message;

            PacketInfoToList();
        }

        private void PacketInfoToList()
        {
            packetInfo = new List<object>();
            packetInfo.Add(packetType);
            packetInfo.Add(message);
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

        public List<object> JSONToList(string json)
        {
            JsonTextParser parser = new JsonTextParser();
            JsonObject obj = parser.Parse(json);
            List<object> objects = new List<object>();
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

                objects.Add(value);
            }

            return objects;
        }
    
        public string PacketType
        {
            set { packetType = value; }
            get { return packetType; }
        }
        public List<object> PacketInfo
        {
            set { packetInfo = value; }
            get { return packetInfo; }
        }
        public string Message
        {
            get{return message;}
        }
    }
}
