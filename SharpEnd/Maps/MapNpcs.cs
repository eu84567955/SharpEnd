namespace SharpEnd.Maps
{
    internal sealed class MapNpcs : MapEntities<Npc>
    {
        public MapNpcs(Map map) : base(map) { }

        public override void Add(Npc entity)
        {
            base.Add(entity);
        }

        public override void Remove(Npc entity)
        {
            base.Remove(entity);
        }
    }
}
