using MySql.Data.MySqlClient;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Utility;
using System.Collections.Generic;

namespace SharpEnd.Game.Players
{
    public sealed class PlayerSPTable : Dictionary<byte, int>
    {
        public Player Parent { get; private set; }

        public PlayerSPTable(Player parent, DatabaseQuery query)
            : base()
        {
            Parent = parent;

            while (query.NextRow())
            {
                byte advancement = query.Get<byte>("advancement");
                int points = query.Get<int>("points");

                Add(advancement, points);
            }
        }

        public void Save()
        {
            foreach (KeyValuePair<byte, int> entry in this)
            {
                bool exists = (long)Database.Scalar("SELECT COUNT(*) FROM player_sp_table WHERE player_identifier=@player_identifier AND advancement=@advancement",
                                new MySqlParameter("player_identifier", Parent.ID),
                                new MySqlParameter("advancement", entry.Key)) != 0;

                if (exists)
                {
                    Database.Execute("UPDATE player_sp_table SET points=@points WHERE player_identifier=@player_identifier AND advancement=@advancement",
                                   new MySqlParameter("player_identifier", Parent.ID),
                                   new MySqlParameter("advancement", entry.Key),
                                   new MySqlParameter("points", entry.Value));
                }
                else
                {
                    Database.Execute("INSERT INTO player_sp_table VALUES(@player_identifier, @advancement, @points);",
                                    new MySqlParameter("player_identifier", Parent.ID),
                                    new MySqlParameter("advancement", entry.Key),
                                    new MySqlParameter("points", entry.Value));
                }
            }
        }

        public void WriteGeneral(OutPacket outPacket)
        {
            outPacket.WriteByte((byte)Count);

            foreach (KeyValuePair<byte, int> entry in this)
            {
                outPacket
                    .WriteByte(entry.Key)
                    .WriteInt(entry.Value);
            }
        }

        public void SetSkillPoints(byte advancement, int value)
        {
            this[advancement] = value;

            //Parent.Send(PlayerPackets.PlayerUpdate(Parent, EPlayerUpdate.SkillPoints));
        }
    }
}
