using SharpEnd.Players;
using SharpEnd.Servers;

namespace SharpEnd.Game.Commands.Gm
{
    internal static class InventoryCommands
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

        [GmCommand("meso", "Sets your meso.")]
        public static void MesoCommand(Player player, int amount)
        {
            player.Items.SetMeso(amount);
        }
    }
}