using SharpEnd.Drawing;
using static SharpEnd.Game.Data.MapData;

namespace SharpEnd.Game.Maps
{
    internal sealed class Portal
    {
        public sbyte Identifier { get; private set; }
        public Point Position { get; private set; }
        public string Label { get; private set; }
        public int ToMap { get; private set; }
        public string ToName { get; private set; }
        public string Script { get; private set; }

        public Portal(MapPortalData data)
        {
            Identifier = data.Identifier;
            Position = data.Position;
            Label = data.Label;
            ToMap = data.ToMap;
            ToName = data.ToName;
            Script = data.Script;
        }
    }
}
