using SharpEnd.Game.Maps;
using SharpEnd.Packets;
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
            mob.Controller = m_player;

            m_player.Send(MobPackets.MobControlRequest(mob));

            Add(mob.ObjectIdentifier, mob);
        }

        public void Remove(Mob mob)
        {
            Remove(mob.ObjectIdentifier);

            mob.Controller = null;

            m_player.Send(MobPackets.MobControlCancel(mob.ObjectIdentifier));
        }

        public new void Clear()
        {
            List<Mob> toRemove = new List<Mob>();

            foreach (Mob mob in this.Values)
            {
                toRemove.Add(mob);
            }

            foreach (Mob mob in toRemove)
            {
                Remove(mob);
            }
        }
    }
}
