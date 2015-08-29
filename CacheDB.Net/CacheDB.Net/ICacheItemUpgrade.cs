using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net
{
    public interface ICacheItemUpgrade
    {
        void Upgrade(ICacheItem item, ILevelCached levelcached);
    }
}
