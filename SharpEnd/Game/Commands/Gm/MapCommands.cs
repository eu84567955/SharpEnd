using SharpEnd.Game.Players;

namespace SharpEnd.Game.Commands.Gm
{
    public static class MapCommands
    {
        [GmCommand("map", "Teleports you to the desired map.")]
        public static void MapCommand(Player player, int mapID, sbyte portal = 0)
        {
            if (true) // TODO: Check if the data files contain a file with this identifier.
            {
                //player.SetMap(mapID);
            }
            else
            {
                //player.Notify("[Command] Invalid map.");
            }
        }
    }
}
