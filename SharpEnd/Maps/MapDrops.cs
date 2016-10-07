using SharpEnd.Packets;
using SharpEnd.Utility;

namespace SharpEnd.Maps
{
    internal sealed class MapDrops : MapEntities<Drop>
    {
        private const int ExpirationTime = 60 * 1000;

        public MapDrops(Map map) : base(map) { }

        public override void Add(Drop drop)
        {
            base.Add(drop);

            drop.Expiry = new Delay(ExpirationTime, () =>
            {
                Remove(drop);
            });

            Map.Send(DropPackets.SpawnDrop(drop, EDropAnimation.Pop));
            Map.Send(DropPackets.SpawnDrop(drop, EDropAnimation.New));
        }

        public override void Remove(Drop drop)
        {
            if (drop.Expiry != null)
            {
                // TODO: Dispose
            }

            Map.Send(DropPackets.DespawnDrop(drop.ObjectIdentifier, drop.Picker));

            base.Remove(drop);
        }
    }
}
