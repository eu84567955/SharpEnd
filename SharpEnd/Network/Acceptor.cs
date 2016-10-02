using System;
using System.Net;
using System.Net.Sockets;

namespace SharpEnd.Network
{
    internal sealed class Acceptor
    {
        public ushort Port { get; private set; }

        private readonly TcpListener m_listener;

        private bool m_disposed;

        public Action<Socket> OnClientAccepted;

        public Acceptor(ushort port)
            : this(IPAddress.Any, port)
        {
        }

        public Acceptor(IPAddress ip, ushort port)
        {
            Port = port;
            m_listener = new TcpListener(ip, port);
            OnClientAccepted = null;
            m_disposed = false;
        }

        public void Start()
        {
            m_listener.Start(50);
            m_listener.BeginAcceptSocket(EndAccept, null);
        }

        public void Stop()
        {
            Dispose();
        }

        private void EndAccept(IAsyncResult iar)
        {
            if (m_disposed) { return; }

            Socket client = m_listener.EndAcceptSocket(iar);

            OnClientAccepted?.Invoke(client);

            m_listener.BeginAcceptSocket(EndAccept, null);

        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                m_disposed = true;
                m_listener.Server.Close();
            }
        }
    }
}