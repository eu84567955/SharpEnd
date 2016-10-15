using reNX;
using reNX.NXProperties;
using SharpEnd.Game.Data;
using SharpEnd.Game.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static SharpEnd.Game.Data.NpcData;

namespace WvsData
{
    internal static class NpcExport
    {
        public static void Export(string inputPath, string outputPath)
        {
            Console.Write(" > Exporting npcs... ");

            // NOTE: Npc shops are in separate file, we're reading them first into a dictionary.
            Dictionary<int, NpcShopData> shops = new Dictionary<int, NpcShopData>();

            {
                string[] lines = File.ReadAllLines("Shops.txt");

                int shopIdentifier = -1;

                foreach (string line in lines)
                {
                    if (line.StartsWith("#"))
                    {
                        shopIdentifier = int.Parse(line.Substring(1));
                    }
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        string[] split = line.Split(' ');

                        if (!shops.ContainsKey(shopIdentifier))
                        {
                            shops.Add(shopIdentifier, new NpcShopData
                            {
                                Identifier = shopIdentifier,
                                Items = new List<NpcShopItemData>()
                            });
                        }

                        int i = 0;

                        shops[shopIdentifier].Items.Add(new NpcShopItemData
                        {
                            Type = int.Parse(split[i++]),
                            Discount = int.Parse(split[i++]),
                            WorldBlock = int.Parse(split[i++]) == 1,
                            MinimumLevel = ushort.Parse(split[i++]),
                            MaximumLevel = ushort.Parse(split[i++]),
                            MaxPerSlot = ushort.Parse(split[i++]),
                            Quantity = ushort.Parse(split[i++]),
                            ItemIdentifier = int.Parse(split[i++]),
                            Price = int.Parse(split[i++]),
                            TokenItemIdentifier = int.Parse(split[i++]),
                            TokenPrice = int.Parse(split[i++]),
                            PointQuestIdentifier = int.Parse(split[i++]),
                            PointPrice = int.Parse(split[i++]),
                            StarCoin = int.Parse(split[i++]),
                            QuestExIdentifier = int.Parse(split[i++]),
                            //QuestExValue = int.Parse(split[i++]),
                            TimePeriod = int.Parse(split[i++]),
                            LevelLimited = int.Parse(split[i++]),
                            QuestIdentifier = int.Parse(split[i++]),
                            TabIndex = int.Parse(split[i++]),
                            PotentialGrade = int.Parse(split[i++]),
                            BuyLimit = int.Parse(split[i++]),
                            //QuestExKey = split[i++]
                        });
                    }
                }
            }

            Dictionary<int, NpcData> npcs = new Dictionary<int, NpcData>();

            using (NXFile file = new NXFile(inputPath))
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
                        continue;
                    }

                    NXNode infoNode = node["info"];

                    NpcData npc = new NpcData();

                    npc.Identifier = identifier;
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

                    if (shops.ContainsKey(identifier))
                        npc.Shop = shops[identifier];

                    npcs.Add(identifier, npc);
                }
            }

            foreach (NpcData npc in npcs.Values)
            {
                using (FileStream stream = File.Create(Path.Combine(outputPath, npc.Identifier.ToString() + ".shd")))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII))
                    {
                        npc.Save(writer);
                    }
                }
            }

            Console.WriteLine("\t\tDone ({0}).", npcs.Count);
        }
    }
}