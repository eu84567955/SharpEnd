using MySql.Data.MySqlClient;
using SharpEnd.Game.Data;
using SharpEnd.Game.Maps;
using SharpEnd.Packets;
using SharpEnd.Utility;
using System;

namespace SharpEnd.Game.Players
{
    public sealed class PlayerItem : Drop
    {
        public PlayerItems Parent { get; set; }

        public int ID { get; private set; }
        public short Slot { get; set; }
        public ushort Quantity { get; set; }
        public DateTime Expiration { get; private set; }
        public byte Slots { get; private set; }
        public byte Scrolls { get; private set; }
        public short Strength { get; private set; }
        public short Dexterity { get; private set; }
        public short Intelligence { get; private set; }
        public short Luck { get; private set; }
        public short Health { get; private set; }
        public short Mana { get; private set; }
        public short WeaponAttack { get; private set; }
        public short MagicAttack { get; private set; }
        public short WeaponDefense { get; private set; }
        public short MagicDefense { get; private set; }
        public short Accuracy { get; private set; }
        public short Avoidability { get; private set; }
        public short Hands { get; private set; }
        public short Speed { get; private set; }
        public short Jump { get; private set; }
        public string Creator { get; private set; }
        public ushort Flags { get; private set; }

        public bool IsEquipped
        {
            get
            {
                return Slot < 0;
            }
        }

        public EInventoryType Inventory
        {
            get
            {
                return GameLogicUtilities.GetInventory(ID);
            }
        }

        public Player Player
        {
            get
            {
                return Parent.Parent;
            }
        }

        public PlayerItem(DatabaseQuery query)
        {
            ID = query.Get<int>("item_identifier");

            Slot = query.Get<short>("inventory_slot");

            Quantity = query.Get<ushort>("quantity");

            Expiration = query.Get<DateTime>("expiration");

            Slots = query.Get<byte>("slots");
            Scrolls = query.Get<byte>("scrolls");
            Strength = query.Get<short>("strength");
            Dexterity = query.Get<short>("dexterity");
            Intelligence = query.Get<short>("intelligence");
            Luck = query.Get<short>("luck");
            Health = query.Get<short>("health");
            Mana = query.Get<short>("mana");
            WeaponAttack = query.Get<short>("weapon_attack");
            MagicAttack = query.Get<short>("magic_attack");
            WeaponDefense = query.Get<short>("weapon_defense");
            MagicDefense = query.Get<short>("magic_defense");
            Accuracy = query.Get<short>("accuracy");
            Avoidability = query.Get<short>("avoidability");
            Hands = query.Get<short>("hands");
            Speed = query.Get<short>("speed");
            Jump = query.Get<short>("jump");
            Creator = query.Get<string>("creator");
            Flags = query.Get<ushort>("flags");
        }

        public PlayerItem(int identifier, ushort quantity = 1, bool equipped = false)
            : base()
        {
            /*ID = identifier;

            if (equipped)
            {
                Slot = GetEquippedSlot();
            }

            Quantity = quantity;

            Expiration = DateTime.FromFileTimeUtc((long)EExpirationTime.Default);

            Creator = string.Empty;

            Flags = 0;

            if (Inventory == EInventoryType.Equipment)
            {
                ItemEquipData data = ItemDataProvider.Instance[ID].Equip;

                Slots = data.m_slots;
                Strength = data.m_strength;
                Dexterity = data.m_dexterity;
                Intelligence = data.m_intelligence;
                Luck = data.m_luck;
                Health = data.m_hp;
                Mana = data.m_mp;
                WeaponAttack = data.m_weaponAttack;
                MagicAttack = data.m_magicAttack;
                WeaponDefense = data.m_weaponDefense;
                MagicDefense = data.m_magicDefense;
                Accuracy = data.m_accuracy;
                Avoidability = data.m_avoidability;
                Hands = data.m_hands;
                Speed = data.m_seed;
                Jump = data.m_jump;
            }*/
        }

        public void Save()
        {
            Database.Execute(@"INSERT INTO `player_item` 
                             VALUES(@player_identifier, @item_identifier, @inventory_slot, @quantity, @expiration, @slots, @scrolls, @strength, @dexterity, @intelligence, @luck, @health, @mana, @weapon_attack, @magic_attack, @weapon_defense, @magic_defense, @accuracy, @avoidability, @hands, @speed, @jump, @creator, @flags);",
                             new MySqlParameter("player_identifier", Player.Id),
                             new MySqlParameter("inventory_slot", Slot),
                             new MySqlParameter("item_identifier", ID),
                             new MySqlParameter("quantity", Quantity),
                             new MySqlParameter("expiration", Expiration),
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
                             new MySqlParameter("creator", Creator),
                             new MySqlParameter("flags", Flags));
        }

        public void Equip()
        {
            if (Inventory != EInventoryType.Equipment)
            {
                throw new InvalidOperationException("Can only equip equipment items.");
            }

            // TODO: Check requirements.
            // TODO: Check stripped slots.

            short sourceSlot = Slot;
            short destinationSlot = GetEquippedSlot();

            PlayerItem destination = Parent[Inventory, destinationSlot];

            if (destination != null)
            {
                destination.Slot = sourceSlot;
            }

            Slot = destinationSlot;

            /*//player.Send(InventoryPackets.InventoryOperation(true, new InventoryOperation()
            {
                Type = EInventoryOperation.ModifySlot,
                Item = this,
                CurrentSlot = sourceSlot,
                OldSlot = destinationSlot
            }));
    */
        }

        public void Unequip(short destinationSlot = 0)
        {
            if (Inventory != EInventoryType.Equipment)
            {
                throw new InvalidOperationException("Can only unequip equipment items.");
            }

            short sourceSlot = Slot;

            if (destinationSlot == 0)
            {
                destinationSlot = Parent.GetNextFreeSlot(EInventoryType.Equipment);
            }

            Slot = destinationSlot;

            /*//player.Send(InventoryPackets.InventoryOperation(true, new InventoryOperation()
            {
                Type = EInventoryOperation.ModifySlot,
                Item = this,
                CurrentSlot = sourceSlot,
                OldSlot = destinationSlot
            }));*/
        }

        public void Move(short destinationSlot)
        {
            /*short sourceSlot = Slot;

            PlayerItem destination = Parent[Inventory, destinationSlot];

            if (destination != null &&
                destination.ID == ID &&
                GameLogicUtilities.IsStackable(ID))
            {
                ushort maxPerStack = ItemDataProvider.Instance[ID].MaxSlotQuantity;

                if (Quantity + destination.Quantity > maxPerStack)
                {
                    Quantity -= (ushort)(maxPerStack - destination.Quantity);
                    destination.Quantity = maxPerStack;

                   /* //player.Send(InventoryPackets.InventoryOperation(true,
                    new InventoryOperation()
                    {
                        Type = EInventoryOperation.ModifyQuantity,
                        Item = this,
                        CurrentSlot = Slot
                    },
                    new InventoryOperation()
                    {
                        Type = EInventoryOperation.ModifyQuantity,
                        Item = destination,
                        CurrentSlot = destinationSlot
                    }));
                }
                else
                {
                    destination.Quantity += Quantity;

                    /player.Send(InventoryPackets.InventoryOperation(true, new InventoryOperation()
                    {
                        Type = EInventoryOperation.ModifyQuantity,
                        Item = destination,
                        CurrentSlot = destinationSlot
                    }));
                    
                    Parent.Remove(this);
                }
            }
            else
            {
                if (destination != null)
                {
                    destination.Slot = sourceSlot;
                }

                Slot = destinationSlot;

                /player.Send(InventoryPackets.InventoryOperation(true, new InventoryOperation()
                {
                    Type = EInventoryOperation.ModifySlot,
                    Item = this,
                    CurrentSlot = sourceSlot,
                    OldSlot = destinationSlot
                }));
            }*/
        }

        public void Drop(ushort quantity)
        {
            if (quantity == Quantity)
            {
                Dropper = Player;
                Owner = null;

                Player.Map.Drops.Add(this);

                Parent.Remove(this);
            }
            else if (quantity < Quantity)
            {
                // TODO: Stack items.
            }
        }

        private short GetEquippedSlot()
        {
            short slot = 0;

            if (ID >= 1000000 && ID < 1010000)
            {
                slot -= 1;
            }
            else if (ID >= 1010000 && ID < 1020000)
            {
                slot -= 2;
            }
            else if (ID >= 1020000 && ID < 1030000)
            {
                slot -= 3;
            }
            else if (ID >= 1030000 && ID < 1040000)
            {
                slot -= 4;
            }
            else if (ID >= 1040000 && ID < 1060000)
            {
                slot -= 5;
            }
            else if (ID >= 1060000 && ID < 1070000)
            {
                slot -= 6;
            }
            else if (ID >= 1070000 && ID < 1080000)
            {
                slot -= 7;
            }
            else if (ID >= 1080000 && ID < 1090000)
            {
                slot -= 8;
            }
            else if (ID >= 1102000 && ID < 1103000)
            {
                slot -= 9;
            }
            else if (ID >= 1092000 && ID < 1100000)
            {
                slot -= 10;
            }
            else if (ID >= 1300000 && ID < 1800000)
            {
                slot -= 11;
            }
            else if (ID >= 1112000 && ID < 1120000)
            {
                slot -= 12;
            }
            else if (ID >= 1122000 && ID < 1123000)
            {
                slot -= 17;
            }
            else if (ID >= 1900000 && ID < 2000000)
            {
                slot -= 18;
            }

            if (false) // TODO: Cash.
            {
                slot -= 100;
            }

            return slot;
        }
    }
}
