using SharpEnd.Maps;
using SharpEnd.Players;
using SharpEnd.Servers;

namespace SharpEnd.Commands.Gm
{
    internal static class TestCommands
    {
        [GmCommand("testmeso", "Test a meso drop.")]
        public static void MesoTest(Player player)
        {
            MasterServer.Instance.Maps[player.Map].Drops.Add(new Drop(EDropType.FreeForAll, 5000)
            {
                Position = player.Position
            });
        }
    }
}
