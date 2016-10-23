using System;

namespace SharpEnd.Network
{
    public delegate void PacketProcessor(GameClient client, InPacket inPacket);

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class PacketHandlerAttribute : Attribute
    {
        private EHeader m_header;

        public PacketHandlerAttribute(EHeader header)
        {
            m_header = header;
        }

        public EHeader Header { get { return m_header; } }
    }
}