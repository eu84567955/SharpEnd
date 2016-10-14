using MySql.Data.MySqlClient;
using SharpEnd.Utility;
using System.Collections.Generic;

namespace SharpEnd.Players
{
    internal sealed class PlayerVariables : Dictionary<string, string>
    {
        public Player Parent { get; private set; }

        public PlayerVariables(Player parent, DatabaseQuery query)
            : base()
        {
            Parent = parent;

            while (query.NextRow())
            {
                string key = query.Get<string>("key");
                string value = query.Get<string>("value");

                Add(key, value);
            }
        }

        public void Save()
        {
            Database.Execute("DELETE FROM player_variable WHERE player_identifier=@player_identifier", new MySqlParameter("player_identifier", Parent.Identifier));

            foreach(KeyValuePair<string, string> entry in this)
            {
                Database.Execute("INSERT INTO player_variable " +
                                 "VALUES(@player_identifier, @key, @value)",
                                 new MySqlParameter("player_identifier", Parent.Identifier),
                                 new MySqlParameter("key", entry.Key),
                                 new MySqlParameter("value", entry.Value));
            }
        }
    }
}
