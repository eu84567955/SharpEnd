using SharpEnd.Data;
using System.Collections.Generic;

namespace SharpEnd.Maps
{
    internal sealed class MapSeats : List<SeatData>
    {
        public Map Map { get; private set; }

        public MapSeats(Map map)
            : base()
        {
            Map = map;
        }
    }
}
