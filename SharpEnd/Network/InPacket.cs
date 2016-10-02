using System.IO;
using System.Text;

namespace SharpEnd.Network
{
    internal sealed class InPacket : BasePacket
    {
        private BinaryReader m_reader;

        public InPacket(byte[] buffer)
        {
            m_stream = new MemoryStream(buffer, false);

            m_reader = new BinaryReader(m_stream, Encoding.ASCII);

            Header = (EOpcode)ReadUShort();
        }

        public byte[] ReadBytes(int count)
        {
            return m_reader.ReadBytes(count);
        }

        public byte ReadByte()
        {
            return m_reader.ReadByte();
        }

        public sbyte ReadSByte()
        {
            return m_reader.ReadSByte();
        }

        public bool ReadBoolean()
        {
            return m_reader.ReadBoolean();
        }

        public short ReadShort()
        {
            return m_reader.ReadInt16();
        }

        public ushort ReadUShort()
        {
            return m_reader.ReadUInt16();
        }

        public int ReadInt()
        {
            return m_reader.ReadInt32();
        }

        public uint ReadUInt()
        {
            return m_reader.ReadUInt32();
        }

        public long ReadLong()
        {
            return m_reader.ReadInt64();
        }

        public ulong ReadULong()
        {
            return m_reader.ReadUInt64();
        }

        public string ReadString(int count = -1)
        {
            if (count == -1)
            {
                count = ReadUShort();
            }

            char[] chars = m_reader.ReadChars(count);

            return new string(chars);
        }

        protected override void CustomDispose()
        {
            m_reader.Dispose();
        }
    }
}
