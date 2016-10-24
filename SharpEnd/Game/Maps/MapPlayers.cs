using SharpEnd.Game.Life;
using SharpEnd.Packets;
using SharpEnd.Game.Players;

namespace SharpEnd.Game.Maps
{
    public sealed class MapPlayers : MapEntities<Player>
    {
        public MapPlayers(Map map) : base(map) { }

        // TODO: Validate order of objects.
        protected override void InsertItem(Player item)
        {
            foreach (Player player in this)
            {
                item.Client.Send(PlayerPackets.PlayerSpawn(player));
            }

            Map.Send(PlayerPackets.PlayerSpawn(item));

            base.InsertItem(item);

            foreach (Drop drop in Map.Drops)
            {
                item.Client.Send(DropPackets.SpawnDrop(drop, EDropAnimation.Existing));
            }

            foreach (Mob mob in Map.Mobs)
            {
                item.Client.Send(MobPackets.MobSpawn(mob, -1));
            }

            foreach (Npc npc in Map.Npcs)
            {
                item.Client.Send(NpcPackets.NpcSpawn(npc));
            }

            foreach (Reactor reactor in Map.Reactors)
            {
                item.Client.Send(ReactorPackets.ReactorSpawn(reactor));
            }

            // TODO: Run map script.
        }

        protected override void RemoveItem(Player item)
        {
            base.RemoveItem(item);

            Map.Send(PlayerPackets.PlayerDespawn(item.ID));
        }
    }
}
