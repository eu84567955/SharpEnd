using SharpEnd.Game.Players;
using System.Collections.Generic;

namespace SharpEnd.Network
{
    // TODO: Move else-where. Perhaps a network external namespace.
    public sealed class PlayerLog
    {
        private Dictionary<int, Player> m_idToPlayerMap;
        private Dictionary<string, Player> m_nameToPlayerMap;

        public int Count
        {
            get
            {
                return m_nameToPlayerMap.Count;
            }
        }

        public PlayerLog()
        {
            m_idToPlayerMap = new Dictionary<int, Player>();
            m_nameToPlayerMap = new Dictionary<string, Player>();
        }

        public void Add(Player player)
        {
            m_idToPlayerMap.Add(player.Id, player);
            m_nameToPlayerMap.Add(player.Name.ToLower(), player);
        }

        public void Remove(Player player)
        {
            m_idToPlayerMap.Remove(player.Id);
            m_nameToPlayerMap.Remove(player.Name.ToLower());
        }

        public void Clear()
        {
            m_nameToPlayerMap.Clear();
            m_idToPlayerMap.Clear();
        }

        public Player GetPlayer(int id)
        {
            return m_idToPlayerMap.GetOrDefault(id, null);
        }

        public Player GetPlayer(string name)
        {
            return m_nameToPlayerMap.GetOrDefault(name.ToLower(), null);
        }
    }
}
