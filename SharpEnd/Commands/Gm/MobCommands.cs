using SharpEnd.Maps;
using SharpEnd.Players;
using SharpEnd.Servers;
using System.Collections.Generic;

namespace SharpEnd.Commands.Gm
{
    internal static class MobCommands
    {
        [GmCommand("spawn", "Spawns the desired mob.")]
        public static void SpawnCommand(Player player, int mobIdentifier, int amount = 1)
        {
            if (MasterServer.Instance.Mobs.Contains(mobIdentifier))
            {
                while (amount-- > 0)
                {
                    player.Map.Mobs.Add(new Mob(mobIdentifier, player.Position, player.Foothold));
                }
            }
            else
            {
                player.Notify("[Command] Invalid mob.");
            }
        }

        [GmCommand("killall", "Kills all the mobs in your map.")]
        public static void SpawnCommand(Player player)
        {
            List<Mob> toKill = new List<Mob>();

            foreach (Mob mob in player.Map.Mobs.Values)
            {
                toKill.Add(mob);
            }

            foreach (Mob mob in toKill)
            {
                mob.Die();
            }
        }
    }
}
