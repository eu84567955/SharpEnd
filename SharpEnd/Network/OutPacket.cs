using SharpEnd.Drawing;
using System;
using System.IO;
using System.Text;

namespace SharpEnd.Network
{
    public sealed class OutPacket : BasePacket
    {
        private BinaryWriter m_writer;

        public OutPacket()
        {
            m_stream = new MemoryStream();

            m_writer = new BinaryWriter(m_stream, Encoding.ASCII);
        }

        public OutPacket WriteHeader(EHeader header)
        {
            Header = header;

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

        public OutPacket WriteDateTime(DateTime value)
        {
            m_writer.Write(value.ToFileTimeUtc());

            return this;
        }

        public OutPacket WriteString(string value, int count = 0)
        {
            if (count == 0)
            {
                m_writer.Write((ushort)value.Length);

                foreach (char c in value)
                {
                    WriteByte((byte)c);
                }
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

        public OutPacket WriteZero(int count)
        {
            while (count-- > 0)
            {
                WriteByte();
            }

            return this;
        }

        public OutPacket WriteHexString(string value)
        {
            value = value.Replace(" ", "");

            if (value.Length % 2 != 0)
            {
                throw new ArgumentException("Size");
            }

            for (int i = 0; i < value.Length; i += 2)
            {
                WriteByte(byte.Parse(value.Substring(i, 2), System.Globalization.NumberStyles.HexNumber));
            }

            return this;
        }

        public OutPacket WritePoint(Point value)
        {
            WriteShort(value.X);
            WriteShort(value.Y);

            return this;
        }

        public void SetUInt(int offset, uint value)
        {
            int tOffset = Position;
            Position = offset;
            WriteUInt(value);
            Position = tOffset;
        }
    }
}
