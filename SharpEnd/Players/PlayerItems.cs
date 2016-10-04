using MySql.Data.MySqlClient;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Utility;
using System;
using System.Collections.Generic;

namespace SharpEnd.Players
{
    internal sealed class PlayerItems : List<PlayerItem>
    {
        private Player m_player;

        public ulong Meso { get; private set; }

        public byte EquipmentSlots { get; set; }
        public byte UsableSlots { get; set; }
        public byte SetupSlots { get; set; }
        public byte EtceteraSlots { get; set; }
        public byte CashSlots { get; set; }

        public PlayerItems(Player player, DatabaseQuery query, ulong meso, byte equipmentSlots, byte usableSlots, byte setupSlots, byte etceteraSlots, byte cashSlots)
            : base()
        {
            m_player = player;

            Meso = meso;

            EquipmentSlots = equipmentSlots;
            UsableSlots = usableSlots;
            SetupSlots = setupSlots;
            EtceteraSlots = etceteraSlots;
            CashSlots = cashSlots;

            while (query.NextRow())
            {
                Add(new PlayerItem(m_player, query));
            }
        }

        public void Save()
        {
            Database.Execute("DELETE FROM player_item WHERE player_identifier=@player_identifier", new MySqlParameter("player_identifier", m_player.Identifier));

            foreach (PlayerItem item in this)
            {
                item.Save();
            }
        }

        public void WriteInitial(OutPacket outPacket)
        {
            outPacket
                .WriteULong(Meso)
                .WriteInt()
                .WriteInt()
                .WriteInt()
                .WriteInt(m_player.Identifier)
                .WriteInt()
                .WriteInt()
                .WriteInt()
                .WriteInt()
                .WriteInt()
                .WriteInt()
                .WriteInt()
                .WriteByte()
                .WriteByte()
                .WriteByte()
                .WriteByte(EquipmentSlots)
                .WriteByte(UsableSlots)
                .WriteByte(SetupSlots)
                .WriteByte(EtceteraSlots)
                .WriteByte(CashSlots)
                .WriteLong()
                .WriteByte();

            foreach (PlayerItem item in GetEquipped(EEquippedQueryMode.Normal))
            {
                HelpPackets.AddItemInfo(outPacket, item.Slot, item, true);
            }
            outPacket.WriteShort();

            foreach (PlayerItem item in GetEquipped(EEquippedQueryMode.Cash))
            {
                HelpPackets.AddItemInfo(outPacket, item.Slot, item, true);
            }
            outPacket.WriteShort();

            foreach (PlayerItem item in this[EInventoryType.Equipment])
            {
                HelpPackets.AddItemInfo(outPacket, item.Slot, item, true);
            }
            outPacket.WriteShort();

            int count = 12;

            while (count-- > 0)
            {
                outPacket.WriteShort();
            }

            foreach (PlayerItem item in this[EInventoryType.Use])
            {
                HelpPackets.AddItemInfo(outPacket, item.Slot, item);
            }
            outPacket.WriteByte();

            foreach (PlayerItem item in this[EInventoryType.Setup])
            {
                HelpPackets.AddItemInfo(outPacket, item.Slot, item);
            }
            outPacket.WriteByte();

            foreach (PlayerItem item in this[EInventoryType.Etc])
            {
                HelpPackets.AddItemInfo(outPacket, item.Slot, item);
            }
            outPacket.WriteByte();

            foreach (PlayerItem item in this[EInventoryType.Cash])
            {
                HelpPackets.AddItemInfo(outPacket, item.Slot, item);
            }
            outPacket.WriteByte();

            outPacket.WriteZero(21);
        }

        public void WriteEquipment(OutPacket outPacket)
        {
            SortedDictionary<byte, int> visibleLayer = new SortedDictionary<byte, int>();
            SortedDictionary<byte, int> hiddenLayer = new SortedDictionary<byte, int>();
            SortedDictionary<byte, int> totemLayer = new SortedDictionary<byte, int>();

            foreach (PlayerItem item in GetEquipped())
            {
                byte position = (byte)Math.Abs(item.Slot);

                if (position < 100 && !visibleLayer.ContainsKey(position))
                {
                    visibleLayer[position] = item.Identifier;
                }
                else if (position > 100 && position != 111)
                {
                    position -= 100;

                    if (visibleLayer.ContainsKey(position))
                    {
                        hiddenLayer[position] = visibleLayer[position];
                    }

                    visibleLayer[position] = item.Identifier;
                }
                else if (visibleLayer.ContainsKey(position))
                {
                    hiddenLayer[position] = item.Identifier;
                }
            }

            foreach (KeyValuePair<byte, int> entry in visibleLayer)
            {
                outPacket
                    .WriteByte(entry.Key)
                    .WriteInt(entry.Value);
            }
            outPacket.WriteSByte(-1);

            foreach (KeyValuePair<byte, int> entry in hiddenLayer)
            {
                outPacket
                    .WriteByte(entry.Key)
                    .WriteInt(entry.Value);
            }
            outPacket.WriteSByte(-1);

            foreach (KeyValuePair<byte, int> entry in totemLayer)
            {
                outPacket
                    .WriteByte(entry.Key)
                    .WriteInt(entry.Value);
            }
            outPacket.WriteSByte(-1);

            PlayerItem cashWeapon = null;
            PlayerItem weapon = null;
            PlayerItem offhand = null;

            outPacket
                .WriteInt(cashWeapon != null ? cashWeapon.Identifier : 0)
                .WriteInt(weapon != null ? weapon.Identifier : 0)
                .WriteInt(offhand != null ? offhand.Identifier : 0);
        }

        public void Equip(short source, short destination)
        {

        }

        public void Unequip(short sourceSlot, short destinationSlot)
        {
            PlayerItem source = this[EInventoryType.Equipment, sourceSlot];
            PlayerItem destination = this[EInventoryType.Equipment, destinationSlot];

            if (source == null)
            {
                return;
            }

            if (destination != null)
            {

            }
        }

        public void Swap(EInventoryType inventory, short sourceSlot, short destinationSlot)
        {
            PlayerItem source = this[inventory, sourceSlot];
            PlayerItem destination = this[inventory, destinationSlot];

            if (source == null)
            {
                return;
            }

            if (destination != null && source.Identifier == destination.Identifier && GameLogicUtilities.IsStackable(source.Identifier))
            {
                // TODO: Stack items
            }
            else
            {
                source.Slot = destinationSlot;

                if (destination != null)
                {
                    destination.Slot = sourceSlot;
                }

                m_player.Send(InventoryPackets.InventoryOperation(true, new InventoryOperation()
                {
                    Type = EInventoryOperation.ModifySlot,
                    Item = source,
                    CurrentSlot = sourceSlot,
                    OldSlot = destinationSlot
                }));
            }
        }

        public IEnumerable<PlayerItem> GetEquipped(EEquippedQueryMode mode = EEquippedQueryMode.Any)
        {
            foreach (PlayerItem item in this)
            {
                if (item.IsEquipped)
                {
                    switch (mode)
                    {
                        case EEquippedQueryMode.Any:
                            yield return item;
                            break;

                        case EEquippedQueryMode.Normal:
                            if (item.Slot > -100)
                            {
                                yield return item;
                            }
                            break;

                        case EEquippedQueryMode.Cash:
                            if (item.Slot < -100)
                            {
                                yield return item;
                            }
                            break;
                    }
                }
            }
        }

        public IEnumerable<PlayerItem> this[EInventoryType inventory]
        {
            get
            {
                foreach (PlayerItem item in this)
                {
                    if (item.Inventory == inventory && !item.IsEquipped)
                    {
                        yield return item;
                    }
                }
            }
        }

        public PlayerItem this[EInventoryType inventory, short slot]
        {
            get
            {
                foreach (PlayerItem item in this)
                {
                    if (item.Inventory == inventory && item.Slot == slot)
                    {
                        return item;
                    }
                }

                return null;
            }
        }
    }
}
