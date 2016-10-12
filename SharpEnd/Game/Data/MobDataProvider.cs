using reNX;
using reNX.NXProperties;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Data
{
    internal sealed class MobData
    {
        public bool IsBoss { get; set; }
        public bool HasFFALoot { get; set; }
        public bool HasExplosiveReward { get; set; }
        public int Level { get; set; }
        public int WeaponAttack { get; set; }
        public int MagicAttack { get; set; }
        public int WeaponDefense { get; set; }
        public int MagicDefense { get; set; }
        public int PDRate { get; set; }
        public int MDRate { get; set; }
        public int MaxHealth { get; set; }
        public int MaxMana { get; set; }
        public int Accuracy { get; set; }
        public int Avoidability { get; set; }
        public int Speed { get; set; }
        public int KnockbackDistance { get; set; }
        public int Experience { get; set; }
        public int Invincible { get; set; }
        public int FixedDamage { get; set; }
        public int SummonType { get; set; }
        public string Name { get; set; }
        public List<MobSkillData> Skills = new List<MobSkillData>();

        public void Load(NXNode node)
        {
            IsBoss = node.GetBoolean("boss");
            HasFFALoot = node.GetBoolean("publicReward");
            HasExplosiveReward = node.GetBoolean("explosiveReward");
            Level = node.GetInt("level");
            WeaponAttack = node.GetInt("PADamage");
            MagicAttack = node.GetInt("MADamage");
            WeaponDefense = node.GetInt("PDDamage");
            MagicDefense = node.GetInt("MDDamage");
            PDRate = node.GetInt("PDRate");
            MDRate = node.GetInt("MDRate");

            try
            {
                MaxHealth = node.GetInt("maxHP");
            }
            catch
            {
                MaxHealth = 0; // NOTE: Some mobs have hp listed as question marks. Figure out what it is.
            }

            MaxMana = node.GetInt("maxMP");
            Accuracy = node.GetInt("acc");
            Avoidability = node.GetInt("eva");
            Speed = node.GetInt("speed");
            KnockbackDistance = node.GetInt("pushed");
            Experience = node.GetInt("exp");
            Invincible = node.GetInt("invincible");
            FixedDamage = node.GetInt("fixedDamage");
            SummonType = node.GetInt("summonType");
        }
    }

    internal sealed class MobDataProvider : Dictionary<int, MobData>
    {
        public MobDataProvider() : base() { }

        public void Load()
        {
            NXFile file = new NXFile(Path.Combine("nx", "Mob.nx"));

            foreach (NXNode node in file.BaseNode)
            {
                if (node.Name == "QuestCountGroup")
                {
                    continue;
                }

                if (!node.ContainsChild("info"))
                {
                    continue;
                }

                NXNode infoNode = node["info"];

                int identifier = node.GetIdentifier<int>();

                if (ContainsKey(identifier))
                {
                    continue;
                }

                MobData mob = new MobData();

                mob.Load(infoNode);

                Add(identifier, mob);
            }

            file.Dispose();
        }
    }
}