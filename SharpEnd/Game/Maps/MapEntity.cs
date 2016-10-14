namespace SharpEnd.Game.Maps
{
    internal abstract class MapEntity : MovableLife
    {
        public Map Map { get; set; }

        public virtual int ObjectIdentifier { get; set; }
    }
}
