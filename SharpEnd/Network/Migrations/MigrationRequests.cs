using System.Collections.Generic;

namespace SharpEnd.Migrations
{
    public sealed class MigrationRequests : List<MigrationRequest>
    {
        public MigrationRequests() : base() { }

        public bool Contains(int accountID)
        {
            return Exists(r => r.AccountID == accountID);
        }

        public void Register(int playerID, int accountID, string host)
        {
            Add(new MigrationRequest(playerID, accountID, host));
        }

        public int Validate(int playerID, string host)
        {
            MigrationRequest request = Find(r => r.PlayerID == playerID/* && r.Host == host*/);

            if (request == null)
            {
                return 0;
            }

            Remove(request);

            return request.AccountID;
        }
    }
}
