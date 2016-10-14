//using SharpEnd.Game.Maps;
//using SharpEnd.Network;

//namespace SharpEnd.Packets
//{
//    internal static class ReactorPackets
//    {
//        public static byte[] ReactorSpawn(Reactor reactor)
//        {
//            using (OutPacket outPacket = new OutPacket())
//            {
//                outPacket
//                    .WriteHeader(EHeader.SMSG_REACTOR_SPAWN)
//                    .WriteInt(reactor.ObjectIdentifier)
//                    .WriteInt(reactor.Identifier)
//                    .WriteSByte(reactor.State)
//                    .WritePoint(reactor.Position)
//                    .WriteSByte(reactor.Stance)
//                    .WriteString(reactor.Label);

//                return outPacket.ToArray();
//            }
//        }
//    }
//}
