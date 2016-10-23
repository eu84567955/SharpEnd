using SharpEnd.Drawing;
using SharpEnd.Game.Data;

namespace SharpEnd.Game.Maps
{
    public sealed class Portal
    {
        public sbyte ID { get; private set; }
        public Point Position { get; private set; }
        public string Label { get; private set; }
        public int DestinationMapID { get; private set; }
        public string DestinationLabel { get; private set; }
        public string Script { get; private set; }

        public bool IsSpawnPoint
        {
            get
            {
                return Label == "sp";
            }
        }

        public Portal(MapPortalData data)
        {
            //ID = data.ID;
            //Position = data.Position;
            //Label = data.Label;
            //DestinationMapID = data.ToMap;
            //DestinationLabel = data.ToName;
            //Script = data.Script;
        }
    }
}
