using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDB.Net
{
    public class FormaterBuffer
    {
        private byte[] mBuffer;

        private System.IO.MemoryStream mStream;

        private System.IO.BinaryReader mReader;

        private System.IO.BinaryWriter mWriter;

        public FormaterBuffer(int size)
        {
            mBuffer = new byte[size];
            mStream = new System.IO.MemoryStream(mBuffer);
            mReader = new System.IO.BinaryReader(mStream);
            mWriter = new System.IO.BinaryWriter(mStream);
        }

        public System.IO.Stream Stream
        {
            get
            {
                return mStream;
            }
        }

        public System.IO.BinaryWriter Writer
        {
            get
            {
                return mWriter;
            }
        }

        public System.IO.BinaryReader Reader
        {
            get
            {
                return mReader;
            }
        }

        public void Seek(int postion=0)
        {
            mStream.Position = postion;
        }

        public byte[] Array { get { return mBuffer; } }



    }
}
