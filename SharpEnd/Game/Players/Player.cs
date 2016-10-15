using MySql.Data.MySqlClient;
using SharpEnd.Drawing;
using SharpEnd.Game.Maps;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Script;
using SharpEnd.Servers;
using SharpEnd.Utility;

namespace SharpEnd.Players
{
    internal sealed class Player : MapEntity
    {
        public Client Client { get; private set; }

        public bool IsInitialized { get; private set; }

        public int Identifier { get; private set; }
        public string Name { get; private set; }
        public byte Gender { get; private set; }
        public byte Skin { get; private set; }
        public int Face { get; private set; }
        public int Hair { get; private set; }
        public int MapIdentifier { get; set; }
        public sbyte MapSpawnPoint { get; set; }
        public byte PortalCount { get; set; }

        public PlayerStats Stats { get; private set; }
        public PlayerItems Items { get; private set; }
        public PlayerSkills Skills { get; private set; }
        public PlayerQuests Quests { get; private set; }
        public PlayerKeymap Keymap { get; private set; }
        public PlayerSPTable SPTable { get; private set; }
        public PlayerVariables Variables { get; private set; }
        public ControlledMobs ControlledMobs { get; private set; }
        public ControlledNpcs ControlledNpcs { get; private set; }

        public NpcScript NpcScript { get; set; }

        public bool IsGm => Client.Account.Level >= EAccountLevel.Gm;

        public override int ObjectIdentifier
        {
            get
            {
                return Identifier;
            }
            set
            {
                return;
            }
        }

        public Player(Client client, DatabaseQuery query)
        {
            Client = client;

            Identifier = query.Get<int>("identifier");
            Name = query.Get<string>("name");
            Gender = query.Get<byte>("gender");
            Skin = query.Get<byte>("skin");
            Face = query.Get<int>("face");
            Hair = query.Get<int>("hair");
            MapIdentifier = query.Get<int>("map_identifier");
            MapSpawnPoint = query.Get<sbyte>("map_spawn");

            Stats = new PlayerStats(this, query);

            if (IsGm)
            {
                MapIdentifier = 180000000;
                MapSpawnPoint = -1;
            }
            else if (MasterServer.Instance.GetMap(MapIdentifier).ForcedReturnMapIdentifier != 999999999)
            {
                MapIdentifier = MasterServer.Instance.GetMap(MapIdentifier).ForcedReturnMapIdentifier;
                MapSpawnPoint = -1;
            }
            else if (!Stats.IsAlive)
            {
                MapIdentifier = MasterServer.Instance.GetMap(MapIdentifier).ReturnMapIdentifier;
                MapSpawnPoint = -1;
            }

            Map = MasterServer.Instance.GetMap(MapIdentifier);
            Position = Map.Portals[MapSpawnPoint].Position;
            Stance = 0;
            Foothold = 0;

            using (DatabaseQuery itemQuery = Database.Query("SELECT * FROM player_item WHERE player_identifier=@player_identifier", new MySqlParameter("player_identifier", Identifier)))
            {
                long meso = query.Get<long>("meso");

                byte equipmentSlots = query.Get<byte>("equipment_slots");
                byte usableSlots = query.Get<byte>("usable_slots");
                byte setupSlots = query.Get<byte>("setup_slots");
                byte etceteraSlots = query.Get<byte>("etcetera_slots");
                byte cashSlots = query.Get<byte>("cash_slots");

                Items = new PlayerItems(this, itemQuery, meso, equipmentSlots, usableSlots, setupSlots, etceteraSlots, cashSlots);
            }

            using (DatabaseQuery skillQuery = Database.Query("SELECT * FROM player_skill WHERE player_identifier=@player_identifier", new MySqlParameter("player_identifier", Identifier)))
            {
                Skills = new PlayerSkills(this, skillQuery);
            }

            using (DatabaseQuery questQuery = Database.Query("SELECT * FROM player_quest WHERE player_identifier=@player_identifier", new MySqlParameter("player_identifier", Identifier)))
            {
                Quests = new PlayerQuests(this, questQuery);
            }

            using (DatabaseQuery keymapQuery = Database.Query("SELECT * FROM player_keymap WHERE player_identifier=@player_identifier", new MySqlParameter("player_identifier", Identifier)))
            {
                Keymap = new PlayerKeymap(this, keymapQuery);
            }

            using (DatabaseQuery spTableQuery = Database.Query("SELECT * FROM player_sp_table WHERE player_identifier=@player_identifier", new MySqlParameter("player_identifier", Identifier)))
            {
                SPTable = new PlayerSPTable(this, spTableQuery);
            }

            using (DatabaseQuery variableQuery = Database.Query("SELECT * FROM player_variable WHERE player_identifier=@player_identifier", new MySqlParameter("player_identifier", Identifier)))
            {
                Variables = new PlayerVariables(this, variableQuery);
            }

            ControlledMobs = new ControlledMobs(this);
            ControlledNpcs = new ControlledNpcs(this);
        }

        public void Save()
        {
            Map.Players.Remove(this);

            // TODO: Set closest spawn point.

            Database.Execute("UPDATE player SET gender=@gender, skin=@skin, face=@face, hair=@hair, level=@level, job=@job, sub_job=@sub_job, strength=@strength, dexterity=@dexterity, intelligence=@intelligence, luck=@luck, health=@health, max_health=@max_health, mana=@mana, max_mana=@max_mana, ability_points=@ability_points, skill_points=@skill_points, experience=@experience, fame=@fame, map_identifier=@map_identifier, map_spawn=@map_spawn, meso=@meso, equipment_slots=@equipment_slots, usable_slots=@usable_slots, etcetera_slots=@etcetera_slots, cash_slots=@cash_slots WHERE identifier=@identifier",
                               new MySqlParameter("identifier", Identifier),
                               new MySqlParameter("gender", Gender),
                               new MySqlParameter("skin", Skin),
                               new MySqlParameter("face", Face),
                               new MySqlParameter("hair", Hair),
                               new MySqlParameter("level", Stats.Level),
                               new MySqlParameter("job", Stats.Job),
                               new MySqlParameter("sub_job", Stats.SubJob),
                               new MySqlParameter("strength", Stats.Strength),
                               new MySqlParameter("dexterity", Stats.Dexterity),
                               new MySqlParameter("intelligence", Stats.Intelligence),
                               new MySqlParameter("luck", Stats.Luck),
                               new MySqlParameter("health", Stats.Health),
                               new MySqlParameter("max_health", Stats.MaxHealth),
                               new MySqlParameter("mana", Stats.Mana),
                               new MySqlParameter("max_mana", Stats.MaxMana),
                               new MySqlParameter("ability_points", Stats.AbilityPoints),
                               new MySqlParameter("skill_points", Stats.SkillPoints),
                               new MySqlParameter("experience", Stats.Experience),
                               new MySqlParameter("fame", Stats.Fame),
                               new MySqlParameter("map_identifier", MapIdentifier),
                               new MySqlParameter("map_spawn", MapSpawnPoint),
                               new MySqlParameter("meso", Items.Meso),
                               new MySqlParameter("equipment_slots", Items.EquipmentSlots),
                               new MySqlParameter("usable_slots", Items.UsableSlots),
                               new MySqlParameter("setup_slots", Items.SetupSlots),
                               new MySqlParameter("etcetera_slots", Items.EtceteraSlots),
                               new MySqlParameter("cash_slots", Items.CashSlots));

            Items.Save();
            Skills.Save();
            Quests.Save();
            Keymap.Save();
            SPTable.Save();
            Variables.Save();
        }

        public void Initialize()
        {
            Send(PlayerPackets.EventNameTag(new sbyte[5] { -1, -1, -1, -1, -1 }));
            Send(ServerPackets.EventList());
            Send(MapPackets.ChangeMap(this, true));
            Send(PlayerPackets.Keymap(Keymap));

            Map.Players.Add(this);

            IsInitialized = true;
        }

        public void Send(byte[] buffer)
        {
            Client.Send(buffer);
        }

        public void Release()
        {
            Client.Send(PlayerPackets.PlayerUpdate(this, EPlayerUpdate.None, true));
        }

        public void Notify(string text, ENoticeType type = ENoticeType.Pink)
        {
            Client.Send(MessagePackets.Notification(text, type));
        }

        public void SetMap(int mapIdentifier, sbyte portalIdentifier, Point position)
        {
            InternalSetMap(mapIdentifier, portalIdentifier, true, position);
        }

        public void SetMap(int mapIdentifier, Portal portal = null)
        {
            if (!MasterServer.Instance.Maps.ContainsKey(mapIdentifier))
            {
                return;
            }

            Map newMap = MasterServer.Instance.GetMap(mapIdentifier);

            if (portal == null)
            {
                portal = newMap.Portals[-1];
            }

            InternalSetMap(mapIdentifier, portal.Identifier, false, new Point(portal.Position.X, (short)(portal.Position.Y - 40)));
        }

        private void InternalSetMap(int mapIdentifier, sbyte portalIdentifier, bool spawnByPosition, Point position)
        {
            Map newMap = MasterServer.Instance.GetMap(mapIdentifier);

            Map.Players.Remove(this);

            MapIdentifier = mapIdentifier;
            MapSpawnPoint = portalIdentifier;

            Position = position;
            Stance = 0;
            Foothold = 0;

            Send(MapPackets.ChangeMap(this, spawnByPosition: spawnByPosition, position: Position));

            newMap.Players.Add(this);
        }

        public void AcceptDeath(bool wheel)
        {
            int returnMapIdentifier = Map.ReturnMapIdentifier;

            if (wheel)
            {
                returnMapIdentifier = Map.Identifier;
            }

            Stats.SetHealth(50, false);

            SetMap(returnMapIdentifier);
        }
    }
}
