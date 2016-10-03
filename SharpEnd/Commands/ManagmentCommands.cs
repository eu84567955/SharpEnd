using SharpEnd.Players;

namespace SharpEnd.Commands
{
    internal static class ManagmentCommands
    {
        [GmCommand("map", "Teleports you to the selected map.")]
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
