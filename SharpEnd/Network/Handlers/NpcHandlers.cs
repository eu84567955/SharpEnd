using SharpEnd.Game.Life;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Script;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Handlers
{
    internal static class NpcHandlers
    {
        [PacketHandler(EHeader.CMSG_NPC_CONVERSE)]
        public static void NpcConverseHandler(Client client, InPacket inPacket)
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

            if (npc.HasShop)
            {
                client.Send(NpcPackets.NpcShop(npc.Shop));
            }
            else if (npc.IsStorage)
            {
                // TODO: Show storage.
            }
            else if (!string.IsNullOrEmpty(npc.Script))
            {
                if (File.Exists(string.Format("scripts/npcs/{0}.py", npc.Script)))
                {
                    NpcScript script = new NpcScript(player, npc);

                    try
                    {
                        script.Execute();
                    }
                    catch (Exception e)
                    {
                        Log.Error("Error while executing Npc script '{0}': \n{1}", npc.Script, e.Message);
                    }
                }
                else
                {
                    Log.Warn("Missing Npc script '{0}'.", npc.Script);
                }
            }
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

            if (npc.Controller == player)
            {
                byte a = inPacket.ReadByte();
                byte b = inPacket.ReadByte();

                //player.Map.Send(NpcPackets.NpcAction(npc.ObjectIdentifier, a, b));
            }
        }
    }
}
