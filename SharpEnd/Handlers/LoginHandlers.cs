using MySql.Data.MySqlClient;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Players;
using SharpEnd.Servers;
using SharpEnd.Utility;
using System;
using System.Collections.Generic;

namespace SharpEnd.Handlers
{
    internal static class LoginHandlers
    {
        [PacketHandler(EHeader.CMSG_VERSION_INFORMATION)]
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

        [PacketHandler(EHeader.CMSG_AUTH_SERVER)]
        public static void AuthServer(Client client, InPacket inPacket)
        {
            client.Send(LoginPackets.AuthServer(false));
        }

        [PacketHandler(EHeader.CMSG_CLIENT_START)]
        public static void Start(Client client, InPacket inPacket)
        {
            client.Send(LoginPackets.Start());
        }

        [PacketHandler(EHeader.CMSG_STRANGE_DATA)]
        public static void StrangeData(Client client, InPacket inPacket)
        {
            // NOTE: Intentionally left blank
        }

        [PacketHandler(EHeader.CMSG_AUTHENTICATION)]
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

            if (MasterServer.Instance.Migrations.Contains(account.Identifier))
            {
                client.Send(LoginPackets.LoginError(7));

                return;
            }

            client.Account = account;

            client.Send(LoginPackets.LoginSuccess(client));
        }

        [PacketHandler(EHeader.CMSG_WORLD_LIST)]
        public static void WorldList(Client client, InPacket inPacket)
        {
            foreach (WorldServer world in MasterServer.Instance.Worlds)
            {
                client.Send(LoginPackets.WorldInformation(world));
            }

            client.Send(LoginPackets.WorldEnd());
        }

        [PacketHandler(EHeader.CMSG_WORLD_STATUS)]
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

        [PacketHandler(EHeader.CMSG_PLAYER_LIST)]
        public static void PlayerList(Client client, InPacket inPacket)
        {
            inPacket.ReadByte(); // NOTE: Connection method
            byte worldIdentifier = inPacket.ReadByte();
            byte channelIdentifier = inPacket.ReadByte();
            inPacket.ReadInt(); // NOTE: LAN address

            // TODO: Validate world/channel identifier

            client.WorldIdentifier = worldIdentifier;
            client.ChannelIdentifier = channelIdentifier;

            byte count = (byte)(long)Database.Scalar("SELECT COUNT(*) FROM player WHERE account_identifier=@account_identifier", new MySqlParameter("account_identifier", client.Account.Identifier));

            using (DatabaseQuery query = Database.Query("SELECT * FROM player WHERE account_identifier=@account_identifier", new MySqlParameter("account_identifier", client.Account.Identifier)))
            {
                client.Send(LoginPackets.PlayerList(count, query));
            }
        }

        [PacketHandler(EHeader.CMSG_PLAYER_NAME_CHECK)]
        public static void PlayerNameCheck(Client client, InPacket inPacket)
        {
            string name = inPacket.ReadString();

            bool unusable = name.Length < 4 ||
                            name.Length > 16 ||
                            (long)Database.Scalar("SELECT COUNT(*) FROM player WHERE name=@name", new MySqlParameter("@name", name)) != 0;

            client.Send(LoginPackets.PlayerNameCheck(name, unusable));
        }

        private static ushort GetJobFromJobType(int jobType)
        {
            switch (jobType)
            {
                case 0: return 3000; // NOTE: Resistance
                case 1: return 0; // NOTE: Adventurer
                case 2: return 1000;// NOTE: Cygnus
                case 3: return 200; // NOTE: Aran
                case 4: return 2001; // NOTE: Evan
                case 5: return 2002; // NOTE: Mercedes
                case 6: return 3001; // NOTE: Demon
                case 7: return 2003; // NOTE: Phantom
                case 8: return 0; // NOTE: Dual Blade
                case 9: return 5000; // NOTE: Mihile
                case 10: return 2004; // NOTE: Luminous
                case 11: return 6000; // NOTE: Kaiser
                case 12: return 6001; // NOTE: Angelic Buster
                case 13: return 0; // NOTE: Cannoneer
                case 14: return 3002; // NOTE: Xenon
                case 15: return 10112; // NOTE: Zero
                case 16: return 0; // NOTE: Jett
                case 17: return 4001; // NOTE: Hayato
                case 18: return 4002; // NOTE: Kanna 
            }

            throw new ArgumentException("Invalid job type.");
        }

        [PacketHandler(EHeader.CMSG_PLAYER_CREATE)]
        public static void PlayerCreate(Client client, InPacket inPacket)
        {
            string name = inPacket.ReadString();
            inPacket.ReadInt(); // NOTE: Keyboard
            inPacket.ReadInt(); // NOTE: Unknown, always -1
            int jobType = inPacket.ReadInt();
            ushort job = GetJobFromJobType(jobType);
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
                    if (job != 5000)
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

            List<int> items = new List<int>();

            for (int j = i; j < objects.Count; j++)
            {
                items.Add(objects[j]);
            }

            int identifier = Database.InsertAndReturnIdentifier("INSERT INTO player(account_identifier,name,gender,skin,face,hair,job,sub_job,skill_points) " +
                                                                "VALUES(@account_identifier,@name,@gender,@skin,@face,@hair,@job,@sub_job,@skill_points)",
                                                                new MySqlParameter("@account_identifier", client.Account.Identifier),
                                                                new MySqlParameter("@name", name),
                                                                new MySqlParameter("@gender", gender),
                                                                new MySqlParameter("@skin", skin),
                                                                new MySqlParameter("@face", face),
                                                                new MySqlParameter("@hair", hair),
                                                                new MySqlParameter("@job", job),
                                                                new MySqlParameter("@sub_job", subJob),
                                                                new MySqlParameter("@skill_points", new byte[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, }));

            foreach (int item in items)
            {
                Database.Execute("INSERT INTO player_item(player_identifier,item_identifier,inventory_slot,quantity) " +
                                 "VALUES(@player_identifier,@item_identifier,@inventory_slot)",
                                 new MySqlParameter("player_identifier", identifier),
                                 new MySqlParameter("item_identifier", item),
                                 new MySqlParameter("inventory_slot", GetSlot(item)),
                                 new MySqlParameter("quantity", (ushort)1));
            }

            using (DatabaseQuery query = Database.Query("SELECT * FROM player WHERE identifier=@identifier", new MySqlParameter("identifier", identifier)))
            {
                query.NextRow();

                client.Send(LoginPackets.PlayerCreate(query));
            }
        }

        [PacketHandler(EHeader.CMSG_PLAYER_SELECT)]
        public static void PlayerSelect(Client client, InPacket inPacket)
        {
            string pic = inPacket.ReadString();

            if (false) // TODO: Validate PIC
            {
                // TODO: Invalid PIC packet

                return;
            }

            int playerIdentifier = inPacket.ReadInt();

            MasterServer.Instance.Migrations.Register(playerIdentifier, client.Account.Identifier, client.Host);

            ushort port = 8585;

            client.Send(LoginPackets.ServerIP(port, playerIdentifier));
        }

        private static short GetSlot(int item)
        {
            short slot = 0;

            if (item >= 1000000 && item < 1010000)
            {
                slot -= 1;
            }
            else if (item >= 1010000 && item < 1020000)
            {
                slot -= 2;
            }
            else if (item >= 1020000 && item < 1030000)
            {
                slot -= 3;
            }
            else if (item >= 1030000 && item < 1040000)
            {
                slot -= 4;
            }
            else if (item >= 1040000 && item < 1060000)
            {
                slot -= 5;
            }
            else if (item >= 1060000 && item < 1070000)
            {
                slot -= 6;
            }
            else if (item >= 1070000 && item < 1080000)
            {
                slot -= 7;
            }
            else if (item >= 1080000 && item < 1090000)
            {
                slot -= 8;
            }
            else if (item >= 1102000 && item < 1103000)
            {
                slot -= 9;
            }
            else if (item >= 1092000 && item < 1100000)
            {
                slot -= 10;
            }
            else if (item >= 1300000 && item < 1800000)
            {
                slot -= 11;
            }
            else if (item >= 1112000 && item < 1120000)
            {
                slot -= 12;
            }
            else if (item >= 1122000 && item < 1123000)
            {
                slot -= 17;
            }
            else if (item >= 1900000 && item < 2000000)
            {
                slot -= 18;
            }
            else
            {
                slot += 1;
            }

            return slot;
        }
    }
}
