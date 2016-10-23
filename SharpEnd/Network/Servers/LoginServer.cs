using SharpEnd.IO;

namespace SharpEnd.Network.Servers
{
    public sealed class LoginServer
    {
        private bool m_requestPin;
        private bool m_requestPic;
        private bool m_requireStaffIP;
        private ushort m_port;
        private int m_defaultCharacterSlots;
        private int m_invalidLoginThreshold;
        private int m_rankingUpdateFrequency;
        private Acceptor m_acceptor;

        public LoginServer()
        {
            var config = Config.Instance.LoginConfig;

            m_requestPin = config.RequestPin;
            m_requestPic = config.RequestPic;
            m_requireStaffIP = config.RequireStaffIP;
            m_port = config.Port;
            m_defaultCharacterSlots = config.DefaultCharacterSlots;
            m_invalidLoginThreshold = config.InvalidLoginThreshold;
            m_rankingUpdateFrequency = config.RankingUpdateFrequency;
            m_acceptor = new Acceptor(m_port);
        }

        public bool RequestPin { get { return m_requestPin; } }
        public bool RequestPic { get { return m_requestPic; } }
        public bool RequireStaffIP { get { return m_requireStaffIP; } }
        public int DefaultCharacterSlots { get { return m_defaultCharacterSlots; } }

        public void Run()
        {
            m_acceptor.Start();
        }

        public void Shutdown()
        {
            m_acceptor.Stop();
        }
    }
}
