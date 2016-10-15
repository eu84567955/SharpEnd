using SharpEnd.Packets;
using SharpEnd.Players;
using SharpEnd.Scripting;
using System;
using System.Collections.Generic;
using System.IO;

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

            lock (Map.Drops)
            {
                foreach (Drop drop in Map.Drops.Values)
                {
                    player.Send(DropPackets.SpawnDrop(drop, EDropAnimation.Existing));
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

                if (File.Exists(string.Format("scripts/maps/{0}/{1}.py", initial ? "inital_entry" : "entry", name)))
                {
                    MapScript script = new MapScript(player, Map, initial);

                    try
                    {
                        script.Execute();
                    }
                    catch (Exception e)
                    {
                        Log.Error("Exception while executing map script '{0}': \n{1}", Map.EntryScript, e.Message);
                    }
                }
                else
                {
                    Log.Warn("Unscripted map '{0}'.", Map.EntryScript);
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
