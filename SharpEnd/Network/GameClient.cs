using SharpEnd.Packets;
using SharpEnd.Game.Players;
using SharpEnd.Security;
using System;
using System.Net;
using System.Net.Sockets;
using SharpEnd.Game;
using SharpEnd.Utility;
using System.Collections.Generic;

namespace SharpEnd.Network
{
    public sealed class GameClient
    {
        private static readonly byte[] riv = new byte[] { 0x52, 0x61, 0x6A, 0x61 };
        private static readonly byte[] siv = new byte[] { 0x6E, 0x52, 0x30, 0x58 };

        private long m_id;
        private byte m_world;
        private byte m_channel;
        private Account m_account;
        private Player m_player;

        private string m_host;
        private readonly Socket m_socket;

        private MapleCipher m_sendCipher;
        private MapleCipher m_recvCipher;

        private bool m_header;
        private int m_offset;
        private byte[] m_buffer;

        private object m_locker;
        private bool m_disposed;

        public GameClient(Socket socket)
        {
            m_id = -1; // TODO: Use the server to generate a RNG long.
            m_world = 0;
            m_channel = 0;
            m_account = null;
            m_player = null;

            m_host = (socket.RemoteEndPoint as IPEndPoint).Address.ToString();
            m_socket = socket;
            m_socket.NoDelay = true;
            m_socket.ReceiveBufferSize = 0xFFFF;
            m_socket.SendBufferSize = 0xFFFF;

            m_sendCipher = new MapleCipher(Application.Version.Version, siv, MapleCipher.TransformDirection.Encrypt);
            m_recvCipher = new MapleCipher(Application.Version.Version, riv, MapleCipher.TransformDirection.Decrypt);

            m_locker = new object();
            m_disposed = false;

            SendRaw(LoginPackets.Handshake(riv, siv));

            WaitForData(true, 4);
        }

        public long Id { get { return m_id; } }
        public byte World { get { return m_world; } set { m_world = value; } }
        public byte Channel { get { return m_channel; } set { m_channel = value; } }
        public Account Account { get { return m_account; } set { m_account = value; } }
        public Player Player { get { return m_player; } set { m_player = value; } }
        public string Host { get { return m_host; } }

        private void WaitForData(bool header, int size)
        {
            if (m_disposed)
            {
                return;
            }

            m_header = header;
            m_offset = 0;
            m_buffer = new byte[size];

            BeginRead(m_buffer.Length);
        }

        private void BeginRead(int size)
        {
            SocketError outError = SocketError.Success;

            m_socket.BeginReceive(m_buffer, m_offset, size, SocketFlags.None, out outError, ReadCallback, null);

            if (outError != SocketError.Success)
            {
                Close();
            }
        }

        private void ReadCallback(IAsyncResult iar)
        {
            if (m_disposed)
            {
                return;
            }

            SocketError error;

            int received = m_socket.EndReceive(iar, out error);

            if (received == 0 || error != SocketError.Success)
            {
                Close();

                return;
            }

            m_offset += received;

            if (m_offset == m_buffer.Length)
            {
                HandleStream();
            }
            else
            {
                BeginRead(m_buffer.Length - m_offset);
            }
        }

        private void HandleStream()
        {
            if (m_header)
            {
                int size = MapleCipher.GetPacketLength(m_buffer);

                if (size > m_socket.ReceiveBufferSize || !m_recvCipher.CheckServerPacket(m_buffer, 0))
                {
                    Close();

                    return;
                }

                WaitForData(false, size);
            }
            else
            {
                m_recvCipher.Transform(m_buffer);

                InPacket inPacket = new InPacket(m_buffer);

                PacketProcessor processor;

                try
                {
                    processor = HandlerStore.Instance[inPacket.Header];
                }
                catch (KeyNotFoundException)
                {
                    Log.Hex("Unhandled packet 0x{0:X4} from {1}: \n", inPacket.ToArray(), (ushort)inPacket.Header, m_host);

                    return;
                }

                try
                {
                    processor(this, inPacket);
                }
                catch (Exception e) // TODO: Differentiate between content exception (e.g. packet editing) and server exception.
                {
                    Log.Error("Unable to process packet from {0}: \n{1}", m_host, e.ToString());
                }

                WaitForData(true, 4);
            }
        }

        public void Send(params byte[][] packets)
        {
            if (m_disposed)
            {
                return;
            }

            lock (m_locker)
            {
                int length = 0;
                int offset = 0;

                foreach (byte[] buffer in packets)
                {
                    length += 4;                //header length
                    length += buffer.Length;    //packet length
                }

                byte[] finalPacket = new byte[length];

                foreach (byte[] buffer in packets)
                {
                    m_sendCipher.GetHeaderToClient(finalPacket, offset, buffer.Length);

                    offset += 4; //header space

                    m_sendCipher.Transform(buffer);
                    Buffer.BlockCopy(buffer, 0, finalPacket, offset, buffer.Length);

                    offset += buffer.Length; //packet space
                }

                SendRaw(finalPacket); //send the giant crypted packet
            }
        }

        public void SendRaw(byte[] packet)
        {
            if (m_disposed)
            {
                return;
            }

            int offset = 0;

            while (offset < packet.Length)
            {
                SocketError outError = SocketError.Success;

                int sent = m_socket.Send(packet, offset, packet.Length - offset, SocketFlags.None, out outError);

                if (sent == 0 || outError != SocketError.Success)
                {
                    Close();

                    return;
                }

                offset += sent;
            }
        }

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                m_disposed = true;

                m_socket.Shutdown(SocketShutdown.Both);
                m_socket.Close();

                if (m_sendCipher != null)
                    m_sendCipher.Dispose();
                if (m_recvCipher != null)
                    m_recvCipher.Dispose();

                m_offset = 0;
                m_buffer = null;

                m_sendCipher = null;
                m_recvCipher = null;

                m_account?.Save();
                m_player?.Save();
            }
        }
    }

    public sealed class MapleCipher : IDisposable
    {
        private static readonly byte[] sShuffle = new byte[256]
        {
            0xEC, 0x3F, 0x77, 0xA4, 0x45, 0xD0, 0x71, 0xBF, 0xB7, 0x98, 0x20, 0xFC, 0x4B, 0xE9, 0xB3, 0xE1,
            0x5C, 0x22, 0xF7, 0x0C, 0x44, 0x1B, 0x81, 0xBD, 0x63, 0x8D, 0xD4, 0xC3, 0xF2, 0x10, 0x19, 0xE0,
            0xFB, 0xA1, 0x6E, 0x66, 0xEA, 0xAE, 0xD6, 0xCE, 0x06, 0x18, 0x4E, 0xEB, 0x78, 0x95, 0xDB, 0xBA,
            0xB6, 0x42, 0x7A, 0x2A, 0x83, 0x0B, 0x54, 0x67, 0x6D, 0xE8, 0x65, 0xE7, 0x2F, 0x07, 0xF3, 0xAA,
            0x27, 0x7B, 0x85, 0xB0, 0x26, 0xFD, 0x8B, 0xA9, 0xFA, 0xBE, 0xA8, 0xD7, 0xCB, 0xCC, 0x92, 0xDA,
            0xF9, 0x93, 0x60, 0x2D, 0xDD, 0xD2, 0xA2, 0x9B, 0x39, 0x5F, 0x82, 0x21, 0x4C, 0x69, 0xF8, 0x31,
            0x87, 0xEE, 0x8E, 0xAD, 0x8C, 0x6A, 0xBC, 0xB5, 0x6B, 0x59, 0x13, 0xF1, 0x04, 0x00, 0xF6, 0x5A,
            0x35, 0x79, 0x48, 0x8F, 0x15, 0xCD, 0x97, 0x57, 0x12, 0x3E, 0x37, 0xFF, 0x9D, 0x4F, 0x51, 0xF5,
            0xA3, 0x70, 0xBB, 0x14, 0x75, 0xC2, 0xB8, 0x72, 0xC0, 0xED, 0x7D, 0x68, 0xC9, 0x2E, 0x0D, 0x62,
            0x46, 0x17, 0x11, 0x4D, 0x6C, 0xC4, 0x7E, 0x53, 0xC1, 0x25, 0xC7, 0x9A, 0x1C, 0x88, 0x58, 0x2C,
            0x89, 0xDC, 0x02, 0x64, 0x40, 0x01, 0x5D, 0x38, 0xA5, 0xE2, 0xAF, 0x55, 0xD5, 0xEF, 0x1A, 0x7C,
            0xA7, 0x5B, 0xA6, 0x6F, 0x86, 0x9F, 0x73, 0xE6, 0x0A, 0xDE, 0x2B, 0x99, 0x4A, 0x47, 0x9C, 0xDF,
            0x09, 0x76, 0x9E, 0x30, 0x0E, 0xE4, 0xB2, 0x94, 0xA0, 0x3B, 0x34, 0x1D, 0x28, 0x0F, 0x36, 0xE3,
            0x23, 0xB4, 0x03, 0xD8, 0x90, 0xC8, 0x3C, 0xFE, 0x5E, 0x32, 0x24, 0x50, 0x1F, 0x3A, 0x43, 0x8A,
            0x96, 0x41, 0x74, 0xAC, 0x52, 0x33, 0xF0, 0xD9, 0x29, 0x80, 0xB1, 0x16, 0xD3, 0xAB, 0x91, 0xB9,
            0x84, 0x7F, 0x61, 0x1E, 0xCF, 0xC5, 0xD1, 0x56, 0x3D, 0xCA, 0xF4, 0x05, 0xC6, 0xE5, 0x08, 0x49
        };

        public enum TransformDirection : byte
        {
            Encrypt,
            Decrypt
        }

        private ushort m_majorVersion;
        private byte[] m_IV;
        private TransformDirection m_direction;

        private Action<byte[]> m_transformer;

        public ushort MajorVersion
        {
            get { return m_majorVersion; }
        }
        public TransformDirection TransformationDirection
        {
            get { return m_direction; }
        }

        public MapleCipher(ushort majorVersion, byte[] IV, TransformDirection transformDirection)
        {
            m_majorVersion = majorVersion;

            m_IV = new byte[4];
            Buffer.BlockCopy(IV, 0, m_IV, 0, 4);

            m_direction = transformDirection;

            m_transformer = m_direction == TransformDirection.Encrypt ? new Action<byte[]>(EncryptTransform) : new Action<byte[]>(DecryptTransform);
        }

        public void Transform(byte[] data)
        {
            m_transformer(data);
            byte[] newIV = Shuffle(m_IV);
            m_IV = newIV;
        }

        private void EncryptTransform(byte[] data) // TODO: If version is greater than .148.
        {
            //CustomEncryption.Encrypt(data);
            AesCryptograph.Transform(data, m_IV);
        }

        private void DecryptTransform(byte[] data) // TODO: If version is greater than .148.
        {
            AesCryptograph.Transform(data, m_IV);
            //CustomEncryption.Decrypt(data);
        }

        public void GetHeaderToClient(byte[] packet, int offset, int size)
        {
            int a = m_IV[3] * 0x100 + m_IV[2];
            a ^= -(m_majorVersion + 1);
            int b = a ^ size;
            packet[offset] = (byte)(a % 0x100);
            packet[offset + 1] = (byte)((a - packet[0]) / 0x100);
            packet[offset + 2] = (byte)(b ^ 0x100);
            packet[offset + 3] = (byte)((b - packet[2]) / 0x100);
        }

        public void GetHeaderToServer(byte[] packet, int offset, int size)
        {
            int a = m_IV[3] * 0x100 + m_IV[2];
            a = a ^ (m_majorVersion);
            int b = a ^ size;
            packet[offset] = (byte)(a % 0x100);
            packet[offset + 1] = (byte)(a / 0x100);
            packet[offset + 2] = (byte)(b % 0x100);
            packet[offset + 3] = (byte)(b / 0x100);
        }

        public static int GetPacketLength(byte[] packetHeader)
        {
            return (packetHeader[0] + (packetHeader[1] << 8)) ^ (packetHeader[2] + (packetHeader[3] << 8));
        }

        public bool CheckServerPacket(byte[] packet, int offset)
        {
            int a = packet[offset] ^ m_IV[2];
            int b = m_majorVersion;
            int c = packet[offset + 1] ^ m_IV[3];
            int d = m_majorVersion >> 8;
            return (a == b && c == d);
        }

        private static byte[] Shuffle(byte[] IV)
        {
            byte[] start = new byte[4] { 0xF2, 0x53, 0x50, 0xC6 };
            for (int i = 0; i < 4; i++)
            {

                byte inputByte = IV[i];

                byte a = start[1];
                byte b = a;
                uint c, d;
                b = sShuffle[b];
                b -= inputByte;
                start[0] += b;
                b = start[2];
                b ^= sShuffle[inputByte];
                a -= b;
                start[1] = a;
                a = start[3];
                b = a;
                a -= start[0];
                b = sShuffle[b];
                b += inputByte;
                b ^= start[2];
                start[2] = b;
                a += sShuffle[inputByte];
                start[3] = a;

                c = (uint)(start[0] + start[1] * 0x100 + start[2] * 0x10000 + start[3] * 0x1000000);
                d = c;
                c >>= 0x1D;
                d <<= 0x03;
                c |= d;
                start[0] = (byte)(c % 0x100);
                c /= 0x100;
                start[1] = (byte)(c % 0x100);
                c /= 0x100;
                start[2] = (byte)(c % 0x100);
                start[3] = (byte)(c / 0x100);
            }
            return start;
        }

        public void Dispose()
        {
            m_transformer = null;
            m_IV = null;
        }
    }

}
