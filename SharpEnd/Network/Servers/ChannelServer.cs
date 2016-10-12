using SharpEnd.Data;
using SharpEnd.Maps;
using SharpEnd.Network;
using System;
using System.Net.Sockets;

namespace SharpEnd.Servers
{
    internal sealed class ChannelServer
    {
        public byte Identifier { get; private set; }

        private Acceptor m_acceptor;

        public Maps Maps { get; private set; }

        public ChannelServer(byte identifier, ushort port)
        {
            Identifier = identifier;

            m_acceptor = new Acceptor(port);

            m_acceptor.OnClientAccepted = OnClientAccepted;

            Maps = new Maps();
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

    // TODO: Find a better name and refactor.
    // This class purpose is to load and unload maps.
    internal sealed class Maps
    {
        private MapDataProvider m_provider;

        public Maps()
        {
            m_provider = new MapDataProvider();
        }

        public bool Contains(int identifier)
        {
            return m_provider.Contains(identifier);
        }

        public Map this[int identifier]
        {
            get
            {
                return m_provider[identifier];
            }
        }
    }
}
