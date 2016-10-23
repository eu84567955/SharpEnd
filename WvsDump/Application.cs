using System;
using WvsDump.Utility;

namespace WvsDump
{
    internal class Application
    {
        internal static long AllDataCounter = 0;
        internal static int TotalCount = 0;
        internal static int CurrentCount = 0;
        internal static int CurrentPercent = 0;

        private static void Main(string[] args)
        {
            Console.Title = "WvsDump";

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: WvsDump [path]");

                return;
            }

            string inputPath = args[0];
            string outputPath = "data";

            Console.WriteLine("  {0,-24}___{1,-16}___{2,-24}", new string('_', 24), new string('_', 16), new string('_', 24));
            Console.WriteLine(" /{0,-24}   {1,-16}   {2,-24}\\", "Data", "Count", "Time");
            Console.WriteLine("| {0,-24} | {1,-16} | {2,-24} |", new string('-', 24), new string('-', 16), new string('-', 24));
            PerformanceTimer timer = new PerformanceTimer();
            timer.Unpause();

            BeautyAndEquipExport.Export(inputPath, outputPath);
            NpcExport.Export(inputPath, outputPath);
            MobExport.Export(inputPath, outputPath);
            QuestExport.Export(inputPath, outputPath);
            ReactorExport.Export(inputPath, outputPath);
            MountExport.Export(inputPath, outputPath);
            MapExport.Export(inputPath, outputPath);

            timer.Pause();
            Console.WriteLine("| {0,-24} | {1,-16} | {2,-24} |", new string('-', 24), new string('-', 16), new string('-', 24));
            Console.WriteLine("| {0,-24} | {1,-16} | {2,-24} |", "All", AllDataCounter, timer.Duration);
            Console.WriteLine("| {0,-24} | {1,-16} | {2,-24} |", new string('-', 24), new string('-', 16), new string('-', 24));
            Console.WriteLine(" \\{0,-24}___{1,-16}___{2,-24}/", new string('_', 24), new string('_', 16), new string('_', 24));

            Console.Title = "WvsDump - Operation Done";

            Console.WriteLine();
            Console.Write("Press any key to quit . . .");
            Console.ReadKey(true);
        }

        internal static void ResetCounter(int pTotalCount)
        {
            CurrentPercent = 0;
            CurrentCount = 0;
            TotalCount = pTotalCount;
            Console.Title = "Progress: 0%";
        }

        internal static void IncrementCounter()
        {
            ++CurrentCount;
            int percent = (CurrentCount * 100) / TotalCount;
            if (percent > CurrentPercent)
            {
                CurrentPercent = percent;
                Console.Title = "Progress: " + CurrentPercent + "%";
            }
        }
    }
}
