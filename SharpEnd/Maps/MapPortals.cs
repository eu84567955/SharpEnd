using SharpEnd.Data;
using SharpEnd.Drawing;
using System.Collections.Generic;

namespace SharpEnd.Maps
{
    internal sealed class MapPortals
    {
        public Map Map { get; private set; }

        public List<PortalData> Regular { get; private set; }
        public List<PortalData> SpawnPoints { get; private set; }

        public MapPortals(Map map)
            : base()
        {
            Map = map;

            Regular = new List<PortalData>();
            SpawnPoints = new List<PortalData>();
        }

        public PortalData this[string label]
        {
            get
            {
                foreach (PortalData portal in Regular)
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
            if (portalIdentifier == -1)
            {
                portalIdentifier = Randomizer.NextSByte(0, SpawnPoints.Count);
            }

            return SpawnPoints[portalIdentifier];
        }

        public PortalData GetNearestSpawnPoint(Point position)
        {
            PortalData closestSpawnPoint = SpawnPoints[0];

            double shortestDistance = double.PositiveInfinity;

            foreach (PortalData spawnPoint in SpawnPoints)
            {
                double distance = spawnPoint.Position.DistanceFrom(position);

                if (distance < shortestDistance)
                {
                    closestSpawnPoint = spawnPoint;

                    shortestDistance = distance;
                }
            }

            return closestSpawnPoint;
        }
    }
}
