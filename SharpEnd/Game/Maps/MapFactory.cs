using SharpEnd.Game.Data;
using System.Collections.Generic;
using System.Linq;

namespace SharpEnd.Game.Maps
{
    // TOOD: Thread-safe implementation.
    public sealed class MapFactory
    {
        private Dictionary<int, Map> m_maps;
        private List<Map> m_instanceMaps;

        public MapFactory()
        {
            m_maps = new Dictionary<int, Map>();
            m_instanceMaps = new List<Map>();
        }

        public Map GetMap(int mapID)
        {
            Map map;

            if (!m_maps.TryGetValue(mapID, out map))
            {
                map = new Map(mapID);
            }

            return map;
        }

        public Map MakeInstanceMap(int mapID)
        {
            Map map = new Map(mapID);

            m_instanceMaps.Add(map);

            return map;
        }

        public void DestroyInstanceMap(Map map)
        {
            m_instanceMaps.Remove(map);
        }

        public void Clear()
        {
            m_maps.Clear();
        }

        public IEnumerable<Map> GetMaps()
        {
            return m_maps.Values.AsEnumerable();
        }

        public IEnumerable<Map> GetInstanceMaps()
        {
            return m_instanceMaps.AsEnumerable();
        }
    }
}
