using reNX;
using reNX.NXProperties;
using SharpEnd.Game.Data;
using System;
using System.Collections.Generic;
using System.IO;
using WvsDump.Utility;

namespace WvsDump
{
    internal static class MobExport
    {
        public static void Export(string inputPath, string outputPath)
        {
            PerformanceTimer timer = new PerformanceTimer();
            long dataCount = 0;
            timer.Unpause();

            inputPath = Path.Combine(inputPath, "Mob.nx");
            outputPath = Path.Combine(outputPath, "mobs.bin");

            List<MobData> mobs = new List<MobData>();

            using (NXFile file = new NXFile(inputPath))
            {
                Application.ResetCounter(file.BaseNode.ChildCount);

                foreach (NXNode node in file.BaseNode)
                {
                    if (node.Name == "QuestCountGroup")
                    {
                        continue;
                    }

                    NXNode infoNode = node["info"];

                    int id = node.GetID<int>();

                    if (id > 8090000)
                    {
                        continue;
                    }

                    MobData mob = new MobData();

                    mob.ID = id;
                    mob.Level = infoNode.GetByte("level");
                    mob.MaxHP = infoNode.GetInt("maxHP");
                    mob.MaxMP = infoNode.GetInt("maxMP");
                    mob.Experience = infoNode.GetInt("exp");

                    mobs.Add(mob);
                    ++dataCount;
                    ++Application.AllDataCounter;
                    Application.IncrementCounter();
                }
            }

            using (FileStream stream = File.Create(outputPath))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(mobs.Count);
                    mobs.ForEach(m => m.Save(writer));
                }
            }

            timer.Pause();
            Console.WriteLine("| {0,-24} | {1,-16} | {2,-24} |", "Mobs", dataCount, timer.Duration);
        }
    }
}
