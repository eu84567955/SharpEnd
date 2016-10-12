using SharpEnd.Players;
using SharpEnd.Servers;
using SharpEnd.Utility;

namespace SharpEnd.Commands.Gm
{
    internal static class ItemCommands
    {
        [GmCommand("item", "Gives you the desired item.")]
        public static void ItemCommand(Player player, int itemIdentifier, ushort quantity = 1)
        {
            if (MasterServer.Instance.Items.ContainsKey(itemIdentifier))
            {
                player.Items.Add(new PlayerItem(itemIdentifier, quantity));
            }
            else
            {
                player.Notify("[Command] Invalid item.");
            }
        }
    }
}