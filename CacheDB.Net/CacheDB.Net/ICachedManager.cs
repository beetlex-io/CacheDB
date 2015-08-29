using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net
{
    public interface ICachedManager:ICacheItemUpgrade
    {

        IList<ILevelCached> LevelCacheds { get; }

        object Get(string key,Type type);

        T Get<T>(string key);

        ICacheItem GetCacheItem(string key);

        void Set(string key, object data);

        IDBManager DBManager { get;  }

        void AddLevel(string name, int maximum, int upgradeValue);

        void AddPolicy(Type type,IPolicy policy);


        IPolicy GetPolicy(Type type);
    }
}
