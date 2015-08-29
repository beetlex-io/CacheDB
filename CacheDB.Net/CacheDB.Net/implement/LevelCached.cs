using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net.implement
{
    class LevelCached : ILevelCached
    {
        private LinkedList<ICacheItem> mItemLink = new LinkedList<ICacheItem>();

        private Dictionary<string, LinkedListNode<ICacheItem>> mItemTable = new Dictionary<string, LinkedListNode<ICacheItem>>();

        public int UpgradeValue
        {
            get;
            set;
        }

        public int LevelValue
        {
            get;
            set;
        }

        public ICacheItem Get(string key)
        {
            ICacheItem result = null;
            LinkedListNode<ICacheItem> item = null;
            mItemTable.TryGetValue(key, out item);
            if (item != null)
            {
                result = item.Value;
                result.Increment();
                if ( LevelValue > 0 && result.Upgrade() && UpgradeHandler != null  )
                {
                    OnUpgrade(item);
                }
                else
                {
                    lock (this)
                    {
                        if (item.List == mItemLink)
                        {
                            mItemLink.Remove(item);
                            mItemLink.AddFirst(item);
                        }
                    }
                }
            }
            return result;
        }

        public void Set(ICacheItem data)
        {
            lock (this)
            {
                data.UpgradeValue = UpgradeValue;
                data.Reset();
                data.LevelCached = Name;
                LinkedListNode<ICacheItem> item = null;
                mItemTable.TryGetValue(data.Key, out item);
                if (item != null)
                {
                    item.Value = data;
                }
                else
                {
                    if (mItemTable.Count >= Maximum)
                    {
                        LinkedListNode<ICacheItem> lastItem = mItemLink.Last;
                        if (lastItem != null)
                        {
                            mItemLink.Remove(lastItem);
                            mItemTable.Remove(lastItem.Value.Key);
                        }
                    }
                    item = mItemLink.AddFirst(data);
                    mItemTable[data.Key] = item;
                }
            }
        }

        protected virtual void OnUpgrade(LinkedListNode<ICacheItem> item)
        {
            lock (this)
            {

                if (item.List == mItemLink)
                {

                    ICacheItem data = item.Value;
                    mItemLink.Remove(item);
                    mItemTable.Remove(data.Key);
                    if (UpgradeHandler != null)
                        UpgradeHandler.Upgrade(data, this);
                }
            }
        }

        public ICacheItemUpgrade UpgradeHandler
        {
            get;
            set;
        }

        public int Maximum
        {
            get;
            set;
        }


        public string Name
        {
            get;
            set;
        }
    }
}
