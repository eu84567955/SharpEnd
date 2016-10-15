using SharpEnd.Packets;

namespace SharpEnd.Game.Maps
{
    internal sealed class MapNpcs : MapEntities<Npc>
    {
        public MapNpcs(Map map) : base(map) { }

        protected override void InsertItem(Npc item)
        {
            base.InsertItem(item);

            Map.Send(NpcPackets.NpcSpawn(item));

            item.AssignController();
        }

        protected override void RemoveItem(Npc item)
        {
            item.Controller.ControlledNpcs.Remove(item);

            Map.Send(NpcPackets.NpcDespawn(item.ObjectIdentifier));

            base.RemoveItem(item);
        }
    }
}
