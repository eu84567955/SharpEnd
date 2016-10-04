using SharpEnd.Players;

namespace SharpEnd.Commands.Gm
{
    internal static class StatCommands
    {
        [GmCommand("level", "Sets your level to the desired level.")]
        public static void Level(Player player, byte value)
        {
            player.Stats.SetLevel(value);
        }

        [GmCommand("job", "Sets your job to the desired job.")]
        public static void Job(Player player, ushort value)
        {
            player.Stats.SetJob(value);
        }

        [GmCommand("str", "Sets your strength to the desired value.")]
        public static void Str(Player player, ushort value)
        {
            player.Stats.SetStrength(value);
        }

        [GmCommand("dex", "Sets your dexterity to the desired value.")]
        public static void Dex(Player player, ushort value)
        {
            player.Stats.SetDexterity(value);
        }

        [GmCommand("int", "Sets your intelligence to the desired value.")]
        public static void Int(Player player, ushort value)
        {
            player.Stats.SetIntelligence(value);
        }

        [GmCommand("luk", "Sets your luck to the desired value.")]
        public static void Luk(Player player, ushort value)
        {
            player.Stats.SetLuck(value);
        }
    }
}
