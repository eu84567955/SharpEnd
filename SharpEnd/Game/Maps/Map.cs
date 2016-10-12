using SharpEnd.Players;
using SharpEnd.Utility;

namespace SharpEnd.Maps
{
    internal sealed class Map
    {
        public int Identifier { get; private set; }

        public MapFootholds Footholds { get; private set; }
        public MapPlayers Players { get; private set; }
        public MapPortals Portals { get; private set; }
        public MapSpawnPoints SpawnPoints { get; private set; }
        public MapMobs Mobs { get; private set; }
        public MapNpcs Npcs { get; private set; }
        public MapReactors Reactors { get; private set; }
        public MapDrops Drops { get; private set; }
        public MapSeats Seats { get; private set; }

        private AutoIncrement m_objectIncrement;

        public Map(int identifier)
        {
            Identifier = identifier;

            Players = new MapPlayers(this);
            Footholds = new MapFootholds(this);
            Portals = new MapPortals(this);
            SpawnPoints = new MapSpawnPoints(this);
            Mobs = new MapMobs(this);
            Npcs = new MapNpcs(this);
            Reactors = new MapReactors(this);
            Drops = new MapDrops(this);
            Seats = new MapSeats(this);

            m_objectIncrement = new AutoIncrement(1);
        }

        public int AssignObjectIdentifier()
        {
            return m_objectIncrement.Next;
        }

        public void Send(byte[] buffer, Player ignored = null)
        {
            foreach (Player player in Players.Values)
            {
                if (player != ignored)
                {
                    player.Send(buffer);
                }
            }
        }
    }
}
