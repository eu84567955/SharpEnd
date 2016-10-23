using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Game.Data
{
    internal sealed class QuestDataProvider
    {
        private static QuestDataProvider instance;

        public static QuestDataProvider Instance { get { return instance ?? (instance = new QuestDataProvider()); } }

        private Dictionary<int, QuestData> m_quests;

        private QuestDataProvider()
        {
            m_quests = new Dictionary<int, QuestData>();
        }

        public void LoadData()
        {
            using (FileStream stream = File.OpenRead(Path.Combine("data", "quests.bin")))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();

                    while (count-- > 0)
                    {
                        QuestData quest = new QuestData();
                        quest.Load(reader);
                        m_quests[quest.ID] = quest;
                    }
                }
            }
        }
    }

    #region Data Classes
    public sealed class QuestData
    {
        private int m_id;
        private string m_name;
        private bool m_autoStart;
        private bool m_autoPreComplete;
        private QuestRewardData m_startRewards;
        private QuestRewardData m_completeRewards;

        public int ID { get { return m_id; } set { m_id = value; } }
        public string Name { get { return m_name; } set { m_name = value; } }
        public bool AutoStart { get { return m_autoStart; } set { m_autoStart = value; } }
        public bool AutoPreComplete { get { return m_autoPreComplete; } set { m_autoPreComplete = value; } }
        public QuestRewardData StartRewards { get { return m_startRewards; } set { m_startRewards = value; } }
        public QuestRewardData CompleteRewards { get { return m_completeRewards; } set { m_completeRewards = value; } }

        public void Load(BinaryReader reader)
        {
            m_id = reader.ReadInt32();
            m_name = reader.ReadString();
            m_autoStart = reader.ReadBoolean();
            m_autoPreComplete = reader.ReadBoolean();

            if (reader.ReadBoolean())
            {
                m_startRewards = new QuestRewardData();
                m_startRewards.Load(reader);
            }

            if (reader.ReadBoolean())
            {
                m_completeRewards = new QuestRewardData();
                m_completeRewards.Load(reader);
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_id);
            writer.Write(m_name);
            writer.Write(m_autoStart);
            writer.Write(m_autoPreComplete);

            writer.Write(m_startRewards != null);
            if (m_startRewards != null) m_startRewards.Save(writer);

            writer.Write(m_completeRewards != null);
            if (m_completeRewards != null) m_completeRewards.Save(writer);
        }
    }

    public sealed class QuestRewardData
    {
        private int m_nextQuest;
        private int m_experience;
        private int m_meso;
        private int m_fame;
        private int m_petTameness;
        private int m_petSpeed;
        private int m_buff;
        private string m_info;
        private string m_message;
        private string m_npcAction;
        private List<long> m_maps;
        private List<QuestRewardItemData> m_items;
        private List<QuestRewardSkillData> m_skills;

        public int NextQuest { get { return m_nextQuest; } set { m_nextQuest = value; } }
        public int Experience { get { return m_experience; } set { m_experience = value; } }
        public int Meso { get { return m_meso; } set { m_meso = value; } }
        public int Fame { get { return m_fame; } set { m_fame = value; } }
        public int PetTameness { get { return m_petTameness; } set { m_petTameness = value; } }
        public int PetSpeed { get { return m_petSpeed; } set { m_petSpeed = value; } }
        public int Buff { get { return m_buff; } set { m_buff = value; } }
        public string Info { get { return m_info; } set { m_info = value; } }
        public string Message { get { return m_message; } set { m_message = value; } }
        public string NpcAction { get { return m_npcAction; } set { m_npcAction = value; } }
        public List<long> Maps { get { return m_maps; } set { m_maps = value; } }
        public List<QuestRewardItemData> Items { get { return m_items; } set { m_items = value; } }
        public List<QuestRewardSkillData> Skills { get { return m_skills; } set { m_skills = value; } }

        public void Load(BinaryReader reader)
        {
            m_nextQuest = reader.ReadInt32();
            m_experience = reader.ReadInt32();
            m_meso = reader.ReadInt32();
            m_fame = reader.ReadInt32();
            m_petTameness = reader.ReadInt32();
            m_petSpeed = reader.ReadInt32();
            m_buff = reader.ReadInt32();
            m_info = reader.ReadString();
            m_message = reader.ReadString();
            m_npcAction = reader.ReadString();

            m_items = new List<QuestRewardItemData>();
            int itemsCount = reader.ReadInt32();
            while (itemsCount-- > 0)
            {
                QuestRewardItemData item = new QuestRewardItemData();
                item.Load(reader);
                m_items.Add(item);
            }

            m_skills = new List<QuestRewardSkillData>();
            int skillsCount = reader.ReadInt32();
            while (skillsCount-- > 0)
            {
                QuestRewardSkillData skill = new QuestRewardSkillData();
                skill.Load(reader);
                m_skills.Add(skill);
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_nextQuest);
            writer.Write(m_experience);
            writer.Write(m_meso);
            writer.Write(m_fame);
            writer.Write(m_petTameness);
            writer.Write(m_petSpeed);
            writer.Write(m_buff);
            writer.Write(m_info);
            writer.Write(m_message);
            writer.Write(m_npcAction);

            writer.Write(m_items.Count);
            m_items.ForEach(i => i.Save(writer));

            writer.Write(m_skills.Count);
            m_skills.ForEach(s => s.Save(writer));
        }
    }

    public sealed class QuestRewardItemData
    {
        private bool m_named;
        private bool m_resignRemove;
        private bool m_untilMidnight;
        private int m_id;
        private int m_quantity;
        private int m_meso;
        private int m_period;
        private int m_gender;
        private int m_probRate;
        private int m_potentialGrade;
        private int m_potentialCount;
        private int m_madeBySkill;
        private int m_additionalOptCount;
        private int m_additionalGrade;
        private ulong m_dateExpire;

        public bool Named { get { return m_named; } set { m_named = value; } }
        public bool ResignRemove { get { return m_named; } set { m_named = value; } }
        public bool UntilMidnight { get { return m_named; } set { m_named = value; } }
        public int Id { get { return m_id; } set { m_id = value; } }
        public int Quantity { get { return m_quantity; } set { m_quantity = value; } }
        public int Meso { get { return m_meso; } set { m_meso = value; } }
        public int Period { get { return m_period; } set { m_period = value; } }
        public int Gender { get { return m_gender; } set { m_gender = value; } }
        public int ProbRate { get { return m_probRate; } set { m_probRate = value; } }
        public int PotentialGrade { get { return m_potentialGrade; } set { m_potentialGrade = value; } }
        public int PotentialCount { get { return m_potentialCount; } set { m_potentialCount = value; } }
        public int MadeBySkill { get { return m_madeBySkill; } set { m_madeBySkill = value; } }
        public int AdditionalOptCount { get { return m_additionalOptCount; } set { m_additionalOptCount = value; } }
        public int AdditionalGrade { get { return m_additionalGrade; } set { m_additionalGrade = value; } }
        public ulong DateExpire { get { return m_dateExpire; } set { m_dateExpire = value; } }

        public void Load(BinaryReader reader)
        {
            m_named = reader.ReadBoolean();
            m_resignRemove = reader.ReadBoolean();
            m_untilMidnight = reader.ReadBoolean();
            m_id = reader.ReadInt32();
            m_quantity = reader.ReadInt32();
            m_meso = reader.ReadInt32();
            m_period = reader.ReadInt32();
            m_gender = reader.ReadInt32();
            m_probRate = reader.ReadInt32();
            m_potentialGrade = reader.ReadInt32();
            m_potentialCount = reader.ReadInt32();
            m_madeBySkill = reader.ReadInt32();
            m_additionalOptCount = reader.ReadInt32();
            m_additionalGrade = reader.ReadInt32();
            m_dateExpire = reader.ReadUInt64();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_named);
            writer.Write(m_resignRemove);
            writer.Write(m_untilMidnight);
            writer.Write(m_id);
            writer.Write(m_quantity);
            writer.Write(m_meso);
            writer.Write(m_period);
            writer.Write(m_gender);
            writer.Write(m_probRate);
            writer.Write(m_potentialGrade);
            writer.Write(m_potentialCount);
            writer.Write(m_madeBySkill);
            writer.Write(m_additionalOptCount);
            writer.Write(m_additionalGrade);
            writer.Write(m_dateExpire);
        }
    }

    public sealed class QuestRewardSkillData
    {
        private int m_id;
        private int m_level;
        private int m_masterLevel;
        private bool m_onlyMasterLevel;
        private List<long> m_jobs;

        public int Id { get { return m_id; } set { m_id = value; } }
        public int Level { get { return m_level; } set { m_level = value; } }
        public int MasterLevel { get { return m_masterLevel; } set { m_masterLevel = value; } }
        public bool OnlyMasterLevel { get { return m_onlyMasterLevel; } set { m_onlyMasterLevel = value; } }
        public List<long> Jobs { get { return m_jobs; } set { m_jobs = value; } }

        public void Load(BinaryReader reader)
        {
            m_id = reader.ReadInt32();
            m_level = reader.ReadInt32();
            m_masterLevel = reader.ReadInt32();
            m_onlyMasterLevel = reader.ReadBoolean();

            m_jobs = new List<long>();
            int jobsCount = reader.ReadInt32();
            while (jobsCount-- > 0)
            {
                m_jobs.Add(reader.ReadInt64());
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_id);
            writer.Write(m_level);
            writer.Write(m_masterLevel);
            writer.Write(m_onlyMasterLevel);

            writer.Write(m_jobs.Count);
            m_jobs.ForEach(j => writer.Write(j));
        }
    }

    public sealed class QuestRequirementData
    {
        private byte m_minLevel;
        private byte m_maxLevel;
        private short m_minPop;
        private short m_maxPop;
        private int m_minMeso;
        private int m_minInsight;
        private int m_minCharisma;
        private int m_minCharm;
        private int m_minCraft;
        private int m_minWill;
        private string m_startScript;
        private string m_endScript;
        private List<QuestRequirementMobData> m_mobs;
        private List<QuestRequirementSkillData> m_skills;

        public void Load(BinaryReader reader)
        {
            m_minLevel = reader.ReadByte();
            m_maxLevel = reader.ReadByte();
            m_minPop = reader.ReadInt16();
            m_maxPop = reader.ReadInt16();
            m_minMeso = reader.ReadInt32();
            m_minInsight = reader.ReadInt32();
            m_minCharisma = reader.ReadInt32();
            m_minCharm = reader.ReadInt32();
            m_minCraft = reader.ReadInt32();
            m_minWill = reader.ReadInt32();
            m_startScript = reader.ReadString();
            m_endScript = reader.ReadString();

            m_mobs = new List<QuestRequirementMobData>();
            int mobsCount = reader.ReadInt32();
            while (mobsCount-- > 0)
            {
                QuestRequirementMobData mob = new QuestRequirementMobData();
                mob.Load(reader);
                m_mobs.Add(mob);
            }

            m_skills = new List<QuestRequirementSkillData>();
            int skillsCount = reader.ReadInt32();
            while (skillsCount-- > 0)
            {
                QuestRequirementSkillData skill = new QuestRequirementSkillData();
                skill.Load(reader);
                m_skills.Add(skill);
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_minLevel);
            writer.Write(m_maxLevel);
            writer.Write(m_minPop);
            writer.Write(m_maxPop);
            writer.Write(m_minMeso);
            writer.Write(m_minInsight);
            writer.Write(m_minCharisma);
            writer.Write(m_minCharm);
            writer.Write(m_minCraft);
            writer.Write(m_minWill);
            writer.Write(m_startScript);
            writer.Write(m_endScript);

            writer.Write(m_mobs.Count);
            m_mobs.ForEach(m => m.Save(writer));

            writer.Write(m_skills.Count);
            m_skills.ForEach(s => s.Save(writer));
        }
    }

    public sealed class QuestRequirementMobData
    {
        private int m_id;
        private short m_amount;

        public int Id { get { return m_id; } set { m_id = value; } }
        public short Amount { get { return m_amount; } set { m_amount = value; } }

        public void Load(BinaryReader reader)
        {
            m_id = reader.ReadInt32();
            m_amount = reader.ReadInt16();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_id);
            writer.Write(m_amount);
        }
    }

    public sealed class QuestRequirementSkillData
    {
        public void Load(BinaryReader reader)
        {

        }

        public void Save(BinaryWriter writer)
        {

        }
    }
    #endregion
}
