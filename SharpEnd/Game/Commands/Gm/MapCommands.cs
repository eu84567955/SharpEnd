using SharpEnd.Players;

namespace SharpEnd.Game.Commands.Gm
{
    internal static class MapCommands
    {
        [GmCommand("map", "Teleports you to the desired map.")]
        public static void MapCommand(Player player, int mapIdentifier, sbyte portal = 0)
        {
            if (true) // TODO: Check if the data files contain a file with this identifier.
            {
                player.SetMap(mapIdentifier);
            }
            else
            {
                player.Notify("[Command] Invalid map.");
            }
        }
    }
}
