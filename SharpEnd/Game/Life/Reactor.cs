using SharpEnd.Game.Maps;
using static SharpEnd.Game.Data.MapData;

namespace SharpEnd.Game.Life
{
    internal sealed class Reactor : MapEntity
    {
        public int Identifier { get; private set; }
        public bool Flip { get; private set; }
        public sbyte State { get; private set; }
        public string Label { get; private set; }

        public Reactor(int identifier)
            : base()
        {
            Identifier = identifier;
        }

        public Reactor(MapReactorData data)
            : this(data.Identifier)
        {
            Flip = data.Flip;
            Position = data.Position;
            Label = data.Label;
            //RespawnTime = data.RespawnTime;
        }

        public Reactor(int identifier, MovableLife reference)
            : this(identifier)
        {
            Position = reference.Position;
            Stance = reference.Stance;
            Foothold = reference.Foothold;
        }
    }
}
