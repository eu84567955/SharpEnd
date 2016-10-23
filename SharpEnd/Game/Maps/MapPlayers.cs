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
            /*foreach (Player player in this)
            {
                item.Send(PlayerPackets.PlayerSpawn(player));
            }

            Map.Send(PlayerPackets.PlayerSpawn(item));

            base.InsertItem(item);

            foreach (Mob mob in Map.Mobs)
            {
                item.Send(MobPackets.MobSpawn(mob, -1));
            }

            foreach (Npc npc in Map.Npcs)
            {
                item.Send(NpcPackets.NpcSpawn(npc));
            }

            foreach (Reactor reactor in Map.Reactors)
            {
                item.Send(ReactorPackets.ReactorSpawn(reactor));
            }

            foreach (Drop drop in Map.Drops)
            {
                item.Send(DropPackets.SpawnDrop(drop, EDropAnimation.Existing));
            }

            if (!string.IsNullOrEmpty(Map.EntryScript) || !string.IsNullOrEmpty(Map.InitialEntryScript))
            {
                bool initial = !string.IsNullOrEmpty(Map.InitialEntryScript);

                if (initial && Count != 1)
                {
                    // NOTE: Initial map entry scripts are only executed if this is the first player to enter the map.
                    // If the players' count does not equal to 1, we don't execute the script.

                    return;
                }

                string name = initial ? Map.InitialEntryScript : Map.EntryScript;

                // TODO: Run map script.
            }*/
        }

        protected override void RemoveItem(Player item)
        {
            base.RemoveItem(item);
            
            Map.Send(PlayerPackets.PlayerDespawn(item.Id));
        }
    }
}
