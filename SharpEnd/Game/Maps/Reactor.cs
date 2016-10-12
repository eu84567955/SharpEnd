using SharpEnd.Drawing;

namespace SharpEnd.Maps
{
    internal sealed class Reactor : LifeEntity
    {
        public string Label { get; private set; }

        public int Time { get; private set; }

        public sbyte State { get; private set; }

        public Reactor(int identifier, string label, int time, Point position, ushort foothold, bool flip, bool hide)
            : base(identifier, position, foothold, flip, hide)
        {
            Time = time;

            State = 0;

            Label = label;
        }
    }
}
