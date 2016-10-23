using SharpEnd.Network;

namespace SharpEnd.Packets
{
    public static class MessagePackets
    {
        public static byte[] Notification(string text, ENoticeType type)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_NOTIFICATION)
                    .WriteByte((byte)type);

                if (type == ENoticeType.Ticker)
                {
                    outPacket.WriteBoolean(!string.IsNullOrEmpty(text));
                }

                outPacket.WriteString(text);

                if (type == ENoticeType.Blue)
                {
                    outPacket.WriteInt();
                }

                return outPacket.ToArray();
            }
        }

        public static byte[] YellowMessage(string text)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_YELLOW_MESSAGE)
                    .WriteString(text);

                return outPacket.ToArray();
            }
        }
    }
}
