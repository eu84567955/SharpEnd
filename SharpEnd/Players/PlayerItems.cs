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
        public Player Parent { get; private set; }

        public long Meso { get; private set; }

        public byte EquipmentSlots { get; set; }
        public byte UsableSlots { get; set; }
        public byte SetupSlots { get; set; }
        public byte EtceteraSlots { get; set; }
        public byte CashSlots { get; set; }

        public PlayerItems(Player player, DatabaseQuery query, long meso, byte equipmentSlots, byte usableSlots, byte setupSlots, byte etceteraSlots, byte cashSlots)
            : base()
        {
            Parent = player;

            Meso = meso;

            EquipmentSlots = equipmentSlots;
            UsableSlots = usableSlots;
            SetupSlots = setupSlots;
            EtceteraSlots = etceteraSlots;
            CashSlots = cashSlots;

            while (query.NextRow())
            {
                Add(new PlayerItem(query));
            }
        }

        public void Save()
        {
            Database.Execute("DELETE FROM player_item WHERE player_identifier=@player_identifier", new MySqlParameter("player_identifier", Parent.Identifier));

            foreach (PlayerItem item in this)
            {
                item.Save();
            }
        }

        public new void Add(PlayerItem item)
        {
            if (item.Quantity > 0)
            {
                item.Parent = this;

                if (item.Slot == 0)
                {
                    item.Slot = GetNextFreeSlot(item.Inventory);
                }

                base.Add(item);

                if (Parent.IsInitialized)
                {
                    Parent.Send(InventoryPackets.InventoryOperation(true, new InventoryOperation()
                    {
                        Type = EInventoryOperation.AddItem,
                        Item = item,
                        OldSlot = 0,
                        CurrentSlot = item.Slot
                    }));
                }
            }
        }

        public new void Remove(PlayerItem item)
        {
            Parent.Send(InventoryPackets.InventoryOperation(true, new InventoryOperation()
            {
                Type = EInventoryOperation.RemoveItem,
                Item = item,
                CurrentSlot = item.Slot
            }));

            bool wasEquipped = item.IsEquipped;

            base.Remove(item);

            if (wasEquipped)
            {
                // TODO: Update look
            }
        }

        public void WriteInitial(OutPacket outPacket)
        {
            outPacket
                .WriteLong(Meso)
                .WriteInt()
                .WriteInt()
                .WriteInt()
                .WriteInt(Parent.Identifier)
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

            outPacket
                .WriteInt(hiddenLayer.GetOrDefault((byte)11, 0))
                .WriteInt(visibleLayer.GetOrDefault((byte)11, 0))
                .WriteInt(visibleLayer.GetOrDefault((byte)15, 0)); // TODO: Find the correct slot
        }

        public void SetMeso(int amount, bool sendPacket = false)
        {

        }

        public bool ModifyMeso(int mod, bool sendPacket = false)
        {
            if (mod < 0)
            {
                if (-mod > Meso)
                {
                    return false;
                }

                Meso += mod;
            }
            else
            {
                if (Meso + mod < 0)
                {
                    return false;
                }

                Meso += mod;
            }

            Parent.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Meso, Meso, sendPacket));

            return true;
        }

        public short GetNextFreeSlot(EInventoryType inventory)
        {
            for (short i = 1; i <= 24; i++) // TODO: Change 24 to the max slots of the inventory type
            {
                if (this[inventory, i] == null)
                {
                    return i;
                }
            }

            return -1; // TODO: Throw exception
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
