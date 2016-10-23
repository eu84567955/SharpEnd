using SharpEnd.Game.Maps;
using SharpEnd.Game.Players;
using SharpEnd.Utility;

namespace SharpEnd.Game.Scripting
{
    public class ScriptMap
    {
        private Script m_script;
        private Map m_map;

        public ScriptMap(Script script, Map map)
        {
            m_script = script;
            m_map = map;
        }

        protected Map GetMap()
        {
            return m_map;
        }

        public int GetID()
        {
            return m_map.ID;
        }

        public int GetPlayerCount()
        {
            return m_map.Players.Count;
        }

        public void TransferPlayers(int mapID)
        {
            foreach (Player player in m_map.Players)
            {
                //player.SetMap(mapID);
            }
        }
    }
}
