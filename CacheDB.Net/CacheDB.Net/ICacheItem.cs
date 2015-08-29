using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net
{
    public interface ICacheItem
    {
        int UpgradeValue { get; set; }

        void Increment();

        void Reset();

        bool Upgrade();

        string Key { get; set; }

        object Data { get; set; }

        DateTime CreateTime { get; set; }

        string LevelCached { get; set; }
    }
}
