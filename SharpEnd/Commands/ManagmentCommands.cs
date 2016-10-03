using SharpEnd.Players;

namespace SharpEnd.Commands
{
    internal static class ManagmentCommands
    {
        [GmCommand("map", "Teleports you to the selected map.")]
        public static void Map(Player player, int mapId, sbyte portalId = 0)
        {
            player.Notify("You wanted to travel to " + mapId + ", eh?");
        }
    }
}
