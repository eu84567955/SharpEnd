using SharpEnd.Players;
using SharpEnd.Servers;
using System.Collections.Generic;

namespace SharpEnd.Commands.Gm
{
    internal static class InfoCommands
    {
        [GmCommand("search", "Searches")]
        public static void SearchCommand(Player player, string category, string name)
        {
            player.Notify("[Results]");

            switch (category)
            {
                case "map":
                    {
                        Dictionary<string, int> results = MasterServer.Instance.Strings.GetMaps(name);

                        if (results.Count > 0)
                        {
                            foreach (KeyValuePair<string, int> result in results)
                            {
                                player.Notify(string.Format("   -{0}: {1}", result.Key, result.Value));
                            }
                        }
                        else
                        {
                            player.Notify("   No result found.");
                        }
                    }
                    break;

                default:
                    {

                    }
                    break;
            }
        }
    }
}
