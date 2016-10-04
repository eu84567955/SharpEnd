using SharpEnd.Players;

namespace SharpEnd.Commands.Gm
{
    internal static class MapCommands
    {
        [GmCommand("map", "Teleports you to the desired map.")]
        public static void Map(Player player, int mapIdentifier)
        {
            player.SetMap(mapIdentifier);
        }
    }
}
