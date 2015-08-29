using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LevelDB
{
    public class LevelDBException : Exception
    {
        public LevelDBException(string message) : base(message) { }

        public static void Check(IntPtr error)
        {
            if (error != IntPtr.Zero)
            {
                try
                {
                    var message = Marshal.PtrToStringAnsi(error);
                    throw new LevelDBException(message);
                }
                finally
                {
                    LevelDBInterop.leveldb_free(error);
                }
            }
        }
    }
}
