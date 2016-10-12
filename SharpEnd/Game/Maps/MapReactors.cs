namespace SharpEnd.Maps
{
    internal sealed class MapReactors : MapEntities<Reactor>
    {
        public MapReactors(Map map) : base(map) { }

        public override void Add(Reactor entity)
        {
            base.Add(entity);
        }

        public override void Remove(Reactor entity)
        {
            base.Remove(entity);
        }
    }
}
