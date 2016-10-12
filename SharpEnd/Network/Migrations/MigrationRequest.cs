using System;

namespace SharpEnd.Migrations
{
    internal sealed class MigrationRequest
    {
        public int PlayerIdentifier { get; private set; }
        public int AccountIdentifier { get; private set; }
        public string Host { get; private set; }
        public DateTime Started { get; private set; }

        public MigrationRequest(int playerIdentifier, int accountIdentifier, string host)
        {
            PlayerIdentifier = playerIdentifier;
            AccountIdentifier = accountIdentifier;
            Host = host;
            Started = DateTime.Now;
        }
    }
}
