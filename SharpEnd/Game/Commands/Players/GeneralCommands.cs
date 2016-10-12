using SharpEnd.Players;
using SharpEnd.Servers;

namespace SharpEnd.Commands.Players
{
    internal static class GeneralCommands
    {
        [PlayerCommand("help", "Displays a list of available commands.")]
        public static void HelpCommand(Player player)
        {
            player.Notify("[Help]");

            foreach (var command in MasterServer.Instance.Commands[ECommandType.Player])
            {
                if (command.Key == "help")
                {
                    continue;
                }

                player.Notify(string.Format("    {0} - {1}", command.Value.Syntax, command.Value.Description));
            }
        }

        [PlayerCommand("save", "Saves your progress.")]
        public static void SaveCommand(Player player)
        {
            player.Save();
        }

        [PlayerCommand("release", "Unstucks your player.")]
        public static void ReleaseCommand(Player player)
        {
            player.Release();
        }
    }
}
