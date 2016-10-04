using SharpEnd.Packets;
using SharpEnd.Players;
using SharpEnd.Servers;
using System.Collections.Generic;

namespace SharpEnd.Commands
{
    internal static class ManagmentCommands
    {
        [GmCommand("help", "Shows you a list of available commands.")]
        public static void Help(Player player)
        {
            player.Notify("[Command] Available commands:");

            foreach (Command command in MasterServer.Instance.Commands.Values)
            {
                player.Notify(string.Format("    {0}: {1}", command.Syntax, command.Description));
            }
        }

        private static sbyte counter = 0;

        [GmCommand("test", "Test")]
        public static void Test(Player player)
        {
            player.Map = 100000100;
            player.SpawnPoint = counter++;

            player.Send(MapPackets.ChangeMap(player));
        }

        [GmCommand("map", "Teleports you to the desired map.")]
        public static void Map(Player player, int mapIdentifier)
        {
            player.SetMap(mapIdentifier);
        }

        [GmCommand("level", "Sets your level to the desired level.")]
        public static void Level(Player player, byte value)
        {
            player.Stats.SetLevel(value);
        }

        [GmCommand("job", "Sets your job to the desired job.")]
        public static void Job(Player player, ushort value)
        {
            player.Stats.SetJob(value);
        }
    }
}
