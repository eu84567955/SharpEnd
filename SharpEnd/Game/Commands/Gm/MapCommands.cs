using SharpEnd.Players;
using SharpEnd.Servers;

namespace SharpEnd.Game.Commands.Gm
{
    internal static class MapCommands
    {
        [GmCommand("map", "Teleports you to the desired map.")]
        public static void MapCommand(Player player, int mapIdentifier, sbyte portal = 0)
        {
            if (MasterServer.Instance.Maps.ContainsKey(mapIdentifier))
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
