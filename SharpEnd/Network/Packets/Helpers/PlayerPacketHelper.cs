using SharpEnd.Extensions;
using SharpEnd.Network;
using SharpEnd.Game.Players;
using SharpEnd.Utility;
using System;

namespace SharpEnd.Packets.Helpers
{
    public static class PlayerPacketHelper
    {
        public static void AddPlayerStats(OutPacket outPacket, Player player)
        {
            outPacket
                .WriteInt(player.ID)
                .WriteInt(player.ID)
                .WriteInt(45)
                .WriteString(player.Name, 13)
                .WriteByte(player.Gender)
                .WriteByte(player.Skin)
                .WriteInt(player.Face)
                .WriteInt(player.Hair)
                .WriteSByte(-1)
                .WriteSByte()
                .WriteSByte()
                .WriteByte(player.Level)
                .WriteShort(player.Job)
                .WriteShort(player.Strength)
                .WriteShort(player.Dexterity)
                .WriteShort(player.Intelligence)
                .WriteShort(player.Luck)
                .WriteInt(player.HP)
                .WriteInt(player.MaxHP)
                .WriteInt(player.MP)
                .WriteInt(player.MaxMP)
                .WriteShort() // NOTE: Ability points.
                .WriteByte() // NOTE: Skill tree.
                .WriteLong(player.Experience)
                .WriteInt(player.Fame)
                .WriteInt() // NOTE: Weapon points (has to do with Zero/Mercedes).
                .WriteInt() // NOTE: Gachapon experience (unusued anymore).
                .WriteInt(player.Map.ID)
                .WriteSByte(player.SpawnPoint)
                .WriteInt() // NOTE: Unknown.
                .WriteUShort() // NOTE: Special job (to differentiate between explorer jobs e.g. Dual Blade).
                .WriteByte() // NOTE: Fatigue.
                .WriteIntDateTime(DateTime.Now)
                .WriteZero(36) // NOTE: Traits.
                .WriteHexString("00 00 40 E0 FD 3B 37 4F 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 23 24 D1 01 00 66 08 1D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 42 2C D2 01 B0 BD B0 08 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
        }

        public static void AddItemInfo(OutPacket outPacket, short slot, PlayerItem item, bool shortSlot = false)
        {
            if (slot != 0)
            {
                if (shortSlot)
                {
                    slot = Math.Abs(slot);

                    outPacket.WriteShort(slot);
                }
                else
                {
                    slot = Math.Abs(slot);

                    if (slot > 100)
                    {
                        slot -= 100;
                    }

                    outPacket.WriteByte((byte)slot);
                }
            }

            outPacket
                .WriteByte((byte)(item.Inventory == EInventoryType.Equipment ? 1 : 2))
                .WriteInt(item.ID)
                .WriteBoolean(false); // NOTE: Cash item

            if (false) // NOTE: Cash item
            {
                outPacket.WriteLong(); // NOTE: Unique identifier
            }

            outPacket
                .WriteDateTime(item.Expiration)
                .WriteInt(-1); // NOTE: Bag

            if (item.Inventory == EInventoryType.Equipment)
            {
                // NOTE: Part 1
                {
                    uint flag = 0;

                    int offset = outPacket.Position;

                    outPacket.WriteUInt();

                    outPacket.WriteFlaggedValue(0x00000001, item.Slots, ref flag);
                    outPacket.WriteFlaggedValue(0x00000002, item.Scrolls, ref flag);
                    outPacket.WriteFlaggedValue(0x00000004, item.Strength, ref flag);
                    outPacket.WriteFlaggedValue(0x00000008, item.Dexterity, ref flag);
                    outPacket.WriteFlaggedValue(0x00000010, item.Intelligence, ref flag);
                    outPacket.WriteFlaggedValue(0x00000020, item.Luck, ref flag);
                    outPacket.WriteFlaggedValue(0x00000040, item.Health, ref flag);
                    outPacket.WriteFlaggedValue(0x00000080, item.Mana, ref flag);
                    outPacket.WriteFlaggedValue(0x00000100, item.WeaponAttack, ref flag);
                    outPacket.WriteFlaggedValue(0x00000200, item.MagicAttack, ref flag);
                    outPacket.WriteFlaggedValue(0x00000400, item.WeaponDefense, ref flag);
                    outPacket.WriteFlaggedValue(0x00000800, item.MagicDefense, ref flag);
                    outPacket.WriteFlaggedValue(0x00001000, item.Accuracy, ref flag);
                    outPacket.WriteFlaggedValue(0x00002000, item.Avoidability, ref flag);
                    outPacket.WriteFlaggedValue(0x00004000, item.Hands, ref flag);
                    outPacket.WriteFlaggedValue(0x00008000, item.Speed, ref flag);
                    outPacket.WriteFlaggedValue(0x00010000, item.Jump, ref flag);
                    outPacket.WriteFlaggedValue(0x20000, item.Flags, ref flag);
                    outPacket.WriteFlaggedValue(0x40000, (byte)0, ref flag); // NOTE: INC_SKILL.
                    outPacket.WriteFlaggedValue(0x40000, (byte)0, ref flag); // NOTE: ITEM_LEVEL.
                    outPacket.WriteFlaggedValue(0x40000, (long)0, ref flag); // NOTE: ITEM_EXP.

                    outPacket.SetUInt(offset, flag);
                }

                // NOTE: Part 2
                {
                    uint flag = 0;

                    int offset = outPacket.Position;

                    outPacket.WriteUInt();

                    outPacket.WriteFlaggedValue(0x01, 0, ref flag);
                    outPacket.WriteFlaggedValue(0x02, 0, ref flag);
                    outPacket.WriteFlaggedValue(0x04, (sbyte)-1, ref flag);
                    outPacket.WriteFlaggedValue(0x08, (long)0, ref flag);
                    outPacket.WriteFlaggedValue(0x10, 256, ref flag);

                    outPacket.SetUInt(offset, flag);
                }

                outPacket
                    .WriteString(string.Empty)
                    .WriteByte()
                    .WriteByte()
                    .WriteShort() // NOTE: Potential 1
                    .WriteShort() // NOTE: Potential 2
                    .WriteShort() // NOTE: Potential 3
                    .WriteShort() // NOTE: Potential 4
                    .WriteShort() // NOTE: Potential 5
                    .WriteShort() // NOTE: Potential 6
                    .WriteShort()
                    .WriteShort()
                    .WriteShort()
                    .WriteShort()
                    .WriteShort()
                    .WriteLong()
                    .WriteLong()
                    .WriteInt(-1)
                    .WriteLong()
                    .WriteLong()
                    .WriteLong()
                    .WriteLong()
                    .WriteShort()
                    .WriteShort()
                    .WriteShort();
            }
            else
            {
                outPacket
                    .WriteUShort(item.Quantity)
                    .WriteString(string.Empty)
                    .WriteUShort(item.Flags);

                if (GameLogicUtilities.IsRechargeable(item.ID))
                {
                    outPacket.WriteLong();
                }
            }
        }

        public static void AddPlayerDisplay(OutPacket outPacket, Player player)
        {
            outPacket
                .WriteByte(player.Gender)
                .WriteByte(player.Skin)
                .WriteInt(player.Face)
                .WriteInt(player.Job)
                .WriteBoolean(false)
                .WriteInt(player.Hair);

            player.Items.WriteEquipment(outPacket);

            outPacket.WriteBoolean(false); // NOTE: Elf ears

            // NOTE: Pets
            {
                sbyte count = 3;

                while (count-- > 0)
                {
                    outPacket.WriteInt();
                }
            }

            outPacket
                .WriteSByte() // NOTE: Mixed hair
                .WriteSByte(); // NOTE: Mixed hair
        }
    }
}
