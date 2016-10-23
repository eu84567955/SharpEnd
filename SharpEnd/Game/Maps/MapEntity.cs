namespace SharpEnd.Game.Maps
{
    public abstract class MapEntity : MovableLife
    {
        public Map Map { get; set; }

        public virtual int ObjectID { get; set; }
    }
}
