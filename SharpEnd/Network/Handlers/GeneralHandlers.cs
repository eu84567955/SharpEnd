using SharpEnd.Network;
using SharpEnd.Packets;
using System;

namespace SharpEnd.Handlers
{
    internal static class GeneralHandlers
    {
        [PacketHandler(EHeader.CMSG_PRIVATE_SERVER_AUTH)]
        public static void PrivateServerAuthHandler(Client client, InPacket inPacket)
        {
            int request = inPacket.ReadInt();
            int response = request ^ (int)EHeader.SMSG_PRIVATE_SERVER_AUTH;

            client.Send(LoginPackets.PrivateServerAuth(response));
        }

        [PacketHandler(EHeader.CMSG_CLIENT_ERROR)]
        public static void ClientErrorHandler(Client client, InPacket inPacket)
        {
            ushort type = inPacket.ReadUShort();
            int error = inPacket.ReadInt();

            if (error == 38)
            {
                inPacket.ReadUShort(); // NOTE: Packet length
                inPacket.Skip(4); // NOTE: Unknown
                ushort header = inPacket.ReadUShort();
                inPacket.ReadLeftoverBytes(); // NOTE: Packet data

                if (Enum.IsDefined(typeof(EHeader), inPacket.Header))
                {
                    Log.Error("Client {0} crashed with error 38 by header {1}.", client.Host, ((EHeader)header).ToString());
                }
                else
                {
                    Log.Error("Client {0} crashed with error 38 by header 0x{1:X4}.", client.Host, header);
                }
            }
        }

        [PacketHandler(EHeader.CMSG_BUTTON_PRESS)]
        public static void ButtonPressHandler(Client client, InPacket inPacket)
        {
            // TODO: Handle button presses (useful against auto-clickers)
        }
    }
}
