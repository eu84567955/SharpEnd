using SharpEnd.Drawing;
using SharpEnd.Maps;
using SharpEnd.Network;
using SharpEnd.Servers;
using System.Collections.Generic;

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

        [PacketHandler(EHeader.CMSG_INVENTORY_MESO_DROP)]
        public static void MesoDrop(Client client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.ReadInt(); // NOTE: Ticks
            int amount = inPacket.ReadInt();

            if (amount < 10 || amount > 50000 || amount > player.Items.Meso)
            {
                return;
            }

            player.Items.ModifyMeso(-amount, true);

            MasterServer.Instance.Maps[player.Map].Drops.Add(new Drop(EDropType.Normal, amount)
            {
                Position = player.Position
            });
        }

        [PacketHandler(EHeader.CMSG_INVENTORY_PICKUP)]
        public static void Pickup(Client client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.ReadInt(); // NOTE: Ticks
            inPacket.Skip(1);
            Point position = inPacket.ReadPoint();
            int objectIdentifier = inPacket.ReadInt();

            Drop drop;

            try
            {
                drop = MasterServer.Instance.Maps[player.Map].Drops[objectIdentifier];
            }
            catch (KeyNotFoundException)
            {
                return;
            }

            // TODO: Check if the drop is a part of a quest
            // TODO: Check if the drop owner matches
            // TODO: Check for the drop type (party, etcetera)
            // TODO: Check for the drop distance relative to the player

            if (drop.IsMeso)
            {
                player.Items.ModifyMeso(drop.Meso, true);

                MasterServer.Instance.Maps[player.Map].Drops.Remove(drop, 2);
            }
        }
    }
}
