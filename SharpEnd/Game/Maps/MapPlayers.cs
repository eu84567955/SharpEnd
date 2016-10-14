using SharpEnd.Drawing;
using SharpEnd.Packets;
using SharpEnd.Players;
using System.Collections.Generic;

namespace SharpEnd.Game.Maps
{
    internal sealed class MapPlayers : MapEntities<Player>
    {
        public MapPlayers(Map map) : base(map) { }

        public Player this[string name]
        {
            get
            {
                lock (this)
                {
                    foreach (Player player in this.Values)
                    {
                        if (player.Name.ToLower() == name.ToLower())
                        {
                            return player;
                        }
                    }

                    throw new KeyNotFoundException();
                }
            }
        }

        // TODO: Validate order of objects.
        public override void Add(Player player)
        {
            lock (this)
            {
                foreach (Player loopPlayer in this.Values)
                {
                    player.Send(PlayerPackets.PlayerSpawn(loopPlayer));
                }

                Map.Send(PlayerPackets.PlayerSpawn(player));

                base.Add(player);
            }

            lock (Map.Mobs)
            {
                foreach (Mob mob in Map.Mobs.Values)
                {
                    player.Send(MobPackets.MobSpawn(mob, -1));
                }
            }

            lock (Map.Npcs)
            {
                foreach (Npc npc in Map.Npcs.Values)
                {
                    player.Send(NpcPackets.NpcSpawn(npc));
                }
            }

            lock (Map.Mobs)
            {
                foreach (Mob mob in Map.Mobs.Values)
                {
                    mob.AssignController();
                }
            }

            lock (Map.Npcs)
            {
                foreach (Npc npc in Map.Npcs.Values)
                {
                    npc.AssignController();
                }
            }
        }

        public override void Remove(Player player)
        {
            lock (this)
            {
                player.ControlledMobs.Clear();
                player.ControlledNpcs.Clear();

                base.Remove(player);

                lock (Map.Mobs)
                {
                    foreach (Mob mob in Map.Mobs.Values)
                    {
                        mob.AssignController();
                    }
                }

                lock (Map.Npcs)
                {
                    foreach (Npc npc in Map.Npcs.Values)
                    {
                        npc.AssignController();
                    }
                }

                Map.Send(PlayerPackets.PlayerDespawn(player.Identifier));
            }
        }
    }
}
