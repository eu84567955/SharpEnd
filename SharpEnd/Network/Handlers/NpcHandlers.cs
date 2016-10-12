﻿using SharpEnd.Maps;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Scripting;
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

            if (File.Exists(string.Format("scripts/npcs/{0}.py", npc.Script)))
            {
                NpcScript script = new NpcScript(player, npc.Script, npc.Identifier);

                try
                {
                    script.Execute();
                }
                catch (Exception e)
                {
                    Log.Error("Error while executing NPC script '{0}': {1}", npc.Script, e.Message);
                }
            }
            else
            {
                Log.Warn("Unscripted NPC '{0}'.", npc.Script);
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

            byte[] data = inPacket.ReadLeftoverBytes();

            player.Map.Send(NpcPackets.NpcAction(npc.ObjectIdentifier, data));
        }
    }
}