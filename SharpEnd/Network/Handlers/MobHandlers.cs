using SharpEnd.Drawing;
using SharpEnd.Game.Life;
using SharpEnd.Network;
using SharpEnd.Packets;
using System.Collections.Generic;

namespace SharpEnd.Handlers
{
    public static class MobHandlers
    {
        [PacketHandler(EHeader.CMSG_MOB_MOVE)]
        public static void MoveHandler(GameClient client, InPacket inPacket)
        {
            /*var player = client.Player;

            int objectID = inPacket.ReadInt();

            Mob mob;

            try
            {
                mob = player.ControlledMobs[objectID];
            }
            catch (KeyNotFoundException)
            {
                return;
            }

            inPacket.ReadByte(); // NOTE: Unknown
            short movementID = inPacket.ReadShort();
            bool usingAbility = inPacket.ReadBoolean();
            byte skillID = inPacket.ReadByte();
            int unknown = inPacket.ReadInt();
            inPacket.ReadByte(); // NOTE: Unknown
            inPacket.ReadByte(); // NOTE: Unknown
            inPacket.ReadByte(); // NOTE: Unknown
            inPacket.ReadInt(); // NOTE: Unknown
            inPacket.ReadInt(); // NOTE: Unknown
            inPacket.ReadInt(); // NOTE: Unknown
            inPacket.Skip(5); // NOTE: Unknown
            inPacket.ReadInt(); // NOLTE: tEncodedGatherDuration
            Point origin = inPacket.ReadPoint();
            inPacket.ReadPoint();

            if (!mob.ParseMovement(inPacket))
            {
                return;
            }

            //player.Send(MobPackets.MobControlAck(objectID, movementID, 0, usingAbility, 0, 0));

            // TODO: Broadcast packet to map.*/
        }
    }
}
