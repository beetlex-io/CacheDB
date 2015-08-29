using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MsgPack.Serialization;

namespace CacheDB.Net.MsgpacketFormater
{
    public class Formater : implement.FormaterBase
    {
        private Stack<byte[]> mBufferPool = new Stack<byte[]>();

        const int BUFFER_SIZE = 1024 * 1024 * 1;

        public Formater()
        {
            for (int i = 0; i < 20; i++)
            {
                mBufferPool.Push(new byte[BUFFER_SIZE]);
            }
        }

        public byte[] Pop()
        {
            lock (mBufferPool)
            {
                if (mBufferPool.Count > 0)
                    return mBufferPool.Pop();
                return new byte[BUFFER_SIZE];
            }
        }

        public void Push(byte[] data)
        {
            lock (mBufferPool)
            {
                mBufferPool.Push(data);
            }
        }

        public override int Serialize(object data, FormaterBuffer buffer, int offset)
        {
            var serializer = SerializationContext.Default.GetSerializer(data.GetType());
            buffer.Seek();
            serializer.Pack(buffer.Stream, data);
            return (int)buffer.Stream.Position;

        }

        public override object Deserialize(Type type, FormaterBuffer buffer, int offset, int count)
        {
            var serializer = SerializationContext.Default.GetSerializer(type);
          
            buffer.Seek(offset);
            return serializer.Unpack(buffer.Stream);

        }
    }
}
