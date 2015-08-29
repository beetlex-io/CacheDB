using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net.UnitTest.Model
{
    public class User:BinaryFormater.IBinaryMessage
    {
        public string Name { get; set; }
        public string EMail { get; set; }

        public void Read(System.IO.BinaryReader reader)
        {
            Name = reader.ReadString();
            EMail = reader.ReadString();
        }

        public void Write(System.IO.BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(EMail);
        }
    }
}
