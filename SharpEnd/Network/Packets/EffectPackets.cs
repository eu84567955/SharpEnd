namespace SharpEnd.Network.Packets
{
    internal static class EffectPackets
    {
        public static byte[] PlayPortalSoundEffect()
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_THEATRICS)
                    .WriteByte(12);

                return outPacket.ToArray();
            }
        }
    }
}
