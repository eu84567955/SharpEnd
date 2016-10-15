using SharpEnd.Game.Maps;
using SharpEnd.Network.Packets;
using SharpEnd.Players;
using System;

namespace SharpEnd.Scripting
{
    internal sealed class PortalScript : ScriptBase
    {
        private Portal m_portal;

        public PortalScript(Player player, Portal portal)
            : base(player, string.Format("scripts/portals/{0}.py", portal.Script))
        {
            m_portal = portal;

            SetEnvironmentVariables();
            SetPortalVariables();
        }

        private void SetEnvironmentVariables()
        {
            Set("system_portal_name", m_portal.Label);
        }

        private void SetPortalVariables()
        {
            Set("playPortalSe", new Action(() => m_player.Send(EffectPackets.PlayPortalSoundEffect())));
        }
    }
}
