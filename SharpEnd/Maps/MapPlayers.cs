using SharpEnd.Packets;
using SharpEnd.Players;
using System.Collections.Generic;

namespace SharpEnd.Maps
{
    internal sealed class MapPlayers : MapEntities<Player>
    {
        public MapPlayers(Map map) : base(map) { }

        public override void Add(Player player)
        {
            Map.Send(PlayerPackets.PlayerSpawn(player));

            foreach (Player loopPlayer in this.Values)
            {
                player.Send(PlayerPackets.PlayerSpawn(loopPlayer));
            }

            base.Add(player);

            foreach (Mob mob in Map.Mobs.Values)
            {
                player.Send(MobPackets.MobSpawn(mob));
            }

            foreach (Npc npc in Map.Npcs.Values)
            {
                player.Send(NpcPackets.NpcSpawn(npc));
            }

            foreach (Reactor reactor in Map.Reactors.Values)
            {
                player.Send(ReactorPackets.ReactorSpawn(reactor));
            }

            foreach (Drop drop in Map.Drops.Values)
            {
                player.Send(DropPackets.SpawnDrop(drop, EDropAnimation.Existing));
            }

            foreach (Mob mob in Map.Mobs.Values)
            {
                mob.AssignController();
            }

            foreach (Npc npc in Map.Npcs.Values)
            {
                npc.AssignController();
            }
        }

        public override void Remove(Player player)
        {
            player.ControlledMobs.Clear();
            player.ControlledNpcs.Clear();

            base.Remove(player);

            foreach (Mob mob in Map.Mobs.Values)
            {
                mob.AssignController();
            }

            foreach (Npc npc in Map.Npcs.Values)
            {
                npc.AssignController();
            }

            Map.Send(PlayerPackets.PlayerDespawn(player.Identifier));
        }
    }
}
