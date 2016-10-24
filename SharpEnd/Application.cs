using SharpEnd.IO;
using SharpEnd.Network.Servers;
using SharpEnd.Threading;
using SharpEnd.Utility;
using System;
using System.Diagnostics;

namespace SharpEnd
{
    public static class Application
    {
        public const string CommandIndicator = "!";
        public const string PlayerCommandIndicator = "@";

        public static readonly MapleVersion Version = new MapleVersion()
        {
            Version = 177,
            Patch = "2",
            Localisation = ELocalisation.Global
        };

        private static Delay m_usageDelay;

        static Application()
        {
            m_usageDelay = new Delay(5 * 1000, () => UpdateTitle(), true);
        }

        private static void Main(string[] args)
        {
            Log.Entitle();
            
            try
            {
                Config.Load();

                Database.Test();

                MasterServer.Instance.Run();
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            while (MasterServer.Instance.IsRunning)
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
            Console.Title = $"SharpEnd | Memory Usage: {Math.Round(Process.GetCurrentProcess().WorkingSet64 / 1024f) } KB";
        }
    }
}
