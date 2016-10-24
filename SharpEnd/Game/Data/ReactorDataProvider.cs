using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Game.Data
{
    internal sealed class ReactorDataProvider
    {
        private static ReactorDataProvider instance;

        public static ReactorDataProvider Instance { get { return instance ?? (instance = new ReactorDataProvider()); } }

        private Dictionary<int, ReactorData> m_reactors;

        private ReactorDataProvider()
        {
            m_reactors = new Dictionary<int, ReactorData>();
        }

        public void LoadData()
        {
            using (FileStream stream = File.OpenRead(Path.Combine("data", "reactors.bin")))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();

                    while (count-- > 0)
                    {
                        ReactorData reactor = new ReactorData();
                        reactor.Load(reader);
                        m_reactors[reactor.ID] = reactor;
                    }
                }
            }
        }

        public bool IsValidReactor(int reactorID)
        {
            return m_reactors.ContainsKey(reactorID);
        }

        public ReactorData GetReactorData(int reactorID)
        {
            return m_reactors[reactorID];
        }
    }

    #region Data Classes
    public sealed class ReactorData
    {
        private int m_id;
        private List<ReactorEventData> m_events;

        public int ID { get { return m_id; } set { m_id = value; } }
        public List<ReactorEventData> Events { get { return m_events; } set { m_events = value; } }

        public void Load(BinaryReader reader)
        {
            m_id = reader.ReadInt32();

            m_events = new List<ReactorEventData>();
            int eventsCount = reader.ReadInt32();
            while (eventsCount-- > 0)
            {
                ReactorEventData evt = new ReactorEventData();
                evt.Load(reader);
                m_events.Add(evt);
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_id);

            writer.Write(m_events.Count);
            m_events.ForEach(s => s.Save(writer));
        }
    }

    public sealed class ReactorEventData
    {
        private byte m_state;
        private short m_type;

        public byte State { get { return m_state; } set { m_state = value; } }
        public short Type { get { return m_type; } set { m_type = value; } }

        public void Load(BinaryReader reader)
        {
            m_state = reader.ReadByte();
            m_type = reader.ReadInt16();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_state);
            writer.Write(m_type);
        }
    }
    #endregion
}
