using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Game.Data
{
    internal sealed class NpcDataProvider
    {
        private static NpcDataProvider instance;

        public static NpcDataProvider Instance { get { return instance ?? (instance = new NpcDataProvider()); } }

        private Dictionary<int, NpcData> m_npcs;

        private NpcDataProvider()
        {
            m_npcs = new Dictionary<int, NpcData>();
        }

        public void LoadData()
        {
            using (FileStream stream = File.OpenRead(Path.Combine("data", "npcs.bin")))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();

                    while (count-- > 0)
                    {
                        NpcData npc = new NpcData();
                        npc.Load(reader);
                        m_npcs[npc.ID] = npc;
                    }
                }
            }
        }

        public bool IsValidNpc(int npcID)
        {
            return m_npcs.ContainsKey(npcID);
        }

        public NpcData GetNpcData(int npcID)
        {
            return m_npcs[npcID];
        }
    }

    #region Data Classes
    public sealed class NpcData
    {
        private int m_id;
        private bool m_move;
        private bool m_parcel;
        private bool m_rpsGame;
        private bool m_storeBank;
        private bool m_guildRank;
        private string m_name;
        private int m_trunkGet;
        private int m_trunkPut;
        private List<NpcScriptData> m_scripts;

        public int ID { get { return m_id; } set { m_id = value; } }
        public bool Move { get { return m_move; } set { m_move = value; } }
        public bool Parcel { get { return m_parcel; } set { m_parcel = value; } }
        public bool RPSGame { get { return m_rpsGame; } set { m_rpsGame = value; } }
        public bool StoreBank { get { return m_storeBank; } set { m_storeBank = value; } }
        public bool GuildRank { get { return m_guildRank; } set { m_guildRank = value; } }
        public string Name { get { return m_name; } set { m_name = value; } }
        public int TrunkGet { get { return m_trunkGet; } set { m_trunkGet = value; } }
        public int TrunkPut { get { return m_trunkPut; } set { m_trunkPut = value; } }
        public List<NpcScriptData> Scripts { get { return m_scripts; } set { m_scripts = value; } }

        public void Load(BinaryReader reader)
        {
            m_id = reader.ReadInt32();
            m_move = reader.ReadBoolean();
            m_parcel = reader.ReadBoolean();
            m_rpsGame = reader.ReadBoolean();
            m_storeBank = reader.ReadBoolean();
            m_guildRank = reader.ReadBoolean();
            m_name = reader.ReadString();
            m_trunkGet = reader.ReadInt32();
            m_trunkPut = reader.ReadInt32();

            m_scripts = new List<NpcScriptData>();
            int scriptsCount = reader.ReadInt32();
            while (scriptsCount-- > 0)
            {
                NpcScriptData script = new NpcScriptData();
                script.Load(reader);
                m_scripts.Add(script);
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_id);
            writer.Write(m_move);
            writer.Write(m_parcel);
            writer.Write(m_rpsGame);
            writer.Write(m_storeBank);
            writer.Write(m_guildRank);
            writer.Write(m_name);
            writer.Write(m_trunkGet);
            writer.Write(m_trunkPut);

            writer.Write(m_scripts.Count);
            m_scripts.ForEach(s => s.Save(writer));
        }
    }

    public sealed class NpcScriptData
    {
        private int m_startDate;
        private int m_endDate;
        private string m_script;

        public int StartDate { get { return m_startDate; } set { m_startDate = value; } }
        public int EndDate { get { return m_endDate; } set { m_endDate = value; } }
        public string Script { get { return m_script; } set { m_script = value; } }

        public void Load(BinaryReader reader)
        {
            m_startDate = reader.ReadInt32();
            m_endDate = reader.ReadInt32();
            m_script = reader.ReadString();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(m_startDate);
            writer.Write(m_endDate);
            writer.Write(m_script);
        }
    }
    #endregion
}
