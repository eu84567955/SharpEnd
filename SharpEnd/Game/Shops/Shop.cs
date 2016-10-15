using System.Collections.Generic;
using static SharpEnd.Game.Data.NpcData;

namespace SharpEnd.Game.Shops
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
}
