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

        public Account(DatabaseQuery query)
        {
            m_id = query.Get<int>("account_id");
            m_username = query.Get<string>("username");
            m_password = query.Get<string>("password");
            m_pin = query.Get<string>("pin");
            m_pic = query.Get<string>("pic");
        }

        public int Id { get { return m_id; } }
        public string Username { get { return m_username; } }
        public string Password { get { return m_password; } }
        public string Pin { get { return m_pin; } }
        public string Pic { get { return m_pic; } }

        public void Save()
        {

        }
    }
}
