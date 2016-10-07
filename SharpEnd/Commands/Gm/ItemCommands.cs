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
            if (GameLogicUtilities.GetInventory(itemIdentifier) == EInventoryType.Equipment && !MasterServer.Instance.Equips.Contains(itemIdentifier))
            {
                player.Notify("[Command] Invalid item.");
            }
            else if (!MasterServer.Instance.Items.Contains(itemIdentifier))
            {
                player.Notify("[Command] Invalid item.");
            }
            else
            {
                player.Items.Add(new PlayerItem(itemIdentifier, quantity));
            }
        }
    }
}