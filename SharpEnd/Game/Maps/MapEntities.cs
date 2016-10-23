using SharpEnd.Collections;

namespace SharpEnd.Game.Maps
{
    public abstract class MapEntities<T> : SafeKeyedCollection<int, T> where T : MapEntity
    {
        public Map Map { get; private set; }

        public MapEntities(Map map)
            : base()
        {
            Map = map;
        }

        protected override int GetKeyForItem(T item)
        {
            return item.ObjectID;
        }

        protected override void InsertItem(T item)
        {
            item.Map = Map;
            item.ObjectID = Map.AssignObjectID();

            base.InsertItem(item);
        }

        protected override void RemoveItem(T item)
        {
            base.RemoveItem(item);

            item.Map = null;
            item.ObjectID = -1;
        }
    }
}
