using SharpEnd.Packets;
using SharpEnd.Players;
using System.Collections.Generic;

namespace SharpEnd.Maps
{
    internal sealed class MapPlayers : List<Player>
    {
        public Map Map { get; private set; }

        public MapPlayers(Map map)
            : base()
        {
            Map = map;
        }

        public new void Add(Player player)
        {
            Map.Send(PlayerPackets.PlayerSpawn(player));

            foreach (Player loopPlayer in this)
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

            foreach (Mob mob in Map.Mobs.Values)
            {
                mob.AssignController();
            }

            foreach (Npc npc in Map.Npcs.Values)
            {
                npc.AssignController();
            }
        }

        public new void Remove(Player player)
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
