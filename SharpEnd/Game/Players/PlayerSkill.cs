using MySql.Data.MySqlClient;
using SharpEnd.Network;
using SharpEnd.Utility;
using System;

namespace SharpEnd.Players
{
    internal sealed class PlayerSkill
    {
        public PlayerSkills Parent { get; set; }

        public int Identifier { get; private set; }
        public int Level { get; private set; }
        public int MaxLevel { get; private set; }
        public DateTime Expiration { get; private set; }

        private bool m_assigned;

        public PlayerSkill(DatabaseQuery query)
        {
            Identifier = query.Get<int>("skill_identifier");
            Level = query.Get<int>("level");
            Expiration = query.Get<DateTime>("expiration");

            m_assigned = true;
        }

        public PlayerSkill(int identifier, int level = 1)
        {
            Identifier = identifier;
            Level = level;
            MaxLevel = 0; // TODO: Get from SkillDataProvider.
            Expiration = DateTime.FromBinary(150842304000000000);
        }

        public void Save()
        {
            if (m_assigned)
            {
                // TODO: Update.
            }
            else
            {
                Database.Execute("INSERT INTO player_skill VALUES(@player_identifier, @skill_identifier, @level, @max_level, @expiration);",
                                new MySqlParameter("player_identifier", Parent.Parent.Identifier),
                                new MySqlParameter("skill_identifier", Identifier),
                                new MySqlParameter("level", Level),
                                new MySqlParameter("max_level", MaxLevel),
                                new MySqlParameter("expiration", Expiration));
            }
        }

        public void WriteGeneral(OutPacket outPacket)
        {
            outPacket
                .WriteInt(Identifier)
                .WriteInt(Level)
                .WriteLong(Expiration.ToBinary());
        }
    }
}
