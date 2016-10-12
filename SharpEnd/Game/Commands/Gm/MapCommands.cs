using SharpEnd.Players;
using SharpEnd.Servers;

namespace SharpEnd.Commands.Gm
{
    internal static class MapCommands
    {
        [GmCommand("map", "Teleports you to the desired map.")]
        public static void MapCommand(Player player, int mapIdentifier)
        {
            if (MasterServer.Instance.GetMaps(player.Client.ChannelIdentifier).Contains(mapIdentifier))
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
