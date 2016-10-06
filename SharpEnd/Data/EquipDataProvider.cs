using reNX;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Data
{
    internal sealed class EquipDataProvider
    {
        private Dictionary<int, EquipData> m_equips;

        public EquipDataProvider()
        {
            m_equips = new Dictionary<int, EquipData>();
        }

        private static List<string> skippedCategories = new List<string>()
        {
            "Afterimage"
        };

        public void Load()
        {
            using (NXFile file = new NXFile(Path.Combine("nx", "Character.nx")))
            {
                foreach (var category in file.BaseNode)
                {
                    if (category.Name.Contains(".img") || skippedCategories.Contains(category.Name))
                    {
                        continue;
                    }

                    foreach (var node in category)
                    {
                        int identifier = node.GetIdentifier<int>();

                        EquipData item = new EquipData();

                        item.Identifier = identifier;

                        // NOTE: Equip 1102380 is duplicated
                        if (m_equips.ContainsKey(identifier))
                        {
                            continue;
                        }

                        m_equips.Add(identifier, item);
                    }
                }
            }
        }

        public EquipData this[int identifier]
        {
            get
            {
                return m_equips.GetOrDefault(identifier, null);
            }
        }
    }

    internal sealed class EquipData
    {
        public int Identifier { get; set; }
    }
}
