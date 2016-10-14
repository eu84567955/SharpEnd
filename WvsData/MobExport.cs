using reNX;
using reNX.NXProperties;
using SharpEnd.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WvsData
{
    internal static class MobExport
    {
        public static void Export(string path)
        {
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
                            mob.IsBoss = infoNode.GetBoolean("boss");
                            mob.HasFFALoot = infoNode.GetBoolean("publicReward");
                            mob.HasExplosiveReward = infoNode.GetBoolean("explosiveReward");
                            mob.Level = infoNode.GetInt("level");
                            mob.WeaponAttack = infoNode.GetInt("PADamage");
                            mob.MagicAttack = infoNode.GetInt("MADamage");
                            mob.WeaponDefense = infoNode.GetInt("PDDamage");
                            mob.MagicDefense = infoNode.GetInt("MDDamage");
                            mob.PDRate = infoNode.GetInt("PDRate");
                            mob.MDRate = infoNode.GetInt("MDRate");

                            try
                            {
                                mob.MaxHealth = infoNode.GetInt("maxHP");
                            }
                            catch
                            {
                                mob.MaxHealth = 0; // NOTE: Some mobs have hp listed as question marks. Figure out what it is.
                            }

                            mob.MaxMana = infoNode.GetInt("maxMP");
                            mob.Accuracy = infoNode.GetInt("acc");
                            mob.Avoidability = infoNode.GetInt("eva");
                            mob.Speed = infoNode.GetInt("speed");
                            mob.KnockbackDistance = infoNode.GetInt("pushed");
                            mob.Experience = infoNode.GetInt("exp");
                            mob.Invincible = infoNode.GetInt("invincible");
                            mob.FixedDamage = infoNode.GetInt("fixedDamage");
                            mob.SummonType = infoNode.GetInt("summonType");

                            mobs.Add(identifier, mob);
                        }
                    }

                    writer.Write(mobs.Count);

                    foreach (MobData mob in mobs.Values)
                    {
                        mob.Write(writer);
                    }

                    Console.WriteLine("Mobs: {0}", mobs.Count);
                }
            }
        }
    }
}
