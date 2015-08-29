using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net.TestConsole
{
    class Program
    {
        private static IDBManager db;

        private static int mCount;

        private static int mLastCount;

        private static IList<string> keys = new List<string>(1024 * 1024 * 5);

        private static int mIndex = 0;

        static void Main(string[] args)
        {
            int threads = 0;
            DbManagerConfig config = new DbManagerConfig();
            config.Type = "CacheDB.Net.implement.LevelDBManager,CacheDB.Net";
            // config.Formater = "CacheDB.Net.implement.JsnoFormater, CacheDB.Net";
            //config.Formater = "CacheDB.Net.implement.ProtobufFormater, CacheDB.Net";
            //config.Formater = "CacheDB.Net.MsgpacketFormater.Formater,CacheDB.Net.MsgpacketFormater";
            config.Formater = "CacheDB.Net.BinaryFormater.Formater,CacheDB.Net.BinaryFormater";
            config.Properties.Add(new PropertyConfig { Name = "Path", Value = "test" });
            db = Factory.CreateDBManager(config);
            db.Open();
        START:
            Console.Clear();
            Console.WriteLine("1.set test");
            Console.WriteLine("2.get test");
            string value = Console.ReadLine();
            int menuid;
            int.TryParse(value, out menuid);
            if (menuid != 1 && menuid != 2)
            {
                goto START;
            }
        INPUT_THREADS:
            Console.Write("test thread(1-50):");
            value = Console.ReadLine();
            int.TryParse(value, out threads);
            if (threads < 1 || threads > 50)
                goto INPUT_THREADS;
            if (menuid == 1)
            {

                for (int i = 0; i < threads; i++)
                {
                    System.Threading.ThreadPool.QueueUserWorkItem(WriteTest);
                }
            }
            else
            {
                Console.WriteLine("get key loading ...");
                IEnumerator<KeyValuePair<byte[], byte[]>> items = db.DataBase.GetEnumerator();
                while (items.MoveNext())
                {
                    keys.Add(Encoding.UTF8.GetString(items.Current.Key));
                }
                Console.WriteLine("get testing ...");
                for (int i = 0; i < threads; i++)
                {
                    System.Threading.ThreadPool.QueueUserWorkItem(ReadTest);
                }
            }

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (menuid == 1)
                {
                    Console.Write("set testing ...");
                }
                else
                {
                    Console.Write("get testing ...");
                }
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine(" [{0}/{1}]", mCount - mLastCount, mCount);
                mLastCount = mCount;
            }
        }

        public static void ReadTest(object state)
        {
            while (true)
            {
                User user = db.Get<User>(keys[mIndex % keys.Count]);
                System.Threading.Interlocked.Increment(ref mIndex);
                System.Threading.Interlocked.Increment(ref mCount);
                //System.Threading.Thread.Sleep(1);
            }
        }

        public static void WriteTest(object state)
        {
            while (true)
            {

                User user = new User();
                string id = Guid.NewGuid().ToString("N");
                user.Name = id;
                user.EMail = id + "@msn.com";
                user.CrateTime = DateTime.Now;
                user.City = "GZ" + id;
                user.Country = "CN" + id;
                db.Set(user.Name, user);
                System.Threading.Interlocked.Increment(ref mCount);
                //System.Threading.Thread.Sleep(1);
            }
        }


    }
    [ProtoBuf.ProtoContract]
    public class User 
    {
        public User()
        {

        }
        [ProtoBuf.ProtoMember(1)]
        public string Name { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public string EMail { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public DateTime CrateTime { get; set; }
        [ProtoBuf.ProtoMember(4)]
        public string City { get; set; }
        [ProtoBuf.ProtoMember(5)]
        public string Country { get; set; }


        public void Read(System.IO.BinaryReader reader)
        {
            Name = reader.ReadString();
            EMail = reader.ReadString();
            CrateTime = new DateTime(reader.ReadInt64());
            City = reader.ReadString();
            Country = reader.ReadString();
        }

        public void Write(System.IO.BinaryWriter writer)
        {

            writer.Write(Name);
            writer.Write(EMail);
            writer.Write(CrateTime.Ticks);
            writer.Write(City);
            writer.Write(Country);
        }
    }
}
