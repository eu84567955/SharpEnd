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
            player.NpcScript = this;

            m_npc = npc;

            SetEnvrionmentVariables();
            SetNpcVariables();
        }

        private void SetEnvrionmentVariables()
        {

        }

        private void SetNpcVariables()
        {
            Set("addText", new Action<string>((text) => m_text += text));
            Set("sendOk", new Action(() => m_player.Send(NpcPackets.NpcOkDialog(m_npc.Identifier, m_text))));
        }
    }
}
