using System;

namespace SharpEnd.Migrations
{
    public sealed class MigrationRequest
    {
        public int PlayerID { get; private set; }
        public int AccountID { get; private set; }
        public string Host { get; private set; }
        public DateTime Started { get; private set; }

        public MigrationRequest(int playerID, int accountID, string host)
        {
            PlayerID = playerID;
            AccountID = accountID;
            Host = host;
            Started = DateTime.Now;
        }
    }
}
