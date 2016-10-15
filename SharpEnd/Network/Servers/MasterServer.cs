using SharpEnd.Game.Data;
using SharpEnd.Game.Commands;
using SharpEnd.Game.Data;
using SharpEnd.Game.Maps;
using SharpEnd.Migrations;
using SharpEnd.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        
        public Commands Commands { get; private set; }

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

            Commands = new Commands();
        }

        public void Run()
        {
            LoadData();

            Login.Run();

            foreach (WorldServer world in Worlds)
            {
                world.Run();
            }

            Running = true;

            Log.Success("SharpEnd is online.");
        }

        private void LoadData()
        {
            Handlers.Load();
            Commands.Load();
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

        private Dictionary<int, Map> maps = new Dictionary<int, Map>();
        public Map GetMap(int identifier)
        {
            if (!maps.ContainsKey(identifier))
            {
                var data = MapDataProvider.Instance[identifier];

                maps.Add(identifier, new Map(data));
            }

            return maps[identifier];
        }
    }
}
