using SharpEnd.Game.Maps;
using SharpEnd.Network.Packets;
using SharpEnd.Players;
using System;

namespace SharpEnd.Scripting
{
    internal sealed class MapScript : ScriptBase
    {
        private Map m_map;

        public MapScript(Player player, Map map, bool initial = false)
            : base(player, string.Format("scripts/maps/{0}/{1}.py", initial ? "initial_entry" : "entry", initial ? map.InitialEntryScript : map.EntryScript))
        {
            m_map = map;

            SetEnvironmentVariables();
            SetMapVariables();
        }

        private void SetEnvironmentVariables()
        {
            // TODO: Set environment variables like map identifier, name, etcetera.
        }

        private void SetMapVariables()
        {
            Set("showMapEffect", new Action<string>((text) => m_player.Send(EffectPackets.ShowMapEffect(text))));
        }
    }
}
