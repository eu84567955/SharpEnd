using SharpEnd.Game.Data;
using SharpEnd.Game.Players;

namespace SharpEnd.Game.Maps
{
    public sealed class Map
    {
        private MapData m_data;
        private MapPlayers m_players;
        private MapFootholds m_footholds;
        private MapPortals m_portals;
        private MapDrops m_drops;
        private MapMobs m_mobs;
        private MapNpcs m_npcs;
        private MapReactors m_reactors;

        public Map(int mapID)
        {
            m_data = MapDataProvider.Instance.GetMapData(mapID);
            m_players = new MapPlayers(this);
            m_footholds = new MapFootholds(this);
            m_portals = new MapPortals(this);
            m_drops = new MapDrops(this);
            m_mobs = new MapMobs(this);
            m_npcs = new MapNpcs(this);
            m_reactors = new MapReactors(this);
        }

        public int ID { get { return m_data.ID; } }
        public int ReturnMap { get { return m_data.ReturnMap; } }
        public int ForcedMap { get { return m_data.ForcedMap; } }
        public MapPlayers Players { get { return m_players; } }
        public MapFootholds Footholds { get { return m_footholds; } }
        public MapPortals Portals { get { return m_portals; } }
        public MapDrops Drops { get { return m_drops; } }
        public MapMobs Mobs { get { return m_mobs; } }
        public MapNpcs Npcs { get { return m_npcs; } }
        public MapReactors Reactors { get { return m_reactors; } }

        // TODO: Refactor.
        private int m_objectIDs = 0;
        public int AssignObjectID()
        {
            return ++m_objectIDs;
        }

        public void Send(byte[] buffer, Player ignored = null)
        {
            foreach (Player player in Players)
            {
                if (player != ignored)
                {
                    //player.Send(buffer);
                }
            }
        }
    }
}
