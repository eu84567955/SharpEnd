using SharpEnd.Network;
using System;
using System.Net.Sockets;

namespace SharpEnd.Servers
{
    internal sealed class LoginServer
    {
        private Acceptor m_acceptor;

        public LoginServer(ushort port)
        {
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
