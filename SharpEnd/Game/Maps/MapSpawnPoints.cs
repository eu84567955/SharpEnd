using System.Collections.Generic;

namespace SharpEnd.Maps
{
    internal sealed class MapSpawnPoints : List<SpawnPoint>
    {
        public Map Map { get; private set; }

        public MapSpawnPoints(Map map)
        {
            Map = map;
        }

        public new void Add(SpawnPoint spawnPoint)
        {
            spawnPoint.Map = Map;

            base.Add(spawnPoint);
        }

        public new void Remove(SpawnPoint spawnPoint)
        {
            base.Remove(spawnPoint);

            spawnPoint.Map = null;
        }
    }
}
