using static SharpEnd.Game.Data.NpcData;

namespace SharpEnd.Game.Shops
{
    public sealed class ShopItem
    {
        public int Type { get; private set; }
        public int Discount { get; private set; }
        public bool WorldBlock { get; private set; }
        public ushort MinimumLevel { get; private set; }
        public ushort MaximumLevel { get; private set; }
        public ushort MaxPerSlot { get; private set; }
        public ushort Quantity { get; private set; }
        public int ItemID { get; private set; }
        public int Price { get; private set; }
        public int TokenItemID { get; private set; }
        public int TokenPrice { get; private set; }
        public int PointQuestID { get; private set; }
        public int PointPrice { get; private set; }
        public int StarCoin { get; private set; }
        public int QuestExID { get; private set; }
        public int QuestExValue { get; private set; }
        public int TimePeriod { get; private set; }
        public int LevelLimited { get; private set; }
        public int QuestID { get; private set; }
        public int TabIndex { get; private set; }
        public int PotentialGrade { get; private set; }
        public int BuyLimit { get; private set; }
        public string QuestExKey { get; private set; }

        /*public ShopItem(NpcShopItemData data)
        {
            Type = data.Type;
            Discount = data.Discount;
            WorldBlock = data.WorldBlock;
            MinimumLevel = data.MinimumLevel;
            MaximumLevel = data.MaximumLevel;
            MaxPerSlot = data.MaxPerSlot;
            Quantity = data.Quantity;
            ItemID = data.ItemID;
            Price = data.Price;
            TokenItemID = data.TokenItemID;
            TokenPrice = data.TokenPrice;
            PointQuestID = data.PointQuestID;
            PointPrice = data.PointPrice;
            StarCoin = data.StarCoin;
            QuestExID = data.QuestExID;
            QuestExValue = data.QuestExValue;
            TimePeriod = data.TimePeriod;
            LevelLimited = data.LevelLimited;
            QuestID = data.QuestID;
            TabIndex = data.TabIndex;
            PotentialGrade = data.PotentialGrade;
            BuyLimit = data.BuyLimit;
            QuestExKey = string.Empty;
        }*/
    }
}