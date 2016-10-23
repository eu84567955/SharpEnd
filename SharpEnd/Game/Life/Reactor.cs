using SharpEnd.Drawing;
using SharpEnd.Game.Data;
using SharpEnd.Game.Maps;

namespace SharpEnd.Game.Life
{
    public sealed class Reactor : MapEntity
    {
        private int m_id;
        private bool m_flip;
        private string m_label;

        public Reactor(MapReactorData data)
        {
            //m_id = data.Id;
            //m_position = new Point(data.X, data.Y);
            //m_flip = data.Flip;
            //m_label = data.Label;
        }

        public int Id { get { return m_id; } }
        public bool Flip { get { return m_flip; } }
        public string Label { get { return m_label; } }
    }
}
