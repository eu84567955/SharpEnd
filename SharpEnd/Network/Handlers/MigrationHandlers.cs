using MySql.Data.MySqlClient;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Game.Players;
using SharpEnd.Network.Servers;
using SharpEnd.Utility;
using SharpEnd.Game;

namespace SharpEnd.Handlers
{
    public static class MigrationHandlers
    {
        [PacketHandler(EHeader.CMSG_MIGRATE_TO_CHANNEL)]
        public static void MigrateToChannelHandler(GameClient client, InPacket inPacket)
        {
            inPacket.ReadInt(); // NOTE: Unknown.
            int accountID;
            int playerID = inPacket.ReadInt();

            if ((accountID = MasterServer.Instance.Worlds[client.World].Channels[client.Channel].Migrations.Validate(playerID, client.Host)) == 0)
            {
                client.Close();

                return;
            }

            using (DatabaseQuery query = Database.Query("SELECT * FROM accounts WHERE account_id=@account_id", new MySqlParameter("@account_id", accountID)))
            {
                query.NextRow();

                client.Account = new Account(query);
            }

            using (DatabaseQuery query = Database.Query("SELECT * FROM players WHERE player_id=@player_id", new MySqlParameter("@player_id", playerID)))
            {
                query.NextRow();

                client.Player = new Player(client, query);
            }

            client.Send(PlayerPackets.EventNameTag(new sbyte[5] { -1, -1, -1, -1, -1 }));
            client.Send(ServerPackets.EventList());
            client.Send(MapPackets.ChangeMap(client.Player, true));

            client.Player.Map.Players.Add(client.Player);

            string tickerMessage = MasterServer.Instance.Worlds[client.World].TickerMessage;

            client.Send(MessagePackets.Notification(tickerMessage, ENoticeType.Ticker));
        }

        [PacketHandler(EHeader.CMSG_MIGRATE_TO_CASH_SHOP)]
        public static void MigrateToCashShopHandler(GameClient client, InPacket inPacket)
        {

        }

        [PacketHandler(EHeader.CMSG_MIGRATE_TO_MONSTER_LIFE)]
        public static void MigrateToMonsterLifeHandler(GameClient client, InPacket inPacket)
        {

        }
    }
}
