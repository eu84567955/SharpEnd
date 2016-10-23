using SharpEnd.Game.Life;
using SharpEnd.Packets;

namespace SharpEnd.Game.Maps
{
    public sealed class MapReactors : MapEntities<Reactor>
    {
        public MapReactors(Map map) : base(map) { }

        protected override void InsertItem(Reactor item)
        {
            base.InsertItem(item);

            Map.Send(ReactorPackets.ReactorSpawn(item));
        }

        protected override void RemoveItem(Reactor item)
        {
            // TODO: Despawn packet.

            base.RemoveItem(item);
        }
    }
}
