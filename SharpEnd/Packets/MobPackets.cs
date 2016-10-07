using SharpEnd.Maps;
using SharpEnd.Network;

namespace SharpEnd.Packets
{
    internal static class MobPackets
    {
        public static byte[] MobSpawn(Mob mob)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_MOB_SPAWN)
                    .WriteBoolean(false)
                    .WriteInt(mob.ObjectIdentifier)
                    .WriteBoolean(true)
                    .WriteInt(mob.Identifier)
                    .WriteHexString("01 41 00 00 00 41 00 00 00 0A 00 00 00 14 00 00 00 10 00 00 00 0A 00 00 00 0A 00 00 00 0A 00 00 00 00 00 00 00 0A 00 00 00 00 00 00 00 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 60 80 FF A7 00 00 00 00 00 00 00 00 00 6F 74 00 00 00 00 00 00 00 00 6F 74 00 00 00 00 00 00 00 00 6F 74 00 00 00 00 00 00 00 00 6F 74 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00")
                    .WritePoint(mob.Position)
                    .WriteSByte(mob.Stance)
                    .WriteUShort(mob.Foothold)
                    .WriteUShort(mob.Foothold)
                    .WriteHexString("FF FF FF 41 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 64 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00");

                return outPacket.ToArray();
            }
        }

        public static byte[] MobControlRequest(Mob mob)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_MOB_CONTROL)
                    .WriteBoolean(true)
                    .WriteInt(mob.ObjectIdentifier)
                    .WriteBoolean(true)
                    .WriteInt(mob.Identifier)
                    .WriteHexString("01 41 00 00 00 41 00 00 00 0A 00 00 00 14 00 00 00 10 00 00 00 0A 00 00 00 0A 00 00 00 0A 00 00 00 00 00 00 00 0A 00 00 00 00 00 00 00 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 60 80 FF A7 00 00 00 00 00 00 00 00 00 6F 74 00 00 00 00 00 00 00 00 6F 74 00 00 00 00 00 00 00 00 6F 74 00 00 00 00 00 00 00 00 6F 74 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00")
                    .WritePoint(mob.Position)
                    .WriteSByte(mob.Stance)
                    .WriteUShort(mob.Foothold)
                    .WriteUShort(mob.Foothold)
                    .WriteHexString("FF FF FF 41 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 64 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00");

                return outPacket.ToArray();
            }
        }

        public static byte[] MobControlCancel(int objectIdentifier)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_MOB_CONTROL)
                    .WriteBoolean(false)
                    .WriteInt(objectIdentifier);

                return outPacket.ToArray();
            }
        }

        public static byte[] MobControlAck(int objectIdentifier, short movementIdentifier, uint mana, bool usingAbility, int skillIdentifier, byte skillLevel)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_MOB_CONTROL_ACK)
                    .WriteInt(objectIdentifier)
                    .WriteShort(movementIdentifier)
                    .WriteBoolean(usingAbility)
                    .WriteUInt(mana)
                    .WriteInt(skillIdentifier)
                    .WriteByte(skillLevel)
                    .WriteInt(); // NOTE: Attack identifier

                return outPacket.ToArray();
            }
        }

        public static byte[] MobDespawn(int objectIdentifier, byte effect)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_MOB_DESPAWN)
                    .WriteInt(objectIdentifier)
                    .WriteByte(effect);

                return outPacket.ToArray();
            }
        }
    }
}
