using System.Collections.Generic;
using static SharpEnd.Game.Data.NpcData;

namespace SharpEnd.Game.Maps
{
    internal sealed class Shop : List<ShopItem>
    {
        public int Identifier { get; private set; }

        public Shop(NpcShopData data)
        {
            Identifier = data.Identifier;
            data.Items.ForEach(i => Add(new ShopItem(i)));
        }
    }

    internal sealed class ShopItem
    {
        public int Type { get; private set; }
        public int Discount { get; private set; }
        public bool WorldBlock { get; private set; }
        public ushort MinimumLevel { get; private set; }
        public ushort MaximumLevel { get; private set; }
        public ushort MaxPerSlot { get; private set; }
        public ushort Quantity { get; private set; }
        public int ItemIdentifier { get; private set; }
        public int Price { get; private set; }
        public int TokenItemIdentifier { get; private set; }
        public int TokenPrice { get; private set; }
        public int PointQuestIdentifier { get; private set; }
        public int PointPrice { get; private set; }
        public int StarCoin { get; private set; }
        public int QuestExIdentifier { get; private set; }
        public int QuestExValue { get; private set; }
        public int TimePeriod { get; private set; }
        public int LevelLimited { get; private set; }
        public int QuestIdentifier { get; private set; }
        public int TabIndex { get; private set; }
        public int PotentialGrade { get; private set; }
        public int BuyLimit { get; private set; }
        public string QuestExKey { get; private set; }

        public ShopItem(NpcShopItemData data)
        {
            Type = data.Type;
            Discount = data.Discount;
            WorldBlock = data.WorldBlock;
            MinimumLevel = data.MinimumLevel;
            MaximumLevel = data.MaximumLevel;
            MaxPerSlot = data.MaxPerSlot;
            Quantity = data.Quantity;
            ItemIdentifier = data.ItemIdentifier;
            Price = data.Price;
            TokenItemIdentifier = data.TokenItemIdentifier;
            TokenPrice = data.TokenPrice;
            PointQuestIdentifier = data.PointQuestIdentifier;
            PointPrice = data.PointPrice;
            StarCoin = data.StarCoin;
            QuestExIdentifier = data.QuestExIdentifier;
            QuestExValue = data.QuestExValue;
            TimePeriod = data.TimePeriod;
            LevelLimited = data.LevelLimited;
            QuestIdentifier = data.QuestIdentifier;
            TabIndex = data.TabIndex;
            PotentialGrade = data.PotentialGrade;
            BuyLimit = data.BuyLimit;
            QuestExKey = string.Empty;
        }
    }
}
