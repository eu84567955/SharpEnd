using SharpEnd.Data;
using SharpEnd.Maps;
using SharpEnd.Migrations;
using SharpEnd.Utility;
using System;
using System.IO;

namespace SharpEnd.Servers
{
    internal sealed class MasterServer
    {
        public static MasterServer Instance { get; } = new MasterServer();

        public bool Running { get; private set; }

        public LoginServer Login { get; private set; }
        public WorldServer[] Worlds { get; private set; }

        public HandlerStore Handlers { get; private set; }

        public MigrationRequests Migrations { get; private set; }

        public ItemDataProvider Items { get; private set; }
        public MobDataProvider Mobs { get; private set; }
        public NpcDataProvider Npcs { get; private set; }
        public QuestDataProvider Quests { get; private set; }
        public ReactorDataProvider Reactors { get; private set; }
        public SkillDataProvider Skills { get; private set; }
        public TamingMobDataProvider TamingMobs { get; private set; }
        public ValidCharDataProvider ValidCharData { get; private set; }
        public StringDataProvider Strings { get; private set; }
        public Commands.Commands Commands { get; private set; }

        private MasterServer()
        {
            Login = new LoginServer(8484);

            Worlds = new WorldServer[1];

            for (byte i = 0; i < 1; i++)
            {
                Worlds[i] = new WorldServer(i, 8585, 1);
            }

            Handlers = new HandlerStore();

            Migrations = new MigrationRequests();

            Items = new ItemDataProvider();
            Mobs = new MobDataProvider();
            Npcs = new NpcDataProvider();
            Quests = new QuestDataProvider();
            Reactors = new ReactorDataProvider();
            Skills = new SkillDataProvider();
            TamingMobs = new TamingMobDataProvider();
            ValidCharData = new ValidCharDataProvider();
            Strings = new StringDataProvider();
            Commands = new Commands.Commands();
        }

        public void Run()
        {
            // Data is prioritized
            var now = DateTime.Now;

            Items.Load();
            Mobs.Load();
            Npcs.Load();
            Quests.Load();
            Reactors.Load();
            Skills.Load();
            TamingMobs.Load();
            //ValidCharData.Load();
            Strings.Load();
            Commands.Load();

            // TODO: Move else-where.
            {
                string[] lines = File.ReadAllLines("monster_drops.txt");

                int mobIdentifier = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Length == 0)
                    {
                        continue;
                    }

                    if (lines[i][0] == '#')
                    {
                        mobIdentifier = int.Parse(lines[i].Substring(1));
                    }
                    else
                    {
                        string[] split = lines[i].Split(' ');

                        Loot loot = new Loot();

                        loot.ItemIdentifier = int.Parse(split[0]);
                        loot.Chance = int.Parse(split[1]);
                        loot.MinimumQuantity = int.Parse(split[2]);
                        loot.MaximumQuantity = int.Parse(split[3]);
                        loot.QuestIdentifier = int.Parse(split[4]);

                        /*if (Mobs.Contains(mobIdentifier))
                        {
                            Mobs[mobIdentifier].Loots.Add(loot);
                        }*/
                    }
                }
            }

            Log.Inform("Maple data loaded in {0:N3} seconds.", (DateTime.Now - now).TotalSeconds);

            Handlers.Load();

            Login.Run();

            foreach (WorldServer world in Worlds)
            {
                world.Run();
            }

            Running = true;

            Log.Success("SharpEnd is online.");
        }

        public void Shutdown()
        {
            Login.Shutdown();

            foreach (WorldServer world in Worlds)
            {
                world.Shutdown();
            }

            Running = false;

            Log.Inform("SharpEnd is offline.");
        }

        public Maps GetMaps(byte channelIdentifier)
        {
            return MasterServer.Instance.Worlds[0].Channels[channelIdentifier].Maps;
        }
    }
}
