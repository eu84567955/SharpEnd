using SharpEnd.Maps;
using System.Collections.Generic;

namespace SharpEnd.Players
{
    internal sealed class ControlledMobs : Dictionary<int, Mob>
    {
        private Player m_player;

        public ControlledMobs(Player player)
        {
            m_player = player;
        }

        public void Add(Mob mob)
        {
            // TODO: Set controller

            // TODO: Packet

            Add(mob.ObjectIdentifier, mob);
        }

        public void Remove(Mob mob)
        {
            Remove(mob.ObjectIdentifier);

            // TODO: Set controller

            // TODO: Packet
        }
    }
}
