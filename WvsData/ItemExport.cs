using reNX;
using reNX.NXProperties;
using SharpEnd.Game.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WvsData
{
    internal static class ItemExport
    {
        public static void Export(string equipInputPath, string itemInputPath, string itemOutputPath, string petOutputPath)
        {
            NXNode petNode = null;

            Dictionary<int, ItemData> items = new Dictionary<int, ItemData>();

            using (NXFile file = new NXFile(equipInputPath))
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

                        int identifier = node.GetIdentifier<int>();

                        if (items.ContainsKey(identifier))
                        {
                            continue;
                        }

                        NXNode infoNode = node["info"];

                        ItemData item = new ItemData();

                        item.Identifier = identifier;
                        item.IsOnly = infoNode.GetBoolean("only");
                        item.IsNotSale = infoNode.GetBoolean("notSale");
                        item.IsCash = infoNode.GetBoolean("cash");
                        item.IsTradeBlock = infoNode.GetBoolean("tradeBlock");
                        item.IsAccountSharable = infoNode.GetBoolean("accountSharable");
                        item.IsQuest = infoNode.GetBoolean("quest");
                        item.MaxSlotQuantity = infoNode.GetUShort("maxSlot", 1);
                        item.SalePrice = infoNode.GetInt("price");

                        ItemEquipData equip = new ItemEquipData();

                        equip.Slots = infoNode.GetByte("tuc");
                        equip.ReqLevel = infoNode.GetByte("reqLevel");
                        equip.ReqStr = infoNode.GetShort("reqSTR");
                        equip.ReqDex = infoNode.GetShort("reqDEX");
                        equip.ReqInt = infoNode.GetShort("reqINT");
                        equip.ReqLuk = infoNode.GetShort("reqLUK");
                        equip.Strength = infoNode.GetShort("incSTR");
                        equip.Dexterity = infoNode.GetShort("incINT");
                        equip.Intelligence = infoNode.GetShort("incINT");
                        equip.Luck = infoNode.GetShort("incLUK");
                        equip.WeaponAttack = infoNode.GetShort("incPAD");
                        equip.MagicAttack = infoNode.GetShort("incMAD");
                        equip.WeaponDefense = infoNode.GetShort("incPDD");
                        equip.MagicDefense = infoNode.GetShort("incMDD");
                        equip.MaxHealth = infoNode.GetShort("incMHP");
                        equip.MaxMana = infoNode.GetShort("incMMP");
                        equip.Accuracy = infoNode.GetShort("incACC");
                        equip.Avoidability = infoNode.GetShort("incEVA");
                        equip.Hands = infoNode.GetShort("incHands"); // TODO: Validate this.
                        equip.Speed = infoNode.GetShort("incSpeed");
                        equip.Jump = infoNode.GetShort("incJump");

                        item.Equip = equip;

                        items.Add(identifier, item);
                    }
                }
            }

            using (NXFile file = new NXFile(itemInputPath))
            {
                foreach (NXNode category in file.BaseNode)
                {
                    switch (category.Name)
                    {
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

                                        if (items.ContainsKey(identifier))
                                        {
                                            continue;
                                        }

                                        NXNode infoNode = node["info"];

                                        ItemData item = new ItemData();

                                        item.Identifier = identifier;
                                        item.IsOnly = infoNode.GetBoolean("only");
                                        item.IsNotSale = infoNode.GetBoolean("notSale");
                                        item.IsCash = infoNode.GetBoolean("cash");
                                        item.IsTradeBlock = infoNode.GetBoolean("tradeBlock");
                                        item.IsAccountSharable = infoNode.GetBoolean("accountSharable");
                                        item.IsQuest = infoNode.GetBoolean("quest");
                                        item.MaxSlotQuantity = (ushort)(Math.Min(infoNode.GetInt("slotMax", 1), ushort.MaxValue));
                                        item.SalePrice = infoNode.GetInt("price");

                                        if (node.ContainsChild("spec"))
                                        {
                                            NXNode specNode = node["spec"];

                                            ItemConsumeData consume = new ItemConsumeData();

                                            consume.HpR = specNode.GetShort("hpR");
                                            consume.MpR = specNode.GetShort("mpR");
                                            consume.Hp = specNode.GetInt("hp");
                                            consume.Mp = specNode.GetInt("mp");
                                            consume.Speed = specNode.GetInt("speed");
                                            consume.Time = specNode.GetInt("time");

                                            consume.CharismaExp = specNode.GetInt("charismaEXP");
                                            consume.CharmExp = specNode.GetInt("charmEXP");
                                            consume.CraftExp = specNode.GetInt("craftEXP");
                                            consume.InsightExp = specNode.GetInt("insightEXP");
                                            consume.SenseExp = specNode.GetInt("senseEXP");
                                            consume.WillExp = specNode.GetInt("willEXP");

                                            item.Consume = consume;
                                        }

                                        items.Add(identifier, item);
                                    }
                                }
                            }
                            break;

                        case "Pet":
                            {
                                PetExport.Export(category, petOutputPath);
                            }
                            break;
                    }
                }
            }

            Console.Write(" > Exporting items... ");

            foreach (ItemData item in items.Values)
            {
                using (FileStream stream = File.Create(Path.Combine(itemOutputPath, item.Identifier.ToString() + ".shd")))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII))
                    {
                        item.Save(writer);
                    }
                }
            }

            Console.WriteLine("\t\tDone ({0}).", items.Count);
        }
    }
}