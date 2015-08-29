using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace LevelDB
{
    /// <summary>
    /// A DB is a persistent ordered map from keys to values.
    /// A DB is safe for concurrent access from multiple threads without any external synchronization.
    /// </summary>
    public class DB : LevelDBHandle, IEnumerable<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<byte[], byte[]>>
    {
        Options Options;

        /// <summary>
        /// Open the database with the specified "name".
        /// </summary>
        public DB(string name)
            : this(name, new Options())
        {
        }

        /// <summary>
        /// Open the database with the specified "name".
        /// Options should not be modified after calling this method.
        /// </summary>
        public DB(string name, Options options)
        {
            Options = options ?? new Options();
            IntPtr error;
            Handle = LevelDBInterop.leveldb_open(Options.Handle, name, out error);
            LevelDBException.Check(error);
            GC.KeepAlive(Options);
        }

        /// <summary>
        /// If a DB cannot be opened, you may attempt to call this method to
        /// resurrect as much of the contents of the database as possible.
        /// Some data may be lost, so be careful when calling this function
        /// on a database that contains important information.
        /// </summary>
        public static void Repair(string name)
        {
            Repair(name, new Options());
        }

        /// <summary>
        /// If a DB cannot be opened, you may attempt to call this method to
        /// resurrect as much of the contents of the database as possible.
        /// Some data may be lost, so be careful when calling this function
        /// on a database that contains important information.
        /// Options should not be modified after calling this method.
        /// </summary>
        public static void Repair(string name, Options options)
        {
            IntPtr error;
            LevelDBInterop.leveldb_repair_db(options.Handle, name, out error);
            LevelDBException.Check(error);
            GC.KeepAlive(options);
        }

        /// <summary>
        /// Destroy the contents of the specified database.
        /// Be very careful using this method.
        /// </summary>
        public static void Destroy(string name)
        {
            Destroy(name, new Options());
        }

        /// <summary>
        /// Destroy the contents of the specified database.
        /// Be very careful using this method.
        /// Options should not be modified after calling this method.
        /// </summary>
        public static void Destroy(string name, Options options)
        {
            IntPtr error;
            LevelDBInterop.leveldb_destroy_db(options.Handle, name, out error);
            LevelDBException.Check(error);
            GC.KeepAlive(options);
        }

        /// <summary>
        /// Set the database entry for "key" to "value".  
        /// </summary>
        public void Put(string key, string value)
        {
            Put(key, value, new WriteOptions());
        }

        /// <summary>
        /// Set the database entry for "key" to "value".  
        /// </summary>
        public void Put(string key, string value, WriteOptions options)
        {
            Put(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(value), options);
        }

        /// <summary>
        /// Set the database entry for "key" to "value".  
        /// </summary>
        public void Put(byte[] key, byte[] value)
        {
            Put(key, value, new WriteOptions());
        }
        public void Put(byte[] key, byte[] value, int offset, int length)
        {
            Put(key, value,offset,length,new WriteOptions());
        }
        public void Put(byte[] key, byte[] value, int offset, int length, WriteOptions options)
        {
            IntPtr error;
            LevelDBInterop.leveldb_put(this.Handle, options.Handle, key, (IntPtr)key.Length, value, (IntPtr)length, out error);
            LevelDBException.Check(error);
            GC.KeepAlive(options);
            GC.KeepAlive(this);
        }
        /// <summary>
        /// Set the database entry for "key" to "value".  
        /// </summary>
        public void Put(byte[] key, byte[] value, WriteOptions options)
        {
            IntPtr error;
            LevelDBInterop.leveldb_put(this.Handle, options.Handle, key, (IntPtr)key.Length, value, (IntPtr)value.LongLength, out error);
            LevelDBException.Check(error);
            GC.KeepAlive(options);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Remove the database entry (if any) for "key".  
        /// It is not an error if "key" did not exist in the database.
        /// </summary>
        public void Delete(string key)
        {
            Delete(key, new WriteOptions());
        }

        /// <summary>
        /// Remove the database entry (if any) for "key".  
        /// It is not an error if "key" did not exist in the database.
        /// </summary>
        public void Delete(string key, WriteOptions options)
        {
            Delete(Encoding.UTF8.GetBytes(key), options);
        }

        /// <summary>
        /// Remove the database entry (if any) for "key".  
        /// It is not an error if "key" did not exist in the database.
        /// </summary>
        public void Delete(byte[] key)
        {
            Delete(key, new WriteOptions());
        }

        /// <summary>
        /// Remove the database entry (if any) for "key".  
        /// It is not an error if "key" did not exist in the database.
        /// </summary>
        public void Delete(byte[] key, WriteOptions options)
        {
            IntPtr error;
            LevelDBInterop.leveldb_delete(this.Handle, options.Handle, key, (IntPtr)key.Length, out error);
            LevelDBException.Check(error);
            GC.KeepAlive(options);
            GC.KeepAlive(this);
        }

        public void Write(WriteBatch batch)
        {
            Write(batch, new WriteOptions());
        }

        public void Write(WriteBatch batch, WriteOptions options)
        {
            IntPtr error;
            LevelDBInterop.leveldb_write(this.Handle, options.Handle, batch.Handle, out error);
            LevelDBException.Check(error);
            GC.KeepAlive(batch);
            GC.KeepAlive(options);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// If the database contains an entry for "key" return the value,
        /// otherwise return null.
        /// </summary>
        public string Get(string key)
        {
            return Get(key, new ReadOptions());
        }

        /// <summary>
        /// If the database contains an entry for "key" return the value,
        /// otherwise return null.
        /// </summary>
        public string Get(string key, ReadOptions options)
        {
            var value = Get(Encoding.UTF8.GetBytes(key), options);
            return value != null ? Encoding.UTF8.GetString(value) : null;
        }

        /// <summary>
        /// If the database contains an entry for "key" return the value,
        /// otherwise return null.
        /// </summary>
        public byte[] Get(byte[] key)
        {
            return Get(key, new ReadOptions());
        }
        public long Get(byte[] key, byte[] buffer)
        {
            return Get(key, buffer, new ReadOptions());
        }
        public unsafe long Get(byte[] key, byte[] buffer, ReadOptions options)
        {
            IntPtr error;
            IntPtr lengthPtr;
            var valuePtr = LevelDBInterop.leveldb_get(this.Handle, options.Handle, key, (IntPtr)key.Length, out lengthPtr, out error);
            LevelDBException.Check(error);
            if (valuePtr == IntPtr.Zero)
                return 0;
            try
            {
                var length = (long)lengthPtr;
                var valueNative = (byte*)valuePtr.ToPointer();
                Marshal.Copy((IntPtr)valuePtr, buffer, 0, (int)length);
                return length;
            }
            finally
            {
                LevelDBInterop.leveldb_free(valuePtr);
              
            }
        }
        /// <summary>
        /// If the database contains an entry for "key" return the value,
        /// otherwise return null.
        /// </summary>
        public unsafe byte[] Get(byte[] key, ReadOptions options)
        {
            IntPtr error;
            IntPtr lengthPtr;
            var valuePtr = LevelDBInterop.leveldb_get(this.Handle, options.Handle, key, (IntPtr)key.Length, out lengthPtr, out error);
            LevelDBException.Check(error);
            if (valuePtr == IntPtr.Zero)
                return null;
            try
            {
                var length = (long)lengthPtr;
                var value = new byte[length];
                var valueNative = (byte*)valuePtr.ToPointer();
                for (long i = 0; i < length; ++i)
                    value[i] = valueNative[i];
                return value;
            }
            finally
            {
                LevelDBInterop.leveldb_free(valuePtr);
                GC.KeepAlive(options);
                GC.KeepAlive(this);
            }
        }

        /// <summary>
        /// Return an iterator over the contents of the database.
        /// The result of CreateIterator is initially invalid (caller must
        /// call one of the Seek methods on the iterator before using it).
        /// </summary>
        public Iterator CreateIterator()
        {
            return this.CreateIterator(new ReadOptions());
        }

        /// <summary>
        /// Return an iterator over the contents of the database.
        /// The result of CreateIterator is initially invalid (caller must
        /// call one of the Seek methods on the iterator before using it).
        /// </summary>
        public Iterator CreateIterator(ReadOptions options)
        {
            var result = new Iterator(LevelDBInterop.leveldb_create_iterator(this.Handle, options.Handle));
            GC.KeepAlive(options);
            GC.KeepAlive(this);
            return result;
        }

        /// <summary>
        /// Return a handle to the current DB state.  
        /// Iterators and Gets created with this handle will all observe a stable snapshot of the current DB state.  
        /// </summary>
        public SnapShot CreateSnapshot()
        {
            var result = new SnapShot(LevelDBInterop.leveldb_create_snapshot(this.Handle), this);
            GC.KeepAlive(this);
            return result;
        }

        /// <summary>
        /// DB implementations can export properties about their state
        /// via this method.  If "property" is a valid property understood by this
        /// DB implementation, fills "*value" with its current value and returns
        /// true.  Otherwise returns false.
        ///
        /// Valid property names include:
        ///
        ///  "leveldb.num-files-at-level<N>" - return the number of files at level <N>,
        ///     where <N> is an ASCII representation of a level number (e.g. "0").
        ///  "leveldb.stats" - returns a multi-line string that describes statistics
        ///     about the internal operation of the DB.
        /// </summary>
        public string PropertyValue(string name)
        {
            var ptr = LevelDBInterop.leveldb_property_value(this.Handle, name);
            if (ptr == IntPtr.Zero)
                return null;
            try
            {
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                LevelDBInterop.leveldb_free(ptr);
                GC.KeepAlive(this);
            }
        }

        protected override void FreeUnManagedObjects()
        {
            if (this.Handle != default(IntPtr))
                LevelDBInterop.leveldb_close(this.Handle);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
        {
            using (var sn = this.CreateSnapshot())
            using (var iterator = this.CreateIterator(new ReadOptions { Snapshot = sn }))
            {
                iterator.SeekToFirst();
                while (iterator.IsValid())
                {
                    yield return new KeyValuePair<string, string>(iterator.GetStringKey(), iterator.GetStringValue());
                    iterator.Next();
                }
            }
        }

        public IEnumerator<KeyValuePair<byte[], byte[]>> GetEnumerator()
        {
            using (var sn = this.CreateSnapshot())
            using (var iterator = this.CreateIterator(new ReadOptions { Snapshot = sn }))
            {
                iterator.SeekToFirst();
                while (iterator.IsValid())
                {
                    yield return new KeyValuePair<byte[], byte[]>(iterator.GetKey(), iterator.GetValue());
                    iterator.Next();
                }
            }
        }
    }
}
