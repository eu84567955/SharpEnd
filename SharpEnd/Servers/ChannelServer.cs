using SharpEnd.Network;
using System;
using System.Net.Sockets;

namespace SharpEnd.Servers
{
    internal sealed class ChannelServer
    {
        public byte Identifier { get; private set; }

        private Acceptor m_acceptor;

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
            var client = new Client(socket);

            Console.WriteLine("Connection accepted from {0}.", client.Host);
        }
    }
}
