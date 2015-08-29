using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net.implement
{
    public abstract class FormaterBase:IFormater
    {
         private Stack<FormaterBuffer> mBufferPool = new Stack<FormaterBuffer>();

        const int BUFFER_SIZE = 1024 * 1024 * 1;

        public FormaterBase()
        {
            for (int i = 0; i < 20; i++)
            {
                mBufferPool.Push(new FormaterBuffer(BUFFER_SIZE));
            }
        }
        public FormaterBuffer Pop()
        {
            lock (mBufferPool)
            {
                if(mBufferPool.Count>0)
                    return mBufferPool.Pop();
                return new FormaterBuffer(BUFFER_SIZE);
            }
        }
        public void Push(FormaterBuffer data)
        {
            lock (mBufferPool)
            {
                mBufferPool.Push(data);
            }
        }
       
        public abstract int Serialize(object data, FormaterBuffer buffer, int offset);
       
        public abstract object Deserialize(Type type, FormaterBuffer buffer, int offset, int count);
        
    }
}
