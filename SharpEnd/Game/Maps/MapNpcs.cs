using SharpEnd.Game.Life;
using SharpEnd.Packets;

namespace SharpEnd.Game.Maps
{
    public sealed class MapNpcs : MapEntities<Npc>
    {
        public MapNpcs(Map map) : base(map) { }

        protected override void InsertItem(Npc item)
        {
            base.InsertItem(item);

            Map.Send(NpcPackets.NpcSpawn(item));
        }

        protected override void RemoveItem(Npc item)
        {
            Map.Send(NpcPackets.NpcDespawn(item.ObjectID));

            base.RemoveItem(item);
        }
    }
}
