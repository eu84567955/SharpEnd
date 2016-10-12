using SharpEnd.Packets;
using SharpEnd.Players;
using System;

namespace SharpEnd.Scripting
{
    internal sealed class NpcScript : ScriptBase
    {
        private int m_identifier;

        public NpcScript(Player player, string name, int identifier)
            : base(player, string.Format("scripts/npcs/{0}.py", name))
        {
            m_identifier = identifier;

            SetVariable("sendOk", new Action<string>(SendOk));
        }

        private void SendOk(string text)
        {
            m_player.Send(NpcPackets.NpcOkDialog(m_identifier, text));
        }
    }
}
