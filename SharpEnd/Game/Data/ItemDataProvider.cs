using SharpEnd.Collections;
using System.IO;
using System.Text;

namespace SharpEnd.Game.Data
{
    #region Data Classes
    public class ItemData
    {
        public int Identifier { get; set; }
        public bool IsOnly { get; set; }
        public bool IsNotSale { get; set; }
        public bool IsCash { get; set; }
        public bool IsTradeBlock { get; set; }
        public bool IsAccountSharable { get; set; }
        public bool IsQuest { get; set; }
        public ushort MaxSlotQuantity { get; set; }
        public int SalePrice { get; set; }

        public ItemEquipData Equip { get; set; }
        public ItemConsumeData Consume { get; set; }

        public void Load(BinaryReader reader)
        {
            Identifier = reader.ReadInt32();
            IsOnly = reader.ReadBoolean();
            IsNotSale = reader.ReadBoolean();
            IsCash = reader.ReadBoolean();
            IsTradeBlock = reader.ReadBoolean();
            IsAccountSharable = reader.ReadBoolean();
            IsQuest = reader.ReadBoolean();
            MaxSlotQuantity = reader.ReadUInt16();
            SalePrice = reader.ReadInt32();

            if (reader.ReadBoolean())
            {
                Equip = new ItemEquipData();

                Equip.Load(reader);
            }

            if (reader.ReadBoolean())
            {
                Consume = new ItemConsumeData();

                Consume.Load(reader);
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Identifier);
            writer.Write(IsOnly);
            writer.Write(IsNotSale);
            writer.Write(IsCash);
            writer.Write(IsTradeBlock);
            writer.Write(IsAccountSharable);
            writer.Write(IsQuest);
            writer.Write(MaxSlotQuantity);
            writer.Write(SalePrice);

            writer.Write(Equip != null);
            if (Equip != null) Equip.Save(writer);

            writer.Write(Consume != null);
            if (Consume != null) Consume.Save(writer);
        }
    }

    public sealed class ItemEquipData
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

        public void Load(BinaryReader reader)
        {
            Slots = reader.ReadByte();
            ReqLevel = reader.ReadByte();
            ReqStr = reader.ReadInt16();
            ReqDex = reader.ReadInt16();
            ReqInt = reader.ReadInt16();
            ReqLuk = reader.ReadInt16();
            Strength = reader.ReadInt16();
            Dexterity = reader.ReadInt16();
            Intelligence = reader.ReadInt16();
            Luck = reader.ReadInt16();
            WeaponAttack = reader.ReadInt16();
            MagicAttack = reader.ReadInt16();
            WeaponDefense = reader.ReadInt16();
            MagicDefense = reader.ReadInt16();
            MaxHealth = reader.ReadInt16();
            MaxMana = reader.ReadInt16();
            Accuracy = reader.ReadInt16();
            Avoidability = reader.ReadInt16();
            Hands = reader.ReadInt16();
            Speed = reader.ReadInt16();
            Jump = reader.ReadInt16();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Slots);
            writer.Write(ReqLevel);
            writer.Write(ReqStr);
            writer.Write(ReqDex);
            writer.Write(ReqInt);
            writer.Write(ReqLuk);
            writer.Write(Strength);
            writer.Write(Dexterity);
            writer.Write(Intelligence);
            writer.Write(Luck);
            writer.Write(WeaponAttack);
            writer.Write(MagicAttack);
            writer.Write(WeaponDefense);
            writer.Write(MagicDefense);
            writer.Write(MaxHealth);
            writer.Write(MaxMana);
            writer.Write(Accuracy);
            writer.Write(Avoidability);
            writer.Write(Hands);
            writer.Write(Speed);
            writer.Write(Jump);
        }
    }


    public sealed class ItemConsumeData
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

        public void Load(BinaryReader reader)
        {
            HpR = reader.ReadInt16();
            MpR = reader.ReadInt16();
            Hp = reader.ReadInt32();
            Mp = reader.ReadInt32();
            Speed = reader.ReadInt32();
            Time = reader.ReadInt32();
            MoveTo = reader.ReadInt32();

            CraftExp = reader.ReadInt32();
            CharmExp = reader.ReadInt32();
            CharismaExp = reader.ReadInt32();
            InsightExp = reader.ReadInt32();
            WillExp = reader.ReadInt32();
            SenseExp = reader.ReadInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(HpR);
            writer.Write(MpR);
            writer.Write(Hp);
            writer.Write(Mp);
            writer.Write(Speed);
            writer.Write(Time);
            writer.Write(MoveTo);

            writer.Write(CraftExp);
            writer.Write(CharmExp);
            writer.Write(CharismaExp);
            writer.Write(InsightExp);
            writer.Write(WillExp);
            writer.Write(SenseExp);
        }
    }
    #endregion

    internal sealed class ItemDataProvider : SafeKeyedCollection<int, ItemData>
    {
        private static ItemDataProvider instance;

        public static ItemDataProvider Instance
        {
            get
            {
                return instance ?? (instance = new ItemDataProvider());
            }
        }

        private ItemDataProvider() : base() { }

        protected override int GetKeyForItem(ItemData item)
        {
            return item.Identifier;
        }

        public new ItemData this[int identifier]
        {
            get
            {
                if (!Contains(identifier))
                {
                    using (FileStream stream = File.Open(Path.Combine("data", "items", identifier.ToString() + ".shd"), FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII))
                        {
                            ItemData item = new ItemData();

                            item.Load(reader);

                            Add(item);
                        }
                    }
                }

                return base[identifier];
            }
        }
    }
}
