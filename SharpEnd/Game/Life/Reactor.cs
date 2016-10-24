using SharpEnd.Drawing;
using SharpEnd.Game.Data;
using SharpEnd.Game.Maps;

namespace SharpEnd.Game.Life
{
    public sealed class Reactor : MapEntity
    {
        private ReactorData m_data;
        private string m_name;

        public Reactor(int reactorID)
        {
            m_data = ReactorDataProvider.Instance.GetReactorData(reactorID);
        }

        public Reactor(MapReactorData data)
            : this(data.ID)
        {
            m_position = new Point(data.X, data.Y);
            m_stance = (byte)(data.Flip ? 1 : 2);
            m_name = data.Name;
        }

        public int ID { get { return m_data.ID; } }
        public string Name { get { return m_name; } }
    }
}
