using SharpEnd.Drawing;
using SharpEnd.Players;

namespace SharpEnd.Maps
{
    internal sealed class Mob : LifeEntity, IControllable
    {
        public int RespawnTime { get; private set; }

        public Player Controller { get; set; }

        public Mob(int identifier, int respawnTime, Point position, ushort foothold, bool flip, bool hide)
            : base(identifier, position, foothold, flip, hide)
        {
            RespawnTime = respawnTime;
        }

        public Mob(int identifier, Point position, ushort foothold) : base(identifier, position, foothold) { }

        public void AssignController()
        {
            if (Controller == null)
            {
                int leastControlled = int.MaxValue;

                Player newController = null;

                foreach (Player player in Map.Players.Values)
                {
                    if (player.ControlledMobs.Count < leastControlled)
                    {
                        leastControlled = player.ControlledMobs.Count;

                        newController = player;
                    }
                }

                if (newController != null)
                {
                    newController.ControlledMobs.Add(this);
                }
            }
        }

        public void Die()
        {
            Map.Mobs.Remove(this);
        }
    }
}
