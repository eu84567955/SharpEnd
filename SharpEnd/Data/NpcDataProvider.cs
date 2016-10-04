using reNX;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Data
{
    internal sealed class NpcDataProvider
    {
        private Dictionary<int, NpcData> m_npcs;

        public NpcDataProvider()
        {
            m_npcs = new Dictionary<int, NpcData>();
        }

        public void Load()
        {
            using (NXFile file = new NXFile(Path.Combine(Application.NXPath, "Npc.nx")))
            {
                foreach (var node in file.BaseNode)
                {
                    int identifier = node.GetIdentifier<int>();

                    NpcData item = new NpcData();

                    item.Identifier = identifier;

                    m_npcs.Add(identifier, item);
                }
            }

            Log.Inform($"Loaded {m_npcs.Count} npcs.");
        }

        public NpcData this[int identifier]
        {
            get
            {
                return m_npcs.GetOrDefault(identifier, null);
            }
        }
    }

    internal sealed class NpcData
    {
        public int Identifier { get; set; }
    }
}