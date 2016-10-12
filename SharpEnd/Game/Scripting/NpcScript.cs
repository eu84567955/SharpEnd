using SharpEnd.Packets;
using SharpEnd.Players;
using System;

namespace SharpEnd.Scripting
{
    internal sealed class NpcScript : ScriptBase
    {
        private int m_identifier;
        private string m_text;

        public NpcScript(Player player, string name, int identifier)
            : base(player, string.Format("scripts/npcs/{0}.py", name))
        {
            m_identifier = identifier;

            Expose("addText", new Action<string>(AddText));
            Expose("sendOk", new Action(SendOk));
        }

        private void AddText(string text)
        {
            m_text += text;
        }

        private void SendOk()
        {
            m_player.Send(NpcPackets.NpcOkDialog(m_identifier, m_text));
        }
    }
}
