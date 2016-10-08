using SharpEnd.Drawing;
using SharpEnd.Network;
using SharpEnd.Players;

namespace SharpEnd.Packets
{
    internal static class MapPackets
    {
        public static byte[] ChangeMap(Player player, bool initial = false, bool spawnByPosition = false, Point position = null)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_CHANGE_MAP)
                    .WriteShort() // NOTE: Random loop
                    .WriteInt() // NOTE: Channel identifier
                    .WriteBoolean(false) // NOTE: bDev
                    .WriteInt() // NOTE: wOldDriverID
                    .WriteByte(++player.PortalCount)
                    .WriteInt() // NOTE: Unknown
                    .WriteInt(800) // NOTE: nFieldWidth
                    .WriteInt(600) // NOTE: nFieldHeight
                    .WriteBoolean(initial)
                    .WriteShort(); // NOTE: Messages

                if (initial)
                {
                    long flag = -1;

                    // NOTE: RNG
                    {
                        outPacket
                            .WriteInt(1337) // NOTE: Seed 1
                            .WriteInt(1337) // NOTE: Seed 2
                            .WriteInt(1337); // NOTE: Seed 3
                    }

                    outPacket
                        .WriteLong(flag)
                        .WriteByte() // NOTE: Combat orders
                        .WriteInt(-18) // NOTE: Pet 1 active skill cooldown
                        .WriteInt(-18) // NOTE: Pet 2 active skill cooldown
                        .WriteInt(-18) // NOTE: Pet 3 active skill cooldown
                        .WriteByte() // NOTE: Unknown
                        .WriteByte() // NOTE: Unknown
                        .WriteInt() // NOTE: Unknown
                        .WriteByte(); // NOTE: Unknown

                    // NOTE: Stats
                    {
                        outPacket
                            .WriteInt(player.Identifier)
                            .WriteInt(player.Identifier)
                            .WriteInt(45)
                            .WriteString(player.Name, 13)
                            .WriteByte(player.Gender)
                            .WriteByte(player.Skin)
                            .WriteInt(player.Face)
                            .WriteInt(player.Hair)
                            .WriteSByte(-1)
                            .WriteSByte()
                            .WriteSByte();

                        player.Stats.WriteInitial(outPacket);

                        outPacket
                            .WriteInt() // NOTE: Waru points
                            .WriteInt() // NOTE: Gachapon experience
                            .WriteInt(player.MapIdentifier)
                            .WriteSByte(player.SpawnPoint)
                            .WriteInt() // NOTE: Unknown
                            .WriteUShort(player.Stats.SubJob)
                            .WriteByte() // NOTE: Fatigue
                            .WriteInt(); // NOTE: Date

                        // NOTE: Traits
                        {
                            outPacket.WriteZero(36);
                        }

                        outPacket
                            .WriteByte() // NOTE: Unknown
                            .WriteLong() // NOTE: Unknown
                            .WriteInt() // NOTE: Battle experience
                            .WriteByte() // NOTE: Battle rank
                            .WriteInt() // NOTE: Battle points
                            .WriteByte(5) // NOTE: Battle mode level
                            .WriteByte(6) // NOTE: Battle mode type
                            .WriteInt(); // NOTE: Event points

                        // NOTE: Part Time Job
                        {
                            outPacket
                                .WriteByte() // NOTE: Job
                                .WriteLong() // NOTE: Time left
                                .WriteInt() // NOTE: Reward/duration
                                .WriteBoolean(false); // NOTE: Finished
                        }

                        // NOTE: Cards
                        {
                            int count = 9;

                            while (count-- > 0)
                            {
                                outPacket
                                    .WriteInt()
                                    .WriteByte()
                                    .WriteInt();
                            }
                        }

                        outPacket
                            .WriteLong() // NOTE: Last login date
                            .WriteBoolean(false); // NOTE: Burning event
                    }

                    outPacket.WriteByte(50); // NOTE: Buddy capcity

                    player.Skills.WriteBlessings(outPacket);
                    player.Items.WriteInitial(outPacket);
                    player.Skills.WriteInitial(outPacket);
                    player.Quests.WriteInitial(outPacket);

                    // NOTE: Match Records
                    {
                        outPacket.WriteShort();
                    }

                    // NOTE: Rings
                    {
                        outPacket
                            .WriteShort() // NOTE: Couple
                            .WriteShort() // NOTE: Friendship
                            .WriteShort(); // NOTE: Marriage
                    }

                    // NOTE: Teleport Rocks
                    {
                        int count = 41;

                        while (count-- > 0)
                        {
                            outPacket.WriteInt(999999999);
                        }
                    }

                    outPacket.WriteInt(); // NOTE: Unknown

                    // NOTE: Monster Book
                    {
                        outPacket
                            .WriteBoolean(false) // NOTE: Not completed
                            .WriteShort() // NOTE: Cards
                            .WriteInt(-1); // NOTE: Set
                    }

                    outPacket.WriteShort(); // NOTE: Unknown

                    // NOTE: New Year Cards
                    {
                        outPacket.WriteShort();
                    }

                    outPacket.WriteInt(); // NOTE: Unknown

                    // NOTE: Party Quests
                    {
                        outPacket.WriteShort();
                    }

                    outPacket
                        .WriteShort() // NOTE: Unknown
                        .WriteInt() // NOTE: Unknown
                        .WriteShort(); // NOTE: Unknown

                    // NOTE: Steal skills
                    {
                        outPacket.WriteZero(80);
                    }

                    // NOTE: Inner ability
                    {
                        outPacket.WriteShort();
                    }

                    outPacket
                        .WriteShort() // NOTE: Unknown
                        .WriteInt() // NOTE: Unknown
                        .WriteByte() // NOTE: Unknown
                        .WriteInt(1) // NOTE: Honor level
                        .WriteInt() // NOTE: Honor experience
                        .WriteByte(1) // NOTE: Unknown
                        .WriteShort() // NOTE: Unknown
                        .WriteByte() // NOTE: Unknown
                        .WriteInt() // NOTE: Angelic Buster face
                        .WriteInt() // NOTE: Angelic buster hair
                        .WriteInt() // NOTE: Angelic buster overall
                        .WriteByte() // NOTE: Unknown
                        .WriteInt(-1) // NOTE: Unknown
                        .WriteInt() // NOTE: Unknown
                        .WriteInt() // NOTE: Unknown
                        .WriteInt() // NOTE: Unknown
                        .WriteInt() // NOTE: Unknown
                        .WriteLong(94354848000000000) // NOTE: Unknown
                        .WriteString(string.Empty) // NOTE: Unknown
                        .WriteInt() // NOTE: Unknown
                        .WriteShort() // NOTE: Unknown
                        .WriteShort(); // NOTE: Unknown

                    // NOTE: FARM_POTENTIAL
                    {
                        outPacket.WriteInt();
                    }

                    // NOTE: FarmUserInfo
                    // NOTE: FarmSubInfo
                    {
                        // NOTE: FarmUserInfo
                        {
                            outPacket
                                .WriteString("Creating...") // NOTE: Name
                                .WriteInt() // NOTE: Waru
                                .WriteInt() // NOTE: Level
                                .WriteInt() // NOTE: Experience
                                .WriteInt() // NOTE: Aesthetic points
                                .WriteInt() // NOTE: Gems
                                .WriteByte(2)
                                .WriteInt() // NOTE: Theme
                                .WriteInt() // NOTE: Slot extend
                                .WriteInt(1); // NOTE: Locker slot count
                        }

                        // NOTE: FarmSubInfo
                        {
                            outPacket
                                .WriteInt()
                                .WriteInt();
                        }
                    }

                    outPacket.WriteHexString("00 00 00 00 00 00 40 E0 FD 3B 37 4F 01 00 00 00 00 46 DA 12 00 00 00 00 00 00 00 00 00 00 00 00 00 00 40 E0 FD 3B 37 4F 01 0A 00 00 00 02 00 09 00 00 00 17 00 63 68 65 63 6B 31 3D 30 3B 63 44 61 74 65 3D 31 36 2F 30 39 2F 33 30 06 00 00 00 0C 00 65 6E 74 65 72 3D 32 30 31 36 30 36 00 00 00 00 00 00 00 01 00 01 00 00 00 00 00 00 00 00 00 00 00 00 40 E0 FD 3B 37 4F 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 30 59 44 C0 D5 1D D2 01 00 01 00 00 1F 9C 7C 01 46 DA 12 00 04 00 00 00 00 00 00 00 75 96 8F 00 00 00 00 00 76 96 8F 00 00 00 00 00 77 96 8F 00 00 00 00 00 78 96 8F 00 00 00 00 00 00 00 00 00");
                }
                else
                {
                    outPacket
                        .WriteBoolean(false) // NOTE: bUsingBuffProtector
                        .WriteInt(player.MapIdentifier)
                        .WriteSByte(player.SpawnPoint)
                        .WriteUInt(player.Stats.Health) // NOTE: Health
                        .WriteBoolean(spawnByPosition);

                    if (spawnByPosition)
                    {
                        outPacket.WritePoint(position);
                    }
                }

                outPacket
                    .WriteByte() // NOTE: White fade in-and-out
                    .WriteByte() // NOTE: Overlapping screen anmation
                    .WriteLong(131199279167150000) // NOTE: Unknown
                    .WriteInt(100) // NOTE: Unknown
                    .WriteBoolean(false) // NOTE: Party experience
                    .WriteBoolean(false) // NOTE: Unknown
                    .WriteBoolean(true) // NOTE: Unknown
                    .WriteBoolean(false) // NOTE: Unknown
                    .WriteBoolean(false) // NOTE: starplanet
                    .WriteBoolean(false) // NOTE: aStarPlanetRoundInfo
                    .WriteInt() // NOTE: Unknown
                    .WriteByte() // NOTE: nAccountType
                    .WriteInt() // NOTE: dwAccountID
                    .WriteInt() // NOTE: dwEventBestFriendAID
                    .WriteInt(); // NOTE: Unknown

                return outPacket.ToArray();
            }
        }

        public static byte[] MapSeat(int playerIdentifier, short seatIdentifier)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_SIT)
                    .WriteInt(playerIdentifier)
                    .WriteBoolean(true)
                    .WriteShort(seatIdentifier);

                return outPacket.ToArray();
            }
        }

        public static byte[] MapSeatCancel(int playerIdentifier)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_SIT)
                    .WriteInt(playerIdentifier)
                    .WriteBoolean(false);

                return outPacket.ToArray();
            }
        }
    }
}
