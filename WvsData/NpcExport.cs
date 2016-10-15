using reNX;
using reNX.NXProperties;
using SharpEnd.Data;
using SharpEnd.Game.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WvsData
{
    internal static class NpcExport
    {
        public static void Export(string path)
        {
            Dictionary<int, NpcData> npcs = new Dictionary<int, NpcData>();

            using (FileStream stream = File.Create("data/Npcs.bin"))
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII))
                {
                    using (NXFile file = new NXFile(Path.Combine(path, "Npc.nx")))
                    {
                        foreach (NXNode node in file.BaseNode)
                        {
                            if (!node.ContainsChild("info"))
                            {
                                continue;
                            }

                            int identifier = node.GetIdentifier<int>();

                            if (npcs.ContainsKey(identifier))
                            {
                                Console.WriteLine("Duplicate npc {0}", identifier);

                                continue;
                            }

                            NXNode infoNode = node["info"];

                            NpcData npc = new NpcData();

                            npc.Identifier = identifier;
                            npc.IsShop = infoNode.GetBoolean("shop");
                            npc.StorageCost = infoNode.GetInt("trunkPut");

                            string script = string.Empty;

                            if (infoNode.ContainsChild("script"))
                            {
                                if (infoNode["script"].ChildCount > 0)
                                {
                                    script = infoNode["script"]["0"].GetString("script");
                                }
                            }

                            npc.Script = script;
                            npc.Start = DateTime.MinValue;
                            npc.End = DateTime.MaxValue;

                            npcs.Add(identifier, npc);
                        }
                    }

                    writer.Write(npcs.Count);

                    foreach (NpcData npc in npcs.Values)
                    {
                        npc.Save(writer);
                    }

                    Console.WriteLine("Npcs: {0}", npcs.Count);
                }
            }
        }
    }
}
