using SharpEnd.Drawing;
using SharpEnd.Game.Data;
using SharpEnd.Game.Maps;

namespace SharpEnd.Game.Life
{
    public sealed class Npc : MapEntity
    {
        private NpcData m_data;
        private short m_rx0;
        private short m_rx1;
        private bool m_hide;

        public Npc(int npcID)
        {
            m_data = NpcDataProvider.Instance.GetNpcData(npcID);
        }

        public Npc(MapSpawnData spawn)
            : this(spawn.ID)
        {
            m_position = new Point(spawn.X, spawn.CY); // TOOD: Figure out why the client uses CY instead of Y for positioning npcs.
            m_stance = (byte)(spawn.Flip ? 1 : 0);
            m_foothold = spawn.Foothold;
            m_rx0 = spawn.RX0;
            m_rx1 = spawn.RX1;
            m_hide = spawn.Hide;
        }

        public int ID { get { return m_data.ID; } }
        public short RX0 { get { return m_rx0; } }
        public short RX1 { get { return m_rx1; } }
        public bool Hide { get { return m_hide; } }
    }
}
