using SharpEnd.Packets;
using SharpEnd.Players;
using SharpEnd.Threading;

namespace SharpEnd.Game.Maps
{
    internal sealed class MapDrops : MapEntities<Drop>
    {
        private const int ExpirationTime = 60 * 1000;

        public MapDrops(Map map) : base(map) { }

        public override void Add(Drop drop)
        {
            base.Add(drop);

            if (drop.Expiry != null)
            {
                drop.Expiry.Cancel();
            }

            drop.Expiry = new Delay(ExpirationTime, () =>
            {
                if (drop.Map == Map)
                {
                    Remove(drop);
                }
            });

            Map.Send(DropPackets.SpawnDrop(drop, EDropAnimation.Pop));
            Map.Send(DropPackets.SpawnDrop(drop, EDropAnimation.New));
        }

        public void Remove(Drop drop, Player picker = null)
        {
            if (drop.Expiry != null)
            {
                drop.Expiry.Cancel();
            }

            Map.Send(DropPackets.DespawnDrop(drop.ObjectIdentifier, picker));

            base.Remove(drop);
        }
    }
}
