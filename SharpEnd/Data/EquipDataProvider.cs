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

                        EquipData equip = new EquipData();

                        if (node.ContainsChild("info"))
                        {
                            var infoNode = node["info"];

                            equip.Slots = infoNode.GetByte("tuc");
                            equip.Strength = infoNode.GetShort("incSTR");
                            equip.Dexterity = infoNode.GetShort("incDEX");
                            equip.Intelligence = infoNode.GetShort("incINT");
                            equip.Luck = infoNode.GetShort("incLUK");
                            equip.Health = infoNode.GetShort("incHP");
                            equip.Mana = infoNode.GetShort("incMP");
                            equip.WeaponAttack = infoNode.GetShort("incPAD");
                            equip.MagicAttack = infoNode.GetShort("incMAD");
                            equip.WeaponDefense = infoNode.GetShort("incPDD");
                            equip.MagicDefense = infoNode.GetShort("incMDD");
                            equip.Accuracy = infoNode.GetShort("incACC");
                            equip.Avoidability = infoNode.GetShort("incEVA");
                            equip.Hands = infoNode.GetShort("incHands");
                            equip.Jump = infoNode.GetShort("incJump");
                            equip.Speed = infoNode.GetShort("incSpeed");
                            equip.RequiredLevel = infoNode.GetByte("reqLevel");
                            equip.RequiredStrength = infoNode.GetShort("reqSTR");
                            equip.RequiredDexterity = infoNode.GetShort("reqDEX");
                            equip.RequiredIntelligence = infoNode.GetShort("reqINT");
                            equip.RequiredLuck = infoNode.GetShort("reqLUK");
                            equip.RequiredFame = infoNode.GetShort("reqPOP");
                        }

                        // NOTE: Equip 1102380 is duplicated.
                        if (m_equips.ContainsKey(identifier))
                        {
                            continue;
                        }

                        m_equips.Add(identifier, equip);
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
        public byte Slots;
        public short Strength;
        public short Dexterity;
        public short Intelligence;
        public short Luck;
        public short Health;
        public short Mana;
        public short WeaponAttack;
        public short MagicAttack;
        public short WeaponDefense;
        public short MagicDefense;
        public short Accuracy;
        public short Avoidability;
        public short Hands;
        public short Jump;
        public short Speed;
        public byte RequiredLevel;
        public short RequiredStrength;
        public short RequiredDexterity;
        public short RequiredIntelligence;
        public short RequiredLuck;
        public short RequiredFame;
    }
}
