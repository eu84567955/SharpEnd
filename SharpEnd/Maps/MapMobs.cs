using SharpEnd.Packets;

namespace SharpEnd.Maps
{
    internal sealed class MapMobs : MapEntities<Mob>
    {
        public MapMobs(Map map) : base(map) { }

        public override void Add(Mob mob)
        {
            base.Add(mob);

            Map.Send(MobPackets.MobSpawn(mob));
        }

        public override void Remove(Mob mob)
        {
            // TODO: Packet

            base.Remove(mob);
        }
    }
}
