using SharpEnd.Data;
using SharpEnd.Migrations;
using SharpEnd.Utility;
using System;

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

        public EquipDataProvider Equips { get; private set; }
        public ItemDataProvider Items { get; private set; }
        public MapDataProvider Maps { get; private set; }
        public MobDataProvider Mobs { get; private set; }
        public NpcDataProvider Npcs { get; private set; }
        public QuestDataProvider Quests { get; private set; }
        public ReactorDataProvider Reactors { get; private set; }
        public SkillDataProvider Skills { get; private set; }
        public TamingMobDataProvider TamingMobs { get; private set; }
        public ValidCharDataProvider ValidCharData { get; private set; }
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

            Equips = new EquipDataProvider();
            Items = new ItemDataProvider();
            Maps = new MapDataProvider();
            Mobs = new MobDataProvider();
            Npcs = new NpcDataProvider();
            Quests = new QuestDataProvider();
            Reactors = new ReactorDataProvider();
            Skills = new SkillDataProvider();
            TamingMobs = new TamingMobDataProvider();
            ValidCharData = new ValidCharDataProvider();
            Commands = new Commands.Commands();
        }

        public void Run()
        {
            // Data is prioritized
            var now = DateTime.Now;

            Equips.Load();
            Items.Load();
            Maps.Load();
            Mobs.Load();
            Npcs.Load();
            Quests.Load();
            Reactors.Load();
            Skills.Load();
            TamingMobs.Load();
            ValidCharData.Load();
            Commands.Load();

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
    }
}
