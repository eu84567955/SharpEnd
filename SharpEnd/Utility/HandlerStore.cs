using SharpEnd.Network;
using System.Collections.Generic;

namespace SharpEnd.Utility
{
    public sealed class HandlerStore : Dictionary<EHeader, PacketProcessor>
    {
        private static HandlerStore m_instance;

        public static HandlerStore Instance { get { return m_instance ?? (m_instance = new HandlerStore()); } }

        public void Initialize()
        {
            foreach (var handler in Reflector.FindAllMethods<PacketHandlerAttribute, PacketProcessor>())
            {
                Add(handler.Item1.Header, handler.Item2);
            }
        }
    }
}
