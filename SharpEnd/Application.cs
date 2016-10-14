using SharpEnd.Servers;
using SharpEnd.Threading;
using SharpEnd.Utility;
using System;

namespace SharpEnd
{
    internal static class Application
    {
        public const string CommandIndicator = "!";
        public const string PlayerCommandIndicator = "@";

        public static readonly MapleVersion Version = new MapleVersion()
        {
            Version = 176,
            Patch = "3",
            Localisation = ELocalisation.Global
        };

        private static Delay m_usageDelay;

        static Application()
        {
            m_usageDelay = new Delay(15 * 1000, () => UpdateTitle(), true);
        }

        private static void Main(string[] args)
        {
            Log.Entitle();

            try
            {
                Database.Initialize();

                MasterServer.Instance.Run();
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            while (MasterServer.Instance.Running)
            {
                Console.Read();
            }

            // TODO: Dispose.

            Log.SkipLine();

            Console.WriteLine(" Press any key to quit...");

            Console.ReadKey();
        }

        private static void UpdateTitle()
        {
            Console.Title = $"SharpEnd | Memory Usage: {Math.Round(GC.GetTotalMemory(true) / 1024f) } KB";
        }
    }
}
