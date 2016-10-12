using reNX;
using reNX.NXProperties;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Data
{
    internal class ItemData
    {
        public bool IsOnly { get; set; }
        public bool IsNotSale { get; set; }
        public bool IsCash { get; set; }
        public bool IsTradeBlock { get; set; }
        public bool IsAccountSharable { get; set; }
        public bool IsQuest { get; set; }
        public ushort MaxSlotQuantity { get; set; }
        public int SalePrice { get; set; }
        public string Name { get; set; }

        public virtual void Load(NXNode infoNode, ushort maxSlotQuantity = 1)
        {
            IsOnly = infoNode.GetBoolean("only");
            IsNotSale = infoNode.GetBoolean("notSale");
            IsCash = infoNode.GetBoolean("cash");
            IsTradeBlock = infoNode.GetBoolean("tradeBlock");
            IsAccountSharable = infoNode.GetBoolean("accountSharable");
            IsQuest = infoNode.GetBoolean("quest");
            MaxSlotQuantity = infoNode.GetUShort("maxSlot", (ushort)(IsOnly ? 1 : maxSlotQuantity));
            SalePrice = infoNode.GetInt("price");
        }
    }

    internal sealed class ItemConsumeData : ItemData
    {
        public short HpR { get; set; }
        public short MpR { get; set; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int Speed { get; set; }
        public int Time { get; set; }
        public int MoveTo { get; set; }

        public int CraftExp { get; set; }
        public int CharmExp { get; set; }
        public int CharismaExp { get; set; }
        public int InsightExp { get; set; }
        public int WillExp { get; set; }
        public int SenseExp { get; set; }

        public void Load(NXNode infoNode, NXNode specNode, ushort maxSlotQuantity = 1)
        {
            base.Load(infoNode, maxSlotQuantity);

            HpR = specNode.GetShort("hpR");
            MpR = specNode.GetShort("mpR");
            Hp = specNode.GetInt("hp");
            Mp = specNode.GetInt("mp");
            Speed = specNode.GetInt("speed");
            Time = specNode.GetInt("time");

            CharismaExp = specNode.GetInt("charismaEXP");
            CharmExp = specNode.GetInt("charmEXP");
            CraftExp = specNode.GetInt("craftEXP");
            InsightExp = specNode.GetInt("insightEXP");
            SenseExp = specNode.GetInt("senseEXP");
            WillExp = specNode.GetInt("willEXP");
        }
    }

    internal sealed class ItemPetData : ItemData
    {
        public bool IsMultiable { get; set; }
        public bool IsPermanent { get; set; }
        public bool HasPickupItem { get; set; }
        public bool HasAutoBuff { get; set; }
        public int Life { get; set; }
        public int Hunger { get; set; }

        public override void Load(NXNode infoNode, ushort maxSlotQuantity = 1)
        {
            base.Load(infoNode, maxSlotQuantity);

            IsMultiable = infoNode.GetBoolean("multiPet");
            IsPermanent = infoNode.GetBoolean("permanent");
            HasPickupItem = infoNode.GetBoolean("pickupItem");
            HasAutoBuff = infoNode.GetBoolean("autoBuff");
            Life = infoNode.GetInt("life");
            Hunger = infoNode.GetInt("hungry");
        }
    }

    internal sealed class ItemEquipData : ItemData
    {
        public byte Slots { get; set; }
        public byte ReqLevel { get; set; }
        public short ReqStr { get; set; }
        public short ReqDex { get; set; }
        public short ReqInt { get; set; }
        public short ReqLuk { get; set; }
        public short Strength { get; set; }
        public short Dexterity { get; set; }
        public short Intelligence { get; set; }
        public short Luck { get; set; }
        public short WeaponAttack { get; set; }
        public short MagicAttack { get; set; }
        public short WeaponDefense { get; set; }
        public short MagicDefense { get; set; }
        public short MaxHealth { get; set; }
        public short MaxMana { get; set; }
        public short Accuracy { get; set; }
        public short Avoidability { get; set; }
        public short Hands { get; set; }
        public short Speed { get; set; }
        public short Jump { get; set; }

        public override void Load(NXNode infoNode, ushort maxSlotQuantity = 1)
        {
            base.Load(infoNode, maxSlotQuantity);

            Slots = infoNode.GetByte("tuc");
            ReqLevel = infoNode.GetByte("reqLevel");
            ReqStr = infoNode.GetShort("reqSTR");
            ReqDex = infoNode.GetShort("reqDEX");
            ReqInt = infoNode.GetShort("reqINT");
            ReqLuk = infoNode.GetShort("reqLUK");
            Strength = infoNode.GetShort("incSTR");
            Dexterity = infoNode.GetShort("incINT");
            Intelligence = infoNode.GetShort("incINT");
            Luck = infoNode.GetShort("incLUK");
            WeaponAttack = infoNode.GetShort("incPAD");
            MagicAttack = infoNode.GetShort("incMAD");
            WeaponDefense = infoNode.GetShort("incPDD");
            MagicDefense = infoNode.GetShort("incMDD");
            MaxHealth = infoNode.GetShort("incMHP");
            MaxMana = infoNode.GetShort("incMMP");
            Accuracy = infoNode.GetShort("incACC");
            Avoidability = infoNode.GetShort("incEVA");
            Hands = infoNode.GetShort("incHands"); // TODO: Validate this.
            Speed = infoNode.GetShort("incSpeed");
            Jump = infoNode.GetShort("incJump");
        }
    }

    internal sealed class ItemDataProvider : Dictionary<int, ItemData>
    {
        public ItemDataProvider() : base() { }

        public void Load()
        {
            using (NXFile file = new NXFile(Path.Combine("nx", "Character.nx")))
            {
                foreach (NXNode category in file.BaseNode)
                {
                    if (category.Name.Contains(".img") ||
                        category.Name == "Afterimage" ||
                        category.Name == "Face" ||
                        category.Name == "Hair")
                    {
                        continue;
                    }

                    foreach (NXNode node in category)
                    {
                        if (!node.ContainsChild("info"))
                        {
                            continue;
                        }

                        NXNode infoNode = node["info"];

                        int identifier = node.GetIdentifier<int>();

                        if (ContainsKey(identifier))
                        {
                            continue;
                        }

                        ItemEquipData equip = new ItemEquipData();

                        equip.Load(infoNode);

                        Add(identifier, equip);
                    }
                }
            }

            using (NXFile file = new NXFile(Path.Combine("nx", "Item.nx")))
            {
                foreach (NXNode category in file.BaseNode)
                {
                    switch (category.Name)
                    {
                        case "ItemOption.img":
                            {
                                // TODO: Potential.
                            }
                            break;

                        case "Cash":
                        case "Consume":
                        case "Etc":
                        case "Install":
                            {
                                foreach (NXNode container in category)
                                {
                                    foreach (NXNode node in container)
                                    {
                                        if (!node.ContainsChild("info"))
                                        {
                                            continue;
                                        }

                                        int identifier = node.GetIdentifier<int>();

                                        if (ContainsKey(identifier))
                                        {
                                            continue;
                                        }

                                        NXNode infoNode = node["info"];

                                        if (node.ContainsChild("spec"))
                                        {
                                            NXNode specNode = node["spec"];

                                            ItemConsumeData consume = new ItemConsumeData();

                                            consume.Load(infoNode, specNode, 200);

                                            Add(identifier, consume);
                                        }
                                        else
                                        {
                                            ItemData item = new ItemData();

                                            item.Load(infoNode, 200);

                                            Add(identifier, item);
                                        }
                                    }
                                }
                            }
                            break;

                        case "Pet":
                            {
                                foreach (NXNode node in category)
                                {
                                    if (!node.ContainsChild("info"))
                                    {
                                        continue;
                                    }

                                    NXNode infoNode = node["info"];

                                    int identifier = node.GetIdentifier<int>();

                                    if (ContainsKey(identifier))
                                    {
                                        continue;
                                    }

                                    ItemPetData pet = new ItemPetData();

                                    pet.Load(infoNode);

                                    Add(identifier, pet);
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
}
