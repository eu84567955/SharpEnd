using MySql.Data.MySqlClient;
using SharpEnd.Extensions;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Security;
using SharpEnd.Network.Servers;
using SharpEnd.Utility;
using System.Collections.Generic;
using SharpEnd.Game;
using System;

namespace SharpEnd.Handlers
{
    public static class LoginHandlers
    {
        [PacketHandler(EHeader.CMSG_VERSION_INFORMATION)]
        public static void VersionInformationHandler(GameClient client, InPacket inPacket)
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

        [PacketHandler(EHeader.CMSG_UNKNOWN)]
        public static void UnknownHandler(GameClient client, InPacket inPacket)
        {
            // NOTE: Intentionally left blank.
        }

        [PacketHandler(EHeader.CMSG_STRANGE_DATA)]
        public static void StrangeDataHandler(GameClient client, InPacket inPacket)
        {
            // NOTE: Intentionally left blank.
        }

        [PacketHandler(EHeader.CMSG_HOTFIX)]
        public static void ApplyHotfixHandler(GameClient client, InPacket inPacket)
        {
            client.Send(LoginPackets.ApplyHotfix());
        }

        [PacketHandler(EHeader.CMSG_NMCO)]
        public static void NMCOHandler(GameClient client, InPacket inPacket)
        {
            client.Send(LoginPackets.NMCOResult(false));
        }

        [PacketHandler(EHeader.CMSG_AUTHENTICATION)]
        public static void AuthenticationHandler(GameClient client, InPacket inPacket)
        {
            inPacket.ReadByte();
            string password = inPacket.ReadString();
            string username = inPacket.ReadString();

            Account account = null;

            using (DatabaseQuery query = Database.Query("SELECT * FROM accounts WHERE username=@username", new MySqlParameter("username", username)))
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

            // TODO: Check if any of the ChannelServer instances contain the account in their player storage.
            // Check if any of them contain the account in the migration process as well.

            client.Account = account;

            client.Send(LoginPackets.LoginSuccess(client));
        }

        [PacketHandler(EHeader.CMSG_WORLD_LIST), PacketHandler(EHeader.CMSG_WORLD_LIST_RELIST)]
        public static void WorldListHandler(GameClient client, InPacket inPacket)
        {
            foreach (WorldServer world in MasterServer.Instance.Worlds)
            {
                client.Send(LoginPackets.WorldInformation(world));
            }

            client.Send(LoginPackets.WorldEnd());
            client.Send(LoginPackets.HighlightWorld(0)); // TODO: Get the world with the last logged in player.
            client.Send(LoginPackets.RecommendedWorld(true, 0, "Try Scania!@The newest world@on the server.")); // TODO: Configuration for this.
        }

        [PacketHandler(EHeader.CMSG_WORLD_STATUS)]
        public static void WorldStatusHandler(GameClient client, InPacket inPacket)
        {
            byte worldId = inPacket.ReadByte();

            WorldServer world;

            try
            {
                world = MasterServer.Instance.Worlds[worldId];
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }

            client.Send(LoginPackets.WorldStatus(world.Status));
        }

        [PacketHandler(EHeader.CMSG_PLAYER_LIST)]
        public static void PlayerListHandler(GameClient client, InPacket inPacket)
        {
            inPacket.ReadByte(); // NOTE: Connection method (GameLaunching, WebStart, etcetera).
            byte worldId = inPacket.ReadByte();

            WorldServer world;

            try
            {
                world = MasterServer.Instance.Worlds[worldId];
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }

            byte channelId = inPacket.ReadByte();

            ChannelServer channel;

            try
            {
                channel = MasterServer.Instance.Worlds[worldId].Channels[channelId];
            }
            catch (KeyNotFoundException)
            {
                return;
            }

            inPacket.ReadInt(); // NOTE: LAN address.

            client.World = worldId;
            client.Channel = channelId;

            byte count = (byte)(long)Database.Scalar("SELECT COUNT(*) FROM players WHERE account_id=@account_id", new MySqlParameter("account_id", client.Account.Id));

            int characterSlots = 3;

            using (DatabaseQuery query = Database.Query("SELECT * FROM players WHERE account_id=@account_id", new MySqlParameter("account_id", client.Account.Id)))
            {
                client.Send(LoginPackets.PlayerList(count, query, characterSlots));
            }
        }

        [PacketHandler(EHeader.CMSG_PLAYER_NAME_CHECK)]
        public static void PlayerNameCheckHandler(GameClient client, InPacket inPacket)
        {
            string name = inPacket.ReadString();

            bool unusable = name.IsAlphaNumeric() ||
                            name.Length < 4 ||
                            name.Length > 16 ||
                            (long)Database.Scalar("SELECT COUNT(*) FROM players WHERE name=@name", new MySqlParameter("@name", name)) != 0;

            client.Send(LoginPackets.PlayerNameCheck(name, unusable));
        }

        [PacketHandler(EHeader.CMSG_PLAYER_CREATE)]
        public static void PlayerCreateHandler(GameClient client, InPacket inPacket)
        {
            string name = inPacket.ReadString();

            bool unusable = name.IsAlphaNumeric() ||
                            name.Length < 4 ||
                            name.Length > 16 ||
                            (long)Database.Scalar("SELECT COUNT(*) FROM players WHERE name=@name", new MySqlParameter("@name", name)) != 0;

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

            // TODO: Validate object count & objects.

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

            List<int> itemIDs = new List<int>();

            for (int j = i; j < objects.Count; j++)
            {
                itemIDs.Add(objects[j]);
            }

            if (true) // TODO: Validate player creation.
            {
                int playerID = Database.InsertAndReturnID("INSERT INTO players(account_id, world_id, name, gender, skin, face, hair, job) " +
                                                          "VALUES(@account_id, @world_id, @name, @gender, @skin, @face, @hair, @job)",
                                                          new MySqlParameter("account_id", client.Account.Id),
                                                          new MySqlParameter("world_id", client.World),
                                                          new MySqlParameter("name", name),
                                                          new MySqlParameter("gender", gender),
                                                          new MySqlParameter("skin", skin),
                                                          new MySqlParameter("face", face),
                                                          new MySqlParameter("hair", hair),
                                                          new MySqlParameter("job", (ushort)job));
                /*
                foreach (int itemID in itemIDs)
                {
                    PlayerItem item = new PlayerItem(itemID, equipped: true);

                    Database.Execute("INSERT INTO player_item " +
                                     "VALUES(@player_identifier, @item_identifier, @inventory_slot, @quantity, @slots, @scrolls, @strength, @dexterity, @intelligence, @luck, @health, @mana, @weapon_attack, @magic_attack, @weapon_defense, @magic_defense, @accuracy, @avoidability, @hands, @speed, @jump, @creator, @flags)",
                                     new MySqlParameter("player_identifier", playerID),
                                     new MySqlParameter("item_identifier", item.ID),
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
                                     new MySqlParameter("player_identifier", playerID),
                                     new MySqlParameter("key_identifier", key.Item1),
                                     new MySqlParameter("type", (byte)key.Item2),
                                     new MySqlParameter("action", key.Item3));
                }
                */

                using (DatabaseQuery query = Database.Query("SELECT * FROM players WHERE player_id=@player_id", new MySqlParameter("player_id", playerID)))
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

        // TODO: Combine all player selection handlers to one.
        [PacketHandler(EHeader.CMSG_PLAYER_SELECT_PIC)]
        public static void PlayerSelectPicHandler(GameClient client, InPacket inPacket)
        {
            string pic = inPacket.ReadString();

            if (ShaCryptograph.Encrypt(EShaMode.SHA512, pic) != client.Account.Pic)
            {
                // TODO: Send invalid pic packet.
            }

            int playerID = inPacket.ReadInt();

            ChannelServer destination = MasterServer.Instance.Worlds[client.World].Channels[client.Channel];

            destination.Migrations.Register(playerID, client.Account.Id, client.Host);

            client.Send(LoginPackets.ServerIP(destination.Port, playerID));
        }

        [PacketHandler(EHeader.CMSG_PLAYER_SELECT)]
        public static void PlayerSelectHandler(GameClient client, InPacket inPacket)
        {
            int playerID = inPacket.ReadInt();

            ChannelServer destination = MasterServer.Instance.Worlds[client.World].Channels[client.Channel];

            destination.Migrations.Register(playerID, client.Account.Id, client.Host);

            client.Send(LoginPackets.ServerIP(destination.Port, playerID));
        }
    }
}
