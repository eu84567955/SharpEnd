using SharpEnd.Network;
using SharpEnd.Players;

namespace SharpEnd.Packets
{
    internal static class SkillPackets
    {
        public static byte[] AddSkill(PlayerSkill skill)
        {
            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_SKILL_ADD)
                    .WriteByte(1)
                    .WriteShort()
                    .WriteShort(1)
                    .WriteInt(skill.Identifier)
                    .WriteInt(skill.Level)
                    .WriteInt(skill.MaxLevel)
                    .WriteDateTime(skill.Expiration)
                    .WriteByte(7); // NOTE: Unknown.

                return outPacket.ToArray();
            }
        }
    }
}
