using SharpEnd.Data;
using SharpEnd.Drawing;
using System.Collections.Generic;

namespace SharpEnd.Maps
{
    internal sealed class MapFootholds : List<FootholdData>
    {
        public Map Map { get; private set; }

        public MapFootholds(Map map)
            : base()
        {
            Map = map;
        }

        public Point FindBelow(Point point)
        {
            return point;
        }
    }
}
