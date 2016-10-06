using SharpEnd.Players;

namespace SharpEnd.Commands.Gm
{
    internal static class ItemCommands
    {
        [GmCommand("item", "Gives you the desired item.")]
        public static void ItemCommand(Player player, int itemIdentifier)
        {
            player.Items.Add(new PlayerItem(itemIdentifier));
        }
    }
}
