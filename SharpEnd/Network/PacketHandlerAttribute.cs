using System;

namespace SharpEnd.Network
{
    internal sealed class PacketHandlerAttribute : Attribute
    {
        public EOpcode Header { get; private set; }

        public PacketHandlerAttribute(EOpcode header)
        {
            Header = header;
        }
    }
}
