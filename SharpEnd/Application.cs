using SharpEnd.Servers;
using SharpEnd.Utility;
using System;

namespace SharpEnd
{
    internal static class Application
    {
        public static string NXPath = "nx";

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
                m_usageDelay.Start();

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

            // Dispose

            m_usageDelay.Stop();
            m_usageDelay = null;

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
