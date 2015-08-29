using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LevelDB
{
    /// <summary>
    /// An iterator yields a sequence of key/value pairs from a database.
    /// </summary>
    public class Iterator : LevelDBHandle
    {
        internal Iterator(IntPtr handle)
        {
            Handle = handle;
        }

        /// <summary>
        /// An iterator is either positioned at a key/value pair, or
        /// not valid.  
        /// </summary>
        /// <returns>This method returns true iff the iterator is valid.</returns>
        public bool IsValid()
        {
            var result = LevelDBInterop.leveldb_iter_valid(Handle) != 0;
            GC.KeepAlive(this);
            return result;
        }

        /// <summary>
        /// Position at the first key in the source.  
        /// The iterator is IsValid() after this call iff the source is not empty.
        /// </summary>
        public void SeekToFirst()
        {
            LevelDBInterop.leveldb_iter_seek_to_first(Handle);
            Throw();
        }

        /// <summary>
        /// Position at the last key in the source.  
        /// The iterator is IsValid() after this call iff the source is not empty.
        /// </summary>
        public void SeekToLast()
        {
            LevelDBInterop.leveldb_iter_seek_to_last(Handle);
            Throw();
        }

        /// <summary>
        /// Position at the first key in the source that at or past target
        /// The iterator is IsValid() after this call iff the source contains
        /// an entry that comes at or past target.
        /// </summary>
        public void Seek(byte[] key)
        {
            LevelDBInterop.leveldb_iter_seek(Handle, key, (IntPtr)key.Length);
            Throw();
        }

        /// <summary>
        /// Position at the first key in the source that at or past target
        /// The iterator is IsValid() after this call iff the source contains
        /// an entry that comes at or past target.
        /// </summary>
        public void Seek(string key)
        {
            Seek(Encoding.UTF8.GetBytes(key));
        }

        /// <summary>
        /// Moves to the next entry in the source.  
        /// After this call, IsValid() is true iff the iterator was not positioned at the last entry in the source.
        /// REQUIRES: IsValid()
        /// </summary>
        public void Next()
        {
            LevelDBInterop.leveldb_iter_next(Handle);
            Throw();
        }

        /// <summary>
        /// Moves to the previous entry in the source.  
        /// After this call, IsValid() is true iff the iterator was not positioned at the first entry in source.
        /// REQUIRES: IsValid()
        /// </summary>
        public void Prev()
        {
            LevelDBInterop.leveldb_iter_prev(Handle);
            Throw();
        }

        /// <summary>
        /// Return the key for the current entry.  
        /// REQUIRES: IsValid()
        /// </summary>
        public string GetStringKey()
        {
            return Encoding.UTF8.GetString(GetKey());
        }

        /// <summary>
        /// Return the key for the current entry.  
        /// REQUIRES: IsValid()
        /// </summary>
        public byte[] GetKey()
        {
            IntPtr length;
            var key = LevelDBInterop.leveldb_iter_key(Handle, out length);
            Throw();

            var bytes = new byte[(int)length];
            Marshal.Copy(key, bytes, 0, (int)length);
            GC.KeepAlive(this);
            return bytes;
        }

        /// <summary>
        /// Return the value for the current entry.  
        /// REQUIRES: IsValid()
        /// </summary>
        public string GetStringValue()
        {
            return Encoding.UTF8.GetString(GetValue());
        }

        /// <summary>
        /// Return the value for the current entry.  
        /// REQUIRES: IsValid()
        /// </summary>
        public unsafe byte[] GetValue()
        {
            IntPtr length;
            var value = LevelDBInterop.leveldb_iter_value(Handle, out length);
            Throw();

            var bytes = new byte[(long)length];
            var valueNative = (byte*)value.ToPointer();
            for (long i = 0; i < (long)length; ++i)
                bytes[i] = valueNative[i];

            GC.KeepAlive(this);
            return bytes;
        }

        /// <summary>
        /// If an error has occurred, throw it.  
        /// </summary>
        void Throw()
        {
            IntPtr error;
            LevelDBInterop.leveldb_iter_get_error(Handle, out error);
            LevelDBException.Check(error);
            GC.KeepAlive(this);
        }

        protected override void FreeUnManagedObjects()
        {
            LevelDBInterop.leveldb_iter_destroy(Handle);
        }
    }
}
