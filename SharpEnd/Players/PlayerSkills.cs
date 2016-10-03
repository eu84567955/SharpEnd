using SharpEnd.Network;
using SharpEnd.Utility;

namespace SharpEnd.Players
{
    internal sealed class PlayerSkills
    {
        private Player m_player;

        public PlayerSkills(Player player, DatabaseQuery query)
        {
            m_player = player;
        }

        public void Save()
        {

        }

        public void WriteInitial(OutPacket outPacket)
        {
            // NOTE: Skills
            {
                outPacket
                    .WriteBoolean(true)
                    .WriteShort()
                    .WriteShort();
            }

            // NOTE: Cooldowns
            outPacket
                .WriteShort();
        }

        public void WriteInitialBlessings(OutPacket outPacket)
        {
            outPacket
                .WriteBoolean(false) // NOTE: Blessing of fairy
                .WriteBoolean(false) // NOTE: Blessing of empress
                .WriteBoolean(false); // NOTE: Ultimate explorer
        }
    }
}
