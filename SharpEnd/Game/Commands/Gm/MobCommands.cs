using SharpEnd.Game.Life;
using SharpEnd.Game.Maps;
using SharpEnd.Players;
using System.Collections.Generic;

namespace SharpEnd.Game.Commands.Gm
{
    internal static class MobCommands
    {
        [GmCommand("spawn", "Spawns the desired mob.")]
        public static void SpawnCommand(Player player, int mobIdentifier, int amount = 1)
        {
            if (true) // TODO: Check if the data files contain a file with this identifier.
            {
                while (amount-- > 0)
                {
                    player.Map.Mobs.Add(new Mob(mobIdentifier, player));
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

            foreach (Mob mob in player.Map.Mobs)
            {
                toKill.Add(mob);
            }

            foreach (Mob mob in toKill)
            {
                mob.Die();
            }
        }

        [GmCommand("cleardrops", "Clears all the drops in your map.")]
        public static void ClearDropsCommand(Player player)
        {
            List<Drop> toClear = new List<Drop>();

            foreach (Drop drop in player.Map.Drops)
            {
                toClear.Add(drop);
            }

            foreach (Drop drop in toClear)
            {
                player.Map.Drops.Remove(drop);
            }
        }
    }
}
