using SharpEnd.Drawing;

namespace SharpEnd.Maps
{
    internal abstract class LifeEntity : MapEntity
    {
        public int Identifier { get; protected set; }
        public bool Flip { get; private set; }
        public bool Hide { get; private set; }

        protected LifeEntity(int identifier, Point position, ushort foothold, bool flip = false, bool hide = false)
            : base()
        {
            Identifier = identifier;
            Position = position;
            Stance = (sbyte)(Flip ? 1 : 2); // TODO: Validate if this is the correct way of determing initial stance
            Foothold = foothold;
            Flip = flip;
            Hide = hide;
        }
    }
}
