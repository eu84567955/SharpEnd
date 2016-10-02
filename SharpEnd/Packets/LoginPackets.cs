using SharpEnd.Network;
using SharpEnd.Servers;
using SharpEnd.Utility;

namespace SharpEnd.Packets
{
    internal static class LoginPackets
    {
        public static byte[] Handshake(byte[] riv, byte[] siv)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteUShort(15)
                    .WriteUShort(176)
                    .WriteString("3")
                    .WriteBytes(riv)
                    .WriteBytes(siv)
                    .WriteByte(8)
                    .WriteByte();

                return outPacket.ToArray();
            }
        }

        public static byte[] LoginError(uint type)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EOpcode.SMSG_AUTHENTICATION)
                    .WriteUInt(type)
                    .WriteByte()
                    .WriteByte();

                return outPacket.ToArray();
            }
        }

        public static byte[] LoginSuccess(Client client)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EOpcode.SMSG_AUTHENTICATION)
                    .WriteUInt()
                    .WriteByte()
                    .WriteByte()
                    .WriteString(client.Account.Username)
                    .WriteInt(client.Account.Identifier)
                    .WriteZero(11)
                    .WriteString(client.Account.Username)
                    .WriteHexString("03 00 00 00 00 00 00 00 00 00 30 58 67 CF E4 F3 CA 01 3D 00 00 00 01 08 01 01 00 01 01 00 01 01 00 01 01 00 01 01 00 01 01 00 01 01 00 01 01 00 01 01 00 01 01 00 01 01 00 01 01 00 01 01 00 01 01 00 01 01 00 00 01 00 01 01 00 01 01 00 01 01 00 01 01 00 00 01 00 00 01 00 01 01 00 00 FF FF FF FF 01 04")
                    .WriteLong(client.Identifier);

                return outPacket.ToArray();
            }
        }

        public static byte[] WorldInformation(WorldServer world)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EOpcode.SMSG_WORLD_INFORMATION)
                    .WriteByte(world.Identifier)
                    .WriteString("Scania")
                    .WriteByte()
                    .WriteString("Welcome to SharpEnd.")
                    .WriteShort(100)
                    .WriteShort(100)
                    .WriteByte()
                    .WriteByte(1);

                for (byte i = 0; i < 1; i++)
                {
                    outPacket
                        .WriteString($"{"Scania"}-{i}")
                        .WriteInt()
                        .WriteByte(world.Identifier)
                        .WriteShort(i);
                }

                outPacket
                    .WriteShort()
                    .WriteInt()
                    .WriteByte();

                return outPacket.ToArray();
            }
        }

        public static byte[] WorldEnd()
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EOpcode.SMSG_WORLD_INFORMATION)
                    .WriteSByte(-1)
                    .WriteBoolean(false)
                    .WriteBoolean(false)
                    .WriteBoolean(false);

                return outPacket.ToArray();
            }
        }

        public static byte[] WorldStatus(short status)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EOpcode.SMSG_WORLD_STATUS)
                    .WriteShort(status);

                return outPacket.ToArray();
            }
        }

        public static byte[] PlayerList(byte count, DatabaseQuery query)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EOpcode.SMSG_PLAYER_LIST)
                    .WriteByte()
                    .WriteString("normal")
                    .WriteInt()
                    .WriteByte(1)
                    .WriteInt()
                    .WriteLong()
                    .WriteByte()
                    .WriteInt();

                outPacket.WriteByte(count);

                while (query.NextRow())
                {
                    AddPlayerEntry(outPacket, query);

                    outPacket
                        .WriteByte()
                        .WriteBoolean(false); // NOTE: Rankings
                }

                outPacket
                    .WriteByte(1)
                    .WriteByte()
                    .WriteInt(3)
                    .WriteInt()
                    .WriteInt(-1)
                    .WriteLong()
                    .WriteByte()
                    .WriteZero(5);


                return outPacket.ToArray();
            }
        }

        private static void AddPlayerEntry(OutPacket outPacket, DatabaseQuery query)
        {
            // NOTE: Statistics
            outPacket
                .WriteInt(query.Get<int>("identifier"))
                .WriteInt(query.Get<int>("identifier"))
                .WriteInt()
                .WriteString(query.Get<string>("name"), 13)
                .WriteByte(query.Get<byte>("gender"))
                .WriteByte(query.Get<byte>("skin"))
                .WriteInt(query.Get<int>("face"))
                .WriteInt(query.Get<int>("hair"))
                .WriteSByte(-1) // NOTE: nMixBaseHairColor
                .WriteSByte() // NOTE: nMixAddHairColor
                .WriteSByte() // NOTE: nMixHairBaseProb
                .WriteByte(query.Get<byte>("level"))
                .WriteUShort(query.Get<ushort>("job"))
                .WriteUShort(query.Get<ushort>("strength"))
                .WriteUShort(query.Get<ushort>("dexterity"))
                .WriteUShort(query.Get<ushort>("intelligence"))
                .WriteUShort(query.Get<ushort>("luck"))
                .WriteUInt(query.Get<uint>("health"))
                .WriteUInt(query.Get<uint>("max_health"))
                .WriteUInt(query.Get<uint>("mana"))
                .WriteUInt(query.Get<uint>("max_mana"))
                .WriteUShort(query.Get<ushort>("ability_points"))
                .WriteSByte()
                .WriteULong(query.Get<ulong>("experience"))
                .WriteInt(query.Get<int>("fame"))
                .WriteInt()
                .WriteInt()
                .WriteInt(query.Get<int>("map_identifier"))
                .WriteByte(query.Get<byte>("map_spawn"))
                .WriteInt() // NOTE: Unknown
                .WriteShort() // NOTE: Subcategory
                .WriteByte() // NOTE: Fatigue
                .WriteInt(); // NOTE: Date

            // NOTE: Traits
            {
                int count = 6;

                while (count-- > 0)
                {
                    outPacket.WriteInt();
                }

                count = 6;

                while (count-- > 0)
                {
                    outPacket.WriteShort();
                }
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

            // NOTE: Appereance
            outPacket
                .WriteByte(query.Get<byte>("gender"))
                .WriteByte(query.Get<byte>("skin"))
                .WriteInt(query.Get<int>("face"))
                .WriteInt(query.Get<ushort>("job"))
                .WriteBoolean(true)
                .WriteInt(query.Get<int>("hair"));

            // NOTE: Equipment layers
            outPacket
                .WriteSByte(-1) // NOTE: Shown layer
                .WriteSByte(-1) // NOTE: Hidden layer
                .WriteSByte(-1); // NOTE: Totem later

            outPacket
                .WriteInt() // NOTE: Cash weapon
                .WriteInt() // NOTE: Weapon
                .WriteInt() // NOTE: Offhand
                .WriteBoolean(false) // NOTE: Elf ears
                .WriteInt() // NOTE: Pet 1
                .WriteInt() // NOTE: Pet 2
                .WriteInt() // NOTE: Pet 3
                .WriteByte() // NOTE: Mixed hair color
                .WriteByte(); // NOTE: Mixed hair percent
        }

        public static byte[] PrivateServerAuth(int response)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EOpcode.SMSG_PRIVATE_SERVER_AUTH)
                    .WriteInt(response);

                return outPacket.ToArray();
            }
        }

        public static byte[] Start()
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EOpcode.SMSG_START)
                    .WriteBoolean(true);

                return outPacket.ToArray();
            }
        }

        public static byte[] AuthServer(bool enable)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EOpcode.SMSG_AUTH_SERVER)
                    .WriteBoolean(enable);

                return outPacket.ToArray();
            }
        }
    }
}
