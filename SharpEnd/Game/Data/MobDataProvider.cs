using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpEnd.Data
{
    public sealed class MobData
    {
        public sealed class MobDropData
        {
            public int ItemIdentifier { get; set; }
            public int Minimum { get; set; }
            public int Maximum { get; set; }
            public ushort QuestIdentifier { get; set; }
            public int Chance { get; set; }
            
            public void Read(BinaryReader reader)
            {
                ItemIdentifier = reader.ReadInt32();
                Minimum = reader.ReadInt32();
                Maximum = reader.ReadInt32();
                QuestIdentifier = reader.ReadUInt16();
                Chance = reader.ReadInt32();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(ItemIdentifier);
                writer.Write(Minimum);
                writer.Write(Maximum);
                writer.Write(QuestIdentifier);
                writer.Write(Chance);
            }
        }

        public int Identifier { get; set; }
        //public EMobFlags Flags { get; set; }
        public byte Level { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int HealthRecovery { get; set; }
        public int ManaRecovery { get; set; }
        public int ExplodeHP { get; set; }
        public int Experience { get; set; }
        public int LinkIdentifier { get; set; }
        public byte SummonType { get; set; }
        public int Knockback { get; set; }
        public ushort FixedDamage { get; set; }
        public int DeathBuffIdentifier { get; set; }
        public int DeathAfter { get; set; }
        public byte Traction { get; set; }
        public int DamagedBySkillIdentifierOnly { get; set; }
        public int DamagedByMobIdentifierOnly { get; set; }
        public byte HPBarColor { get; set; }
        public byte HPBarBackgroundColor { get; set; }
        public byte CarnivalPoints { get; set; }
        public ushort PhysicalAttack { get; set; }
        public ushort PhysicalDefense { get; set; }
        public ushort MagicalAttack { get; set; }
        public ushort MagicalDefense { get; set; }
        public short Accuracy { get; set; }
        public ushort Avoidance { get; set; }
        public short Speed { get; set; }
        public short ChaseSpeed { get; set; }
        //public EMobMagicModifier IceModifier { get; set; }
        //public EMobMagicModifier FireModifier { get; set; }
        //public EMobMagicModifier PoisonModifier { get; set; }
        //public EMobMagicModifier LightningModifier { get; set; }
        //public EMobMagicModifier HolyModifier { get; set; }
        //public EMobMagicModifier NonElementalModifier { get; set; }
        //public List<MobAbilityData> Abilities { get; set; }
        //public List<MobAttackData> Attacks { get; set; }
        public List<int> Summons { get; set; }
        public List<MobDropData> Drops { get; set; }

        public void Read(BinaryReader reader)
        {
            Identifier = reader.ReadInt32();
            //Flags = (EMobFlags)reader.ReadUInt16();
            Level = reader.ReadByte();
            Health = reader.ReadInt32();
            Mana = reader.ReadInt32();
            HealthRecovery = reader.ReadInt32();
            ManaRecovery = reader.ReadInt32();
            ExplodeHP = reader.ReadInt32();
            Experience = reader.ReadInt32();
            LinkIdentifier = reader.ReadInt32();
            SummonType = reader.ReadByte();
            Knockback = reader.ReadInt32();
            FixedDamage = reader.ReadUInt16();
            DeathBuffIdentifier = reader.ReadInt32();
            DeathAfter = reader.ReadInt32();
            Traction = reader.ReadByte();
            DamagedBySkillIdentifierOnly = reader.ReadInt32();
            DamagedByMobIdentifierOnly = reader.ReadInt32();
            HPBarColor = reader.ReadByte();
            HPBarBackgroundColor = reader.ReadByte();
            CarnivalPoints = reader.ReadByte();
            PhysicalAttack = reader.ReadUInt16();
            PhysicalDefense = reader.ReadUInt16();
            MagicalAttack = reader.ReadUInt16();
            MagicalDefense = reader.ReadUInt16();
            Accuracy = reader.ReadInt16();
            Avoidance = reader.ReadUInt16();
            Speed = reader.ReadInt16();
            ChaseSpeed = reader.ReadInt16();
            /*IceModifier = (EMobMagicModifier)reader.ReadByte();
            FireModifier = (EMobMagicModifier)reader.ReadByte();
            PoisonModifier = (EMobMagicModifier)reader.ReadByte();
            LightningModifier = (EMobMagicModifier)reader.ReadByte();
            HolyModifier = (EMobMagicModifier)reader.ReadByte();
            NonElementalModifier = (EMobMagicModifier)reader.ReadByte();*/

            /*int abilitiesCount = reader.ReadInt32();
            Abilities = new List<MobAbilityData>(abilitiesCount);
            while (abilitiesCount-- > 0)
            {
                MobAbilityData ability = new MobAbilityData();
                ability.Load(reader);
                Abilities.Add(ability);
            }

            int attacksCount = reader.ReadInt32();
            Attacks = new List<MobAttackData>(attacksCount);
            while (attacksCount-- > 0)
            {
                MobAttackData attack = new MobAttackData();
                attack.Load(reader);
                Attacks.Add(attack);
            }*/

            int summonsCount = reader.ReadInt32();
            Summons = new List<int>(summonsCount);
            while (summonsCount-- > 0) Summons.Add(reader.ReadInt32());

            int dropsCount = reader.ReadInt32();
            Drops = new List<MobDropData>(dropsCount);
            while (dropsCount-- > 0)
            {
                MobDropData drop = new MobDropData();
                drop.Read(reader);
                Drops.Add(drop);
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Identifier);
            //writer.Write((ushort)Flags);
            writer.Write(Level);
            writer.Write(Health);
            writer.Write(Mana);
            writer.Write(HealthRecovery);
            writer.Write(ManaRecovery);
            writer.Write(ExplodeHP);
            writer.Write(Experience);
            writer.Write(LinkIdentifier);
            writer.Write(SummonType);
            writer.Write(Knockback);
            writer.Write(FixedDamage);
            writer.Write(DeathBuffIdentifier);
            writer.Write(DeathAfter);
            writer.Write(Traction);
            writer.Write(DamagedBySkillIdentifierOnly);
            writer.Write(DamagedByMobIdentifierOnly);
            writer.Write(HPBarColor);
            writer.Write(HPBarBackgroundColor);
            writer.Write(CarnivalPoints);
            writer.Write(PhysicalAttack);
            writer.Write(PhysicalDefense);
            writer.Write(MagicalAttack);
            writer.Write(MagicalDefense);
            writer.Write(Accuracy);
            writer.Write(Avoidance);
            writer.Write(Speed);
            writer.Write(ChaseSpeed);
            /*writer.Write((byte)IceModifier);
            writer.Write((byte)FireModifier);
            writer.Write((byte)PoisonModifier);
            writer.Write((byte)LightningModifier);
            writer.Write((byte)HolyModifier);
            writer.Write((byte)NonElementalModifier);*/

            /*writer.Write(Abilities.Count);
            Abilities.ForEach(a => a.Save(writer));

            writer.Write(Attacks.Count);
            Attacks.ForEach(a => a.Save(writer));*/

            writer.Write(Summons.Count);
            Summons.ForEach(s => writer.Write(s));

            writer.Write(Drops.Count);
            Drops.ForEach(d => d.Write(writer));
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