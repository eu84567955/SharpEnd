using SharpEnd.Network;
using SharpEnd.Game.Players;
using SharpEnd.Utility;
using System.Collections.Generic;
using SharpEnd.Packets.Helpers;

namespace SharpEnd.Packets
{
    public static class PlayerPackets
    {
        public static byte[] PlayerUpdate(Player player, EPlayerUpdate bits = EPlayerUpdate.None, bool itemReaction = false)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                /*outPacket
                    .WriteHeader(EHeader.SMSG_PLAYER_UPDATE)
                    .WriteBoolean(itemReaction)
                    .WriteULong((ulong)bits);

                // NOTE: For now it only accepts updateBits as a single unit.
                // Might be a collection later when we need it.
                switch (bits)
                {
                    case EPlayerUpdate.Skin: outPacket.WriteByte(player.Skin); break;
                    case EPlayerUpdate.Face: outPacket.WriteInt(player.Face); break;
                    case EPlayerUpdate.Hair: outPacket.WriteInt(player.Hair); break;
                    case EPlayerUpdate.Level: outPacket.WriteByte(player.Stats.Level); break;
                    case EPlayerUpdate.Job:
                        {
                            outPacket
                                .WriteUShort(player.Stats.Job)
                                .WriteUShort(player.Stats.SubJob);
                        }
                        break;
                    case EPlayerUpdate.Strength: outPacket.WriteUShort(player.Stats.Strength); break;
                    case EPlayerUpdate.Dexterity: outPacket.WriteUShort(player.Stats.Dexterity); break;
                    case EPlayerUpdate.Intelligence: outPacket.WriteUShort(player.Stats.Intelligence); break;
                    case EPlayerUpdate.Luck: outPacket.WriteUShort(player.Stats.Luck); break;
                    case EPlayerUpdate.Health: outPacket.WriteUInt(player.Stats.Health); break;
                    case EPlayerUpdate.MaxHealth: outPacket.WriteUInt(player.Stats.MaxHealth); break;
                    case EPlayerUpdate.Mana: outPacket.WriteUInt(player.Stats.Mana); break;
                    case EPlayerUpdate.MaxMana: outPacket.WriteUInt(player.Stats.MaxMana); break;
                    case EPlayerUpdate.AbilityPoints: outPacket.WriteUShort(player.Stats.AbilityPoints); break;
                    case EPlayerUpdate.SkillPoints:
                        {
                            if (GameLogicUtilities.HasSeparatedSkillPoints(player.Stats.Job))
                            {
                                player.SPTable.WriteGeneral(outPacket);
                            }
                            else
                            {
                                outPacket.WriteUShort(player.Stats.SkillPoints);
                            }
                        }
                        break;
                    case EPlayerUpdate.Experience: outPacket.WriteULong(player.Stats.Experience); break;
                    case EPlayerUpdate.Fame: outPacket.WriteInt(player.Stats.Fame); break;
                    case EPlayerUpdate.Meso: outPacket.WriteLong(player.Items.Meso); break;
                    case EPlayerUpdate.Pet:
                        {
                            // TODO: Handle special case.
                        }
                        break;
                }

                outPacket
                    .WriteByte() // NOTE: Unknown
                    .WriteByte() // NOTE: Unknown
                    .WriteByte() // NOTE: Unknown
                    .WriteByte() // NOTE: Unknown
                    .WriteByte(); // NOTE: Unknown*/

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

                    count++;
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
                    .WriteInt(player.ID)
                    .WriteByte(player.Level)
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

                PlayerPacketHelper.AddPlayerDisplay(outPacket, player);

                outPacket
                    .WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00")
                    .WritePoint(player.Position)
                    .WriteByte(player.Stance)
                    .WriteShort(player.Foothold)
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

        public static byte[] PlayerDespawn(int playerID)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_PLAYER_DESPAWN)
                    .WriteInt(playerID);

                return outPacket.ToArray();
            }
        }

        private const int KEYMAP_SIZE = 89;

        public static byte[] Keymap(Dictionary<int, Shortcut> keymap)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_KEYMAP)
                    .WriteBoolean(keymap.Count == 0);

                for (int i = 0; i < KEYMAP_SIZE; i++)
                {
                    if (keymap.ContainsKey(i))
                    {
                        outPacket
                            .WriteByte(keymap[i].Type)
                            .WriteInt(keymap[i].Action);
                    }
                    else
                    {
                        outPacket
                            .WriteByte()
                            .WriteInt();
                    }
                }

                return outPacket.ToArray();
            }
        }

        public static byte[] QuickSlot(int[] binds)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_KEYMAP)
                    .WriteBoolean(true);

                if (binds.Length == 28)
                {
                    for (int i = 0; i < 28; i++)
                    {
                        outPacket.WriteInt(binds[i]);
                    }
                }
                else
                {
                    outPacket.WriteZero(112);
                }

                return outPacket.ToArray();
            }
        }
    }
}
