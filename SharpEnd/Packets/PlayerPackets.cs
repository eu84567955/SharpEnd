using SharpEnd.Network;
using SharpEnd.Players;
using SharpEnd.Utility;

namespace SharpEnd.Packets
{
    internal static class PlayerPackets
    {
        public static byte[] PlayerStatUpdate(EStatisticType updateBits, long value, bool itemReaction = false)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_PLAYER_STAT_UPDATE)
                    .WriteBoolean(itemReaction)
                    .WriteULong((ulong)updateBits);

                // NOTE: For now it only accepts updateBits as a single unit
                // Might be a collection later when we need it
                switch (updateBits)
                {
                    case EStatisticType.Skin:
                    case EStatisticType.Level:
                        outPacket.WriteByte((byte)value);
                        break;

                    case EStatisticType.Job:
                    case EStatisticType.Strength:
                    case EStatisticType.Dexterity:
                    case EStatisticType.Intelligence:
                    case EStatisticType.Luck:
                    case EStatisticType.AbilityPoints:
                        {
                            outPacket.WriteUShort((ushort)value);

                            if (updateBits == EStatisticType.Job)
                            {
                                outPacket.WriteUShort(); // NOTE: Subcategory.
                            }
                        }
                        break;

                    case EStatisticType.Face:
                    case EStatisticType.Hair:
                    case EStatisticType.Health:
                    case EStatisticType.MaxHealth:
                    case EStatisticType.Mana:
                    case EStatisticType.MaxMana:
                    case EStatisticType.Fame:
                        outPacket.WriteInt((int)value);
                        break;

                    case EStatisticType.Experience:
                    case EStatisticType.Meso:
                        outPacket.WriteULong((ulong)value);
                        break;

                    case EStatisticType.SkillPoints:
                        {
                            // TODO: Skill points special case
                        }
                        break;

                    case EStatisticType.Pet:
                        {
                            // TODO: Pet special case
                        }
                        break;
                }

                outPacket
                    .WriteByte() // NOTE: Unknown
                    .WriteByte() // NOTE: Unknown
                    .WriteByte() // NOTE: Unknown
                    .WriteByte() // NOTE: Unknown
                    .WriteByte(); // NOTE: Unknown

                return outPacket.ToArray();
            }
        }

        public static byte[] EventNameTag(sbyte[] activeEventNameTag)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket.WriteHeader(EHeader.SMSG_EVENT_NAME_TAG_INFO);

                int count = 0;
                do
                {
                    outPacket
                        .WriteString(string.Empty)
                        .WriteSByte(activeEventNameTag[count]);
                } while (count < 5);

                return outPacket.ToArray();
            }
        }

        public static byte[] PlayerSpawn(Player player)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_PLAYER_SPAWN)
                    .WriteInt(player.Identifier)
                    .WriteByte(player.Stats.Level)
                    .WriteString(player.Name)
                    .WriteString(string.Empty); // NOTE: Ultimate Explorer

                // NOTE: Guild
                {
                    outPacket
                        .WriteString(string.Empty) // NOTE: Name
                        .WriteShort() // NOTE: Logo background
                        .WriteByte() // NOTE: Logo background color
                        .WriteShort() // NOTE: Logo
                        .WriteByte(); // NOTE: Logo color
                }

                // NOTE: Buffs
                {
                    outPacket
                        .WriteByte()
                        .WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 C0 00 00 00 00 00 00 00 00 00 18 00 00 00 00 00 00 00 20 14 00 10 80 00 00 00 00 00 00 80 00 F0 0F 00 00 00 00 00")
                        .WriteInt(-1)
                        .WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                }

                // NOTE: Magic
                {
                    outPacket.WriteHexString("01 1A B7 01 00 00 00 00 00 00 00 00 00 01 1A B7 01 00 00 00 00 00 00 00 00 00 00 00 01 1A B7 01 00 00 00 00 00 00 00 00 00 00 00 01 1A B7 01 00 00 00 00 00 00 00 00 00 01 1A B7 01 00 00 47 84 B0 C2 00 00 00 00 00 00 00 00 00 00 01 1A B7 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 1A B7 01 00 00 00 00 00 00 00 00 00 00 00 01 1A B7 01 00 00 00 00 00 00 00 00 00 00 00");
                }

                HelpPackets.AddPlayerDisplay(outPacket, player);

                outPacket
                    .WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00")
                    .WritePoint(player.Position)
                    .WriteSByte(player.Stance)
                    .WriteUShort(player.Foothold)
                    .WriteHexString("00 00 00 01 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

                // NOTE: Farm
                {
                    outPacket.WriteHexString("0B 00 43 72 65 61 74 69 6E 67 2E 2E 2E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00");
                }

                // NOTE: Titles
                {
                    for (sbyte i = 0; i < 5; i++)
                    {
                        outPacket.WriteSByte(-1);
                    }
                }

                outPacket.WriteHexString("00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

                return outPacket.ToArray();
            }
        }

        public static byte[] PlayerDespawn(int playerIdentifier)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_PLAYER_DESPAWN)
                    .WriteInt(playerIdentifier);

                return outPacket.ToArray();
            }
        }
    }
}
