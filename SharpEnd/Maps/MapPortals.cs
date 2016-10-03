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

        public PortalData this[string label]
        {
            get
            {
                foreach (PortalData portal in this)
                {
                    if (portal.Label == label)
                    {
                        return portal;
                    }
                }

                return null;
            }
        }

        public PortalData GetSpawnPoint(sbyte portalIdentifier = -1)
        {
            List<PortalData> spawnPoints = new List<PortalData>();

            foreach (PortalData portal in this)
            {
                if (portal.Label.StartsWith("sp"))
                {
                    spawnPoints.Add(portal);
                }
            }

            sbyte identifier = (sbyte)(portalIdentifier != -1 ?
                        portalIdentifier :
                        Randomizer.NextInt(0, spawnPoints.Count));

            return spawnPoints[identifier];
        }
    }
}
