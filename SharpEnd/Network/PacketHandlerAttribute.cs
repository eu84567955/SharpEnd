using System;

namespace SharpEnd.Network
{
    internal delegate void PacketProcessor(Client pClient, InPacket inPacket);

    internal sealed class PacketHandlerAttribute : Attribute
    {
        public readonly EHeader Opcode;
        public PacketProcessor Processor;

        public PacketHandlerAttribute(EHeader pOpcode) { Opcode = pOpcode; }
    }
}