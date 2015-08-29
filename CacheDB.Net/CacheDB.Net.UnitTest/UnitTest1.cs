using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;

namespace CacheDB.Net.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private static ICachedManager cache;

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            cache = Factory.GetCachedManager();
        }

        [TestMethod]
        public void Get()
        {

            Model.User user = new Model.User { Name = "henry", EMail = "henryfan@msn.com" };
            cache.Set("henry", user);
            Model.User result = cache.Get<Model.User>("henry");
            Assert.AreEqual(user.EMail, result.EMail);

        }

        [TestMethod]
        public void ListData()
        {
            IEnumerator<KeyValuePair<byte[], byte[]>> items = cache.DBManager.DataBase.GetEnumerator();
            while (items.MoveNext())
            {
                Console.WriteLine(Encoding.UTF8.GetString(items.Current.Key));
            }

        }

        [TestMethod]
        public void WriteTest()
        {
            Model.User user = new Model.User { Name = "henry" + Guid.NewGuid().ToString("N"), EMail = "henryfan@msn.com" };
            cache.Set(user.Name, user);
        }

        [TestMethod]
        public void GetCacheItem()
        {
            Model.User user = new Model.User { Name = "henry" + Guid.NewGuid().ToString("N"), EMail = "henryfan@msn.com" };
            cache.Set(user.Name, user);
            cache.Get(user.Name, typeof(Model.User));
            ICacheItem item = cache.GetCacheItem(user.Name);
            Console.WriteLine(item.LevelCached);
        }

        [TestMethod]
        public void Upgrade()
        {

            Model.User user = new Model.User { Name = "henry", EMail = "henryfan@msn.com" };
            cache.Set("henry", user);
            Model.User result = cache.Get<Model.User>("henry");
            ICacheItem item = cache.GetCacheItem(user.Name);
            Assert.AreEqual(item.LevelCached, "l3");
            for (int i = 0; i < 11; i++)
            {
                item = cache.GetCacheItem(user.Name);
            }
            Assert.AreEqual(item.LevelCached, "l2");
            for (int i = 0; i < 11; i++)
            {
                item = cache.GetCacheItem(user.Name);
            }
            Assert.AreEqual(item.LevelCached, "l1");
           
        }
    }
}
