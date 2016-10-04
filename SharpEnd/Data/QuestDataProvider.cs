using reNX;
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
            using (NXFile file = new NXFile(Path.Combine(Application.NXPath, "Quest.nx")))
            {

            }

            Log.Inform($"Loaded {m_quests.Count} quests.");
        }

        public QuestData this[ushort identifier]
        {
            get
            {
                return m_quests.GetOrDefault(identifier, null);
            }
        }
    }

    internal sealed class QuestData
    {
        public ushort Identifier { get; set; }
    }
}