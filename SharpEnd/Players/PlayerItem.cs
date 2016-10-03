using MySql.Data.MySqlClient;
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

            Slots = query.Get<byte>("slots");
            Scrolls = query.Get<byte>("scrolls");
            Strength = query.Get<ushort>("strength");
            Dexterity = query.Get<ushort>("dexterity");
            Intelligence = query.Get<ushort>("intelligence");
            Luck = query.Get<ushort>("luck");
            Health = query.Get<ushort>("health");
            Mana = query.Get<ushort>("mana");
            WeaponAttack = query.Get<ushort>("weapon_attack");
            MagicAttack = query.Get<ushort>("magic_attack");
            WeaponDefense = query.Get<ushort>("weapon_defense");
            MagicDefense = query.Get<ushort>("magic_defense");
            Accuracy = query.Get<ushort>("accuracy");
            Avoidability = query.Get<ushort>("avoidability");
            Hands = query.Get<ushort>("hands");
            Speed = query.Get<ushort>("speed");
            Jump = query.Get<ushort>("jump");
            Owner = query.Get<string>("owner");
            Flags = query.Get<ushort>("flags");
        }

        public void Save()
        {
            Database.Execute(@"INSERT INTO `player_item` 
                             VALUES(@player_identifier, @item_identifier, @inventory_slot, @quantity, @slots, @scrolls, @strength, @dexterity, @intelligence, @luck, @health, @mana, @weapon_attack, @magic_attack, @weapon_defense, @magic_defense, @accuracy, @avoidability, @hands, @speed, @jump, @owner, @flags);",
                             new MySqlParameter("player_identifier", m_player.Identifier),
                             new MySqlParameter("inventory_slot", Slot),
                             new MySqlParameter("item_identifier", Identifier),
                             new MySqlParameter("quantity", Quantity),
                             new MySqlParameter("slots", Slots),
                             new MySqlParameter("scrolls", Scrolls),
                             new MySqlParameter("strength", Strength),
                             new MySqlParameter("dexterity", Dexterity),
                             new MySqlParameter("intelligence", Intelligence),
                             new MySqlParameter("luck", Luck),
                             new MySqlParameter("health", Health),
                             new MySqlParameter("mana", Mana),
                             new MySqlParameter("weapon_attack", WeaponAttack),
                             new MySqlParameter("magic_attack", MagicAttack),
                             new MySqlParameter("weapon_defense", WeaponDefense),
                             new MySqlParameter("magic_defense", MagicDefense),
                             new MySqlParameter("accuracy", Accuracy),
                             new MySqlParameter("avoidability", Avoidability),
                             new MySqlParameter("hands", Hands),
                             new MySqlParameter("speed", Speed),
                             new MySqlParameter("jump", Jump),
                             new MySqlParameter("owner", Owner),
                             new MySqlParameter("flags", Flags));
        }
    }
}
