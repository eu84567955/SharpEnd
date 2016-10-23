using reNX;
using reNX.NXProperties;
using SharpEnd.Game.Data;
using System;
using System.Collections.Generic;
using System.IO;
using WvsDump.Utility;

namespace WvsDump
{
    internal static class MountExport
    {
        public static void Export(string inputPath, string outputPath)
        {
            PerformanceTimer timer = new PerformanceTimer();
            long dataCount = 0;
            timer.Unpause();

            inputPath = Path.Combine(inputPath, "TamingMob.nx");
            outputPath = Path.Combine(outputPath, "mounts.bin");

            List<MountData> mounts = new List<MountData>();

            using (NXFile file = new NXFile(inputPath))
            {
                Application.ResetCounter(file.BaseNode.ChildCount);

                foreach (NXNode node in file.BaseNode)
                {
                    NXNode infoNode = node["info"];

                    int mountID = node.GetID<int>();

                    MountData mount = new MountData();

                    mount.ID = mountID;
                    mount.FS = infoNode.GetDouble("fs");
                    mount.Swim = infoNode.GetDouble("swim");
                    mount.Fatigue = infoNode.GetInt("fatigue");
                    mount.Jump = infoNode.GetInt("jump");
                    mount.Speed = infoNode.GetInt("speed");

                    mounts.Add(mount);
                    ++dataCount;
                    ++Application.AllDataCounter;
                    Application.IncrementCounter();
                }
            }

            using (FileStream stream = File.Create(outputPath))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(mounts.Count);
                    mounts.ForEach(m => m.Save(writer));
                }
            }

            timer.Pause();
            Console.WriteLine("| {0,-24} | {1,-16} | {2,-24} |", "Mounts", dataCount, timer.Duration);
        }
    }
}
