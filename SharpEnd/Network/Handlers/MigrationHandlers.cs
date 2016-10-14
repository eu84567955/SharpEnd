using MySql.Data.MySqlClient;
using SharpEnd.Network;
using SharpEnd.Players;
using SharpEnd.Servers;
using SharpEnd.Utility;

namespace SharpEnd.Handlers
{
    internal static class MigrationHandlers
    {
        [PacketHandler(EHeader.CMSG_MIGRATE_TO_CHANNEL)]
        public static void MigrateToChannelHandler(Client client, InPacket inPacket)
        {
            inPacket.ReadInt(); // NOTE: World alliance identifier.
            int accountIdentifier;
            int playerIdentifier = inPacket.ReadInt();

            if ((accountIdentifier = MasterServer.Instance.Migrations.Validate(playerIdentifier, client.Host)) == 0)
            {
                client.Close();

                return;
            }

            using (DatabaseQuery query = Database.Query("SELECT * FROM account WHERE identifier=@identifier", new MySqlParameter("@identifier", accountIdentifier)))
            {
                query.NextRow();

                client.Account = new Account(query);
            }

            using (DatabaseQuery query = Database.Query("SELECT * FROM player WHERE identifier=@identifier", new MySqlParameter("@identifier", playerIdentifier)))
            {
                query.NextRow();

                client.Player = new Player(client, query);
            }

            client.Player.Initialize();
        }

        [PacketHandler(EHeader.CMSG_MIGRATE_TO_CASH_SHOP)]
        public static void MigrateToCashShopHandler(Client client, InPacket inPacket)
        {

        }

        [PacketHandler(EHeader.CMSG_MIGRATE_TO_MONSTER_LIFE)]
        public static void MigrateToMonsterLifeHandler(Client client, InPacket inPacket)
        {

        }
    }
}
