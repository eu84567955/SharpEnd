using SharpEnd.Drawing;
using SharpEnd.Network;
using SharpEnd.Players;

namespace SharpEnd.Packets
{
    internal static class PlayersPackets
    {
        public static byte[] PlayerDetails(Player player, bool self)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_PLAYER_DETAILS)
                    .WriteInt(player.Identifier)
                    .WriteBoolean(false) // NOTE: Unknown
                    .WriteByte(player.Stats.Level)
                    .WriteUShort(player.Stats.Job)
                    .WriteUShort(player.Stats.SubJob)
                    .WriteSByte(0x10) // NOTE: Battle rank
                    .WriteInt(player.Stats.Fame)
                    .WriteBoolean(false) // NOTE: Marriage
                    .WriteSByte() // NOTE: Professions
                    .WriteString("-") // NOTE: Guild name
                    .WriteString(string.Empty) // NOTE: Alliance name
                    .WriteHexString("FF 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 DD 1B 25 00 00 30 00 54 68 69 73 20 69 73 20 61 20 62 61 73 69 63 20 44 61 6D 61 67 65 20 53 6B 69 6E 2E 5C 72 5C 6E 5C 72 5C 6E 5C 72 5C 6E 5C 72 5C 6E 5C 72 5C 6E FF FF FF FF 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 82 9F BA 02 0B 00 43 72 65 61 74 69 6E 67 2E 2E 2E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1E 00 00 00 00 00 00 00");

                return outPacket.ToArray();
            }
        }

        public static byte[] PlayerChat(int playerIdentifier, string text, bool isGm, bool shout)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_PLAYER_CHAT)
                    .WriteInt(playerIdentifier)
                    .WriteBoolean(isGm)
                    .WriteString(text)
                    .WriteBoolean(shout)
                    .WriteByte()
                    .WriteSByte(-1); // NOTE: World identifier for cross-world party quests

                return outPacket.ToArray();
            }
        }

        public static byte[] PlayerMove(int playerIdentifier, Point position, byte[] buffer)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_PLAYER_MOVE)
                    .WriteInt(playerIdentifier)
                    .WriteInt() // NOTE: Unknown
                    .WritePoint(position)
                    .WriteShort() // NOTE: Unknown
                    .WriteShort(); // NOTE: Unknown

                if (buffer != null)
                {
                    outPacket.WriteBytes(buffer);
                }

                return outPacket.ToArray();
            }
        }
    }
}
