using SharpEnd.Game.Maps;
using SharpEnd.Packets;
using SharpEnd.Players;
using System;

namespace SharpEnd.Scripting
{
    internal sealed class NpcScript : ScriptBase
    {
        private Npc m_npc;
        private string m_text;

        public NpcScript(Player player, Npc npc)
            : base(player, string.Format("scripts/npcs/{0}.py", npc.Script))
        {
            m_npc = npc;

            Expose("addText", new Action<string>(AddText));
            Expose("sendOk", new Action(SendOk));
        }

        private void AddText(string text)
        {
            m_text += text;
        }

        private void SendOk()
        {
            m_player.Send(NpcPackets.NpcOkDialog(m_npc.Identifier, m_text));
        }
    }
}
