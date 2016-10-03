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
    }
}
