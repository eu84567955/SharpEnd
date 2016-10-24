using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Game.Data
{
    internal sealed class EquipDataProvider
    {
        private static EquipDataProvider instance;

        public static EquipDataProvider Instance { get { return instance ?? (instance = new EquipDataProvider()); } }

        private Dictionary<int, EquipData> m_equips;

        private EquipDataProvider()
        {
            m_equips = new Dictionary<int, EquipData>();
        }

        public void LoadData()
        {
            using (FileStream stream = File.OpenRead(Path.Combine("data", "equips.bin")))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();

                    while (count-- > 0)
                    {
                        EquipData equip = new EquipData();
                        equip.Load(reader);
                        m_equips[equip.ID] = equip;
                    }
                }
            }
        }

        public bool IsValidEquip(int equipID)
        {
            return m_equips.ContainsKey(equipID);
        }

        public EquipData GetEquipData(int equipID)
        {
            return m_equips[equipID];
        }
    }

    #region Data Classes
    public sealed class EquipData
    {
        private int m_id;
        private byte m_slots;
        private byte m_requiredLevel;
        private short m_requiredJob;
        private short m_requiredSpecialJob;
        private short m_requiredStrength;
        private short m_requiredDexterity;
        private short m_requiredIntelligence;
        private short m_requiredLuck;
        private short m_strength;
        private short m_dexterity;
        private short m_intelligence;
        private short m_luck;
        private short m_weaponAttack;
        private short m_magicAttack;
        private short m_weaponDefense;
        private short m_magicDefense;
        private short m_hp;
        private short m_mp;
        private short m_accuracy;
        private short m_avoidability;
        private short m_hands;
        private short m_jump;
        private short m_speed;
        private short m_charmExperience;
        private short m_charismaExperience;
        private short m_willExperience;
        private short m_insightExperience;
        private short m_senseExperience;
        private short m_craftExperience;

        public int ID { get { return m_id; } set { m_id = value; } }
        public byte Slots { get { return m_slots; } set { m_slots = value; } }
        public byte RequiredLevel { get { return m_requiredLevel; } set { m_requiredLevel = value; } }
        public short RequiredJob { get { return m_requiredJob; } set { m_requiredJob = value; } }
        public short RequiredSpecialJob { get { return m_requiredSpecialJob; } set { m_requiredSpecialJob = value; } }
        public short RequiredStrength { get { return m_requiredStrength; } set { m_requiredStrength = value; } }
        public short RequiredDexterity { get { return m_requiredDexterity; } set { m_requiredDexterity = value; } }
        public short RequiredIntelligence { get { return m_requiredIntelligence; } set { m_requiredIntelligence = value; } }
        public short RequiredLuck { get { return m_requiredLuck; } set { m_requiredLuck = value; } }
        public short Strength { get { return m_strength; } set { m_strength = value; } }
        public short Dexterity { get { return m_dexterity; } set { m_dexterity = value; } }
        public short Intelligence { get { return m_intelligence; } set { m_intelligence = value; } }
        public short Luck { get { return m_luck; } set { m_luck = value; } }
        public short WeaponAttack { get { return m_weaponAttack; } set { m_weaponAttack = value; } }
        public short MagicAttack { get { return m_magicAttack; } set { m_magicAttack = value; } }
        public short WeaponDefense { get { return m_weaponDefense; } set { m_weaponDefense = value; } }
        public short MagicDefense { get { return m_magicDefense; } set { m_magicDefense = value; } }
        public short HP { get { return m_hp; } set { m_hp = value; } }
        public short MP { get { return m_mp; } set { m_mp = value; } }
        public short Accuracy { get { return m_accuracy; } set { m_accuracy = value; } }
        public short Avoidability { get { return m_avoidability; } set { m_avoidability = value; } }
        public short Hands { get { return m_hands; } set { m_hands = value; } }
        public short Jump { get { return m_jump; } set { m_jump = value; } }
        public short Speed { get { return m_speed; } set { m_speed = value; } }
        public short CharmExperience { get { return m_charmExperience; } set { m_charmExperience = value; } }
        public short CharismaExperience { get { return m_charismaExperience; } set { m_charismaExperience = value; } }
        public short WillExperience { get { return m_willExperience; } set { m_willExperience = value; } }
        public short InsightExperience { get { return m_insightExperience; } set { m_insightExperience = value; } }
        public short SenseExperience { get { return m_senseExperience; } set { m_senseExperience = value; } }
        public short CraftExperience { get { return m_craftExperience; } set { m_craftExperience = value; } }

        public void Load(BinaryReader reader)
        {
            m_id = reader.ReadInt32();
            m_slots = reader.ReadByte();
            m_requiredLevel = reader.ReadByte();
            m_requiredJob = reader.ReadInt16();
            m_requiredSpecialJob = reader.ReadInt16();
            m_requiredStrength = reader.ReadInt16();
            m_requiredDexterity = reader.ReadInt16();
            m_requiredIntelligence = reader.ReadInt16();
            m_requiredLuck = reader.ReadInt16();
            m_strength = reader.ReadInt16();
            m_dexterity = reader.ReadInt16();
            m_intelligence = reader.ReadInt16();
            m_luck = reader.ReadInt16();
            m_weaponAttack = reader.ReadInt16();
            m_magicAttack = reader.ReadInt16();
            m_weaponDefense = reader.ReadInt16();
            m_magicDefense = reader.ReadInt16();
            m_hp = reader.ReadInt16();
            m_mp = reader.ReadInt16();
            m_accuracy = reader.ReadInt16();
            m_avoidability = reader.ReadInt16();
            m_hands = reader.ReadInt16();
            m_jump = reader.ReadInt16();
            m_speed = reader.ReadInt16();
            m_charmExperience = reader.ReadInt16();
            m_charismaExperience = reader.ReadInt16();
            m_willExperience = reader.ReadInt16();
            m_insightExperience = reader.ReadInt16();
            m_senseExperience = reader.ReadInt16();
            m_craftExperience = reader.ReadInt16();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_id);
            writer.Write(m_slots);
            writer.Write(m_requiredLevel);
            writer.Write(m_requiredJob);
            writer.Write(m_requiredSpecialJob);
            writer.Write(m_requiredStrength);
            writer.Write(m_requiredDexterity);
            writer.Write(m_requiredIntelligence);
            writer.Write(m_requiredLuck);
            writer.Write(m_strength);
            writer.Write(m_dexterity);
            writer.Write(m_intelligence);
            writer.Write(m_luck);
            writer.Write(m_weaponAttack);
            writer.Write(m_magicAttack);
            writer.Write(m_weaponDefense);
            writer.Write(m_magicDefense);
            writer.Write(m_hp);
            writer.Write(m_mp);
            writer.Write(m_accuracy);
            writer.Write(m_avoidability);
            writer.Write(m_hands);
            writer.Write(m_jump);
            writer.Write(m_speed);
            writer.Write(m_charmExperience);
            writer.Write(m_charismaExperience);
            writer.Write(m_willExperience);
            writer.Write(m_insightExperience);
            writer.Write(m_senseExperience);
            writer.Write(m_craftExperience);
        }
    }
    #endregion
}
