using MySql.Data.MySqlClient;
using System;

namespace SharpEnd.Utility
{
    public class TemporarySchema : IDisposable
    {
        private string oldSchema;

        public TemporarySchema(string schema)
        {
            this.oldSchema = Database.Schema;
            Database.Schema = schema;
        }

        public void Dispose()
        {
            Database.Schema = this.oldSchema;
        }
    }

    public sealed class Database
    {
        public static string Host { get; internal set; }
        public static string Schema { get; internal set; }
        public static string Username { get; internal set; }
        public static string Password { get; internal set; }

        public static string ConnectionString
        {
            get
            {
                return string.Format("server={0}; database={1}; uid={2}; password={3}; convertzerodatetime=yes;",
                    Database.Host,
                    Database.Schema,
                    Database.Username,
                    Database.Password);
            }
        }

        public static void Test()
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            connection.Close();

            Log.Inform("Able to connect to database '{0}'.", Database.Schema);
        }

        public static DatabaseQuery Query(string pQuery, params MySqlParameter[] pParams)
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = pQuery;
            Array.ForEach(pParams, p => command.Parameters.Add(p));
            return new DatabaseQuery(connection, command.ExecuteReader());
        }

        public static void Execute(string pStatement, params MySqlParameter[] pParams)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = pStatement;
                Array.ForEach(pParams, p => command.Parameters.Add(p));
                command.ExecuteNonQuery();
            }
        }

        public static object Scalar(string pQuery, params MySqlParameter[] pParams)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = pQuery;
                Array.ForEach(pParams, p => command.Parameters.Add(p));
                return command.ExecuteScalar();
            }
        }

        public static int InsertAndReturnID(string pStatement, params MySqlParameter[] pParams)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = pStatement;
                Array.ForEach(pParams, p => command.Parameters.Add(p));
                command.ExecuteNonQuery();
                command.CommandText = "SELECT LAST_INSERT_ID()";
                command.Parameters.Clear();
                return (int)(ulong)command.ExecuteScalar();
            }
        }

        public static TemporarySchema TemporarySchema(string schema)
        {
            return new TemporarySchema(schema);
        }
    }
}