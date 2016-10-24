using SharpEnd.Game.Players;

namespace SharpEnd.Game.Commands.Gm
{
    public static class InventoryCommands
    {
        [GmCommand("item", "Gives you the desired item.")]
        public static void ItemCommand(Player player, int itemID, ushort quantity = 1)
        {
            if (true) // TODO: Check if the data files contain a file with this identifier.
            {
                player.Items.Add(new PlayerItem(itemID, quantity));
            }
            else
            {
                //player.Notify("[Command] Invalid item.");
            }
        }

        [GmCommand("meso", "Sets your meso.")]
        public static void MesoCommand(Player player, int amount)
        {
            //player.Items.SetMeso(amount);
        }
    }
}