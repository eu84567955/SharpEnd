using System;
using SharpEnd.Collections;

namespace SharpEnd.Game.Maps
{
    internal abstract class MapEntities<T> : SafeKeyedCollection<int, T> where T : MapEntity
    {
        public Map Map { get; private set; }

        public MapEntities(Map map)
            : base()
        {
            Map = map;
        }

        protected override int GetKeyForItem(T item)
        {
            return item.ObjectIdentifier;
        }

        protected override void InsertItem(T item)
        {
            item.Map = Map;
            item.ObjectIdentifier = Map.AssignObjectIdentifier();

            base.InsertItem(item);
        }

        protected override void RemoveItem(T item)
        {
            base.RemoveItem(item);

            item.Map = null;
            item.ObjectIdentifier = -1;
        }
    }
}
