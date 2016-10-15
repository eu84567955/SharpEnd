using MySql.Data.MySqlClient;
using SharpEnd.Data;
using SharpEnd.Game.Maps;
using SharpEnd.Packets;
using SharpEnd.Servers;
using SharpEnd.Utility;
using System;

namespace SharpEnd.Players
{
    internal sealed class PlayerItem : Drop
    {
        public Player Player => Parent.Parent;

        public PlayerItems Parent { get; set; }

        public int Identifier { get; private set; }

        public short Slot { get; set; }
        public bool IsEquipped => Slot < 0;

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

        public EInventoryType Inventory => GameLogicUtilities.GetInventory(Identifier);

        public PlayerItem(DatabaseQuery query)
        {
            Identifier = query.Get<int>("item_identifier");

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
            Identifier = identifier;

            if (equipped)
            {
                Slot = GetEquippedSlot();
            }

            Quantity = quantity;

            Expiration = DateTime.FromFileTimeUtc((long)EExpirationTime.Permanent);

            Creator = string.Empty;

            Flags = 0;

            if (Inventory == EInventoryType.Equipment)
            {
                ItemEquipData data = MasterServer.Instance.Items[Identifier].Equip;

                Slots = data.Slots;
                Strength = data.Strength;
                Dexterity = data.Dexterity;
                Intelligence = data.Intelligence;
                Luck = data.Luck;
                Health = data.MaxHealth;
                Mana = data.MaxMana;
                WeaponAttack = data.WeaponAttack;
                MagicAttack = data.MagicAttack;
                WeaponDefense = data.WeaponDefense;
                MagicDefense = data.MagicDefense;
                Accuracy = data.Accuracy;
                Avoidability = data.Avoidability;
                Hands = data.Hands;
                Speed = data.Speed;
                Jump = data.Jump;
            }
        }

        public void Save()
        {
            Database.Execute(@"INSERT INTO `player_item` 
                             VALUES(@player_identifier, @item_identifier, @inventory_slot, @quantity, @expiration, @slots, @scrolls, @strength, @dexterity, @intelligence, @luck, @health, @mana, @weapon_attack, @magic_attack, @weapon_defense, @magic_defense, @accuracy, @avoidability, @hands, @speed, @jump, @creator, @flags);",
                             new MySqlParameter("player_identifier", Player.Identifier),
                             new MySqlParameter("inventory_slot", Slot),
                             new MySqlParameter("item_identifier", Identifier),
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

            Player.Send(InventoryPackets.InventoryOperation(true, new InventoryOperation()
            {
                Type = EInventoryOperation.ModifySlot,
                Item = this,
                CurrentSlot = sourceSlot,
                OldSlot = destinationSlot
            }));
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

            Player.Send(InventoryPackets.InventoryOperation(true, new InventoryOperation()
            {
                Type = EInventoryOperation.ModifySlot,
                Item = this,
                CurrentSlot = sourceSlot,
                OldSlot = destinationSlot
            }));
        }

        public void Move(short destinationSlot)
        {
            short sourceSlot = Slot;

            PlayerItem destination = Parent[Inventory, destinationSlot];

            if (destination != null && false)
            {
                // TODO: Stack both items.
            }
            else
            {
                if (destination != null)
                {
                    destination.Slot = sourceSlot;
                }

                Slot = destinationSlot;

                Player.Send(InventoryPackets.InventoryOperation(true, new InventoryOperation()
                {
                    Type = EInventoryOperation.ModifySlot,
                    Item = this,
                    CurrentSlot = sourceSlot,
                    OldSlot = destinationSlot
                }));
            }
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

            if (Identifier >= 1000000 && Identifier < 1010000)
            {
                slot -= 1;
            }
            else if (Identifier >= 1010000 && Identifier < 1020000)
            {
                slot -= 2;
            }
            else if (Identifier >= 1020000 && Identifier < 1030000)
            {
                slot -= 3;
            }
            else if (Identifier >= 1030000 && Identifier < 1040000)
            {
                slot -= 4;
            }
            else if (Identifier >= 1040000 && Identifier < 1060000)
            {
                slot -= 5;
            }
            else if (Identifier >= 1060000 && Identifier < 1070000)
            {
                slot -= 6;
            }
            else if (Identifier >= 1070000 && Identifier < 1080000)
            {
                slot -= 7;
            }
            else if (Identifier >= 1080000 && Identifier < 1090000)
            {
                slot -= 8;
            }
            else if (Identifier >= 1102000 && Identifier < 1103000)
            {
                slot -= 9;
            }
            else if (Identifier >= 1092000 && Identifier < 1100000)
            {
                slot -= 10;
            }
            else if (Identifier >= 1300000 && Identifier < 1800000)
            {
                slot -= 11;
            }
            else if (Identifier >= 1112000 && Identifier < 1120000)
            {
                slot -= 12;
            }
            else if (Identifier >= 1122000 && Identifier < 1123000)
            {
                slot -= 17;
            }
            else if (Identifier >= 1900000 && Identifier < 2000000)
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
