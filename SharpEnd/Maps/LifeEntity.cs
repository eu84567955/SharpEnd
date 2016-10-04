using SharpEnd.Drawing;

namespace SharpEnd.Maps
{
    internal abstract class LifeEntity : MapEntity
    {
        public int Identifier { get; protected set; }
        public bool Flip { get; private set; }
        public bool Hide { get; private set; }

        protected LifeEntity(int identifier, Point position, ushort foothold, bool flip, bool hide)
            : base()
        {
            Identifier = identifier;
            Position = position;
            Foothold = foothold;
            Flip = flip;
            Hide = hide;
        }
    }
}
