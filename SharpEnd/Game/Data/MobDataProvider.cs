using reNX;
using reNX.NXProperties;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpEnd.Data
{
    public sealed class MobData
    {
        public int Identifier;
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

        public void Read(BinaryReader reader)
        {
            Identifier = reader.ReadInt32();
            IsBoss = reader.ReadBoolean();
            HasFFALoot = reader.ReadBoolean();
            HasExplosiveReward = reader.ReadBoolean();
            Level = reader.ReadInt32();
            WeaponAttack = reader.ReadInt32();
            MagicAttack = reader.ReadInt32();
            WeaponDefense = reader.ReadInt32();
            MagicDefense = reader.ReadInt32();
            PDRate = reader.ReadInt32();
            MDRate = reader.ReadInt32();
            MaxHealth = reader.ReadInt32();
            MaxMana = reader.ReadInt32();
            Accuracy = reader.ReadInt32();
            Avoidability = reader.ReadInt32();
            Speed = reader.ReadInt32();
            KnockbackDistance = reader.ReadInt32();
            Experience = reader.ReadInt32();
            Invincible = reader.ReadInt32();
            FixedDamage = reader.ReadInt32();
            SummonType = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Identifier);
            writer.Write(IsBoss);
            writer.Write(HasFFALoot);
            writer.Write(HasExplosiveReward);
            writer.Write(Level);
            writer.Write(WeaponAttack);
            writer.Write(MagicAttack);
            writer.Write(WeaponDefense);
            writer.Write(MagicDefense);
            writer.Write(PDRate);
            writer.Write(MDRate);
            writer.Write(MaxHealth);
            writer.Write(MaxMana);
            writer.Write(Accuracy);
            writer.Write(Avoidability);
            writer.Write(Speed);
            writer.Write(KnockbackDistance);
            writer.Write(Experience);
            writer.Write(Invincible);
            writer.Write(FixedDamage);
            writer.Write(SummonType);
        }
    }

    internal sealed class MobDataProvider : Dictionary<int, MobData>
    {
        public MobDataProvider() : base() { }

        public void Load()
        {
            using (FileStream stream = File.Open("data/Mobs.bin", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII))
                {
                    int count = reader.ReadInt32();

                    while (count-- > 0)
                    {
                        MobData mob = new MobData();

                        mob.Read(reader);

                        Add(mob.Identifier, mob);
                    }
                }
            }
        }
    }
}