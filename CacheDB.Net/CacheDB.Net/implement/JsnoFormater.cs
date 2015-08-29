using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net.implement
{
    public class JsnoFormater:FormaterBase
    {
       
        public int Serialize(object data, byte[] buffer, int offset)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(json, 0, json.Length, buffer, offset);
        }

        public override int Serialize(object data, FormaterBuffer buffer, int offset)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(json, 0, json.Length, buffer.Array, offset);
        }

        public override object Deserialize(Type type, FormaterBuffer buffer, int offset, int count)
        {
            string value = Encoding.UTF8.GetString(buffer.Array, offset, count);
            return Newtonsoft.Json.JsonConvert.DeserializeObject(value, type);
        }
    }
}
