using SharpEnd.Drawing;
using SharpEnd.Servers;
using static SharpEnd.Game.Data.MapData;

namespace SharpEnd.Game.Maps
{
    internal sealed class Portal
    {
        public sbyte Identifier { get; private set; }
        public Point Position { get; private set; }
        public string Label { get; private set; }
        public int DestinationMapIdentifier { get; private set; }
        public string DestinationLabel { get; private set; }
        public string Script { get; private set; }

        public bool IsSpawnPoint
        {
            get
            {
                return Label == "sp";
            }
        }

        public Map DestinationMap
        {
            get
            {
                return MasterServer.Instance.GetMap(DestinationMapIdentifier);
            }
        }

        public Portal Link
        {
            get
            {
                return MasterServer.Instance.GetMap(DestinationMapIdentifier).Portals[DestinationLabel];
            }
        }

        public Portal(MapPortalData data)
        {
            Identifier = data.Identifier;
            Position = data.Position;
            Label = data.Label;
            DestinationMapIdentifier = data.ToMap;
            DestinationLabel = data.ToName;
            Script = data.Script;
        }
    }
}
