using SharpEnd.Players;
using System;

namespace SharpEnd.Scripting
{
    internal sealed class PortalScript : ScriptBase
    {
        private int m_identifier;

        public PortalScript(Player player, string name)
            : base(player, string.Format("scripts/portals/{0}.py", name))
        {

        }
    }
}
