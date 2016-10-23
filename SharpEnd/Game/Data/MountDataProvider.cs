using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Game.Data
{
    internal sealed class MountDataProvider
    {
        private static MountDataProvider instance;

        public static MountDataProvider Instance { get { return instance ?? (instance = new MountDataProvider()); } }

        private Dictionary<int, MountData> m_mounts;

        private MountDataProvider()
        {
            m_mounts = new Dictionary<int, MountData>();
        }

        public void LoadData()
        {
            using (FileStream stream = File.OpenRead(Path.Combine("data", "mounts.bin")))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();

                    while (count-- > 0)
                    {
                        MountData mount = new MountData();
                        mount.Load(reader);
                        m_mounts[mount.ID] = mount;
                    }
                }
            }
        }

        public bool IsValidMount(int mountID)
        {
            return m_mounts.ContainsKey(mountID);
        }

        public MountData GetMountData(int mountID)
        {
            return m_mounts[mountID];
        }
    }

    #region Data Classes
    public sealed class MountData
    {
        private int m_id;
        private double m_fs;
        private double m_swim;
        private int m_fatigue;
        private int m_jump;
        private int m_speed;

        public int ID { get { return m_id; } set { m_id = value; } }
        public double FS { get { return m_fs; } set { m_fs = value; } }
        public double Swim { get { return m_swim; } set { m_swim = value; } }
        public int Fatigue { get { return m_fatigue; } set { m_fatigue = value; } }
        public int Jump { get { return m_jump; } set { m_jump = value; } }
        public int Speed { get { return m_speed; } set { m_speed = value; } }

        public void Load(BinaryReader reader)
        {
            m_id = reader.ReadInt32();
            m_fs = reader.ReadDouble();
            m_swim = reader.ReadDouble();
            m_fatigue = reader.ReadInt32();
            m_jump = reader.ReadInt32();
            m_speed = reader.ReadInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_id);
            writer.Write(m_fs);
            writer.Write(m_swim);
            writer.Write(m_fatigue);
            writer.Write(m_jump);
            writer.Write(m_speed);
        }
    }
    #endregion
}
