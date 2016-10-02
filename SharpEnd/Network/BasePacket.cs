using System;
using System.IO;

namespace SharpEnd.Network
{
    internal abstract class BasePacket : IDisposable
    {
        public EOpcode Header { get; protected set; }

        protected MemoryStream m_stream;

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
