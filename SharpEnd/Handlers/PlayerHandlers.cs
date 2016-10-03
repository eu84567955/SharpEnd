using SharpEnd.Data;
using SharpEnd.Network;
using SharpEnd.Servers;

namespace SharpEnd.Handlers
{
    internal static class PlayerHandlers
    {
        [PacketHandler(EHeader.CMSG_CHANGE_MAP)]
        public static void ChangeMap(Client client, InPacket inPacket)
        {
            var player = client.Player;

            byte portalCount = inPacket.ReadByte();

            if (portalCount != player.PortalCount)
            {
                return;
            }

            int mode = inPacket.ReadInt();

            switch (mode)
            {
                case 0:
                    {
                        if (!player.Stats.IsAlive())
                        {
                            inPacket.ReadString();
                            inPacket.ReadByte();
                            bool wheel = inPacket.ReadBoolean();

                            if (wheel)
                            {

                            }

                            player.AcceptDeath(wheel);
                        }
                    }
                    break;

                case -1:
                    {
                        string label = inPacket.ReadString();

                        PortalData portal = MasterServer.Instance.Maps[player.Map].Portals[label];

                        if (portal == null)
                        {
                            return;
                        }

                        PortalData destinationPortal = MasterServer.Instance.Maps[portal.DestinationMap].Portals[portal.DestinationLabel];

                        player.SetMap(portal.DestinationMap, destinationPortal);
                    }
                    break;

                default:
                    {

                    }
                    break;
            }
        }

        [PacketHandler(EHeader.CMSG_PLAYER_MOVE)]
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

        [PacketHandler(EHeader.CMSG_PLAYER_CHAT)]
        public static void PlayerChat(Client client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.ReadInt(); // NOTE: Ticks
            string text = inPacket.ReadString();
            bool shout = inPacket.ReadBoolean();

            if (!MasterServer.Instance.Commands.Execute(player, text))
            {

            }
        }

        [PacketHandler(EHeader.CMSG_PLAYER_EMOTE)]
        public static void PlayerEmote(Client client, InPacket inPacket)
        {

        }
    }
}
