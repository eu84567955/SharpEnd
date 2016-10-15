using SharpEnd.Drawing;
using static SharpEnd.Game.Data.MapData;

namespace SharpEnd.Game.Maps
{
    internal sealed class Foothold
    {
        public ushort Identifier { get; private set; }
        public ushort NextIdentifier { get; private set; }
        public ushort PreviousIdentifier { get; private set; }
        public short DragForce { get; private set; }
        public Line Line { get; private set; }

        public Foothold(MapFootholdData data)
        {
            Identifier = data.Identifier;
            NextIdentifier = data.NextIdentifier;
            PreviousIdentifier = data.PreviousIdentifier;
            DragForce = data.DragForce;
            Line = new Line(data.Point1, data.Point2);
        }
    }
}
