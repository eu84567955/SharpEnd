using reNX;
using reNX.NXProperties;
using SharpEnd.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static SharpEnd.Data.MobData;

namespace WvsData
{
    internal static class MobExport
    {
        public static void Export(string path)
        {
            // NOTE: Mob drops are in separate file, we're reading them first into a dictionary.
            Dictionary<int, List<MobDropData>> drops = new Dictionary<int, List<MobDropData>>();

            {
                string[] lines = File.ReadAllLines("MonsterDrops.txt");

                int mobIdentifier = -1;

                foreach (string line in lines)
                {
                    if (line.StartsWith("#"))
                    {
                        mobIdentifier = int.Parse(line.Substring(1));
                    }
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        string[] split = line.Split(' ');

                        if (!drops.ContainsKey(mobIdentifier))
                        {
                            drops.Add(mobIdentifier, new List<MobDropData>());
                        }

                        drops[mobIdentifier].Add(new MobDropData
                        {
                            ItemIdentifier = int.Parse(split[0]),
                            Chance = int.Parse(split[1]),
                            Minimum = int.Parse(split[2]),
                            Maximum = int.Parse(split[3]),
                            QuestIdentifier = ushort.Parse(split[4])
                        });
                    }
                }
            }

            Dictionary<int, MobData> mobs = new Dictionary<int, MobData>();

            using (FileStream stream = File.Create("data/Mobs.bin"))
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII))
                {
                    using (NXFile file = new NXFile(path + "/Mob.nx"))
                    {
                        foreach (NXNode node in file.BaseNode)
                        {
                            if (!node.ContainsChild("info"))
                            {
                                continue;
                            }

                            int identifier = node.GetIdentifier<int>();

                            if (mobs.ContainsKey(identifier))
                            {
                                Console.WriteLine("Duplicate mob {0}", identifier);

                                continue;
                            }

                            NXNode infoNode = node["info"];

                            MobData mob = new MobData();

                            mob.Identifier = identifier;
                            mob.Level = infoNode.GetByte("level");
                            try
                            {
                                mob.Health = infoNode.GetInt("maxHP");
                            }
                            catch { }
                            mob.Mana = infoNode.GetInt("maxMP");
                            mob.HealthRecovery = infoNode.GetInt("hpR");
                            mob.ManaRecovery = infoNode.GetInt("mpR");
                            mob.ExplodeHP = 0;
                            mob.Experience = infoNode.GetInt("exp");
                            mob.LinkIdentifier = 0;
                            mob.SummonType = 0;
                            mob.Knockback = 0;
                            mob.FixedDamage = 0;
                            mob.DeathBuffIdentifier = 0;
                            mob.DeathAfter = 0;
                            mob.Traction = 0;
                            mob.DamagedBySkillIdentifierOnly = 0;
                            mob.DamagedByMobIdentifierOnly = 0;
                            mob.HPBarColor = 0;
                            mob.HPBarBackgroundColor = 0;
                            mob.CarnivalPoints = 0;
                            mob.PhysicalAttack = 0;
                            mob.PhysicalDefense = 0;
                            mob.MagicalAttack = 0;
                            mob.MagicalDefense = 0;
                            mob.Accuracy = 0;
                            mob.Avoidance = 0;
                            mob.Speed = 0;
                            mob.ChaseSpeed = 0;
                            mob.Summons = new List<int>();
                            if (drops.ContainsKey(mob.Identifier)) mob.Drops = drops[mob.Identifier];
                            else mob.Drops = new List<MobDropData>();

                            mobs.Add(identifier, mob);
                        }
                    }

                    writer.Write(mobs.Count);

                    foreach (MobData mob in mobs.Values)
                    {
                        mob.Save(writer);
                    }

                    Console.WriteLine("Mobs: {0}", mobs.Count);
                }
            }
        }
    }
}
