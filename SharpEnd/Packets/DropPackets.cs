using SharpEnd.Drawing;
using SharpEnd.Maps;
using SharpEnd.Network;

namespace SharpEnd.Packets
{
    internal static class DropPackets
    {
        public static byte[] SpawnDrop(Drop drop, EDropAnimation animation)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_DROP_SPAWN)
                    .WriteByte() // NOTE: Unknown
                    .WriteSByte((sbyte)animation)
                    .WriteInt(drop.ObjectIdentifier)
                    .WriteBoolean(drop.IsMeso)
                    .WriteInt() // NOTE: nDropMotionType
                    .WriteInt() // NOTE: nDropSpeed
                    .WriteInt() // NOTE: bNoMove
                    .WriteInt(drop.IsMeso ? drop.Meso : drop.Item.Identifier)
                    .WriteInt() // NOTE: Owner identifier
                    .WriteSByte((sbyte)drop.Type)
                    .WritePoint(drop.Position)
                    .WriteInt(); // NOTE: dwSourceID

                if (animation != EDropAnimation.Existing)
                {
                    outPacket
                        .WritePoint(drop.Position) // NOTE: Origin, but should be fine
                        .WriteInt(); // NOTE: tDelay
                }

                outPacket.WriteBoolean(false); // OTE: bExplosiveDrop

                if (!drop.IsMeso)
                {
                    outPacket.WriteLong(150842304000000000);
                }

                outPacket
                    .WriteBoolean(false) // NOTE: Pet drop
                    .WriteByte() // NOTE: Unknown
                    .WriteShort() // NOTE: nFallingVY
                    .WriteByte() // NOTE: nFadeInEffect
                    .WriteByte() // NOTE: nMakeType
                    .WriteInt() // NOTE: bCollisionPickup
                    .WriteByte() // NOTE: nItemGrade
                    .WriteBoolean(false); // NOTE: bPrepareCollisionPickUp

                return outPacket.ToArray();
            }
        }

        public static byte[] DespawnDrop(sbyte animation, int objectIdentifier, int ownerIdentifier)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_DROP_DESPAWN)
                    .WriteSByte(animation)
                    .WriteInt(objectIdentifier);

                if (animation >= 2)
                {
                    outPacket.WriteInt(ownerIdentifier);
                }

                return outPacket.ToArray();
            }
        }
    }
}
