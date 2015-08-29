using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net
{
    public interface ILevelCached
    {
        int UpgradeValue
        {
            get;
            set;
        }      
        int LevelValue { get; set; }

        ICacheItem Get(string key);

        void Set(ICacheItem data);

        int Maximum { get; set; }

        ICacheItemUpgrade UpgradeHandler { get; set; }

        string Name { get; set; }
    }
}
