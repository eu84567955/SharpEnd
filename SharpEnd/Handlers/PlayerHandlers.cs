using SharpEnd.Network;

namespace SharpEnd.Handlers
{
    internal static class PlayerHandlers
    {
        [PacketHandler(EOpcode.CMSG_PLAYER_MOVE)]
        public static void PlayerMove(Client client, InPacket inPacket)
        {
            var player = client.Player;

            byte portalCount = inPacket.ReadByte();

            if (player.PortalCount != portalCount)
            {
                return;
            }

            inPacket.Skip(21);

            if (!player.ParseMovement(inPacket))
            {
                return;
            }

            player.Notify($"X: {player.Position.X}, Y: {player.Position.Y}, Foothold: {player.Foothold}, Stance: {player.Stance}");

            // TODO: Rewind the packet & broadcast to map
        }
    }
}
