using SharpEnd.Maps;
using SharpEnd.Network;
using SharpEnd.Packets;
using System.Collections.Generic;

namespace SharpEnd.Handlers
{
    internal static class NpcHandlers
    {
        [PacketHandler(EHeader.CMSG_NPC_CONVERSE)]
        public static void NpcConverseHandler(Client client, InPacket inPacket)
        {

        }

        [PacketHandler(EHeader.CMSG_NPC_ACTION)]
        public static void ActionHandler(Client client, InPacket inPacket)
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

            byte[] data = inPacket.ReadLeftoverBytes();

            player.Map.Send(NpcPackets.NpcAction(npc.ObjectIdentifier, data));
        }
    }
}
