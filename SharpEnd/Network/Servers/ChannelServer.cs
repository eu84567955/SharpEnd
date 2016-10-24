using SharpEnd.Game.Maps;
using SharpEnd.Game.Scripting;
using SharpEnd.Migrations;
using SharpEnd.Threading;

namespace SharpEnd.Network.Servers
{
    public sealed class ChannelServer
    {
        private byte m_worldId;
        private byte m_channelId;
        private ushort m_port;
        private MapFactory m_mapFactory;
        private EventManager m_eventManager;
        private MigrationRequests m_migrations;
        private PlayerLog m_storage;
        private Acceptor m_acceptor;
        private Delay m_respawnTimer;

        public ChannelServer(byte worldId, byte channelId, ushort port, string[] backgroundEvents)
        {
            m_worldId = worldId;
            m_channelId = channelId;
            m_port = port;
            m_mapFactory = new MapFactory();
            m_eventManager = new EventManager(this, backgroundEvents);
            m_migrations = new MigrationRequests();
            m_storage = new PlayerLog();
            m_acceptor = new Acceptor(m_port);
            m_respawnTimer = new Delay(15 * 1000, () => { }, true);
        }

        public byte Id { get { return m_channelId; } }
        public ushort Port { get { return m_port; } }
        public MapFactory MapFactory { get { return m_mapFactory; } }
        public EventManager EventManager { get { return m_eventManager; } }
        public MigrationRequests Migrations { get { return m_migrations; } }

        public void Run()
        {
            m_eventManager.Initialize();
            m_acceptor.Start();
        }

        public void Shutdown()
        {
            m_acceptor.Stop();
            m_respawnTimer.Cancel();
        }
    }
}
