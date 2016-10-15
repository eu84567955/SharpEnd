using SharpEnd.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpEnd.Game.Data
{
    #region Data Classes
    public sealed class NpcData
    {
        public sealed class NpcShopData
        {
            public int Identifier { get; set; }
            public List<NpcShopItemData> Items { get; set; }

            public void Load(BinaryReader reader)
            {
                Identifier = reader.ReadInt32();

                int itemCount = reader.ReadInt32();

                Items = new List<NpcShopItemData>();
                while (itemCount-- > 0)
                {
                    NpcShopItemData item = new NpcShopItemData();
                    item.Load(reader);
                    Items.Add(item);
                }
            }

            public void Save(BinaryWriter writer)
            {
                writer.Write(Identifier);
                writer.Write(Items.Count);
                Items.ForEach(i => i.Save(writer));
            }
        }

        public sealed class NpcShopItemData
        {
            public int Type { get; set; }
            public int Discount { get; set; }
            public bool WorldBlock { get; set; }
            public ushort MinimumLevel { get; set; }
            public ushort MaximumLevel { get; set; }
            public ushort MaxPerSlot { get; set; }
            public ushort Quantity { get; set; }
            public int ItemIdentifier { get; set; }
            public int Price { get; set; }
            public int TokenItemIdentifier { get; set; }
            public int TokenPrice { get; set; }
            public int PointQuestIdentifier { get; set; }
            public int PointPrice { get; set; }
            public int StarCoin { get; set; }
            public int QuestExIdentifier { get; set; }
            public int QuestExValue { get; set; }
            public int TimePeriod { get; set; }
            public int LevelLimited { get; set; }
            public int QuestIdentifier { get; set; }
            public int TabIndex { get; set; }
            public int PotentialGrade { get; set; }
            public int BuyLimit { get; set; }
            public string QuestExKey { get; set; }

            public void Load(BinaryReader reader)
            {
                Type = reader.ReadInt32();
                Discount = reader.ReadInt32();
                WorldBlock = reader.ReadBoolean();
                MinimumLevel = reader.ReadUInt16();
                MaximumLevel = reader.ReadUInt16();
                MaxPerSlot = reader.ReadUInt16();
                Quantity = reader.ReadUInt16();
                ItemIdentifier = reader.ReadInt32();
                Price = reader.ReadInt32();
                TokenItemIdentifier = reader.ReadInt32();
                TokenPrice = reader.ReadInt32();
                PointQuestIdentifier = reader.ReadInt32();
                PointPrice = reader.ReadInt32();
                StarCoin = reader.ReadInt32();
                QuestExIdentifier = reader.ReadInt32();
                QuestExValue = reader.ReadInt32();
                TimePeriod = reader.ReadInt32();
                LevelLimited = reader.ReadInt32();
                QuestIdentifier = reader.ReadInt32();
                TabIndex = reader.ReadInt32();
                PotentialGrade = reader.ReadInt32();
                BuyLimit = reader.ReadInt32();
                QuestExKey = string.Empty;
            }

            public void Save(BinaryWriter writer)
            {
                writer.Write(Type);
                writer.Write(Discount);
                writer.Write(WorldBlock);
                writer.Write(MinimumLevel);
                writer.Write(MaximumLevel);
                writer.Write(MaxPerSlot);
                writer.Write(Quantity);
                writer.Write(ItemIdentifier);
                writer.Write(Price);
                writer.Write(TokenItemIdentifier);
                writer.Write(TokenPrice);
                writer.Write(PointQuestIdentifier);
                writer.Write(PointPrice);
                writer.Write(StarCoin);
                writer.Write(QuestExIdentifier);
                writer.Write(QuestExValue);
                writer.Write(TimePeriod);
                writer.Write(LevelLimited);
                writer.Write(QuestIdentifier);
                writer.Write(TabIndex);
                writer.Write(PotentialGrade);
                writer.Write(BuyLimit);
            }
        }

        public int Identifier { get; set; }
        public int StorageCost { get; set; }
        public string Script { get; set; }
        public NpcShopData Shop { get; set; }

        public void Load(BinaryReader reader)
        {
            Identifier = reader.ReadInt32();
            StorageCost = reader.ReadInt32();
            Script = reader.ReadString();
            if (reader.ReadBoolean()) (Shop = new NpcShopData()).Load(reader);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Identifier);
            writer.Write(StorageCost);
            writer.Write(Script);
            writer.Write(Shop != null);
            if (Shop != null) Shop.Save(writer);
        }
    }
    #endregion

    internal sealed class NpcDataProvider : SafeKeyedCollection<int, NpcData>
    {
        private static NpcDataProvider instance;

        public static NpcDataProvider Instance
        {
            get
            {
                return instance ?? (instance = new NpcDataProvider());
            }
        }

        private NpcDataProvider() : base() { }

        protected override int GetKeyForItem(NpcData item)
        {
            return item.Identifier;
        }

        public new NpcData this[int identifier]
        {
            get
            {
                if (!Contains(identifier))
                {
                    using (FileStream stream = File.Open(Path.Combine("data", "npcs", identifier.ToString() + ".shd"), FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII))
                        {
                            NpcData npc = new NpcData();

                            npc.Load(reader);

                            Add(npc);
                        }
                    }
                }

                return base[identifier];
            }
        }
    }
}
