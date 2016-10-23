using SharpEnd.Drawing;
using SharpEnd.Game.Life;
using SharpEnd.Game.Scripting;
using SharpEnd.Network;
using System.Collections.Generic;

namespace SharpEnd.Handlers
{
    public static class NpcHandlers
    {
        [PacketHandler(EHeader.CMSG_NPC_CONVERSE)]
        public static void NpcConverseHandler(GameClient client, InPacket inPacket)
        {
            var player = client.Player;

            int objectID = inPacket.ReadInt();

            Npc npc;

            try
            {
                npc = player.Map.Npcs[objectID];
            }
            catch (KeyNotFoundException)
            {
                return;
            }

            Point playerPosition = inPacket.ReadPoint();

            // TODO: Validate player position.
            // TODO: Check npc distance relative to the player.

            NpcScriptManager.Instance.RunScript(client, npc);
        }

        [PacketHandler(EHeader.CMSG_NPC_ACTION)]
        public static void ActionHandler(GameClient client, InPacket inPacket)
        {
            // TODO: Handler for this.
        }
    }
}
