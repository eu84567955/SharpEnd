using System;
using System.IO;

namespace SharpEnd.Network
{
    internal abstract class BasePacket : IDisposable
    {
        public EOpcode Header { get; protected set; }

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
