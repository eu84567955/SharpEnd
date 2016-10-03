using MySql.Data.MySqlClient;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Players;
using SharpEnd.Utility;

namespace SharpEnd.Handlers
{
    internal static class MigrationHandler
    {
        [PacketHandler(EHeader.CMSG_PLAYER_LOAD)]
        public static void PlayerLoad(Client client, InPacket inPacket)
        {
            inPacket.ReadInt(); // NOTE: Alliance identifier
            int playerIdentifier = inPacket.ReadInt();

            using (DatabaseQuery query = Database.Query("SELECT * FROM player WHERE identifier=@identifier", new MySqlParameter("@identifier", playerIdentifier)))
            {
                if (!query.NextRow())
                {
                    client.Close();

                    return;
                }

                client.Player = new Player(client, query);
            }

            client.Send(MapPackets.ChangeMap(client.Player, true));
        }
    }
}
