using SharpEnd.Network;
using SharpEnd.Packets;

namespace SharpEnd.Handlers
{
    internal static class GeneralHandlers
    {
        [PacketHandler(EHeader.CMSG_PRIVATE_SERVER_AUTH)]
        public static void PrivateServerAuth(Client client, InPacket inPacket)
        {
            int request = inPacket.ReadInt();
            int response = request ^ (int)EHeader.SMSG_PRIVATE_SERVER_AUTH;

            client.Send(LoginPackets.PrivateServerAuth(response));
        }
    }
}
