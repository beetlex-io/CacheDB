using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net
{
    public class CacheDBException:Exception
    {
        public CacheDBException()
        {
        }

        public CacheDBException(string error) : base(error) { }

        public CacheDBException(string error, Exception interError) : base(error, interError) { }

        public CacheDBException(string format, params string[] data) : base(string.Format(format, data)) { }
    }
}
