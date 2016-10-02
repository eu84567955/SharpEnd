using MySql.Data.MySqlClient;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Players;
using SharpEnd.Servers;
using SharpEnd.Utility;

namespace SharpEnd.Handlers
{
    internal static class LoginHandlers
    {
        [PacketHandler(EOpcode.CMSG_VERSION_INFORMATION)]
        public static void VersionInformation(Client client, InPacket inPacket)
        {
            byte localisation = inPacket.ReadByte();
            ushort version = inPacket.ReadUShort();
            ushort patch = inPacket.ReadUShort();

            if (localisation != 8 || version != 176 || patch != 3)
            {
                client.Close();
            }
        }

        [PacketHandler(EOpcode.CMSG_AUTHENTICATION)]
        public static void Authentication(Client client, InPacket inPacket)
        {
            inPacket.ReadByte();
            string password = inPacket.ReadString();
            string username = inPacket.ReadString();

            Account account = null;

            using (DatabaseQuery query = Database.Query("SELECT * FROM account WHERE username=@username", new MySqlParameter("username", username)))
            {
                if (!query.NextRow())
                {
                    client.Send(LoginPackets.LoginError(5));
                    return;
                }

                account = new Account(query);
            }

            if (password != account.Password)
            {
                client.Send(LoginPackets.LoginError(4));
                return;
            }

            client.Account = account;

            client.Send(LoginPackets.LoginSuccess(client));
        }

        [PacketHandler(EOpcode.CMSG_PRIVATE_SERVER_AUTH)]
        public static void PrivateServerAuth(Client client, InPacket inPacket)
        {
            int request = inPacket.ReadInt();
            int response = request ^ (int)EOpcode.SMSG_PRIVATE_SERVER_AUTH;

            client.Send(LoginPackets.PrivateServerAuth(response));
        }

        [PacketHandler(EOpcode.CMSG_CLIENT_START)]
        public static void Start(Client client, InPacket inPacket)
        {
            client.Send(LoginPackets.Start());
        }

        [PacketHandler(EOpcode.CMSG_WORLD_LIST)]
        public static void WorldList(Client client, InPacket inPacket)
        {
            foreach (WorldServer world in MasterServer.Instance.Worlds)
            {
                client.Send(LoginPackets.WorldInformation(world));
            }

            client.Send(LoginPackets.WorldEnd());
        }

        [PacketHandler(EOpcode.CMSG_WORLD_STATUS)]
        public static void WorldStatus(Client client, InPacket inPacket)
        {
            byte identifier = inPacket.ReadByte();

            WorldServer world = MasterServer.Instance.Worlds[identifier];

            if (world == null)
            {
                return;
            }

            client.Send(LoginPackets.WorldStatus(world.Status));
        }

        [PacketHandler(EOpcode.CMSG_PLAYER_LIST)]
        public static void PlayerList(Client client, InPacket inPacket)
        {
            inPacket.ReadByte(); // NOTE: Connection method
            byte worldIdentifier = inPacket.ReadByte();
            byte channelIdentifier = inPacket.ReadByte();
            inPacket.ReadInt(); // NOTE: LAN address

            // TODO: Validate world/channel identifier

            client.WorldIdentifier = worldIdentifier;
            client.ChannelIdentifier = channelIdentifier;

            using (DatabaseQuery query = Database.Query("SELECT * FROM player WHERE account_identifier=@account_identifier", new MySqlParameter("account_identifier", client.Account.Identifier)))
            {
                client.Send(LoginPackets.PlayerList(1, query));
            }
        }

        [PacketHandler(EOpcode.CMSG_AUTH_SERVER)]
        public static void AuthServer(Client client, InPacket inPacket)
        {
            client.Send(LoginPackets.AuthServer(false));
        }
    }
}
