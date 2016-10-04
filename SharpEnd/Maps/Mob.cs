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
    }
}
