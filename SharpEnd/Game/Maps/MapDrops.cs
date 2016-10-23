using SharpEnd.Packets;
using SharpEnd.Threading;

namespace SharpEnd.Game.Maps
{
    public sealed class MapDrops : MapEntities<Drop>
    {
        private const int ExpirationTime = 3 * 60 * 1000;

        public MapDrops(Map map) : base(map) { }

        protected override void InsertItem(Drop item)
        {
            item.Picker = null;

            base.InsertItem(item);

            if (item.Expiry != null)
            {
                item.Expiry.Cancel();
            }

            if (true) // TODO: Check if map has everlasting drops (e.g. christmas maps).
            {
                item.Expiry = new Delay(ExpirationTime, () =>
                {
                    if (item.Map == Map) // NOTE: Is this needed?
                    {
                        Remove(item);
                    }
                });
            }

            Map.Send(DropPackets.SpawnDrop(item, EDropAnimation.Pop));
            Map.Send(DropPackets.SpawnDrop(item, EDropAnimation.New));
        }

        protected override void RemoveItem(Drop item)
        {
            if (item.Expiry != null)
            {
                item.Expiry.Cancel();
            }

            Map.Send(DropPackets.DespawnDrop(item.ObjectID, item.Picker));
            
            base.RemoveItem(item);
        }
    }
}
