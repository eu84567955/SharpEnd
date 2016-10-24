using SharpEnd.IO;

namespace SharpEnd.Network.Servers
{
    public sealed class WorldServer
    {
        private byte m_id;
        private ushort m_port;
        private string m_name;
        private EWorldRibbon m_ribbon;
        private string m_eventMessage;
        private string m_tickerMessage;
        private int m_experienceRate;
        private int m_mesoRate;
        private int m_dropRate;
        private ChannelServer[] m_channels;

        public WorldServer(byte id)
        {
            m_id = id;

            var config = Config.Instance.WorldConfig[id];

            m_port = config.Port;
            m_name = config.Name;
            m_ribbon = config.Ribbon;
            m_eventMessage = config.EventMessage;
            m_tickerMessage = config.TickerMessage;
            m_experienceRate = config.ExperienceRate;
            m_mesoRate = config.MesoRate;
            m_dropRate = config.DropRate;
            m_channels = new ChannelServer[config.Channels];

            for (byte channelId = 0; channelId < config.Channels; channelId++)
            {
                m_channels[channelId] = new ChannelServer(m_id, channelId, GetChannelPort(channelId), config.BackgroundEvents);
            }
        }

        public byte ID { get { return m_id; } }
        public string Name { get { return m_name; } }
        public EWorldRibbon Ribbon { get { return m_ribbon; } }
        public string EventMessage { get { return m_eventMessage; } }
        public string TickerMessage { get { return m_tickerMessage; } }
        public int ExperienceRate { get { return m_experienceRate; } }
        public int MesoRate { get { return m_mesoRate; } }
        public int DropRate { get { return m_dropRate; } }
        public ChannelServer[] Channels { get { return m_channels; } }
        public short Status { get { return 0; } } // TODO: Get status based on connected clients' count.

        public void Run()
        {
            foreach (ChannelServer channel in m_channels)
            {
                channel.Run();
            }
        }

        public void Shutdown()
        {
            foreach (ChannelServer channel in m_channels)
            {
                channel.Shutdown();
            }
        }

        private ushort GetChannelPort(byte channelId)
        {
            return (ushort)(m_port + channelId);
        }
    }
}
