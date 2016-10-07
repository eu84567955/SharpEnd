using SharpEnd.Players;

namespace SharpEnd.Maps
{
    internal sealed class Map
    {
        private int sObjectIdentifiers = 0;

        public int Identifier { get; private set; }

        public MapFootholds Footholds { get; private set; }
        public MapPlayers Players { get; private set; }
        public MapPortals Portals { get; private set; }
        public MapMobs Mobs { get; private set; }
        public MapNpcs Npcs { get; private set; }
        public MapReactors Reactors { get; private set; }
        public MapDrops Drops { get; private set; }

        public Map(int identifier)
        {
            Identifier = identifier;

            Players = new MapPlayers(this);
            Footholds = new MapFootholds(this);
            Portals = new MapPortals(this);
            Mobs = new MapMobs(this);
            Npcs = new MapNpcs(this);
            Reactors = new MapReactors(this);
            Drops = new MapDrops(this);
        }

        public int AssignObjectIdentifier()
        {
            return ++sObjectIdentifiers;
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
