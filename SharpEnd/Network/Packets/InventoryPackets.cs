using SharpEnd.Network;
using SharpEnd.Game.Players;
using SharpEnd.Utility;
using SharpEnd.Packets.Helpers;

namespace SharpEnd.Packets
{
    public struct InventoryOperation
    {
        public EInventoryOperation Type { get; set; }
        public PlayerItem Item { get; set; }
        public short OldSlot { get; set; }
        public short CurrentSlot { get; set; }
    }

    public static class InventoryPackets
    {
        public static byte[] InventoryOperation(bool unknown, params InventoryOperation[] operations)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_INVENTORY_OPERATION)
                    .WriteBoolean(unknown)
                    .WriteByte((byte)operations.Length);

                sbyte addedByte = -1;

                foreach (InventoryOperation operation in operations)
                {
                    outPacket
                        .WriteByte() // NOTE: Unknown
                        .WriteByte((byte)operation.Type)
                        .WriteByte((byte)GameLogicUtilities.GetInventory(operation.Item.ID));

                    switch (operation.Type)
                    {
                        case EInventoryOperation.AddItem:
                            PlayerPacketHelper.AddItemInfo(outPacket, operation.CurrentSlot, operation.Item, true);
                            break;

                        case EInventoryOperation.ModifyQuantity:
                            {
                                outPacket
                                    .WriteShort(operation.CurrentSlot)
                                    .WriteUShort(operation.Item.Quantity);
                            }
                            break;

                        case EInventoryOperation.ModifySlot:
                            {
                                outPacket
                                    .WriteShort(operation.OldSlot)
                                    .WriteShort(operation.CurrentSlot);

                                if (addedByte == -1)
                                {
                                    if (operation.OldSlot < 0)
                                    {
                                        addedByte = 1;
                                    }
                                    else if (operation.CurrentSlot < 0)
                                    {
                                        addedByte = 2;
                                    }
                                }
                            }
                            break;

                        case EInventoryOperation.RemoveItem:
                            outPacket.WriteShort(operation.CurrentSlot);
                            break;
                    }
                }

                if (addedByte != -1)
                {
                    outPacket.WriteSByte(addedByte);
                }

                return outPacket.ToArray();
            }
        }
    }
}
