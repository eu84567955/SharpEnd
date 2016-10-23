using SharpEnd.Collections;
using System.Collections.Generic;

namespace SharpEnd.Game.Maps
{
    public sealed class MapPortals : SafeKeyedCollection<sbyte, Portal>
    {
        public Map Map { get; private set; }

        public MapPortals(Map map)
            : base()
        {
            Map = map;
        }

        protected override sbyte GetKeyForItem(Portal item)
        {
            return item.ID;
        }

        public Portal this[string label]
        {
            get
            {
                foreach (Portal portal in this)
                {
                    if (portal.Label.ToLower() == label.ToLower())
                    {
                        return portal;
                    }
                }

                throw new KeyNotFoundException();
            }
        }

        public new Portal this[sbyte identifier]
        {
            get
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
                    return base[identifier];
                }
            }
        }
    }
}
