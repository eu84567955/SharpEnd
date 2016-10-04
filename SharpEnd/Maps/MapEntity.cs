namespace SharpEnd.Maps
{
    internal abstract class MapEntity : MovableLife
    {
        public Map Map { get; set; }

        public int ObjectIdentifier { get; set; }
    }
}
