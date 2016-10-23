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
            inPacket.ReadInt(); // NOTE: World alliance identifier.
            int accountID;
            int playerID = inPacket.ReadInt();

            if ((accountID = MasterServer.Instance.Worlds[client.World][client.Channel].Migrations.Validate(playerID, client.Host)) == 0)
            {
                client.Close();

                return;
            }

            using (DatabaseQuery query = Database.Query("SELECT * FROM account WHERE identifier=@identifier", new MySqlParameter("@identifier", accountID)))
            {
                query.NextRow();

                client.Account = new Account(query);
            }

            using (DatabaseQuery query = Database.Query("SELECT * FROM player WHERE identifier=@identifier", new MySqlParameter("@identifier", playerID)))
            {
                query.NextRow();

                client.Player = new Player(client, query);
            }

            client.Send(PlayerPackets.EventNameTag(new sbyte[5] { -1, -1, -1, -1, -1 }));
            client.Send(ServerPackets.EventList());
            client.Send(MapPackets.ChangeMap(client.Player, true));

            client.Player.Map.Players.Add(client.Player);

            //client.Send(PlayerPackets.Keymap(client.Player.Keymap));

            string tickerMessage = MasterServer.Instance.Worlds[client.World].TickerMessage;

            client.Send(MessagePackets.Notification(tickerMessage, ENoticeType.Ticker));

            // TODO: Check for expired items.
            // TODO: Check for berserk.
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
