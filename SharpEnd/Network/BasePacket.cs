using System;
using System.IO;

namespace SharpEnd.Network
{
    internal abstract class BasePacket : IDisposable
    {
        public EHeader Header { get; protected set; }

        public int Length
        {
            get
            {
                return (int)m_stream.Length;
            }
        }

        public int Position
        {
            get
            {
                return (int)m_stream.Position;
            }
            set
            {
                m_stream.Position = value;
            }
        }

        public int Available
        {
            get
            {
                return Length - Position;
            }
        }

        protected MemoryStream m_stream;

        public byte[] ToArray()
        {
            return m_stream.ToArray();
        }

        protected virtual void CustomDispose()
        {
            // NOTE: Intentionally left blank
        }

        public void Dispose()
        {
            CustomDispose();

            m_stream.Dispose();
        }
    }
}
