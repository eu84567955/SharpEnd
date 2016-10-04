using reNX;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Data
{
    internal sealed class ItemDataProvider
    {
        private Dictionary<int, ItemData> m_items;

        public ItemDataProvider()
        {
            m_items = new Dictionary<int, ItemData>();
        }

        private static List<string> skippedCategories = new List<string>()
        {
            "ItemOption.img", "ItemSellPriceStandard.img", "Pet", "SkillOption.img", "ThothSearchOption.img"
        };

        public void Load()
        {
            using (NXFile file = new NXFile(Path.Combine(Application.NXPath, "Item.nx")))
            {
                foreach (var category in file.BaseNode)
                {
                    if (skippedCategories.Contains(category.Name))
                    {
                        continue;
                    }

                    foreach (var container in category)
                    {
                        foreach (var node in container)
                        {
                            int identifier = node.GetIdentifier<int>();

                            ItemData item = new ItemData();

                            item.Identifier = identifier;

                            // NOTE: Items 5530052/5530053 are duplicated
                            if (m_items.ContainsKey(identifier))
                            {
                                continue;
                            }

                            m_items.Add(identifier, item);
                        }
                    }
                }
            }

            Log.Inform($"Loaded {m_items.Count} items.");
        }

        public ItemData this[int identifier]
        {
            get
            {
                return m_items.GetOrDefault(identifier, null);
            }
        }
    }

    internal sealed class ItemData
    {
        public int Identifier { get; set; }
        public int Price { get; set; }
        public bool Cash { get; set; }
        public ushort MaxSlot { get; set; }
        public short HP { get; set; }
        public short MP { get; set; }
        public short HPRate { get; set; }
        public short MPRate { get; set; }
        public short WeaponAttack { get; set; }
        public short MagicAttack { get; set; }
        public short Accuracy { get; set; }
        public short Avoidance { get; set; }
        public short Speed { get; set; }
        public int BuffTime { get; set; }

        public byte CureFlags { get; set; }

        public int MoveTo { get; set; }
        public int Mesos { get; set; }

        public byte ScrollSuccessRate { get; set; }
        public byte ScrollCurseRate { get; set; }
        public byte IncStr { get; set; }
        public byte IncDex { get; set; }
        public byte IncInt { get; set; }
        public byte IncLuk { get; set; }
        public byte IncMHP { get; set; }
        public byte IncMMP { get; set; }
        public byte IncWAtk { get; set; }
        public byte IncMAtk { get; set; }
        public byte IncWDef { get; set; }
        public byte IncMDef { get; set; }
        public byte IncAcc { get; set; }
        public byte IncAvo { get; set; }
        public byte IncJump { get; set; }
        public byte IncSpeed { get; set; }

        //public List<ItemSummonInfo> Summons { get; set; }
    }
}
