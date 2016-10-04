using SharpEnd.Drawing;
using SharpEnd.Maps;
using SharpEnd.Network;
using SharpEnd.Packets;
using System.Collections.Generic;

namespace SharpEnd.Handlers
{
    internal static class MobHandlers
    {
        [PacketHandler(EHeader.CMSG_MOB_MOVE)]
        public static void Move(Client client, InPacket inPacket)
        {
            var player = client.Player;

            int objectIdentifier = inPacket.ReadInt();

            Mob mob;

            try
            {
                mob = player.ControlledMobs[objectIdentifier];
            }
            catch (KeyNotFoundException)
            {
                return;
            }

            inPacket.ReadByte(); // NOTE: Unknown
            short movementIdentifier = inPacket.ReadShort();
            bool usingAbility = inPacket.ReadBoolean();
            byte skillIdentifier = inPacket.ReadByte();
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

            if (!mob.ParseMovement(inPacket))
            {
                return;
            }

            player.Send(MobPackets.MobControlAck(objectIdentifier, movementIdentifier, 0, usingAbility, 0, 0));

            // TODO: Broadcast packet to map
        }
    }
}
