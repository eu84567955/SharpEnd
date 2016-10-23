using SharpEnd.Collections;
using SharpEnd.IO;

namespace SharpEnd.Network.Servers
{
    public sealed class WorldServer : SafeKeyedCollection<byte, ChannelServer>
    {
        private byte m_worldId;
        private ushort m_port;
        private string m_name;
        private EWorldRibbon m_ribbon;
        private string m_eventMessage;
        private string m_tickerMessage;
        private int m_experienceRate;
        private int m_mesoRate;
        private int m_dropRate;

        public WorldServer(byte worldId)
        {
            m_worldId = worldId;

            var config = Config.Instance.WorldConfig[worldId];

            m_port = config.Port;
            m_name = config.Name;
            m_ribbon = config.Ribbon;
            m_eventMessage = config.EventMessage;
            m_tickerMessage = config.TickerMessage;
            m_experienceRate = config.ExperienceRate;
            m_mesoRate = config.MesoRate;
            m_dropRate = config.DropRate;

            for (byte channelId = 0; channelId < config.Channels; channelId++)
            {
                Add(new ChannelServer(m_worldId, channelId, GetChannelPort(channelId), config.BackgroundEvents));
            }
        }

        public byte Id { get { return m_worldId; } }
        public string Name { get { return m_name; } }
        public EWorldRibbon Ribbon { get { return m_ribbon; } }
        public string EventMessage { get { return m_eventMessage; } }
        public string TickerMessage { get { return m_tickerMessage; } }
        public int ExperienceRate { get { return m_experienceRate; } }
        public int MesoRate { get { return m_mesoRate; } }
        public int DropRate { get { return m_dropRate; } }
        public short Status { get { return 0; } } // TODO: Get status based on connected clients' count.

        protected override byte GetKeyForItem(ChannelServer item)
        {
            return item.Id;
        }

        public void Run()
        {
            foreach (ChannelServer channel in this)
            {
                channel.Run();
            }
        }

        public void Shutdown()
        {
            foreach (ChannelServer channel in this)
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
