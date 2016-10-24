using SharpEnd.Game.Life;
using SharpEnd.Game.Shops;
using SharpEnd.Network;
using SharpEnd.Utility;

namespace SharpEnd.Packets
{
    public static class NpcPackets
    {
        public static byte[] NpcSpawn(Npc npc)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_NPC_SPAWN)
                    .WriteBytes(NpcInit(npc));

                return outPacket.ToArray();
            }
        }

        public static byte[] NpcControlRequest(Npc npc)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_NPC_CONTROL)
                    .WriteBoolean(true)
                    .WriteBytes(NpcInit(npc));

                return outPacket.ToArray();
            }
        }

        private static byte[] NpcInit(Npc npc)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteInt(npc.ObjectID)
                    .WriteInt(npc.ID)
                    .WritePoint(npc.Position)
                    .WriteByte(npc.Stance)
                    .WriteByte(npc.Stance) // NOTE: Flip.
                    .WriteShort(npc.Foothold)
                    .WriteShort(npc.RX0)
                    .WriteShort(npc.RX1)
                    .WriteBoolean(!npc.Hide)
                    .WriteInt() // NOTE: Unknown.
                    .WriteByte() // NOTE: tPresentTimeState.
                    .WriteInt(-1) // NOTE: tPresent.
                    .WriteInt() // NOTICE: nNoticeBoardType.
                    .WriteInt() // NOTE: Unknown.
                    .WriteInt() // NOTE: Unknown.
                    .WriteString(string.Empty) // NOTE: Unknown.
                    .WriteByte(); // NOTE: Unknown.

                return outPacket.ToArray();
            }
        }

        public static byte[] NpcControlCancel(int objectID)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_NPC_CONTROL)
                    .WriteBoolean(false)
                    .WriteInt(objectID);

                return outPacket.ToArray();
            }
        }

        public static byte[] NpcDespawn(int objectID)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_NPC_DESPAWN)
                    .WriteInt(objectID);

                return outPacket.ToArray();
            }
        }

        public static byte[] NpcAction(int objectID, byte a, byte b)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_NPC_ACTION)
                    .WriteInt(objectID)
                    .WriteByte(a)
                    .WriteByte(b);

                return outPacket.ToArray();
            }
        }

        public static byte[] NpcOkDialog(int identifier, string text)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_NPC_DIALOG)
                    .WriteByte() // NOTE: Dialog type.
                    .WriteInt(identifier)
                    .WriteBoolean(false)
                    .WriteByte() // NOTE: Message type.
                    .WriteByte() // NOTE: Parameters.
                    .WriteByte() // NOTE: Color.
                    .WriteString(text)
                    .WriteBoolean(false) // NOTE: Previous button.
                    .WriteBoolean(false) // NOTE: Next button.
                    .WriteInt(); // NOTE: Seconds.

                return outPacket.ToArray();
            }
        }

        /*public static byte[] NpcShop(Shop shop)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_NPC_SHOP)
                    .WriteByte() // NOTE: Unknown.
                    .WriteInt() // NOTE: nSelectNpcItemID.
                    .WriteInt(shop.ID)
                    .WriteInt() // NOTE: nStarCoin.
                    .WriteInt(); // NOTE: nShopVerNo.

                bool ranks = false;

                outPacket.WriteBoolean(ranks);

                if (ranks)
                {
                    // TODO: Shop ranks. Whatever the fuck it is.
                }

                outPacket.WriteUShort((ushort)shop.Count);

                foreach (ShopItem item in shop)
                {
                    outPacket
                        .WriteInt(item.ItemID)
                        .WriteInt(item.Price)
                        .WriteInt(item.TokenItemID)
                        .WriteInt(item.TokenPrice)
                        .WriteInt(item.PointQuestID)
                        .WriteInt(item.PointPrice)
                        .WriteInt(item.StarCoin)
                        .WriteInt(item.QuestExID)
                        .WriteString(item.QuestExKey)
                        .WriteInt(item.QuestExValue)
                        .WriteInt(item.TimePeriod)
                        .WriteInt(item.LevelLimited)
                        .WriteUShort(item.MinimumLevel)
                        .WriteUShort(item.MaximumLevel)
                        .WriteInt(item.QuestID)
                        .WriteByte() // NOTE: Unknown.
                        .WriteLong((long)EExpirationTime.Zero)
                        .WriteLong((long)EExpirationTime.Default)
                        .WriteInt(item.TabIndex)
                        .WriteBoolean(item.WorldBlock)
                        .WriteInt(item.PotentialGrade)
                        .WriteInt() // NOTE: Expiration.
                        .WriteByte(); // NOTE: Unknown.

                    if (GameLogicUtilities.IsRechargeable(item.ItemID))
                    {
                        outPacket
                            .WriteShort()
                            .WriteInt()
                            .WriteShort(1000); // TODO: Recharageable quantity.
                    }
                    else
                    {
                        outPacket.WriteUShort(item.Quantity);
                    }

                    outPacket
                        .WriteUShort(item.MaxPerSlot)
                        .WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00")
                        .WriteHexString("75 96 8F 00 00 00 00 00 76 96 8F 00 00 00 00 00 77 96 8F 00 00 00 00 00 78 96 8F 00 00 00 00 00"); // NOTE: Red Leaf High.
                }

                return outPacket.ToArray();
            }
        }*/
    }
}
