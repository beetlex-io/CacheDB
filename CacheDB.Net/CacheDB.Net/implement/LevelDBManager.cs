using LevelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
namespace CacheDB.Net.implement
{
    public class LevelDBManager : IDBManager
    {

        public LevelDBManager()
        {
            
        }

        private LevelDB.DB mDataBase;

        public string Path { get; set; }

        public IFormater Formater
        {
            get;
            set;
        }

        public void Open()
        {
            mDataBase = new LevelDB.DB(Path, new Options() { CreateIfMissing = true });
          
        }

        public void Set(string key, object data)
        {

            FormaterBuffer buffer = Formater.Pop();
            try
            {

                int count = Formater.Serialize(data, buffer, 0);
                mDataBase.Put(Encoding.UTF8.GetBytes(key), buffer.Array, 0, count);
            }
            finally
            {
                Formater.Push(buffer);
            }
        }

        public object Get(string key, Type type)
        {
            FormaterBuffer buffer = Formater.Pop();
            long count;
            object result = null;
           
            try
            {
                count = mDataBase.Get(Encoding.UTF8.GetBytes(key), buffer.Array);
                if (count > 0)
                {
                    result = Formater.Deserialize(type, buffer, 0, (int)count);

                }
                return result;
            }
            finally
            {
                Formater.Push(buffer);
            }
          
        }

        public T Get<T>(string key)
        {
            return (T)Get(key, typeof(T));
        }


        public DB DataBase
        {
            get { return mDataBase; }
        }
    }
}
