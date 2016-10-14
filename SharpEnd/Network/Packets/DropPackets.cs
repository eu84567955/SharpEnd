using SharpEnd.Game.Maps;
using SharpEnd.Network;
using SharpEnd.Players;

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
                    .WriteByte() // NOTE: Unknown.
                    .WriteSByte((sbyte)animation)
                    .WriteInt(drop.ObjectIdentifier)
                    .WriteBoolean(drop is Meso)
                    .WriteInt() // NOTE: nDropMotionType.
                    .WriteInt() // NOTE: nDropSpeed.
                    .WriteInt(); // NOTE: bNoMove.

                if (drop is Meso)
                {
                    outPacket.WriteInt(((Meso)drop).Amount);
                }
                else if (drop is PlayerItem)
                {
                    outPacket.WriteInt(((PlayerItem)drop).Identifier);
                }

                outPacket
                    .WriteInt(drop.Owner != null ? drop.Owner.Identifier : 0)
                    .WriteByte() // TODO: Figure this one out.
                    .WritePoint(drop.Position)
                    .WriteInt(drop.Dropper.ObjectIdentifier);

                if (animation != EDropAnimation.Existing)
                {
                    outPacket
                        .WritePoint(drop.Origin)
                        .WriteInt(); // NOTE: tDelay.
                }

                outPacket.WriteBoolean(false); // NOTE: bExplosiveDrop.

                if (!(drop is Meso)) // TODO: Item expiration.
                {
                    outPacket.WriteLong(150842304000000000);
                }

                outPacket
                    .WriteBoolean(false) // NOTE: Pet drop.
                    .WriteByte() // NOTE: Unknown.
                    .WriteShort() // NOTE: nFallingVY.
                    .WriteByte() // NOTE: nFadeInEffect.
                    .WriteByte() // NOTE: nMakeType.
                    .WriteInt() // NOTE: bCollisionPickup.
                    .WriteByte() // NOTE: nItemGrade.
                    .WriteBoolean(false); // NOTE: bPrepareCollisionPickUp.

                return outPacket.ToArray();
            }
        }

        public static byte[] DespawnDrop(int objectIdentifier, Player picker)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_DROP_DESPAWN)
                    .WriteByte((byte)(picker == null ? 0 : 2))
                    .WriteInt(objectIdentifier)
                    .WriteInt(picker != null ? picker.Identifier : 0);

                return outPacket.ToArray();
            }
        }
    }
}
