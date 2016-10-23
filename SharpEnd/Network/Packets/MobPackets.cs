using SharpEnd.Game.Life;
using SharpEnd.Network;

namespace SharpEnd.Packets
{
    public static class MobPackets
    {
        public static byte[] MobSpawn(Mob mob, sbyte spawnType)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_MOB_SPAWN)
                    .WriteByte()
                    .WriteBytes(MobInit(mob));

                return outPacket.ToArray();
            }
        }

        public static byte[] MobControlRequest(Mob mob)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_MOB_CONTROL)
                    .WriteByte(1)
                    .WriteBytes(MobInit(mob));

                return outPacket.ToArray();
            }
        }

        private static byte[] MobInit(Mob mob)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteInt(mob.ObjectID)
                    .WriteBoolean(true)
                    .WriteInt(mob.ID)
                    .WriteByte((byte)mob.ControlStatus)
                    .WriteHexString("41 00 00 00 41 00 00 00 0A 00 00 00 14 00 00 00 10 00 00 00 0A 00 00 00 0A 00 00 00 0A 00 00 00 00 00 00 00 0A 00 00 00 00 00 00 00 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 60 80 FF A7 00 00 00 00 00 00 00 00 00 6F 74 00 00 00 00 00 00 00 00 6F 74 00 00 00 00 00 00 00 00 6F 74 00 00 00 00 00 00 00 00 6F 74 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00")
                    .WritePoint(mob.Position)
                    .WriteByte(mob.Stance)
                    .WriteShort(mob.Foothold)
                    .WriteShort(mob.Foothold) // NOTE: Initial spawned foothold (is it really used by the client?).
                    .WriteHexString("FF FF FF 41 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 64 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00");

                return outPacket.ToArray();
            }
        }

        public static byte[] MobControlCancel(int objectID)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_MOB_CONTROL)
                    .WriteBoolean(false)
                    .WriteInt(objectID);

                return outPacket.ToArray();
            }
        }

        public static byte[] MobControlAck(int objectID, short movementID, uint mana, bool usingAbility, int skillID, byte skillLevel)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_MOB_CONTROL_ACK)
                    .WriteInt(objectID)
                    .WriteShort(movementID)
                    .WriteBoolean(usingAbility)
                    .WriteUInt(mana)
                    .WriteInt(skillID)
                    .WriteByte(skillLevel)
                    .WriteInt(); // NOTE: Attack identifier

                return outPacket.ToArray();
            }
        }

        public static byte[] MobDespawn(int objectID, byte effect)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_MOB_DESPAWN)
                    .WriteInt(objectID)
                    .WriteByte(effect);

                return outPacket.ToArray();
            }
        }

        public static byte[] MobHealth(int objectID, byte percent)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_MOB_HEALTH)
                    .WriteInt(objectID)
                    .WriteByte(percent);

                return outPacket.ToArray();
            }
        }
    }
}
