using SharpEnd.Game.Maps;
using SharpEnd.Packets;
using System.Collections.Generic;

namespace SharpEnd.Players
{
    internal sealed class ControlledNpcs : Dictionary<int, Npc>
    {
        private Player m_player;

        public ControlledNpcs(Player player)
        {
            m_player = player;
        }

        public void Add(Npc npc)
        {
            npc.Controller = m_player;

            m_player.Send(NpcPackets.NpcControlRequest(npc));

            Add(npc.ObjectIdentifier, npc);
        }

        public void Remove(Npc npc)
        {
            Remove(npc.ObjectIdentifier);

            npc.Controller = null;

            m_player.Send(NpcPackets.NpcControlCancel(npc.ObjectIdentifier));
        }

        public new void Clear()
        {
            List<Npc> toRemove = new List<Npc>();

            foreach (Npc npc in this.Values)
            {
                toRemove.Add(npc);
            }

            foreach (Npc npc in toRemove)
            {
                Remove(npc);
            }
        }
    }
}
