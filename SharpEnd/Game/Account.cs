using MySql.Data.MySqlClient;
using SharpEnd.Utility;
using System;
using System.Collections.Generic;

namespace SharpEnd.Game
{
    public sealed class Account
    {
        private int m_id;
        private string m_username;
        private string m_password;
        private string m_pin;
        private string m_pic;

        private Dictionary<string, string> m_variables;

        public Account(DatabaseQuery query)
        {
            m_id = query.Get<int>("account_id");
            m_username = query.Get<string>("username");
            m_password = query.Get<string>("password");
            m_pin = query.Get<string>("pin");
            m_pic = query.Get<string>("pic");

            using (DatabaseQuery variableQuery = Database.Query("SELECT * FROM account_variables WHERE account_id=@account_id", new MySqlParameter("account_id", m_id)))
            {
                m_variables = new Dictionary<string, string>();

                while (variableQuery.NextRow())
                {
                    string key = query.Get<string>("key");
                    string value = query.Get<string>("value");

                    m_variables.Add(key, value);
                }
            }
        }

        public int Id { get { return m_id; } }
        public string Username { get { return m_username; } }
        public string Password { get { return m_password; } }
        public string Pin { get { return m_pin; } }
        public string Pic { get { return m_pic; } }

        public void Save()
        {
            Database.Execute("DELETE FROM account_variables WHERE account_id=@account_id", new MySqlParameter("account_id", m_id));

            foreach (KeyValuePair<string, string> entry in m_variables)
            {
                Database.Execute("INSERT INTO account_variables(account_id, key, value) " +
                                 "VALUES(@account_id, @key, @value)",
                                 new MySqlParameter("account_id", m_id),
                                 new MySqlParameter("key", entry.Key),
                                 new MySqlParameter("value", entry.Value));
            }
        }

        public bool ContainsVariable(string key)
        {
            return m_variables.ContainsKey(key);
        }

        public T GetVariable<T>(string key)
        {
            string value;

            if (m_variables.TryGetValue(key, out value))
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
            }

            return default(T);
        }

        public void SetVariable(string key, object value)
        {
            if (m_variables.ContainsKey(key))
            {
                m_variables[key] = value.ToString();
            }
            else
            {
                m_variables.Add(key, value.ToString());
            }
        }

        public void RemoveVariable(string key)
        {
            if (m_variables.ContainsKey(key))
            {
                m_variables.Remove(key);
            }
        }
    }
}
