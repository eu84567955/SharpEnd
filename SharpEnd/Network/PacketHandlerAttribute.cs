using System;

namespace SharpEnd.Network
{
    internal delegate void PacketProcessor(Client pClient, InPacket inPacket);

    internal sealed class PacketHandlerAttribute : Attribute
    {
        public readonly EOpcode Opcode;
        public PacketProcessor Processor;

        public PacketHandlerAttribute(EOpcode pOpcode) { Opcode = pOpcode; }
    }
}