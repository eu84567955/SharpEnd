using SharpEnd.Players;
using SharpEnd.Servers;
using SharpEnd.Utility;

namespace SharpEnd.Commands.Gm
{
    internal static class ItemCommands
    {
        // TODO: Combine the item validation checks into one.
        [GmCommand("item", "Gives you the desired item.")]
        public static void ItemCommand(Player player, int itemIdentifier, ushort quantity = 1)
        {
            if (GameLogicUtilities.GetInventory(itemIdentifier) == EInventoryType.Equipment)
            {
                if (MasterServer.Instance.Equips.Contains(itemIdentifier))
                {
                    player.Items.Add(new PlayerItem(itemIdentifier, quantity));
                }
                else
                {
                    player.Notify("[Command] Invalid item.");
                }
            }
            else
            {
                if (MasterServer.Instance.Items.Contains(itemIdentifier))
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
}