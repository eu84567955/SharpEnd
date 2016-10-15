using SharpEnd.Packets;
using SharpEnd.Players;
using SharpEnd.Scripting;
using System;
using System.IO;

namespace SharpEnd.Game.Maps
{
    internal sealed class MapPlayers : MapEntities<Player>
    {
        public MapPlayers(Map map) : base(map) { }

        // TODO: Validate order of objects.
        protected override void InsertItem(Player item)
        {
            foreach (Player player in this)
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

            foreach (Drop drop in Map.Drops)
            {
                item.Send(DropPackets.SpawnDrop(drop, EDropAnimation.Existing));
            }

            foreach (Mob mob in Map.Mobs)
            {
                mob.AssignController();
            }

            foreach (Npc npc in Map.Npcs)
            {
                npc.AssignController();
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
                    MapScript script = new MapScript(item, Map, initial);

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

        protected override void RemoveItem(Player item)
        {
            item.ControlledMobs.Clear();
            item.ControlledNpcs.Clear();

            base.RemoveItem(item);

            foreach (Mob mob in Map.Mobs)
            {
                mob.AssignController();
            }

            foreach (Npc npc in Map.Npcs)
            {
                npc.AssignController();
            }

            Map.Send(PlayerPackets.PlayerDespawn(item.Identifier));
        }
    }
}
