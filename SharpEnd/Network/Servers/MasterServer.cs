using SharpEnd.Game.Data;
using SharpEnd.IO;
using SharpEnd.Utility;
using System.Diagnostics;

namespace SharpEnd.Network.Servers
{
    public sealed class MasterServer
    {
        private static MasterServer instance;

        public static MasterServer Instance
        {
            get
            {
                return instance ?? (instance = new MasterServer());
            }
        }

        private bool m_running;
        private LoginServer m_login;
        private WorldServer[] m_worlds;

        private MasterServer()
        {
            m_login = new LoginServer();

            int worldCount = Config.Instance.WorldConfig.Length;

            m_worlds = new WorldServer[worldCount];

            for (byte i = 0; i < worldCount; i++)
            {
                m_worlds[i] = new WorldServer(i);
            }
        }

        public bool IsRunning { get { return m_running; } }
        public LoginServer Login { get { return m_login; } }
        public WorldServer[] Worlds { get { return m_worlds; } }

        public void Run()
        {
            HandlerStore.Instance.Initialize();

            LoadData();

            m_login.Run();

            foreach (WorldServer world in m_worlds)
            {
                world.Run();
            }

            m_running = true;

            Log.Success("SharpEnd started.");
        }

        private void LoadData()
        {
            Stopwatch sw = Stopwatch.StartNew();

            BeautyDataProvider.Instance.LoadData();
            EquipDataProvider.Instance.LoadData();
            MobDataProvider.Instance.LoadData();
            NpcDataProvider.Instance.LoadData();
            ReactorDataProvider.Instance.LoadData();
            QuestDataProvider.Instance.LoadData();
            MapDataProvider.Instance.LoadData();
            MountDataProvider.Instance.LoadData();

            sw.Stop();

            Log.Inform("Maple data loaded in {0:N3} seconds.", sw.Elapsed.TotalSeconds);
        }

        public void Shutdown()
        {
            m_login.Shutdown();

            foreach (WorldServer world in m_worlds)
            {
                world.Shutdown();
            }

            m_running = false;

            Log.Inform("SharpEnd stopped.");
        }
    }
}
