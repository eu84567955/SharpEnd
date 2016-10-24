using System.Collections.Generic;
using System.Linq;

namespace SharpEnd.Game.Maps
{
    public sealed class MapPortals
    {
        private Map m_map;
        private List<Portal> m_portals;

        public Map Map { get; private set; }

        public MapPortals(Map map)
        {
            m_map = map;
            m_portals = new List<Portal>();
        }

        public void Add(Portal portal)
        {
            m_portals.Add(portal);
        }

        public void Remove(Portal portal)
        {
            m_portals.Remove(portal);
        }

        public void Clear()
        {
            m_portals.Clear();
        }

        public Portal this[sbyte id]
        {
            get
            {
                if (id == -1)
                {
                    var spawnPoints = m_portals.Where(p => p.IsSpawnPoint);
                    int toSkip = Randomizer.NextInt(0, spawnPoints.Count());

                    return spawnPoints.Skip(toSkip).Take(1).First();
                }
                else
                {
                    return m_portals.FirstOrDefault(p => p.ID == id);
                }
            }
        }

        public Portal this[string name]
        {
            get
            {
                return m_portals.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            }
        }
    }
}
