using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Game.Data
{
    internal sealed class MapDataProvider
    {
        private static MapDataProvider instance;

        public static MapDataProvider Instance { get { return instance ?? (instance = new MapDataProvider()); } }

        private Dictionary<int, MapData> m_maps;

        private MapDataProvider()
        {
            m_maps = new Dictionary<int, MapData>();
        }

        public void LoadData()
        {
            using (FileStream stream = File.OpenRead(Path.Combine("data", "maps.bin")))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();

                    while (count-- > 0)
                    {
                        MapData map = new MapData();
                        map.Load(reader);
                        m_maps[map.ID] = map;
                    }
                }
            }
        }

        public bool IsValidMap(int mapID)
        {
            return m_maps.ContainsKey(mapID);
        }

        public MapData GetMapData(int mapID)
        {
            return m_maps[mapID];
        }
    }

    #region Data Classes
    public sealed class MapData
    {
        private int m_id;
        private bool m_town;
        private bool m_swim;
        private bool m_clock;
        private bool m_everlasting;
        private bool m_personalShop;
        private bool m_allMoveCheck;
        private double m_recovery;
        private int m_returnMap;
        private int m_forcedMap;
        private int m_limit;
        private int m_autoDecHp;
        private int m_autoDecMp;
        private int m_protectItem;
        private List<MapSeatData> m_seats;
        private List<MapPortalData> m_portals;
        private List<MapSpawnData> m_spawns;
        private List<MapFootholdData> m_footholds;
        private List<MapReactorData> m_reactors;

        public int ID { get { return m_id; } set { m_id = value; } }
        public bool Town { get { return m_town; } set { m_town = value; } }
        public bool Swim { get { return m_swim; } set { m_swim = value; } }
        public bool Clock { get { return m_clock; } set { m_clock = value; } }
        public bool Everlasting { get { return m_everlasting; } set { m_everlasting = value; } }
        public bool PersonalShop { get { return m_personalShop; } set { m_personalShop = value; } }
        public bool AllMoveCheck { get { return m_allMoveCheck; } set { m_allMoveCheck = value; } }
        public double Recovery { get { return m_recovery; } set { m_recovery = value; } }
        public int ReturnMap { get { return m_returnMap; } set { m_returnMap = value; } }
        public int ForcedMap { get { return m_forcedMap; } set { m_forcedMap = value; } }
        public int Limit { get { return m_limit; } set { m_limit = value; } }
        public int AutoDecHp { get { return m_autoDecHp; } set { m_autoDecHp = value; } }
        public int AutoDecMp { get { return m_autoDecMp; } set { m_autoDecMp = value; } }
        public int ProtectItem { get { return m_protectItem; } set { m_protectItem = value; } }
        public List<MapSeatData> Seats { get { return m_seats; } set { m_seats = value; } }
        public List<MapPortalData> Portals { get { return m_portals; } set { m_portals = value; } }
        public List<MapSpawnData> Spawns { get { return m_spawns; } set { m_spawns = value; } }
        public List<MapFootholdData> Footholds { get { return m_footholds; } set { m_footholds = value; } }
        public List<MapReactorData> Reactors { get { return m_reactors; } set { m_reactors = value; } }

        public MapData()
        {
            m_seats = new List<MapSeatData>();
            m_portals = new List<MapPortalData>();
            m_spawns = new List<MapSpawnData>();
            m_footholds = new List<MapFootholdData>();
            m_reactors = new List<MapReactorData>();
        }

        public void Load(BinaryReader reader)
        {
            m_id = reader.ReadInt32();
            m_town = reader.ReadBoolean();
            m_swim = reader.ReadBoolean();
            m_clock = reader.ReadBoolean();
            m_everlasting = reader.ReadBoolean();
            m_personalShop = reader.ReadBoolean();
            m_allMoveCheck = reader.ReadBoolean();
            m_recovery = reader.ReadDouble();
            m_returnMap = reader.ReadInt32();
            m_forcedMap = reader.ReadInt32();
            m_limit = reader.ReadInt32();
            m_autoDecHp = reader.ReadInt32();
            m_autoDecMp = reader.ReadInt32();
            m_protectItem = reader.ReadInt32();

            m_seats = new List<MapSeatData>();
            int seatsCount = reader.ReadInt32();
            while (seatsCount-- > 0)
            {
                MapSeatData seat = new MapSeatData();
                seat.Load(reader);
                m_seats.Add(seat);
            }

            m_portals = new List<MapPortalData>();
            int portalsCount = reader.ReadInt32();
            while (portalsCount-- > 0)
            {
                MapPortalData portal = new MapPortalData();
                portal.Load(reader);
                m_portals.Add(portal);
            }

            m_spawns = new List<MapSpawnData>();
            int spawnsCount = reader.ReadInt32();
            while (spawnsCount-- > 0)
            {
                MapSpawnData spawn = new MapSpawnData();
                spawn.Load(reader);
                m_spawns.Add(spawn);
            }

            m_footholds = new List<MapFootholdData>();
            int footholdsCount = reader.ReadInt32();
            while (footholdsCount-- > 0)
            {
                MapFootholdData foothold = new MapFootholdData();
                foothold.Load(reader);
                m_footholds.Add(foothold);
            }

            m_reactors = new List<MapReactorData>();
            int reactorsCount = reader.ReadInt32();
            while (reactorsCount-- > 0)
            {
                MapReactorData reactor = new MapReactorData();
                reactor.Load(reader);
                m_reactors.Add(reactor);
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_id);
            writer.Write(m_town);
            writer.Write(m_swim);
            writer.Write(m_clock);
            writer.Write(m_everlasting);
            writer.Write(m_personalShop);
            writer.Write(m_allMoveCheck);
            writer.Write(m_recovery);
            writer.Write(m_returnMap);
            writer.Write(m_forcedMap);
            writer.Write(m_limit);
            writer.Write(m_autoDecHp);
            writer.Write(m_autoDecMp);
            writer.Write(m_protectItem);

            writer.Write(m_seats.Count);
            m_seats.ForEach(p => p.Save(writer));

            writer.Write(m_portals.Count);
            m_portals.ForEach(p => p.Save(writer));

            writer.Write(m_spawns.Count);
            m_spawns.ForEach(s => s.Save(writer));

            writer.Write(m_footholds.Count);
            m_footholds.ForEach(f => f.Save(writer));

            writer.Write(m_reactors.Count);
            m_reactors.ForEach(r => r.Save(writer));
        }
    }

    public sealed class MapSeatData
    {
        private short m_id;
        private short m_x;
        private short m_y;

        public short ID { get { return m_id; } set { m_id = value; } }
        public short X { get { return m_x; } set { m_x = value; } }
        public short Y { get { return m_y; } set { m_y = value; } }

        public void Load(BinaryReader reader)
        {
            m_id = reader.ReadInt16();
            m_x = reader.ReadInt16();
            m_y = reader.ReadInt16();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_id);
            writer.Write(m_x);
            writer.Write(m_y);
        }
    }

    public sealed class MapPortalData
    {
        private byte m_id;
        private short m_x;
        private short m_y;
        private int m_type;
        private int m_destinationMap;
        private string m_destinationName;
        private string m_name;
        private string m_script;

        public byte ID { get { return m_id; } set { m_id = value; } }
        public short X { get { return m_x; } set { m_x = value; } }
        public short Y { get { return m_y; } set { m_y = value; } }
        public int Type { get { return m_type; } set { m_type = value; } }
        public int DestinationMap { get { return m_destinationMap; } set { m_destinationMap = value; } }
        public string DestinationName { get { return m_destinationName; } set { m_destinationName = value; } }
        public string Name { get { return m_name; } set { m_name = value; } }
        public string Script { get { return m_script; } set { m_script = value; } }

        public void Load(BinaryReader reader)
        {
            m_id = reader.ReadByte();
            m_x = reader.ReadInt16();
            m_y = reader.ReadInt16();
            m_type = reader.ReadInt32();
            m_destinationMap = reader.ReadInt32();
            m_destinationName = reader.ReadString();
            m_name = reader.ReadString();
            m_script = reader.ReadString();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_id);
            writer.Write(m_x);
            writer.Write(m_y);
            writer.Write(m_type);
            writer.Write(m_destinationMap);
            writer.Write(m_destinationName);
            writer.Write(m_name);
            writer.Write(m_script);
        }
    }

    public sealed class MapSpawnData
    {
        private char m_type;
        private int m_id;
        private bool m_flip;
        private bool m_hide;
        private short m_x;
        private short m_y;
        private short m_cy;
        private short m_foothold;
        private short m_rx0;
        private short m_rx1;
        private int m_mobTime;

        public char Type { get { return m_type; } set { m_type = value; } }
        public int ID { get { return m_id; } set { m_id = value; } }
        public bool Flip { get { return m_flip; } set { m_flip = value; } }
        public bool Hide { get { return m_hide; } set { m_hide = value; } }
        public short X { get { return m_x; } set { m_x = value; } }
        public short Y { get { return m_y; } set { m_y = value; } }
        public short CY { get { return m_cy; } set { m_cy = value; } }
        public short Foothold { get { return m_foothold; } set { m_foothold = value; } }
        public short RX0 { get { return m_rx0; } set { m_rx0 = value; } }
        public short RX1 { get { return m_rx1; } set { m_rx1 = value; } }
        public int MobTime { get { return m_mobTime; } set { m_mobTime = value; } }

        public void Load(BinaryReader reader)
        {
            m_type = reader.ReadChar();
            m_id = reader.ReadInt32();
            m_flip = reader.ReadBoolean();
            m_hide = reader.ReadBoolean();
            m_x = reader.ReadInt16();
            m_y = reader.ReadInt16();
            m_cy = reader.ReadInt16();
            m_foothold = reader.ReadInt16();
            m_rx0 = reader.ReadInt16();
            m_rx1 = reader.ReadInt16();
            m_mobTime = reader.ReadInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_type);
            writer.Write(m_id);
            writer.Write(m_flip);
            writer.Write(m_hide);
            writer.Write(m_x);
            writer.Write(m_y);
            writer.Write(m_cy);
            writer.Write(m_foothold);
            writer.Write(m_rx0);
            writer.Write(m_rx1);
            writer.Write(m_mobTime);
        }
    }

    public sealed class MapFootholdData
    {
        private short m_id;
        private short m_x1;
        private short m_y1;
        private short m_x2;
        private short m_y2;

        public short ID { get { return m_id; } set { m_id = value; } }
        public short X1 { get { return m_x1; } set { m_x1 = value; } }
        public short Y1 { get { return m_y1; } set { m_y1 = value; } }
        public short X2 { get { return m_x2; } set { m_x2 = value; } }
        public short Y2 { get { return m_y2; } set { m_y2 = value; } }

        public void Load(BinaryReader reader)
        {
            m_id = reader.ReadInt16();
            m_x1 = reader.ReadInt16();
            m_y1 = reader.ReadInt16();
            m_x2 = reader.ReadInt16();
            m_y2 = reader.ReadInt16();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_id);
            writer.Write(m_x1);
            writer.Write(m_y1);
            writer.Write(m_x2);
            writer.Write(m_y2);
        }
    }

    public sealed class MapReactorData
    {
        private int m_id;
        private bool m_flip;
        private short m_x;
        private short m_y;
        private int m_reactorTime;
        private string m_name;

        public int ID { get { return m_id; } set { m_id = value; } }
        public bool Flip { get { return m_flip; } set { m_flip = value; } }
        public short X { get { return m_x; } set { m_x = value; } }
        public short Y { get { return m_y; } set { m_y = value; } }
        public int ReactorTime { get { return m_reactorTime; } set { m_reactorTime = value; } }
        public string Name { get { return m_name; } set { m_name = value; } }

        public void Load(BinaryReader reader)
        {
            m_id = reader.ReadInt32();
            m_flip = reader.ReadBoolean();
            m_x = reader.ReadInt16();
            m_y = reader.ReadInt16();
            m_reactorTime = reader.ReadInt32();
            m_name = reader.ReadString();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_id);
            writer.Write(m_flip);
            writer.Write(m_x);
            writer.Write(m_y);
            writer.Write(m_reactorTime);
            writer.Write(m_name);
        }
    }
    #endregion
}
