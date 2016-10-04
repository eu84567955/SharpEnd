using SharpEnd.Network;

namespace SharpEnd.Handlers
{
    internal static class InventoryHandlers
    {
        [PacketHandler(EHeader.CMSG_INVENTORY_SORT)]
        public static void Sort(Client client, InPacket inPacket)
        {
            inPacket.ReadInt(); // NOTE: Ticks
        }

        [PacketHandler(EHeader.CMSG_INVENTORY_GATHER)]
        public static void Gather(Client client, InPacket inPacket)
        {
            inPacket.ReadInt(); // NOTE: Ticks
        }

        [PacketHandler(EHeader.CMSG_INVENTORY_OPERATION)]
        public static void Operation(Client client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.ReadInt(); // NOTE: Ticks
            EInventoryType inventory = (EInventoryType)inPacket.ReadByte();
            short source = inPacket.ReadShort();
            short destination = inPacket.ReadShort();
            ushort quantity = inPacket.ReadUShort();

            if (source < 0 && destination > 0)
            {
                player.Items.Unequip(source, destination);
            }
            else if (destination < 0)
            {
                player.Items.Equip(source, destination);
            }
            else if (destination == 0)
            {
                // TODO: Drop
            }
            else
            {
                player.Items.Swap(inventory, source, destination);
            }
        }
    }
}
