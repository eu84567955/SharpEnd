using System;
using System.Net;
using System.Net.Sockets;

namespace SharpEnd.Network
{
    public sealed class Acceptor
    {
        private ushort m_port;
        private readonly TcpListener m_listener;
        private bool m_disposed;

        public Acceptor(ushort port) : this(IPAddress.Any, port) { }

        public Acceptor(IPAddress ip, ushort port)
        {
            m_port = port;
            m_listener = new TcpListener(ip, port);
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

            new GameClient(m_listener.EndAcceptSocket(iar));

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