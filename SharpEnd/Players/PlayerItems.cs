using MySql.Data.MySqlClient;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Utility;
using System.Collections.Generic;

namespace SharpEnd.Players
{
    internal sealed class PlayerItems : List<PlayerItem>
    {
        private Player m_player;

        public long Meso { get; private set; }

        public PlayerItems(Player player, DatabaseQuery query, long meso, byte equipmentSlots, byte usableSlots, byte setupSlots, byte etceteraSlots, byte cashSlots)
            : base()
        {
            m_player = player;

            Meso = meso;

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
                .WriteLong(Meso)
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
                .WriteByte(24)
                .WriteByte(24)
                .WriteByte(24)
                .WriteByte(24)
                .WriteByte(96)
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
