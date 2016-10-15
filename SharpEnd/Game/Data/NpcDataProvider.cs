using SharpEnd.Collections;
using System;
using System.IO;
using System.Text;

namespace SharpEnd.Game.Data
{
    public sealed class NpcData
    {
        public int Identifier { get; set; }
        public bool IsShop { get; set; }
        public int StorageCost { get; set; }
        public string Script { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public void Load(BinaryReader reader)
        {
            Identifier = reader.ReadInt32();
            IsShop = reader.ReadBoolean();
            StorageCost = reader.ReadInt32();
            Script = reader.ReadString();
            if (reader.ReadBoolean()) Start = DateTime.FromFileTimeUtc(reader.ReadInt64());
            else Start = DateTime.MinValue;
            if (reader.ReadBoolean()) End = DateTime.FromFileTimeUtc(reader.ReadInt64());
            else End = DateTime.MaxValue;
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Identifier);
            writer.Write(IsShop);
            writer.Write(StorageCost);
            writer.Write(Script);
            writer.Write(Start != DateTime.MinValue);
            if (Start != DateTime.MinValue) writer.Write(Start.ToFileTimeUtc());
            writer.Write(End != DateTime.MaxValue);
            if (End != DateTime.MaxValue) writer.Write(End.ToFileTimeUtc());
        }
    }

    internal sealed class NpcDataProvider : SafeKeyedCollection<int, NpcData>
    {
        public NpcDataProvider() : base() { }

        protected override int GetKeyForItem(NpcData item)
        {
            return item.Identifier;
        }

        public void Load()
        {
            using (FileStream stream = File.Open("data/Npcs.bin", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII))
                {
                    int count = reader.ReadInt32();

                    while (count-- > 0)
                    {
                        NpcData npc = new NpcData();

                        npc.Load(reader);

                        Add(npc);
                    }
                }
            }
        }
    }
}
