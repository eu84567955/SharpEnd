using SharpEnd.Network;

namespace SharpEnd.Extensions
{
    public static class PacketExtensions
    {
        public static void WriteFlaggedValue(this OutPacket outPacket, uint flag, sbyte value, ref uint output)
        {
            if (value == 0)
            {
                return;
            }

            outPacket.WriteSByte(value);

            output |= flag;
        }

        public static void WriteFlaggedValue(this OutPacket outPacket, uint flag, byte value, ref uint output)
        {
            if (value == 0)
            {
                return;
            }

            outPacket.WriteByte(value);

            output |= flag;
        }

        public static void WriteFlaggedValue(this OutPacket outPacket, uint flag, short value, ref uint output)
        {
            if (value == 0)
            {
                return;
            }

            outPacket.WriteShort(value);

            output |= flag;
        }

        public static void WriteFlaggedValue(this OutPacket outPacket, uint flag, ushort value, ref uint output)
        {
            if (value == 0)
            {
                return;
            }

            outPacket.WriteUShort(value);

            output |= flag;
        }

        public static void WriteFlaggedValue(this OutPacket outPacket, uint flag, int value, ref uint output)
        {
            if (value == 0)
            {
                return;
            }

            outPacket.WriteInt(value);

            output |= flag;
        }

        public static void WriteFlaggedValue(this OutPacket outPacket, uint flag, uint value, ref uint output)
        {
            if (value == 0)
            {
                return;
            }

            outPacket.WriteUInt(value);

            output |= flag;
        }

        public static void WriteFlaggedValue(this OutPacket outPacket, uint flag, long value, ref uint output)
        {
            if (value == 0)
            {
                return;
            }

            outPacket.WriteLong(value);

            output |= flag;
        }

        public static void WriteFlaggedValue(this OutPacket outPacket, uint flag, ulong value, ref uint output)
        {
            if (value == 0)
            {
                return;
            }

            outPacket.WriteULong(value);

            output |= flag;
        }
    }
}
