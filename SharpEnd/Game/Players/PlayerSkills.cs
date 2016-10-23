using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Utility;
using System.Collections.Generic;

namespace SharpEnd.Game.Players
{
    public sealed class PlayerSkills : List<PlayerSkill>
    {
        public Player Parent { get; private set; }

        public PlayerSkills(Player parent, DatabaseQuery query)
        {
            Parent = parent;

            while (query.NextRow())
            {
                Add(new PlayerSkill(query));
            }
        }

        public new void Add(PlayerSkill skill)
        {
            skill.Parent = this;

            base.Add(skill);

            /*if (Parent.IsInitialized)
            {
                Parent.Send(SkillPackets.AddSkill(skill));
            }*/
        }

        public new void Remove(PlayerSkill skill)
        {
            // TODO: Packet.

            skill.Parent = null;

            base.Remove(skill);
        }

        public void Save()
        {
            foreach (PlayerSkill skill in this)
            {
                skill.Save();
            }
        }

        public void WriteInitial(OutPacket outPacket)
        {
            // NOTE: Skills
            {
                outPacket
                    .WriteBoolean(true)
                    .WriteShort((short)Count);

                foreach (PlayerSkill skill in this)
                {
                    skill.WriteGeneral(outPacket);
                }

                outPacket
                    .WriteShort();
            }

            // NOTE: Cooldowns
            outPacket
                .WriteShort();
        }

        public void WriteBlessings(OutPacket outPacket)
        {
            outPacket
                .WriteBoolean(false) // NOTE: Blessing of fairy
                .WriteBoolean(false) // NOTE: Blessing of empress
                .WriteBoolean(false); // NOTE: Ultimate explorer
        }
    }
}
