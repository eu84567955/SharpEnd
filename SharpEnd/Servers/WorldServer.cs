namespace SharpEnd.Servers
{
    internal sealed class WorldServer
    {
        public byte Identifier { get; private set; }
        public ChannelServer[] Channels { get; private set; }

        public short Status => 0;

        public WorldServer(byte identifier, ushort port, byte channels)
        {
            Identifier = identifier;
            Channels = new ChannelServer[channels];

            for (byte i = 0; i < channels; i++)
            {
                Channels[i] = new ChannelServer(i, port);

                port++;
            }
        }

        public void Run()
        {
            foreach(ChannelServer channel in Channels)
            {
                channel.Run();
            }
        }

        public void Shutdown()
        {
            foreach (ChannelServer channel in Channels)
            {
                channel.Shutdown();
            }
        }
    }
}
