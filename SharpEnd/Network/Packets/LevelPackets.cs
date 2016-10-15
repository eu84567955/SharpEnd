namespace SharpEnd.Network.Packets
{
    internal static class LevelPackets
    {
        public static byte[] ShowExperience(uint amount, bool white, bool inChat)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_SHOW_LOG)
                    .WriteByte(4)
                    .WriteBoolean(white)
                    .WriteUInt(amount)
                    .WriteBoolean(inChat)
                    .WriteHexString("00 00 08 00 00 00 00 00 00 00 00 00"); // TODO: Figure these out.

                return outPacket.ToArray();
            }
        }
    }
}
