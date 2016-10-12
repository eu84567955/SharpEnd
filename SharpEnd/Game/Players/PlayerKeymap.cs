using MySql.Data.MySqlClient;
using SharpEnd.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEnd.Players
{
    internal sealed class Shortcut
    {
        public byte Type { get; private set; }
        public int Action { get; private set; }

        public Shortcut(byte type, int action)
        {
            Type = type;
            Action = action;
        }
    }

    internal sealed class PlayerKeymap : Dictionary<int, Shortcut>
    {
        public Player Parent { get; private set; }

        public bool IsModified { get; set; }

        public PlayerKeymap(Player parent, DatabaseQuery query)
            : base()
        {
            Parent = parent;

            while (query.NextRow())
            {
                int identifier = query.Get<int>("key_identifier");
                byte type = query.Get<byte>("type");
                int action = query.Get<int>("action");

                Add(identifier, new Shortcut(type, action));
            }
        }

        public void Save()
        {
            if (!IsModified) // NOTE: We don't save unless the player actually changed it.
            {
                return;
            }

            Database.Execute("DELETE FROM player_keymap WHERE player_identifier=@player_identifier", new MySqlParameter("player_identifier", Parent.Identifier));

            foreach (KeyValuePair<int, Shortcut> entry in this)
            {
                Database.Execute("INSERT INTO player_keymap VALUES(@player_identifier, @key_identifier, @type, @action)",
                                 new MySqlParameter("player_identifier", Parent.Identifier),
                                 new MySqlParameter("key_identifier", entry.Key),
                                 new MySqlParameter("type", entry.Value.Type),
                                 new MySqlParameter("action", entry.Value.Action));
            }
        }
    }
}
