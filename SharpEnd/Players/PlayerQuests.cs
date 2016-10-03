using SharpEnd.Network;
using SharpEnd.Utility;

namespace SharpEnd.Players
{
    internal sealed class PlayerQuests
    {
        private Player m_player;

        public PlayerQuests(Player player, DatabaseQuery query)
        {
            m_player = player;
        }

        public void Save()
        {

        }

        public void WriteInitial(OutPacket outPacket)
        {
            // NOTE: Started
            {
                outPacket
                    .WriteBoolean(true)
                    .WriteShort();
            }

            // NOTE: NX
            {
                outPacket.WriteShort();
            }

            // NOTE: Completed
            outPacket
                .WriteBoolean(true)
                .WriteShort();
        }
    }
}
