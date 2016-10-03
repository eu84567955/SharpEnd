using SharpEnd.Utility;

namespace SharpEnd.Players
{
    internal sealed class PlayerItem
    {
        private Player m_player;

        public int Identifier { get; private set; }

        public short Slot { get; set; }
        public bool IsEquipped => Slot < 0;

        public ushort Quantity { get; set; }

        public byte Slots { get; private set; }
        public byte Scrolls { get; private set; }
        public ushort Strength { get; private set; }
        public ushort Dexterity { get; private set; }
        public ushort Intelligence { get; private set; }
        public ushort Luck { get; private set; }
        public ushort Health { get; private set; }
        public ushort MaxHealth { get; private set; }
        public ushort Mana { get; private set; }
        public ushort MaxMana { get; private set; }
        public ushort WeaponAttack { get; private set; }
        public ushort MagicAttack { get; private set; }
        public ushort WeaponDefense { get; private set; }
        public ushort MagicDefense { get; private set; }
        public ushort Accuracy { get; private set; }
        public ushort Avoidability { get; private set; }
        public ushort Hands { get; private set; }
        public ushort Speed { get; private set; }
        public ushort Jump { get; private set; }
        public string Owner { get; private set; }
        public ushort Flags { get; private set; }

        public EInventoryType Inventory => GameLogicUtilities.GetInventory(Identifier);

        public PlayerItem(Player player, DatabaseQuery query)
        {
            m_player = player;

            Identifier = query.Get<int>("item_identifier");

            Slot = query.Get<short>("inventory_slot");

            Quantity = query.Get<ushort>("quantity");
        }

        public void Save()
        {

        }
    }
}
