using MySql.Data.MySqlClient;
using SharpEnd.Game.Maps;
using SharpEnd.Network;
using SharpEnd.Network.Servers;
using SharpEnd.Utility;
using SharpEnd.Game.Scripting;

namespace SharpEnd.Game.Players
{
    public sealed class Player : MapEntity
    {
        private GameClient m_client;
        private int m_id;
        private string m_name;
        private byte m_gender;
        private byte m_skin;
        private int m_face;
        private int m_hair;
        private byte m_level;
        private short m_job;
        private short m_strength;
        private short m_dexterity;
        private short m_intelligence;
        private short m_luck;
        private int m_hp;
        private int m_maxHp;
        private int m_mp;
        private int m_maxMp;
        private long m_experience;
        private int m_fame;
        private sbyte m_spawnPoint;
        private long m_meso;

        private PlayerItems m_items;
        private PlayerSkills m_skills;
        private PlayerQuests m_quests;

        private byte m_portals;
        private Map m_map;
        private EventManipulator m_event;

        public override int ObjectID { get { return m_id; } set { return; } }
        public GameClient Client { get { return m_client; } }
        public int ID { get { return m_id; } }
        public string Name { get { return m_name; } }
        public byte Gender { get { return m_gender; } }
        public byte Skin { get { return m_skin; } set { m_skin = value; } }
        public int Face { get { return m_face; } set { m_face = value; } }
        public int Hair { get { return m_hair; } set { m_hair = value; } }
        public byte Level { get { return m_level; } set { m_level = value; } }
        public short Job { get { return m_job; } set { m_job = value; } }
        public short Strength { get { return m_strength; } set { m_strength = value; } }
        public short Dexterity { get { return m_dexterity; } set { m_dexterity = value; } }
        public short Intelligence { get { return m_intelligence; } set { m_intelligence = value; } }
        public short Luck { get { return m_luck; } set { m_luck = value; } }
        public int HP { get { return m_hp; } set { m_hp = value; } }
        public int MaxHP { get { return m_maxHp; } set { m_maxHp = value; } }
        public int MP { get { return m_mp; } set { m_mp = value; } }
        public int MaxMP { get { return m_maxMp; } set { m_maxMp = value; } }
        public long Experience { get { return m_experience; } set { m_experience = value; } }
        public int Fame { get { return m_face; } set { m_fame = value; } }
        public sbyte SpawnPoint { get { return m_spawnPoint; } }
        public long Meso { get { return m_meso; } set { m_meso = value; } }

        public PlayerItems Items { get { return m_items; } }
        public PlayerSkills Skills { get { return m_skills; } }
        public PlayerQuests Quests { get { return m_quests; } }

        public byte Portals { get { return m_portals; } set { m_portals = value; } }
        public new Map Map { get { return m_map; } set { m_map = value; } }
        public EventManipulator Event { get { return m_event; } set { m_event = value; } }

        internal Player(GameClient client, DatabaseQuery query)
        {
            m_client = client;
            m_id = query.Get<int>("player_id");
            m_name = query.Get<string>("name");
            m_gender = query.Get<byte>("gender");
            m_skin = query.Get<byte>("skin");
            m_face = query.Get<int>("face");
            m_hair = query.Get<int>("hair");
            m_level = query.Get<byte>("level");
            m_job = query.Get<short>("job");
            m_strength = query.Get<short>("strength");
            m_dexterity = query.Get<short>("dexterity");
            m_intelligence = query.Get<short>("intelligence");
            m_luck = query.Get<short>("luck");
            m_hp = query.Get<int>("hp");
            m_maxHp = query.Get<int>("max_hp");
            m_mp = query.Get<int>("mp");
            m_maxMp = query.Get<int>("max_mp");
            m_experience = query.Get<long>("experience");
            m_fame = query.Get<int>("fame");
            m_spawnPoint = query.Get<sbyte>("spawn_point");
            m_meso = query.Get<long>("meso");

            m_items = new PlayerItems(this, null);
            m_skills = new PlayerSkills(this, null);
            m_quests = new PlayerQuests(this, null);

            m_map = MasterServer.Instance.Worlds[m_client.World].Channels[m_client.Channel].MapFactory.GetMap(query.Get<int>("map"));

            if (true) // TODO: Gm check.
            {
                m_map = MasterServer.Instance.Worlds[m_client.World].Channels[m_client.Channel].MapFactory.GetMap(180000000);
            }
            else if (m_map.ForcedMap != 999999999)
            {
                m_map = MasterServer.Instance.Worlds[m_client.World].Channels[m_client.Channel].MapFactory.GetMap(m_map.ForcedMap);
            }
            else if (m_hp == 0)
            {
                m_hp = 50;

                m_map = MasterServer.Instance.Worlds[m_client.World].Channels[m_client.Channel].MapFactory.GetMap(m_map.ReturnMap);
            }

            m_position = m_map.Portals[m_spawnPoint].Position;
        }

        public void Save()
        {
            Database.Execute("UPDATE player " +
                             "SET gender=@gender, skin=@skin, face=@face, hair=@hair, level=@level, job=@job, strength=@strength, dexterity=@dexterity, intelligence=@intelligence, luck=@luck, hp=@hp, max_hp=@max_hp, mp=@mp, max_mp=@max_mp, experience=@experience, fame=@fame, map=@map, spawn_point=@spawn_point, meso=@meso, equipment_slots=@equipment_slots, usable_slots=@usable_slots, etcetera_slots=@etcetera_slots, cash_slots=@cash_slots " +
                             "WHERE player_id=@player_id",
                               new MySqlParameter("player_id", m_id),
                               new MySqlParameter("gender", m_gender),
                               new MySqlParameter("skin", m_skin),
                               new MySqlParameter("face", m_face),
                               new MySqlParameter("hair", m_hair),
                               new MySqlParameter("level", m_level),
                               new MySqlParameter("job", m_job),
                               new MySqlParameter("strength", m_strength),
                               new MySqlParameter("dexterity", m_dexterity),
                               new MySqlParameter("intelligence", m_intelligence),
                               new MySqlParameter("luck", m_luck),
                               new MySqlParameter("hp", m_hp),
                               new MySqlParameter("max_hp", m_maxHp),
                               new MySqlParameter("mp", m_mp),
                               new MySqlParameter("max_mp", m_maxMp),
                               new MySqlParameter("experience", m_experience),
                               new MySqlParameter("fame", m_fame),
                               new MySqlParameter("map", m_map.ID),
                               new MySqlParameter("spawn_point", m_spawnPoint),
                               new MySqlParameter("meso", m_meso),
                               new MySqlParameter("equipment_slots", (byte)24),
                               new MySqlParameter("usable_slots", (byte)24),
                               new MySqlParameter("setup_slots", (byte)24),
                               new MySqlParameter("etcetera_slots", (byte)24),
                               new MySqlParameter("cash_slots", (byte)96));

            m_items.Save();
            m_skills.Save();
            m_quests.Save();
        }

        public void Died()
        {
            if (m_event != null)
            {
                m_event.PlayerDied(this);
            }

            byte lossPercent;

            if (false || false) // TOOD: Check for beginner job or max level.
            {
                lossPercent = 0;
            }
            else if (false) // TODO: Check for 5130000 (Safety Charm).
            {
                lossPercent = 0;
            }
            else if (false) // TODO: Check for map reduced experience loss.
            {
                lossPercent = 1;
            }
            else if (false) // TODO: Check for thief job.
            {
                lossPercent = 5;
            }
            else if (false) // TODO: Check for mage job.
            {
                lossPercent = 7;
            }
            else
            {
                lossPercent = 10;
            }

            if (lossPercent != 0)
            {
                // TODO: Lose experience.
            }
        }

        public void ChangeMap(Map map)
        {
            // TODO: Send hide packet if player is Gm.

            // TODO: Party update.

            if (m_event != null)
            {
                m_event.PlayerChangedMap(this);
            }
        }

        public void PrepareLogOff(bool quickCleanup)
        {
            if (m_event != null)
            {
                m_event.PlayerDisconnected(this);
            }

            // TODO: Update buddies.
            // TODO: Update party.
            // TODO: Update guild.
            // TODO: Update chatroom.
        }
    }
}
