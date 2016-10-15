using System;

namespace WvsData
{
    internal static class Application
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: WvsData [path]");
            }
            else
            {
                string path = args[0];

                ItemExport.Export(path);
                MobExport.Export(path);
                NpcExport.Export(path);
                MapExport.Export(path);

                Console.WriteLine("Press any key to continue...");

                Console.ReadKey();
            }
        }
    }
}
