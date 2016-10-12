using SharpEnd.Drawing;

namespace SharpEnd.Maps
{
    internal sealed class SpawnPoint : LifeEntity
    {
        public SpawnPoint(int identifier, Point position, ushort foothold, bool flip, bool hide) : base(identifier, position, foothold, flip, hide) { }

        public void Spawn()
        {
            Map.Mobs.Add(new Mob(this));
        }
    }
}
