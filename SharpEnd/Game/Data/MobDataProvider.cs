using System;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Game.Data
{
    internal sealed class MobDataProvider
    {
        private static MobDataProvider instance;

        public static MobDataProvider Instance { get { return instance ?? (instance = new MobDataProvider()); } }

        private Dictionary<int, MobData> m_mobs;

        private MobDataProvider()
        {
            m_mobs = new Dictionary<int, MobData>();
        }

        public void LoadData()
        {
            using (FileStream stream = File.OpenRead(Path.Combine("data", "mobs.bin")))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();

                    while (count-- > 0)
                    {
                        MobData mob = new MobData();
                        mob.Load(reader);
                        m_mobs.Add(mob.ID, mob);
                    }
                }
            }
        }

        public bool IsValidMob(int mobID)
        {
            return m_mobs.ContainsKey(mobID);
        }

        public MobData GetMobData(int mobID)
        {
            return m_mobs[mobID];
        }
    }

    #region Data Classes
    public sealed class MobData
    {
        private int m_id;
        private byte m_level;
        private int m_maxHp;
        private int m_maxMp;
        private int m_experience;

        /*
        private Dictionary<Element, byte> m_elemAttr;
        private Dictionary<int, byte> m_loseItems;
        private List<int> m_summons;
        private Dictionary<byte, MobAttackData> m_attacks;
        private List<MobSkillData> m_skills;
        private Dictionary<string, int> m_delays;
        private List<ItemDropEntry> m_itemDrops;
        private bool m_undead;
        private int m_removeAfter;
        private byte m_deathAnimation;
        private bool m_hideHp;
        private bool m_hideName;
        private byte m_hpTagColor;
        private byte m_hpTagBgColor;
        private bool m_boss;
        private int m_sd;
        private bool m_invincible;
        private bool m_firstAttack;
        private int m_buff;
        private MesoDropChance m_mesoDrop;
        private byte m_dropItemPeriod;*/

        public MobData()
        {
            //m_elemAttr = new Dictionary<Element, byte>();
            //m_loseItems = new Dictionary<int, byte>();
            //m_summons = new List<int>();
            //m_attacks = new Dictionary<byte, MobAttackData>();
            //m_skills = new List<MobSkillData>();
            //m_delays = new Dictionary<string, int>();
            //m_itemDrops = new List<ItemDropEntry>();
            //m_removeAfter = -1;
            //m_deathAnimation = 2;
        }

        public int ID { get { return m_id; } set { m_id = value; } }
        public byte Level { get { return m_level; } set { m_level = value; } }
        public int MaxHP { get { return m_maxHp; } set { m_maxHp = value; } }
        public int MaxMP { get { return m_maxMp; } set { m_maxMp = value; } }
        public int Experience { get { return m_experience; } set { m_experience = value; } }

        //public int PhysicalAttack { get { return m_pad; } set { m_pad = value; } }
        //public bool Undead { get { return m_undead; } set { m_undead = value; } }
        //public int RemoveAfter { get { return m_removeAfter; } set { m_removeAfter = value; } }
        //public byte DestroyAnimation { get { return m_deathAnimation; } set { m_deathAnimation = value; } }
        //public bool HideHP { get { return m_hideHp; } set { m_hideHp = value; } }
        //public bool HideName { get { return m_hideName; } set { m_hideName = value; } }
        //public byte HpTagColor { get { return m_hpTagColor; } set { m_hpTagColor = value; } }
        //public byte HpTagBgColor { get { return m_hpTagBgColor; } set { m_hpTagBgColor = value; } }
        //public bool Boss { get { return m_boss; } set { m_boss = value; } }
        //public int SelfDestructHp { get { return m_sd; } set { m_sd = value; } }
        //public bool Invincible { get { return m_invincible; } set { m_invincible = value; } }
        //public bool FirstAttack { get { return m_firstAttack; } set { m_firstAttack = value; } }
        //public int DeathBuff { get { return m_buff; } set { m_buff = value; } }
        //public byte DropItemPeriod { get { return m_dropItemPeriod; } set { m_dropItemPeriod = value; } }

        public void Load(BinaryReader reader)
        {
            m_id = reader.ReadInt32();
            m_level = reader.ReadByte();
            m_maxHp = reader.ReadInt32();
            m_maxMp = reader.ReadInt32();
            m_experience = reader.ReadInt32();

            //m_pad = reader.ReadInt32();
            //m_undead = reader.ReadBoolean();
            //m_removeAfter = reader.ReadInt32();
            //m_deathAnimation = reader.ReadByte();
            //m_hideHp = reader.ReadBoolean();
            //m_hideName = reader.ReadBoolean();
            //m_hpTagColor = reader.ReadByte();
            //m_hpTagBgColor = reader.ReadByte();
            //m_boss = reader.ReadBoolean();
            //m_sd = reader.ReadInt32();
            //m_invincible = reader.ReadBoolean();
            //m_firstAttack = reader.ReadBoolean();
            //m_buff = reader.ReadInt32();
            //m_dropItemPeriod = reader.ReadByte();

            //int elementAttributesCount = reader.ReadInt32();
            //while (elementAttributesCount-- > 0)
            //{
            //    Element element = (Element)reader.ReadByte();
            //    byte resistance = reader.ReadByte();

            //    m_elemAttr.Add(element, resistance);
            //}

            //int loseItemsCount = reader.ReadInt32();
            //while (loseItemsCount-- > 0)
            //{
            //    int itemId = reader.ReadInt32();
            //    byte prob = reader.ReadByte();

            //    m_loseItems.Add(itemId, prob);
            //}

            //int summonsCount = reader.ReadInt32();
            //while (summonsCount-- > 0)
            //{
            //    m_summons.Add(reader.ReadInt32());
            //}

            //int attacksCount = reader.ReadInt32();
            //while (attacksCount-- > 0)
            //{
            //    byte attackId = reader.ReadByte();

            //    MobAttackData attack = new MobAttackData();

            //    attack.Load(reader);

            //    m_attacks.Add(attackId, attack);
            //}

            //int skillsCount = reader.ReadInt32();
            //while (skillsCount-- > 0)
            //{
            //    MobSkillData skill = new MobSkillData();

            //    skill.Load(reader);

            //    m_skills.Add(skill);
            //}

            //int delaysCount = reader.ReadInt32();
            //while (delaysCount-- > 0)
            //{
            //    string name = reader.ReadString();
            //    int delay = reader.ReadInt32();

            //    m_delays.Add(name, delay);
            //}

            //int itemDropsCount = reader.ReadInt32();
            //while (itemDropsCount-- > 0)
            //{
            //    ItemDropEntry itemDrop = new ItemDropEntry();

            //    itemDrop.Load(reader);

            //    m_itemDrops.Add(itemDrop);
            //}
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_id);
            writer.Write(m_level);
            writer.Write(m_maxHp);
            writer.Write(m_maxMp);
            writer.Write(m_experience);
        }
    }

        /*
        public byte GetElementalResistance(Element element)
        {
            byte value = 0;

            m_elemAttr.TryGetValue(element, out value);

            return value;
        }

        public void SetElementalAttribute(string attribute)
        {
            for (int i = 0; i < attribute.Length; i += 2)
            {
                Element element;

                switch (attribute[i])
                {
                    case 'F': element = Element.Fire; break;
                    case 'I': element = Element.Ice; break;
                    case 'L': element = Element.Lightning; break;
                    case 'S': element = Element.Poison; break;
                    case 'H': element = Element.Holy; break;
                    case 'P': element = Element.Physical; break;
                    default: throw new ArgumentException("Unknown element char " + attribute[i]);
                }

                byte resistance = (byte)(attribute[i + 1] - '0');

                m_elemAttr.Add(element, resistance);
            }
        }

        public void AddLoseItem(int itemId, byte prob)
        {
            m_loseItems.Add(itemId, prob);
        }

        public List<int> GetItemsToTake()
        {
            List<int> items = new List<int>();

            // TODO: Loop through all of m_loseItems entries.
            // Generate a number between 0 and 100. If it's smaller
            // than the item's prob, add it to the list.

            return items;
        }

        public void AddSummon(int mobId)
        {
            m_summons.Add(mobId);
        }

        public List<int> GetSummons()
        {
            return m_summons;
        }

        public void AddAttack(byte attackId, MobAttackData attack)
        {
            m_attacks.Add(attackId, attack);
        }

        public Dictionary<byte, MobAttackData> GetAttacks()
        {
            return m_attacks;
        }

        public void AddSkill(MobSkillData skill)
        {
            m_skills.Add(skill);
        }

        public List<MobSkillData> GetSkills()
        {
            return m_skills;
        }

        public void AddDelay(string name, int delay)
        {
            m_delays.Add(name, delay);
        }

        public Dictionary<string, int> GetDelays()
        {
            return m_delays;
        }

        // TODO: GetItemsToDrop => List<Item>.

        public void AddItemDrop(int itemId, int chance, short min, short max)
        {
            m_itemDrops.Add(new ItemDropEntry(itemId, chance, min, max));
        }

        public void AddItemDrop(int itemId, int chance, short min, short max, short questId)
        {
            m_itemDrops.Add(new ItemDropEntry(itemId, chance, min, max, questId));
        }

        /// <summary>
        /// Randomly select an amount of meso for this mob to drop when killed,
        /// using the chances that have been given in SetMesoDrop.
        /// </summary>
        /// <returns>The amount of meso that this mob will drop. If this mob should not drop any meso, 0 will be returned.</returns>
        public int GetMesoToDrop()
        {
            return 0;
        }

        public void SetMesoDrop(int chance, int min, int max)
        {
            m_mesoDrop = new MesoDropChance(chance, min, max);
        }
    }

    public sealed class ItemDropEntry
    {
        private int m_itemId;
        private int m_chance;
        private short m_min;
        private short m_max;
        private short m_questId;

        public ItemDropEntry() { }

        public ItemDropEntry(int itemId, int chance, short min, short max, short questId)
        {
            m_itemId = itemId;
            m_chance = chance;
            m_min = min;
            m_max = max;
            m_questId = questId;
        }

        public ItemDropEntry(int itemId, int chance, short min, short max) : this(itemId, chance, min, max, 0) { }

        public int ItemId { get { return m_itemId; } }
        public int DropChance { get { return m_chance; } set { m_chance = value; } }
        public short MinQuantity { get { return m_min; } set { m_min = value; } }
        public short MaxQuantity { get { return m_min; } set { m_max = value; } }
        public short QuestId { get { return m_questId; } set { m_questId = value; } }

        public void Load(BinaryReader reader)
        {
            m_itemId = reader.ReadInt32();
            m_chance = reader.ReadInt32();
            m_min = reader.ReadInt16();
            m_max = reader.ReadInt16();
            m_questId = reader.ReadInt16();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_itemId);
            writer.Write(m_chance);
            writer.Write(m_min);
            writer.Write(m_max);
            writer.Write(m_questId);
        }
    }

    public sealed class MesoDropChance
    {
        private int m_chance;
        private int m_min;
        private int m_max;

        public MesoDropChance(int chance, int min, int max)
        {
            m_chance = chance;
            m_min = min;
            m_max = max;
        }

        public int DropChance { get { return m_chance; } set { m_chance = value; } }
        public int MinMesoDrop { get { return m_min; } set { m_min = value; } }
        public int MaxMesoDrop { get { return m_min; } set { m_max = value; } }
    }

    public sealed class MobAttackData
    {
        private bool m_deadlyAttack;
        private short m_mpBurn;
        private byte m_diseaseSkill;
        private byte m_diseaseLevel;
        private int m_mpCon;

        public bool DeadlyAttack { get { return m_deadlyAttack; } set { m_deadlyAttack = value; } }
        public short MpBurn { get { return m_mpBurn; } set { m_mpBurn = value; } }
        public byte DiseaseSkill { get { return m_diseaseSkill; } set { m_diseaseSkill = value; } }
        public byte DiseaseLevel { get { return m_diseaseLevel; } set { m_diseaseLevel = value; } }
        public int MpConsume { get { return m_mpCon; } set { m_mpCon = value; } }

        public void Load(BinaryReader reader)
        {
            m_deadlyAttack = reader.ReadBoolean();
            m_mpBurn = reader.ReadInt16();
            m_diseaseSkill = reader.ReadByte();
            m_diseaseLevel = reader.ReadByte();
            m_mpCon = reader.ReadInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_deadlyAttack);
            writer.Write(m_mpBurn);
            writer.Write(m_diseaseSkill);
            writer.Write(m_diseaseLevel);
            writer.Write(m_mpCon);
        }
    }

    public sealed class MobSkillData
    {
        private short m_id;
        private byte m_level;
        private short m_effectDelay;

        public short Id { get { return m_id; } set { m_id = value; } }
        public byte Level { get { return m_level; } set { m_level = value; } }
        public short EffectDelay { get { return m_effectDelay; } set { m_effectDelay = value; } }

        public void Load(BinaryReader reader)
        {
            m_id = reader.ReadInt16();
            m_level = reader.ReadByte();
            m_effectDelay = reader.ReadInt16();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_id);
            writer.Write(m_level);
            writer.Write(m_effectDelay);
        }
    }*/
    #endregion
}
