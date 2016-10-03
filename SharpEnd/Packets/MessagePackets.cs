using SharpEnd.Network;

namespace SharpEnd.Packets
{
    internal static class MessagePackets
    {
        public static byte[] Notification(string text, EMessageType type)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EOpcode.SMSG_NOTIFICATION)
                    .WriteByte((byte)type);

                if (type == EMessageType.Header)
                {
                    outPacket.WriteBoolean(!string.IsNullOrEmpty(text));
                }

                outPacket.WriteString(text);

                if (type == EMessageType.Blue)
                {
                    outPacket.WriteInt();
                }

                return outPacket.ToArray();
            }
        }
    }
}
