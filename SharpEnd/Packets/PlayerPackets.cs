using SharpEnd.Network;
using SharpEnd.Players;

namespace SharpEnd.Packets
{
    internal static class PlayerPackets
    {
        public static byte[] PlayerSpawn(Player player)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EOpcode.SMSG_PLAYER_SPAWN)
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
                    .WriteByte(player.Stance)
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
    }
}
