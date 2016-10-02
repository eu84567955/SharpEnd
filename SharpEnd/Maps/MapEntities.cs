using System.Collections.Generic;

namespace SharpEnd.Maps
{
    internal abstract class MapEntities<T> : Dictionary<int, T> where T : MapEntity
    {
        public Map Map { get; private set; }

        public MapEntities(Map map)
            : base()
        {
            Map = map;
        }

        public virtual void Add(T entity)
        {

        }

        public virtual void Remove(T entity)
        {

        }
    }
}
