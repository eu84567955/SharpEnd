using SharpEnd.Data;
using System.Collections.Generic;

namespace SharpEnd.Maps
{
    internal sealed class MapPortals : List<PortalData>
    {
        public Map Map { get; private set; }

        public MapPortals(Map map)
            : base()
        {
            Map = map;
        }
    }
}
