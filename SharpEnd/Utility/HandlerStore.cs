using SharpEnd.Network;
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
            List<Doublet<PacketHandlerAttribute, PacketProcessor>> handlers = Reflector.FindAllMethods<PacketHandlerAttribute, PacketProcessor>();

            handlers.ForEach(d => { d.First.Processor = d.Second; Add(d.First.Opcode, d.First); });
        }
    }
}
