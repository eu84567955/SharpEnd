using SharpEnd.Drawing;
using SharpEnd.Game.Data;

namespace SharpEnd.Game.Maps
{
    public sealed class Portal
    {
        private byte m_id;
        private Point m_position;
        private int m_type;
        private int m_destinationMap;
        private string m_destinationName;
        private string m_name;
        private string m_script;

        public byte ID { get { return m_id; } }
        public Point Position { get { return m_position; } }
        public int Type { get { return m_type; } }
        public int DestinationMap { get { return m_destinationMap; } }
        public string DestinationName { get { return m_destinationName; } }
        public string Name { get { return m_name; } }
        public string Script { get { return m_script; } }
        public bool IsSpawnPoint { get { return m_name == "sp"; } }

        public Portal(MapPortalData data)
        {
            m_id = data.ID;
            m_position = new Point(data.X, data.Y);
            m_type = data.Type;
            m_destinationMap = data.DestinationMap;
            m_destinationName = data.DestinationName;
            m_name = data.Name;
            m_script = data.Script;
        }
    }
}
