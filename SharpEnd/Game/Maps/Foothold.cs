using SharpEnd.Drawing;
using SharpEnd.Game.Data;

namespace SharpEnd.Game.Maps
{
    public sealed class Foothold
    {
        private short m_id;
        private Line m_line;

        public short ID { get { return m_id; } }
        public Line Line { get { return m_line; } }

        public Foothold(MapFootholdData data)
        {
            m_id = data.ID;
            m_line = new Line(
                new Point(data.X1, data.Y1),
                new Point(data.X2, data.Y2)
                );
        }
    }
}
