using reNX;
using reNX.NXProperties;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Data
{
    internal sealed class QuestDataProvider
    {
        private Dictionary<ushort, QuestData> m_quests;

        public QuestDataProvider()
        {
            m_quests = new Dictionary<ushort, QuestData>();
        }

        public void Load()
        {
            using (NXFile file = new NXFile(Path.Combine("nx", "Quest.nx")))
            {
                LoadQuestInfo(file.BaseNode["QuestInfo.img"]);
            }
        }

        private void LoadQuestInfo(NXNode infoNode)
        {
            foreach (var node in infoNode)
            {
                QuestData quest = new QuestData();

                ushort identifier = node.GetIdentifier<ushort>();

                quest.Identifier = identifier;
                quest.Name = node.GetString("name");
                quest.AutoStart = node.GetBoolean("autoStart");
                quest.AutoComplete = node.GetBoolean("autoComplete");

                m_quests.Add(identifier, quest);
            }
        }

        public QuestData this[ushort identifier]
        {
            get
            {
                return m_quests.GetOrDefault(identifier, null);
            }
        }

        public bool IsAutoAlertQuest(ushort questIdentifier)
        {
            return IsAutoStartQuest(questIdentifier) || IsAutoCompletionAlertQuest(questIdentifier);
        }

        public bool IsAutoCompletionAlertQuest(ushort questIdentifier)
        {
            return m_quests[questIdentifier].AutoComplete;
        }

        public bool IsAutoStartQuest(ushort questIdentifier)
        {
            return m_quests[questIdentifier].AutoStart;
        }
    }

    internal sealed class QuestData
    {
        public ushort Identifier { get; set; }
        public string Name { get; set; }
        public bool AutoAccept { get; set; }
        public bool AutoStart { get; set; }
        public bool AutoComplete { get; set; }
    }
}