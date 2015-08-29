using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net.implement
{
    class CacheItem:ICacheItem
    {

        public CacheItem()
        {
            CreateTime = DateTime.Now;
        }

        private int mHit = 0;

        public string Key
        {
            get;
            set;
        }

        public object Data
        {
            get;
            set;
        }

        public DateTime CreateTime
        {
            get;
            set;
        }

        public int UpgradeValue
        {
            get;
            set;
        }

        public void Increment()
        {
            System.Threading.Interlocked.Increment(ref mHit);
        }

        public void Reset()
        {
            System.Threading.Interlocked.Exchange(ref mHit, 0);
        }

        public bool Upgrade()
        {
            return mHit > UpgradeValue;
                
        }


        public string LevelCached { get; set; }
    }
}
