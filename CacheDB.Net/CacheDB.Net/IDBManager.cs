using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net
{
    public interface IDBManager
    {
        IFormater Formater { get; set; }

        void Set(string key, object data);

        object Get(string key, Type type);

        T Get<T>(string key);

        void Open();

        LevelDB.DB DataBase
        {
            get;
        }
       
    }
}
