using SharpEnd.Network;
using System.Net.Sockets;

namespace SharpEnd.Servers
{
    internal sealed class ChannelServer
    {
        private Acceptor m_acceptor;

        public byte Identifier { get; private set; }

        public ChannelServer(byte identifier, ushort port)
        {
            Identifier = identifier;

            m_acceptor = new Acceptor(port);

            m_acceptor.OnClientAccepted = OnClientAccepted;
        }

        public void Run()
        {
            m_acceptor.Start();
        }

        public void Shutdown()
        {
            m_acceptor.Stop();
        }

        private void OnClientAccepted(Socket socket)
        {
            new Client(socket);
        }
    }
}
