using System.Collections.Generic;

namespace SharpEnd.Migrations
{
    internal sealed class MigrationRequests : List<MigrationRequest>
    {
        public MigrationRequests() : base() { }

        public bool Contains(int accountIdentifier)
        {
            return Exists(r => r.AccountIdentifier == accountIdentifier);
        }

        public void Register(int playerIdentifier, int accountIdentifier, string host)
        {
            Add(new MigrationRequest(playerIdentifier, accountIdentifier, host));
        }

        public int Validate(int playerIdentifier, string host)
        {
            MigrationRequest request = Find(r => r.PlayerIdentifier == playerIdentifier && r.Host == host);

            if (request == null)
            {
                return 0;
            }

            Remove(request);

            return request.AccountIdentifier;
        }
    }
}
