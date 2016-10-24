using SharpEnd.Drawing;
using SharpEnd.Game.Data;
using SharpEnd.Game.Maps;
using SharpEnd.Game.Players;

namespace SharpEnd.Game.Life
{
    public sealed class Mob : MapEntity
    {
        private MobData m_data;
        private int m_hp;
        private int m_mp;
        private Player m_controller;

        public Mob(int mobID)
        {
            m_data = MobDataProvider.Instance.GetMobData(mobID);
            m_hp = m_data.MaxHP;
            m_mp = m_data.MaxMP;
        }

        public Mob(MapSpawnData spawn)
            : this(spawn.ID)
        {
            m_position = new Point(spawn.X, spawn.Y);
            m_stance = 5; // TODO.
            m_foothold = spawn.Foothold;
        }

        public int ID { get { return m_data.ID; } }
        public byte Level { get { return m_data.Level; } }
        public int HP { get { return m_hp; } set { m_hp = value; } }
        public int MaxHP { get { return m_data.MaxHP; } }
        public int MP { get { return m_mp; } set { m_mp = value; } }
        public int MaxMP { get { return m_data.MaxMP; } }
        public int Experience { get { return m_data.Experience; } }
        public EMobControlStatus ControlStatus { get { return m_controller == null ? EMobControlStatus.None : EMobControlStatus.Normal; } }
    }
}
