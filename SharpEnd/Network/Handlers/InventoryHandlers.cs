﻿using SharpEnd.Drawing;
using SharpEnd.Game.Maps;
using SharpEnd.Network;
using SharpEnd.Game.Players;
using SharpEnd.Utility;
using System.Collections.Generic;

namespace SharpEnd.Handlers
{
    public static class InventoryHandlers
    {
        [PacketHandler(EHeader.CMSG_INVENTORY_SORT)]
        public static void SortHandler(GameClient client, InPacket inPacket)
        {
            inPacket.ReadInt(); // NOTE: Ticks.
        }

        [PacketHandler(EHeader.CMSG_INVENTORY_GATHER)]
        public static void GatherHandler(GameClient client, InPacket inPacket)
        {
            inPacket.ReadInt(); // NOTE: Ticks.
        }

        [PacketHandler(EHeader.CMSG_INVENTORY_OPERATION)]
        public static void OperationHandler(GameClient client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.ReadInt(); // NOTE: Ticks.
            EInventoryType inventory = (EInventoryType)inPacket.ReadByte();
            short sourceSlot = inPacket.ReadShort();
            short destinationSlot = inPacket.ReadShort();
            ushort quantity = inPacket.ReadUShort();

            PlayerItem item = player.Items[inventory, sourceSlot];

            if (item == null)
            {
                return;
            }

            if (destinationSlot < 0)
            {
                item.Equip();
            }
            else if (sourceSlot < 0 && destinationSlot > 0)
            {
                item.Unequip(destinationSlot);
            }
            else if (destinationSlot == 0)
            {
                item.Drop(quantity);
            }
            else
            {
                item.Move(destinationSlot);
            }
        }

        [PacketHandler(EHeader.CMSG_INVENTORY_CONSUME)]
        public static void InventoryConsumeHandler(GameClient client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.Skip(4); // NOTE: Ticks.
            short slot = inPacket.ReadShort();
            int itemID = inPacket.ReadInt();

            PlayerItem item = player.Items[EInventoryType.Use, slot];

            if (item == null || itemID != item.Slot)
            {
                return;
            }

            /*ItemConsumeData consume = null;// ItemDataProvider.Instance[itemID].Consume;

            // TODO: Check for map limitiations.

            if (consume.Hp != 0)
            {

            }

            if (consume.Mp != 0)
            {

            }

            if (consume.HpR != 0)
            {

            }

            if (consume.MpR != 0)
            {

            }

            if (consume.MoveTo != 0)
            {
                //player.SetMap(consume.MoveTo);
            }*/
        }

        [PacketHandler(EHeader.CMSG_INVENTORY_MESO_DROP)]
        public static void MesoDropHandler(GameClient client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.ReadInt(); // NOTE: Ticks.
            int amount = inPacket.ReadInt();

            /*if (amount < 10 || amount > 50000 || amount > player.Items.Meso)
            {
                return;
            }*/

            //player.Items.ModifyMeso(-amount, true);

            Meso meso = new Meso(amount)
            {
                Dropper = player,
                Owner = null
            };

            player.Map.Drops.Add(meso);
        }

        [PacketHandler(EHeader.CMSG_INVENTORY_PICKUP)]
        public static void PickupHandler(GameClient client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.ReadInt(); // NOTE: Ticks
            inPacket.Skip(1);
            Point position = inPacket.ReadPoint();
            int objectID = inPacket.ReadInt();

            Drop drop;

            try
            {
                drop = player.Map.Drops[objectID];
            }
            catch (KeyNotFoundException)
            {
                return;
            }


            if (drop.Picker == null)
            {
                drop.Picker = player;

                if (drop is Meso)
                {
                    //player.Items.ModifyMeso(((Meso)drop).Amount, true); // TODO: Check for max meso.
                }
                else if (drop is PlayerItem)
                {
                    if (GameLogicUtilities.IsMonsterCard(((PlayerItem)drop).ID))
                    {
                        // TODO: Monster Book handling.
                    }
                    else
                    {
                        ((PlayerItem)drop).Slot = player.Items.GetNextFreeSlot(((PlayerItem)drop).Inventory); // TODO: Check for full inventory.

                        player.Items.Add((PlayerItem)drop);
                    }
                }

                player.Map.Drops.Remove(drop);

                //player.Send(DropPackets.DropGain(drop));
            }
        }
    }
}
