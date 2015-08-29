using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C = System.Console;
namespace LevelDB.Test.Console
{
    class Program
    {
        private static IList<byte[]> mKeys = new List<byte[]>();

        private static DB mDatabase = null;

        static int mTestCount = 10000;

        static void Main(string[] args)
        {
            for (int i = 1; i < 10000; i++)
            {
                mKeys.Add(BitConverter.GetBytes(i));
            }
           
            mDatabase = new DB("mytestdb", new Options() { CreateIfMissing = true });
            IninDB();
            using (mDatabase)
            {
                //byte[] data = mDatabase.Get(mKeys[0]);
               // C.WriteLine(Encoding.UTF8.GetString(data));
                GetTestWithBuffer();
                GetTest();
              
            }
            C.WriteLine("test completed");
            C.Read();
        }

        static void GetTest()
        {
            byte[] data = mDatabase.Get(mKeys[0]);
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            for (int i = 0; i < mTestCount; i++)
            {
                data = mDatabase.Get(mKeys[Environment.TickCount%mKeys.Count]);
            }
            watch.Stop();
            C.WriteLine("get test use time:{0}",watch.Elapsed.TotalMilliseconds);
        }

        static void GetTestWithBuffer()
        {
            byte[] data = new Byte[1024*4];
            long count = mDatabase.Get(mKeys[Environment.TickCount % mKeys.Count], data);
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            for (int i = 0; i < mTestCount; i++)
            {
                  count = mDatabase.Get(mKeys[Environment.TickCount%mKeys.Count],data);
            }
            watch.Stop();
            C.WriteLine("get test with buffer use time:{0}", watch.Elapsed.TotalMilliseconds);
        }

        static void IninDB()
        {
            byte[] data = Encoding.UTF8.GetBytes(@"sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfdsklfjkljjllljjjjjjjlllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll
sdfklsdjfklsdjfklsffsdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl
sdfffffffffffsdfsdfsfskdfjskldjflksdjflksjdflsjljfljsldfjsldfjsdfsdfkjsdl" + Guid.NewGuid().ToString("N"));


            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            foreach (byte[] key in mKeys)
            {
                mDatabase.Put(key, data);
            }

            watch.Stop();
            C.WriteLine("set data use time:{0}", watch.Elapsed.TotalMilliseconds);

        }
        
    }

   
}
