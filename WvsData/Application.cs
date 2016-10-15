using System;
using System.Diagnostics;
using System.IO;

namespace WvsData
{
    internal static class Application
    {
        private static void Main(string[] args)
        {
            Console.Title = "WvsData";

            Console.WriteLine();
            Console.WriteLine("\t\t\t\t\t==================================");
            Console.WriteLine("\t\t\t\t\t=============[WvsData]============");
            Console.WriteLine("\t\t\t\t\t==================================");
            Console.WriteLine("\t\t\t\t\t=====[MapleStory Data Dumper]=====");
            Console.WriteLine("\t\t\t\t\t==================================");
            Console.WriteLine();

            if (args.Length == 0)
            {
                Console.WriteLine(" > Usage: WvsData [path]");
                Console.WriteLine(" > Flags: [version]");
            }
            else
            {
                string path = args[0];

                Stopwatch sw = Stopwatch.StartNew();

                ItemExport.Export(Path.Combine(path, "Character.nx"), Path.Combine(path, "Item.nx"), Path.Combine("data", "items"), Path.Combine("data", "pets"));
                MobExport.Export(Path.Combine(path, "Mob.nx"), Path.Combine("data", "mobs"));
                NpcExport.Export(Path.Combine(path, "Npc.nx"), Path.Combine("data", "npcs"));
                ReactorExport.Export(Path.Combine(path, "Reactor.nx"), Path.Combine("data", "reactors"));
                MapExport.Export(Path.Combine(path, "Map.nx"), Path.Combine("data", "maps"));

                sw.Stop();

                Console.WriteLine(" > Operation took {0:N3} seconds.", sw.Elapsed.TotalSeconds);
            }

            Console.WriteLine();
            Console.WriteLine(" > Press any key to continue...");
            Console.ReadKey();
        }
    }
}
