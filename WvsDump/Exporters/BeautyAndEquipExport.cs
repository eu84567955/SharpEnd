using reNX;
using reNX.NXProperties;
using SharpEnd.Game.Data;
using System;
using System.Collections.Generic;
using System.IO;
using WvsDump.Utility;

namespace WvsDump
{
    internal static class BeautyAndEquipExport
    {
        public static void Export(string inputPath, string outputPath)
        {
            PerformanceTimer timer = new PerformanceTimer();
            long dataCount = 0;
            timer.Unpause();

            inputPath = Path.Combine(inputPath, "Character.nx");
            string beautyOutputPath = Path.Combine(outputPath, "beauty.bin");
            string equipOutputPath = Path.Combine(outputPath, "equips.bin");

            List<byte> skins = new List<byte>() { 0, 1, 2, 3, 4, 5, 9, 10, 11, 12, 13 }; // NOTE: Skins are pre-defined because they don't seem to change much.
            List<int> faces = new List<int>();
            List<int> hairs = new List<int>();
            List<EquipData> equips = new List<EquipData>();

            using (NXFile file = new NXFile(inputPath))
            {
                int count = 0;

                foreach (NXNode categoryNode in file.BaseNode)
                {
                    if (categoryNode.Name.Contains(".img") ||
                        categoryNode.Name == "Afterimage")
                    {
                        continue;
                    }

                    count += categoryNode.ChildCount;
                }

                Application.ResetCounter(count);

                foreach (NXNode categoryNode in file.BaseNode)
                {
                    if (categoryNode.Name.Contains(".img") ||
                        categoryNode.Name == "Afterimage")
                    {
                        continue;
                    }

                    if (categoryNode.Name == "Face")
                    {
                        foreach (NXNode node in categoryNode)
                        {
                            int faceID = node.GetID<int>();

                            faces.Add(faceID);
                        }
                    }
                    else if (categoryNode.Name == "Hair")
                    {
                        foreach (NXNode node in categoryNode)
                        {
                            int hairID = node.GetID<int>();

                            hairs.Add(hairID);
                        }
                    }
                    else
                    {
                        foreach (NXNode node in categoryNode)
                        {
                            int equipID = node.GetID<int>();

                            NXNode infoNode = node["info"];

                            EquipData equip = new EquipData();

                            equip.ID = equipID;
                            equip.Slots = infoNode.GetByte("tuc");
                            equip.RequiredLevel = infoNode.GetByte("reqLevel");
                            equip.RequiredJob = infoNode.GetShort("reqJob");
                            equip.RequiredStrength = infoNode.GetShort("reqSTR");
                            equip.RequiredDexterity = infoNode.GetShort("reqDEX");
                            equip.RequiredIntelligence = infoNode.GetShort("reqINT");
                            equip.RequiredLuck = infoNode.GetShort("reqLUK");
                            equip.Strength = infoNode.GetShort("incSTR");
                            equip.Dexterity = infoNode.GetShort("incDEX");
                            equip.Intelligence = infoNode.GetShort("incINT");
                            equip.Luck = infoNode.GetShort("incLUK");
                            equip.WeaponAttack = infoNode.GetShort("incPAD");
                            equip.MagicAttack = infoNode.GetShort("incMAD");
                            equip.WeaponDefense = infoNode.GetShort("incPDD");
                            equip.MagicDefense = infoNode.GetShort("incMDD");
                            equip.HP = infoNode.GetShort("incMHP");
                            equip.MP = infoNode.GetShort("incMMP");
                            equip.Accuracy = infoNode.GetShort("incACC");
                            equip.Avoidability = infoNode.GetShort("incEVA");
                            equip.Hands = infoNode.GetShort("incCraft");
                            equip.Jump = infoNode.GetShort("incJump");
                            equip.Speed = infoNode.GetShort("incSpeed");
                            equip.CharmExperience = infoNode.GetShort("charmEXP");
                            equip.CharismaExperience = infoNode.GetShort("charismaEXP");
                            equip.WillExperience = infoNode.GetShort("willEXP");
                            equip.InsightExperience = infoNode.GetShort("insightEXP");
                            equip.SenseExperience = infoNode.GetShort("senseEXP");
                            equip.CraftExperience = infoNode.GetShort("craftEXP");

                            equips.Add(equip);
                            ++dataCount;
                            ++Application.AllDataCounter;
                            Application.IncrementCounter();
                        }
                    }
                }
            }

            using (FileStream stream = File.Create(beautyOutputPath))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(skins.Count);
                    skins.ForEach(s => writer.Write(s));

                    writer.Write(faces.Count);
                    faces.ForEach(f => writer.Write(f));

                    writer.Write(hairs.Count);
                    hairs.ForEach(h => writer.Write(h));
                }
            }

            using (FileStream stream = File.Create(equipOutputPath))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(equips.Count);
                    equips.ForEach(e => e.Save(writer));
                }
            }

            timer.Pause();
            Console.WriteLine("| {0,-24} | {1,-16} | {2,-24} |", "Equips", dataCount, timer.Duration);
        }
    }
}
