namespace SharpEnd.Maps
{
    internal sealed class Map
    {
        public int Identifier { get; private set; }

        public MapPlayers Players { get; private set; }
        public MapPortals Portals { get; private set; }
        public MapMobs Mobs { get; private set; }
        public MapNpcs Npcs { get; private set; }
        public MapReactors Reactors { get; private set; }

        public Map(int identifier)
        {
            Identifier = identifier;

            Players = new MapPlayers(this);
            Portals = new MapPortals(this);
            Mobs = new MapMobs(this);
            Npcs = new MapNpcs(this);
            Reactors = new MapReactors(this);
        }
    }
}
