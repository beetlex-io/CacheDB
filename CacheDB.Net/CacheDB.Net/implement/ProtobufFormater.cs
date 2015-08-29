using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net.implement
{
    public class ProtobufFormater:FormaterBase
    {

        public override int Serialize(object data, FormaterBuffer buffer, int offset)
        {
            buffer.Seek(offset);
            ProtoBuf.Meta.RuntimeTypeModel.Default.Serialize(buffer.Stream, data);
            return (int)buffer.Stream.Position;
        }

        public override object Deserialize(Type type, FormaterBuffer buffer, int offset, int count)
        {
            buffer.Stream.SetLength(count + offset);
            buffer.Seek(offset);
            return ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(buffer.Stream, null, type);
        }
    }
}
