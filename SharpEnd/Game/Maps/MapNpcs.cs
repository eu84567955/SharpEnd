using SharpEnd.Packets;

namespace SharpEnd.Maps
{
    internal sealed class MapNpcs : MapEntities<Npc>
    {
        public MapNpcs(Map map) : base(map) { }

        public override void Add(Npc npc)
        {
            base.Add(npc);

            Map.Send(NpcPackets.NpcSpawn(npc));

            npc.AssignController();
        }

        public override void Remove(Npc npc)
        {
            Map.Send(NpcPackets.NpcDespawn(npc.ObjectIdentifier));

            base.Remove(npc);
        }
    }
}
