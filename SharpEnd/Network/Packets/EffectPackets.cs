namespace SharpEnd.Network.Packets
{
    internal static class EffectPackets
    {
        // NOTE: Located in Map.wz/Effect.img.
        public static byte[] ShowMapEffect(string text)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_FIELD_EFFECT)
                    .WriteByte((byte)EFieldEffect.FieldEffect_Screen_Delayed)
                    .WriteString(text)
                    .WriteInt(); // NOTE: Unknown.

                return outPacket.ToArray();
            }
        }

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
