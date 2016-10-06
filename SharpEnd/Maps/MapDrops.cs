using SharpEnd.Packets;

namespace SharpEnd.Maps
{
    internal sealed class MapDrops : MapEntities<Drop>
    {
        public MapDrops(Map map) : base(map) { }

        public override void Add(Drop drop)
        {
            base.Add(drop);

            Map.Send(DropPackets.SpawnDrop(drop, EDropAnimation.Pop));
            Map.Send(DropPackets.SpawnDrop(drop, EDropAnimation.New));
        }

        public void Remove(Drop drop, sbyte animation)
        {
            Map.Send(DropPackets.DespawnDrop(animation, drop.ObjectIdentifier, 1));

            base.Remove(drop);
        }
    }
}
