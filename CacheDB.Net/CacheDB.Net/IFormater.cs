using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net
{
    public interface IFormater
    {
        FormaterBuffer Pop();

        void Push(FormaterBuffer data);

        int Serialize(object data, FormaterBuffer buffer, int offset);

        object Deserialize(Type type, FormaterBuffer buffer, int offset, int count);
    }
}
