using System.Collections.Generic;
using static SharpEnd.Game.Data.NpcData;

namespace SharpEnd.Game.Shops
{
    public sealed class Shop : List<ShopItem>
    {
        public int ID { get; private set; }

        /*public Shop(NpcShopData data)
        {
            ID = data.ID;
            data.Items.ForEach(i => Add(new ShopItem(i)));
        }*/
    }
}
