using reNX;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Data
{
    internal sealed class MobDataProvider
    {
        private Dictionary<int, MobData> m_mobs;

        public MobDataProvider()
        {
            m_mobs = new Dictionary<int, MobData>();
        }

        public void Load()
        {
            using (NXFile file = new NXFile(Path.Combine("nx", "Mob.nx")))
            {
                foreach (var node in file.BaseNode)
                {
                    if (node.Name == "QuestCountGroup")
                    {
                        continue;
                    }

                    int identifier = node.GetIdentifier<int>();

                    MobData item = new MobData();

                    item.Identifier = identifier;

                    m_mobs.Add(identifier, item);
                }
            }
        }

        public MobData this[int identifier]
        {
            get
            {
                return m_mobs.GetOrDefault(identifier, null);
            }
        }
    }

    public class MobData
    {
        public int Identifier { get; set; }
    }
}