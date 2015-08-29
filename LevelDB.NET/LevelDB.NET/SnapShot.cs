using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelDB
{
    /// <summary>
    /// A Snapshot is an immutable object and can therefore be safely
    /// accessed from multiple threads without any external synchronization.
    /// </summary>
    public class SnapShot : LevelDBHandle
    {
        // pointer to parent so that we can call ReleaseSnapshot(this) when disposed
        public WeakReference Parent;  // as DB

        internal SnapShot(IntPtr handle, DB parent)
        {
            Handle = handle;
            Parent = new WeakReference(parent);
        }

        internal SnapShot(IntPtr handle)
        {
            Handle = handle;
            Parent = new WeakReference(null);
        }

        protected override void FreeUnManagedObjects()
        {
            var parent = Parent.Target as DB;
            if (parent != null)
                LevelDBInterop.leveldb_release_snapshot(parent.Handle, Handle);
        }
    }
}
