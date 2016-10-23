﻿using SharpEnd.Network;

namespace SharpEnd.Handlers
{
    public static class PartyHandlers
    {
        [PacketHandler(EHeader.CMSG_PARTY_ALLOW_INVITE)]
        public static void AllowInviteHandler(GameClient client, InPacket inPacket)
        {
            if (inPacket.ReadBoolean())
            {
                // TODO: Remove quest 122901.
            }
            else
            {
                // TODO: Add quest 122901.
            }
        }
    }
}
