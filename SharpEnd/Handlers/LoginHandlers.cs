using MySql.Data.MySqlClient;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Players;
using SharpEnd.Security;
using SharpEnd.Servers;
using SharpEnd.Utility;
using System;
using System.Collections.Generic;

namespace SharpEnd.Handlers
{
    internal static class LoginHandlers
    {
        [PacketHandler(EHeader.CMSG_VERSION_INFORMATION)]
        public static void VersionInformationHandler(Client client, InPacket inPacket)
        {
            ELocalisation localisation = (ELocalisation)inPacket.ReadByte();
            ushort version = inPacket.ReadUShort();
            string patch = inPacket.ReadUShort().ToString();

            if (localisation != Application.Version.Localisation ||
                version != Application.Version.Version ||
                patch != Application.Version.Patch)
            {
                client.Close();
            }
        }

        [PacketHandler(EHeader.CMSG_AUTH_SERVER)]
        public static void AuthServerHandler(Client client, InPacket inPacket)
        {
            client.Send(LoginPackets.AuthServer(false));
        }

        [PacketHandler(EHeader.CMSG_CLIENT_START)]
        public static void StartHandler(Client client, InPacket inPacket)
        {
            client.Send(LoginPackets.Start());
        }

        [PacketHandler(EHeader.CMSG_STRANGE_DATA)]
        public static void StrangeDataHandler(Client client, InPacket inPacket)
        {
            // NOTE: Intentionally left blank.
        }

        [PacketHandler(EHeader.CMSG_AUTHENTICATION)]
        public static void AuthenticationHandler(Client client, InPacket inPacket)
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

            if (ShaCryptograph.Encrypt(EShaMode.SHA512, password) != account.Password)
            {
                client.Send(LoginPackets.LoginError(4));

                return;
            }

            if (MasterServer.Instance.Migrations.Contains(account.Identifier))
            {
                client.Send(LoginPackets.LoginError(7));

                return;
            }

            client.Account = account;

            client.Send(LoginPackets.LoginSuccess(client));
        }

        [PacketHandler(EHeader.CMSG_WORLD_LIST)]
        public static void WorldListHandler(Client client, InPacket inPacket)
        {
            foreach (WorldServer world in MasterServer.Instance.Worlds)
            {
                client.Send(LoginPackets.WorldInformation(world));
            }

            client.Send(LoginPackets.WorldEnd());
        }

        [PacketHandler(EHeader.CMSG_WORLD_STATUS)]
        public static void WorldStatusHandler(Client client, InPacket inPacket)
        {
            byte identifier = inPacket.ReadByte();

            WorldServer world = MasterServer.Instance.Worlds[identifier];

            if (world == null)
            {
                return;
            }

            client.Send(LoginPackets.WorldStatus(world.Status));
        }

        [PacketHandler(EHeader.CMSG_PLAYER_LIST)]
        public static void PlayerListHandler(Client client, InPacket inPacket)
        {
            inPacket.ReadByte(); // NOTE: Connection method (GameLaunching, WebStart, etcetera).
            byte worldIdentifier = inPacket.ReadByte();
            byte channelIdentifier = inPacket.ReadByte();
            inPacket.ReadInt(); // NOTE: LAN address.

            // TODO: Validate world/channel identifier.

            client.WorldIdentifier = worldIdentifier;
            client.ChannelIdentifier = channelIdentifier;

            byte count = (byte)(long)Database.Scalar("SELECT COUNT(*) FROM player WHERE account_identifier=@account_identifier", new MySqlParameter("account_identifier", client.Account.Identifier));

            using (DatabaseQuery query = Database.Query("SELECT * FROM player WHERE account_identifier=@account_identifier", new MySqlParameter("account_identifier", client.Account.Identifier)))
            {
                client.Send(LoginPackets.PlayerList(count, query, client.Account.PICState));
            }
        }

        [PacketHandler(EHeader.CMSG_PLAYER_NAME_CHECK)]
        public static void PlayerNameCheckHandler(Client client, InPacket inPacket)
        {
            string name = inPacket.ReadString();

            bool unusable = name.Length < 4 ||
                            name.Length > 16 ||
                            (long)Database.Scalar("SELECT COUNT(*) FROM player WHERE name=@name", new MySqlParameter("@name", name)) != 0;

            client.Send(LoginPackets.PlayerNameCheck(name, unusable));
        }

        [PacketHandler(EHeader.CMSG_PLAYER_CREATE)]
        public static void PlayerCreateHandler(Client client, InPacket inPacket)
        {
            string name = inPacket.ReadString();

            bool unusable = name.Length < 4 ||
                            name.Length > 16 ||
                            (long)Database.Scalar("SELECT COUNT(*) FROM player WHERE name=@name", new MySqlParameter("@name", name)) != 0;

            if (unusable)
            {
                return;
            }

            EKeyboardLayoutType keyboardLayoutType = (EKeyboardLayoutType)inPacket.ReadInt();
            inPacket.ReadInt(); // NOTE: Unknown, always -1.
            EJobType jobType = (EJobType)inPacket.ReadInt();
            EJob job = jobType.GetJob();
            ushort subJob = inPacket.ReadUShort();
            byte gender = inPacket.ReadByte();
            byte skin = inPacket.ReadByte();

            List<int> objects = new List<int>();

            byte count = inPacket.ReadByte();

            while (count-- > 0)
            {
                objects.Add(inPacket.ReadInt());
            }

            if (!MasterServer.Instance.ValidCharData.Validate(job, objects))
            {
                return;
            }

            int i = 0;

            int face = objects[i++];
            int hair = objects[i++];

            if (objects[i] < byte.MaxValue)
            {
                if (objects[i] % 10 == 0)
                {
                    if (job != EJob.Mihile)
                    {
                        hair = hair + objects[i];
                    }

                    i++;
                }
                else
                {
                    skin = (byte)objects[i++];
                }
            }

            if (objects[i] < byte.MaxValue)
            {
                skin = (byte)objects[i++];
            }

            List<int> itemIdentifiers = new List<int>();

            for (int j = i; j < objects.Count; j++)
            {
                itemIdentifiers.Add(objects[j]);
            }

            if (true) // TODO: Validate player creation.
            {
                int playerIdentifier = Database.InsertAndReturnIdentifier("INSERT INTO player(account_identifier,name,gender,skin,face,hair,job,sub_job) " +
                                         "VALUES(@account_identifier,@name,@gender,@skin,@face,@hair,@job,@sub_job)",
                                         new MySqlParameter("@account_identifier", client.Account.Identifier),
                                         new MySqlParameter("@name", name),
                                         new MySqlParameter("@gender", gender),
                                         new MySqlParameter("@skin", skin),
                                         new MySqlParameter("@face", face),
                                         new MySqlParameter("@hair", hair),
                                         new MySqlParameter("@job", (ushort)job),
                                         new MySqlParameter("@sub_job", subJob));

                foreach (int itemIdentifier in itemIdentifiers)
                {
                    PlayerItem item = new PlayerItem(itemIdentifier, equipped: true);

                    Database.Execute("INSERT INTO player_item " +
                                     "VALUES(@player_identifier, @item_identifier, @inventory_slot, @quantity, @slots, @scrolls, @strength, @dexterity, @intelligence, @luck, @health, @mana, @weapon_attack, @magic_attack, @weapon_defense, @magic_defense, @accuracy, @avoidability, @hands, @speed, @jump, @creator, @flags)",
                                     new MySqlParameter("player_identifier", playerIdentifier),
                                     new MySqlParameter("item_identifier", item.Identifier),
                                     new MySqlParameter("inventory_slot", item.Slot),
                                     new MySqlParameter("quantity", item.Quantity),
                                     new MySqlParameter("slots", item.Slots),
                                     new MySqlParameter("scrolls", item.Scrolls),
                                     new MySqlParameter("strength", item.Strength),
                                     new MySqlParameter("dexterity", item.Dexterity),
                                     new MySqlParameter("intelligence", item.Intelligence),
                                     new MySqlParameter("luck", item.Luck),
                                     new MySqlParameter("health", item.Health),
                                     new MySqlParameter("mana", item.Mana),
                                     new MySqlParameter("weapon_attack", item.WeaponAttack),
                                     new MySqlParameter("magic_attack", item.MagicAttack),
                                     new MySqlParameter("weapon_defense", item.WeaponDefense),
                                     new MySqlParameter("magic_defense", item.MagicDefense),
                                     new MySqlParameter("accuracy", item.Accuracy),
                                     new MySqlParameter("avoidability", item.Avoidability),
                                     new MySqlParameter("hands", item.Hands),
                                     new MySqlParameter("speed", item.Speed),
                                     new MySqlParameter("jump", item.Jump),
                                     new MySqlParameter("creator", item.Creator),
                                     new MySqlParameter("flags", item.Flags));
                }

                var keyLayout = keyboardLayoutType == 0 ? CreationConstants.KeyLayouts.BasicKeyLayout : CreationConstants.KeyLayouts.SecondaryKeyLayout;

                for (int k = 0; k < keyLayout.Length; k++)
                {
                    var key = keyLayout[k];

                    Database.Execute("INSERT INTO player_keymap " +
                                     "VALUES(@player_identifier, @key_identifier, @type, @action)",
                                     new MySqlParameter("player_identifier", playerIdentifier),
                                     new MySqlParameter("key_identifier", key.Item1),
                                     new MySqlParameter("type", (byte)key.Item2),
                                     new MySqlParameter("action", key.Item3));
                }

                using (DatabaseQuery query = Database.Query("SELECT * FROM player WHERE identifier=@identifier", new MySqlParameter("identifier", playerIdentifier)))
                {
                    query.NextRow();

                    client.Send(LoginPackets.PlayerCreate(false, query));
                }
            }
            else
            {
                client.Send(LoginPackets.PlayerCreate(true));
            }
        }

        [PacketHandler(EHeader.CMSG_PLAYER_SELECT_PIC)]
        public static void PlayerSelectPicHandler(Client client, InPacket inPacket)
        {
            string pic = inPacket.ReadString();

            if (ShaCryptograph.Encrypt(EShaMode.SHA512, pic) != client.Account.PIC)
            {
                // TODO: Send invalid pic packet.
            }

            int playerIdentifier = inPacket.ReadInt();

            MasterServer.Instance.Migrations.Register(playerIdentifier, client.Account.Identifier, client.Host);

            ushort port = 8585;

            client.Send(LoginPackets.ServerIP(port, playerIdentifier));
        }

        [PacketHandler(EHeader.CMSG_PLAYER_SELECT)]
        public static void PlayerSelectHandler(Client client, InPacket inPacket)
        {
            int playerIdentifier = inPacket.ReadInt();

            MasterServer.Instance.Migrations.Register(playerIdentifier, client.Account.Identifier, client.Host);

            ushort port = 8585;

            client.Send(LoginPackets.ServerIP(port, playerIdentifier));
        }
    }
}
