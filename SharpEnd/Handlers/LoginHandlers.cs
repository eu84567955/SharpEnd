using SharpEnd.Network;

namespace SharpEnd.Handlers
{
    internal static class LoginHandlers
    {
        [PacketHandler(EOpcode.CMSG_VALIDATE)]
        public static void Validate(Client client, InPacket inPacket)
        {

        }

        [PacketHandler(EOpcode.CMSG_AUTHENTICATION)]
        public static void Authentication(Client client, InPacket inPacket)
        {

        }
    }
}
