using SharpEnd.Maps;
using SharpEnd.Network;

namespace SharpEnd.Packets
{
    internal static class NpcPackets
    {
        public static byte[] NpcSpawn(Npc npc)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_NPC_SPAWN)
                    .WriteInt(npc.ObjectIdentifier)
                    .WriteInt(npc.Identifier)
                    .WritePoint(npc.Position)
                    .WriteByte(npc.Stance)
                    .WriteBoolean(!npc.Flip)
                    .WriteUShort(npc.Foothold)
                    .WriteShort(npc.RX0)
                    .WriteShort(npc.RX1)
                    .WriteBoolean(!npc.Hide)
                    .WriteInt() // NOTE: Unknown
                    .WriteByte() // NOTE: tPresentTimeState
                    .WriteInt(-1) // NOTE: tPresent
                    .WriteInt() // NOTICE: nNoticeBoardType
                    .WriteInt() // NOTE: Unknown
                    .WriteInt() // NOTE: Unknown
                    .WriteString(string.Empty) // NOTE: Unknown
                    .WriteByte(); // NOTE: Unknown

                return outPacket.ToArray();
            }
        }

        public static byte[] NpcControlRequest(Npc npc)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_NPC_CONTROL)
                    .WriteBoolean(true)
                    .WriteInt(npc.ObjectIdentifier)
                    .WriteInt(npc.Identifier)
                    .WritePoint(npc.Position)
                    .WriteByte(npc.Stance)
                    .WriteBoolean(!npc.Flip)
                    .WriteUShort(npc.Foothold)
                    .WriteShort(npc.RX0)
                    .WriteShort(npc.RX1)
                    .WriteBoolean(!npc.Hide)
                    .WriteInt() // NOTE: Unknown
                    .WriteByte() // NOTE: tPresentTimeState
                    .WriteInt(-1) // NOTE: tPresent
                    .WriteInt() // NOTICE: nNoticeBoardType
                    .WriteInt() // NOTE: Unknown
                    .WriteInt() // NOTE: Unknown
                    .WriteString(string.Empty) // NOTE: Unknown
                    .WriteByte(); // NOTE: Unknown

                return outPacket.ToArray();
            }
        }

        public static byte[] NpcControlCancel(int objectIdentifier)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_NPC_CONTROL)
                    .WriteBoolean(false)
                    .WriteInt(objectIdentifier);

                return outPacket.ToArray();
            }
        }

        public static byte[] NpcDespawn(int objectIdentifier)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_NPC_DESPAWN)
                    .WriteInt(objectIdentifier);

                return outPacket.ToArray();
            }
        }

        public static byte[] NpcAction(int objectIdentifier, byte[] buffer)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_NPC_ACTION)
                    .WriteInt(objectIdentifier)
                    .WriteBytes(buffer);

                return outPacket.ToArray();
            }
        }
    }
}
