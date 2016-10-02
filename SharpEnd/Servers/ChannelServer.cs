namespace SharpEnd.Servers
{
    internal sealed class ChannelServer
    {
        public byte Identifier { get; private set; }

        public ChannelServer(byte identifier, ushort port)
        {
            Identifier = identifier;
        }

        public void Run()
        {

        }

        public void Shutdown()
        {

        }
    }
}
