using SharpEnd.Network;
using System;
using System.Collections.Generic;

namespace SharpEnd.Utility
{
    internal sealed class HandlerStore : Dictionary<EHeader, PacketHandlerAttribute>
    {
        public new PacketProcessor this[EHeader header]
        {
            get
            {
                return this.GetOrDefault(header, null)?.Processor;
            }
        }

        public void Load()
        {
            List<Tuple<PacketHandlerAttribute, PacketProcessor>> handlers = Reflector.FindAllMethods<PacketHandlerAttribute, PacketProcessor>();

            handlers.ForEach(h => { h.Item1.Processor = h.Item2; Add(h.Item1.Opcode, h.Item1); });
        }
    }
}
