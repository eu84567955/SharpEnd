using SharpEnd.Maps;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Servers;
using System.Collections.Generic;

namespace SharpEnd.Handlers
{
    internal static class NpcHandlers
    {
        [PacketHandler(EHeader.CMSG_NPC_ACTION)]
        public static void Action(Client client, InPacket inPacket)
        {
            var player = client.Player;

            int objectIdentifier = inPacket.ReadInt();

            Npc npc;

            try
            {
                npc = player.ControlledNpcs[objectIdentifier];
            }
            catch (KeyNotFoundException)
            {
                return;
            }

            int available = inPacket.Available;

            byte[] buffer = null;

            if (available == 6)
            {
                buffer = inPacket.ReadLeftoverBytes();
            }
            else if (available > 6)
            {
                buffer = inPacket.ReadBytes(available - 5);
            }

            MasterServer.Instance.Maps[player.Map].Send(NpcPackets.NpcAction(npc.ObjectIdentifier, buffer));
        }
    }
}
