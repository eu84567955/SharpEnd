using SharpEnd.Players;
using SharpEnd.Servers;

namespace SharpEnd.Commands.Gm
{
    internal static class GeneralCommands
    {
        [GmCommand("help", "Displays a list of available commands.")]
        public static void HelpCommand(Player player)
        {
            player.Notify("[Help]");

            foreach (var command in MasterServer.Instance.Commands[ECommandType.Gm])
            {
               if (command.Key == "help")
                {
                    continue;
                }

                player.Notify(string.Format("    {0}{1} - {2}", (true ? "!" : "@"), command.Value.Syntax, command.Value.Description));
            }
        }

        [GmCommand("pos", "Displays your position information.")]
        public static void PositionCommand(Player player)
        {
            player.Notify($"X: {player.Position.X}, Y: {player.Position.Y}, Stance: {player.Stance}, Foothold: {player.Foothold}");
        }
    }
}
