using System.IO;
using System.Text;

namespace SharpEnd.Network
{
    internal sealed class OutPacket : BasePacket
    {
        private BinaryWriter m_writer;

        public OutPacket()
        {
            m_stream = new MemoryStream();

            m_writer = new BinaryWriter(m_stream, Encoding.ASCII);
        }

        public OutPacket WriteOpcode(EOpcode opcode)
        {
            Header = opcode;

            WriteUShort((ushort)Header);

            return this;
        }

        public OutPacket WriteBytes(byte[] value)
        {
            m_writer.Write(value);

            return this;
        }

        public OutPacket WriteByte(byte value = 0)
        {
            m_writer.Write(value);

            return this;
        }

        public OutPacket WriteSByte(sbyte value = 0)
        {
            m_writer.Write(value);

            return this;
        }

        public OutPacket WriteBoolean(bool value)
        {
            m_writer.Write(value);

            return this;
        }

        public OutPacket WriteShort(short value = 0)
        {
            m_writer.Write(value);

            return this;
        }

        public OutPacket WriteUShort(ushort value = 0)
        {
            m_writer.Write(value);

            return this;
        }

        public OutPacket WriteInt(int value = 0)
        {
            m_writer.Write(value);

            return this;
        }

        public OutPacket WriteUInt(uint value = 0)
        {
            m_writer.Write(value);

            return this;
        }

        public OutPacket WriteLong(long value = 0)
        {
            m_writer.Write(value);

            return this;
        }

        public OutPacket WriteULong(ulong value = 0)
        {
            m_writer.Write(value);

            return this;
        }

        public OutPacket WriteString(string value, int count = 0)
        {
            if (count == 0)
            {
                m_writer.Write((ushort)value.Length);
                m_writer.Write(value);
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    if (i < value.Length)
                    {
                        m_writer.Write(value[i]);
                    }
                    else
                    {
                        WriteByte();
                    }
                }
            }

            return this;
        }
    }
}
