using SharpEnd.Network;
using SharpEnd.Packets;

namespace SharpEnd.Handlers
{
    internal static class GeneralHandler
    {
        [PacketHandler(EOpcode.CMSG_PRIVATE_SERVER_AUTH)]
        public static void PrivateServerAuth(Client client, InPacket inPacket)
        {
            int request = inPacket.ReadInt();
            int response = request ^ (int)EOpcode.SMSG_PRIVATE_SERVER_AUTH;

            client.Send(LoginPackets.PrivateServerAuth(response));
        }
    }
}
