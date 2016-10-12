using SharpEnd.Network;
using SharpEnd.Players;
using SharpEnd.Utility;

namespace SharpEnd.Handlers
{
    internal static class SkillHandlers
    {
        [PacketHandler(EHeader.CMSG_SKILL_ADD)]
        public static void SkillAddHandler(Client client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.Skip(4); // NOTE: Ticks.
            int skillIdentifier = inPacket.ReadInt();
            int amount = inPacket.ReadInt();

            if (GameLogicUtilities.HasSeparatedSkillPoints(player.Stats.Job))
            {
                byte advancement = GameLogicUtilities.GetAdvancementFromSkill(skillIdentifier);

                int current = player.SPTable[advancement];

                if (amount > current)
                {
                    return;
                }

                player.SPTable.SetSkillPoints(advancement, current - amount);
            }
            else
            {
                player.Stats.SetSkillPoints((ushort)(player.Stats.SkillPoints - amount));
            }

            player.Skills.Add(new PlayerSkill(skillIdentifier, amount));
        }

        [PacketHandler(EHeader.CMSG_SKILL_USE)]
        public static void SkillUseHandler(Client client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.Skip(4); // NOTE: Ticks.
            int skillIdentifier = inPacket.ReadInt();
            int skillLevel = inPacket.ReadInt();

            player.Release();
        }
    }
}
