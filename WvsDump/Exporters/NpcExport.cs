using reNX;
using reNX.NXProperties;
using SharpEnd.Game.Data;
using System;
using System.Collections.Generic;
using System.IO;
using WvsDump.Utility;

namespace WvsDump
{
    internal static class NpcExport
    {
        public static void Export(string inputPath, string outputPath)
        {
            PerformanceTimer timer = new PerformanceTimer();
            long dataCount = 0;
            timer.Unpause();

            inputPath = Path.Combine(inputPath, "Npc.nx");
            outputPath = Path.Combine(outputPath, "npcs.bin");

            List<NpcData> npcs = new List<NpcData>();

            using (NXFile file = new NXFile(inputPath))
            {
                Application.ResetCounter(file.BaseNode.ChildCount);

                foreach (NXNode node in file.BaseNode)
                {
                    NXNode infoNode = node["info"];

                    int id = node.GetID<int>();

                    NpcData npc = new NpcData();

                    npc.ID = id;
                    npc.Move = false; // TODO.
                    npc.Parcel = infoNode.GetBoolean("parcel");
                    npc.RPSGame = infoNode.GetBoolean("rpsGame");
                    npc.StoreBank = infoNode.GetBoolean("storeBank");
                    npc.GuildRank = infoNode.GetBoolean("guildRank");
                    npc.Name = ""; // TODO: Get from String.wz.
                    npc.TrunkGet = infoNode.GetInt("trunkGet");
                    npc.TrunkPut = infoNode.GetInt("trunkPut");

                    npc.Scripts = new List<NpcScriptData>();
                    if (infoNode.ContainsChild("script"))
                    {
                        foreach (NXNode scriptNode in infoNode["script"])
                        {
                            NpcScriptData script = new NpcScriptData();

                            script.StartDate = scriptNode.GetInt("start");
                            script.EndDate = scriptNode.GetInt("end");
                            script.Script = scriptNode.GetString("script");
                            // TODO: "quest" child.

                            npc.Scripts.Add(script);
                        }
                    }

                    npcs.Add(npc);
                    ++dataCount;
                    ++Application.AllDataCounter;
                    Application.IncrementCounter();
                }
            }

            using (FileStream stream = File.Create(outputPath))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(npcs.Count);
                    npcs.ForEach(n => n.Save(writer));
                }
            }

            timer.Pause();
            Console.WriteLine("| {0,-24} | {1,-16} | {2,-24} |", "Npcs", dataCount, timer.Duration);
        }
    }
}
