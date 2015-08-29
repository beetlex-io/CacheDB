using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net.implement
{
    class CachedManager : ICachedManager
    {

        private List<ILevelCached> mLevelCacheds = new List<ILevelCached>();

        private Dictionary<Type, IPolicy> mPolicys = new Dictionary<Type, IPolicy>();

        public IList<ILevelCached> LevelCacheds
        {
            get { return mLevelCacheds; }
        }

        public object Get(string key, Type type)
        {
            ICacheItem item = GetCacheItem(key);
            if (item != null)
            {
                IPolicy policy = GetPolicy(type);
                if (policy != null && !policy.IsAvailable(item))
                {
                    item = null;
                }
            }
            if (item == null)
            {
                object data = DBManager.Get(key, type);
                if (data != null)
                {
                    item = new implement.CacheItem { CreateTime=DateTime.Now, Data=data,Key = key };
                }
               
                if (item != null)
                {
                    mLevelCacheds[mLevelCacheds.Count - 1].Set(item);
                    return item.Data;
                }
                
            }
            return null;
        }

        public T Get<T>(string key)
        {
            return (T)Get(key, typeof(T));
        }

        public void Set(string key, object data)
        {
            ICacheItem item = GetCacheItem(key);
            if (item != null)
            {
                item.Data = data;
            }
            DBManager.Set(key, data);
        }

        public ICacheItem GetCacheItem(string key)
        {
            ICacheItem item = null;
            for (int i = 0; i < LevelCacheds.Count; i++)
            {
                item = LevelCacheds[i].Get(key);
                if (item != null)
                    break;
            }
            return item;
        }

        public IDBManager DBManager
        {
            get;
            internal set;

        }

        public void AddLevel(string name, int maximum, int upgradeValue)
        {
            mLevelCacheds.Add(new implement.LevelCached { UpgradeHandler = this, Name = name, Maximum = maximum, LevelValue = mLevelCacheds.Count, UpgradeValue = upgradeValue });

        }

        public void Upgrade(ICacheItem item, ILevelCached levelcached)
        {
            if (levelcached.LevelValue > 0)
            {
                mLevelCacheds[levelcached.LevelValue - 1].Set(item);
            }
        }

        public void AddPolicy(Type type,IPolicy policy)
        {
            mPolicys[type] = policy;
        }

        public IPolicy GetPolicy(Type type)
        {
            IPolicy result = null;
            mPolicys.TryGetValue(type, out result);
            return result;
        }


        
    }
}
