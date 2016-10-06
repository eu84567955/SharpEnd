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
                    .WriteHexString("FF 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 D7 AE 00 00 0F 00 50 72 6F 66 69 6C 65 20 70 69 63 74 75 72 65 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1E 00 00 00 00 00 00 00");

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

        public static byte[] PlayerMove(int playerIdentifier, Point origin, byte[] buffer)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_PLAYER_MOVE)
                    .WriteInt(playerIdentifier)
                    .WriteInt() // NOTE: Unknown
                    .WritePoint(origin)
                    .WriteShort() // NOTE: Unknown
                    .WriteShort() // NOTE: Unknown
                    .WriteBytes(buffer);

                return outPacket.ToArray();
            }
        }
    }
}
