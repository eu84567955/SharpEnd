namespace SharpEnd.Maps
{
    internal sealed class MapMobs : MapEntities<Mob>
    {
        public MapMobs(Map map) : base(map) { }

        public override void Add(Mob entity)
        {
            base.Add(entity);
        }

        public override void Remove(Mob entity)
        {
            base.Remove(entity);
        }
    }
}
