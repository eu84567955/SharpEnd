using SharpEnd.Players;
using System.Collections.Generic;

namespace SharpEnd.Maps
{
    internal sealed class MapPlayers : List<Player>
    {
        public Map Map { get; private set; }

        public MapPlayers(Map map)
            : base()
        {
            Map = map;
        }

        public new void Add(Player player)
        {

        }

        public new void Remove(Player player)
        {

        }
    }
}
