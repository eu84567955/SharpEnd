using System.Collections.Generic;

namespace SharpEnd.Game.Maps
{
    internal sealed class MapPortals : List<Portal>
    {
        public Map Map { get; private set; }

        public MapPortals(Map map)
            : base()
        {
            Map = map;
        }

        public Portal GetPortal(string label)
        {
            foreach (Portal portal in this)
            {
                if (portal.Label.ToLower() == label.ToLower())
                {
                    return portal;
                }
            }

            return null;
        }

        public Portal GetSpawnPoint(sbyte identifier = -1)
        {
            if (identifier == -1)
            {
                List<Portal> spawnPoints = new List<Portal>();

                foreach (Portal portal in this)
                {
                    if (portal.Label == "sp")
                    {
                        spawnPoints.Add(portal);
                    }
                }

                return spawnPoints[Randomizer.NextSByte(0, spawnPoints.Count)];
            }
            else
            {
                foreach (Portal portal in this)
                {
                    if (portal.Identifier == identifier)
                    {
                        return portal;
                    }
                }
            }

            return null;
        }
    }
}
