using MySql.Data.MySqlClient;
using SharpEnd.Network;
using SharpEnd.Utility;
using System;

namespace SharpEnd.Game.Players
{
    public sealed class PlayerSkill
    {
        public PlayerSkills Parent { get; set; }

        public int ID { get; private set; }
        public int Level { get; private set; }
        public int MaxLevel { get; private set; }
        public DateTime Expiration { get; private set; }

        private bool m_assigned;

        public PlayerSkill(DatabaseQuery query)
        {
            ID = query.Get<int>("skill_identifier");
            Level = query.Get<int>("level");
            Expiration = query.Get<DateTime>("expiration");

            m_assigned = true;
        }

        public PlayerSkill(int identifier, int level = 1)
        {
            ID = identifier;
            Level = level;
            MaxLevel = 0; // TODO: Get from SkillDataProvider.
            Expiration = DateTime.FromFileTimeUtc((long)EExpirationTime.Permanent);
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
                                new MySqlParameter("player_identifier", Parent.Parent.ID),
                                new MySqlParameter("skill_identifier", ID),
                                new MySqlParameter("level", Level),
                                new MySqlParameter("max_level", MaxLevel),
                                new MySqlParameter("expiration", Expiration));
            }
        }

        public void WriteGeneral(OutPacket outPacket)
        {
            outPacket
                .WriteInt(ID)
                .WriteInt(Level)
                .WriteDateTime(Expiration);
        }
    }
}
