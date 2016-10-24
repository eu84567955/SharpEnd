using SharpEnd.Drawing;
using SharpEnd.Network;
using SharpEnd.Game.Players;
using SharpEnd.Packets.Helpers;
using System;

namespace SharpEnd.Packets
{
    public static class MapPackets
    {
        public static byte[] ChangeMap(Player player, bool initial = false, bool spawnByPosition = false, Point position = null)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket.WriteHeader(EHeader.SMSG_MAP_CHANGE);
               
                // NOTE: Unknown.
                {
                    short count = 0;

                    outPacket.WriteShort(count);

                    while (count-- > 0)
                    {
                        outPacket
                            .WriteInt() // NOTE: Unknown.
                            .WriteInt(); // NOTE: Unknown.
                    }
                }

                outPacket
                    .WriteInt(player.Client.Channel)
                    .WriteBoolean(false) // NOTE: bDev.
                    .WriteInt() // NOTE: wOldDriverID.
                    .WriteByte(++player.Portals)
                    .WriteInt(22) // NOTE: Unknown.
                    .WriteInt(800) // NOTE: nFieldWidth.
                    .WriteInt(600) // NOTE: nFieldHeight.
                    .WriteBoolean(initial);

                // NOTE: Messages (top left of screen).
                {
                    short count = 0;

                    outPacket.WriteShort(count);

                    while (count-- > 0)
                    {
                        outPacket.WriteString(string.Empty); // NOTE: Text.
                    }
                }

                if (initial)
                {
                    long flag = -1;

                    // NOTE: RNG (for damage calculation).
                    {
                        outPacket
                            .WriteInt(1337) // NOTE: Seed 1.
                            .WriteInt(1337) // NOTE: Seed 2.
                            .WriteInt(1337); // NOTE: Seed 3.
                    }

                    outPacket
                        .WriteLong(flag)
                        .WriteByte() // NOTE: Combat orders.
                        .WriteInt(-18) // NOTE: Pet 1 active skill cooldown.
                        .WriteInt(-18) // NOTE: Pet 2 active skill cooldown.
                        .WriteInt(-18) // NOTE: Pet 3 active skill cooldown.
                        .WriteByte() // NOTE: Unknown.
                        .WriteByte() // NOTE: Unknown.
                        .WriteInt() // NOTE: Unknown.
                        .WriteByte(); // NOTE: Unknown.

                    PlayerPacketHelper.AddPlayerStats(outPacket, player);

                    outPacket.WriteByte(50); // NOTE: Buddies.

                    player.Skills.WriteBlessings(outPacket);
                    player.Items.WriteInitial(outPacket);
                    player.Skills.WriteInitial(outPacket);
                    player.Quests.WriteInitial(outPacket);

                    // NOTE: Match records.
                    {
                        outPacket.WriteShort();
                    }

                    // NOTE: Rings.
                    {
                        outPacket
                            .WriteShort() // NOTE: Couple.
                            .WriteShort() // NOTE: Friendship.
                            .WriteShort(); // NOTE: Marriage.
                    }

                    // NOTE: Teleport Rocks
                    {
                        int count = 41;

                        while (count-- > 0)
                        {
                            outPacket.WriteInt(999999999);
                        }
                    }

                    outPacket.WriteInt(); // NOTE: Unknown.

                    // NOTE: Monster Book.
                    {
                        outPacket
                            .WriteBoolean(false) // NOTE: Complete.
                            .WriteShort() // NOTE: Cards.
                            .WriteInt(-1); // NOTE: Selected set.
                    }

                    outPacket.WriteShort(); // NOTE: Unknown.

                    // NOTE: New year cards.
                    {
                        outPacket.WriteShort();
                    }

                    outPacket.WriteInt(); // NOTE: Unknown.

                    // NOTE: Player variables of some sort.
                    {
                        outPacket.WriteShort();
                    }

                    outPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 40 E0 FD 3B 37 4F 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0B 00 43 72 65 61 74 69 6E 67 2E 2E 2E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 40 E0 FD 3B 37 4F 01 00 00 00 00 01 8D 17 00 00 00 00 00 00 00 00 00 00 00 00 00 00 40 E0 FD 3B 37 4F 01 0A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 09 00 00 00 17 00 63 68 65 63 6B 31 3D 30 3B 63 44 61 74 65 3D 31 36 2F 31 30 2F 31 37 01 00 01 00 00 00 20 00 31 3A 30 3A 32 3A 31 32 3D 32 38 3B 30 3A 30 3A 30 3A 30 3D 31 36 3B 30 3A 31 3A 31 3A 35 3D 36 00 00 00 00 00 00 00 01 00 01 00 00 00 00 00 00 00 00 00 00 00 00 40 E0 FD 3B 37 4F 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 FB 5F 01 4E 2E D2 01 00 01 00 00 82 9F BA 02 01 8D 17 00 04 00 00 00 00 00 00 00 75 96 8F 00 00 00 00 00 76 96 8F 00 00 00 00 00 77 96 8F 00 00 00 00 00 78 96 8F 00 00 00 00 00 00 00 00 00");
                }
                else
                {
                    outPacket
                        .WriteBoolean(false) // NOTE: bUsingBuffProtector
                        .WriteInt(player.Map.ID)
                        .WriteSByte(player.SpawnPoint)
                        .WriteInt(player.HP)
                        .WriteBoolean(spawnByPosition);

                    if (spawnByPosition)
                    {
                        outPacket.WritePoint(position);
                    }
                }

                outPacket
                    .WriteBoolean(true) // NOTE: White fade in-and-out.
                    .WriteBoolean(false) // NOTE: Overlapping screen animation.
                    .WriteLongDateTime(DateTime.Now)
                    .WriteInt(100) // NOTE: Unknown.
                    .WriteBoolean(false) // NOTE: Party experience.
                    .WriteBoolean(false) // NOTE: Unknown.
                    .WriteBoolean(false) // NOTE: Unknown.
                    .WriteBoolean(false) // NOTE: Unknown.
                    .WriteBoolean(false) // NOTE: starplanet.
                    .WriteBoolean(false) // NOTE: aStarPlanetRoundInfo.
                    .WriteInt() // NOTE: Unknown.
                    .WriteByte() // NOTE: nAccountType.
                    .WriteInt() // NOTE: dwAccountID.
                    .WriteInt() // NOTE: dwEventBestFriendAID.
                    .WriteInt(); // NOTE: Unknown.

                return outPacket.ToArray();
            }
        }

        public static byte[] MapSeat(int playerID, short seatID)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_SIT)
                    .WriteInt(playerID)
                    .WriteBoolean(true)
                    .WriteShort(seatID);

                return outPacket.ToArray();
            }
        }

        public static byte[] MapSeatCancel(int playerID)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_SIT)
                    .WriteInt(playerID)
                    .WriteBoolean(false);

                return outPacket.ToArray();
            }
        }

        public static byte[] ShowTimer(int time)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_CLOCK)
                    .WriteByte(2)
                    .WriteInt(time);

                return outPacket.ToArray();
            }
        }
    }
}
