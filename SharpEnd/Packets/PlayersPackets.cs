using SharpEnd.Drawing;
using SharpEnd.Network;

namespace SharpEnd.Packets
{
    internal static class PlayersPackets
    {
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
