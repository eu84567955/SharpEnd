using SharpEnd.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpEnd.Game.Data
{
    public sealed class MapData
    {
        public sealed class MapFootholdData
        {
            public ushort Identifier;
            public ushort NextIdentifier;
            public ushort PreviousIdentifier;
            public short DragForce;
            public Point Point1;
            public Point Point2;

            public void Read(BinaryReader reader)
            {
                Identifier = reader.ReadUInt16();
                NextIdentifier = reader.ReadUInt16();
                PreviousIdentifier = reader.ReadUInt16();
                DragForce = reader.ReadInt16();
                Point1 = new Point(reader.ReadInt16(), reader.ReadInt16());
                Point2 = new Point(reader.ReadInt16(), reader.ReadInt16());
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(Identifier);
                writer.Write(NextIdentifier);
                writer.Write(PreviousIdentifier);
                writer.Write(DragForce);
                writer.Write(Point1.X);
                writer.Write(Point1.Y);
                writer.Write(Point2.X);
                writer.Write(Point2.Y);
            }
        }

        public sealed class MapMobData
        {
            public int Identifier;
            public Point Position;
            public ushort Foothold;
            public bool Flip;
            public bool Hide;
            public int RespawnTime;

            public void Read(BinaryReader reader)
            {
                Identifier = reader.ReadInt32();
                Position = new Point(reader.ReadInt16(), reader.ReadInt16());
                Foothold = reader.ReadUInt16();
                Flip = reader.ReadBoolean();
                Hide = reader.ReadBoolean();
                RespawnTime = reader.ReadInt32();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(Identifier);
                writer.Write(Position.X);
                writer.Write(Position.Y);
                writer.Write(Foothold);
                writer.Write(Flip);
                writer.Write(Hide);
                writer.Write(RespawnTime);
            }
        }

        public sealed class MapNpcData
        {
            public int Identifier;
            public Point Position;
            public ushort Foothold;
            public short MinimumClickX;
            public short MaximumClickX;
            public bool Flip;
            public bool Hide;

            public void Read(BinaryReader reader)
            {
                Identifier = reader.ReadInt32();
                Position = new Point(reader.ReadInt16(), reader.ReadInt16());
                Foothold = reader.ReadUInt16();
                MinimumClickX = reader.ReadInt16();
                MaximumClickX = reader.ReadInt16();
                Flip = reader.ReadBoolean();
                Hide = reader.ReadBoolean();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(Identifier);
                writer.Write(Position.X);
                writer.Write(Position.Y);
                writer.Write(Foothold);
                writer.Write(MinimumClickX);
                writer.Write(MaximumClickX);
                writer.Write(Flip);
                writer.Write(Hide);
            }
        }

        public sealed class MapPortalData
        {
            public sbyte Identifier;
            public Point Position;
            public string Label;
            public int ToMap;
            public string ToName;
            public string Script;

            public void Read(BinaryReader reader)
            {
                Identifier = reader.ReadSByte();
                Position = new Point(reader.ReadInt16(), reader.ReadInt16());
                Label = reader.ReadString();
                ToMap = reader.ReadInt32();
                ToName = reader.ReadString();
                Script = reader.ReadString();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(Identifier);
                writer.Write(Position.X);
                writer.Write(Position.Y);
                writer.Write(Label);
                writer.Write(ToMap);
                writer.Write(ToName);
                writer.Write(Script);
            }
        }

        public sealed class MapSeatData
        {
            public short Identifier;
            public Point Position;

            public void Read(BinaryReader reader)
            {
                Identifier = reader.ReadInt16();
                Position = new Point(reader.ReadInt16(), reader.ReadInt16());
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(Identifier);
                writer.Write(Position.X);
                writer.Write(Position.Y);
            }
        }

        public int Identifier { get; set; }
        //public EMapFlags Flags { get; set; }
        public string ShuffleName { get; set; }
        public string Music { get; set; }
        public byte MinLevelLimit { get; set; }
        public ushort TimeLimit { get; set; }
        public byte RegenRate { get; set; }
        public float Traction { get; set; }
        public short LeftTopX { get; set; }
        public short LeftTopY { get; set; }
        public short RightBottomX { get; set; }
        public short RightBottomY { get; set; }
        public int ReturnMapIdentifier { get; set; }
        public int ForcedReturnMapIdentifier { get; set; }
        //public EMapFieldType FieldTypes { get; set; }
        //public EMapFieldLimit FieldLimits { get; set; }
        public byte DecreaseHP { get; set; }
        public ushort DamagePerSecond { get; set; }
        public int ProtectItemIdentifier { get; set; }
        public float MobRate { get; set; }
        public int LinkIdentifier { get; set; }
        public List<MapFootholdData> Footholds { get; set; }
        public List<MapMobData> Mobs { get; set; }
        public List<MapNpcData> Npcs { get; set; }
        //public List<MapReactorData> Reactors { get; set; }
        public List<MapPortalData> Portals { get; set; }
        public List<MapSeatData> Seats { get; set; }
        
        public void Read(BinaryReader reader)
        {
            Identifier = reader.ReadInt32();
            //Flags = (EMapFlags)pReader.ReadUInt16();
            ShuffleName = reader.ReadString();
            Music = reader.ReadString();
            MinLevelLimit = reader.ReadByte();
            TimeLimit = reader.ReadUInt16();
            RegenRate = reader.ReadByte();
            Traction = reader.ReadSingle();
            LeftTopX = reader.ReadInt16();
            LeftTopY = reader.ReadInt16();
            RightBottomX = reader.ReadInt16();
            RightBottomY = reader.ReadInt16();
            ReturnMapIdentifier = reader.ReadInt32();
            ForcedReturnMapIdentifier = reader.ReadInt32();
            //FieldTypes = (EMapFieldType)pReader.ReadUInt16();
            //FieldLimits = (EMapFieldLimit)pReader.ReadUInt32();
            DecreaseHP = reader.ReadByte();
            DamagePerSecond = reader.ReadUInt16();
            ProtectItemIdentifier = reader.ReadInt32();
            MobRate = reader.ReadSingle();
            LinkIdentifier = reader.ReadInt32();

            int footholdsCount = reader.ReadInt32();
            Footholds = new List<MapFootholdData>(footholdsCount);
            while (footholdsCount-- > 0)
            {
                MapFootholdData foothold = new MapFootholdData();
                foothold.Read(reader);
                Footholds.Add(foothold);
            }

            int mobsCount = reader.ReadInt32();
            Mobs = new List<MapMobData>(mobsCount);
            while (mobsCount-- > 0)
            {
                MapMobData mob = new MapMobData();
                mob.Read(reader);
                Mobs.Add(mob);
            }

            int npcsCount = reader.ReadInt32();
            Npcs = new List<MapNpcData>(npcsCount);
            while (npcsCount-- > 0)
            {
                MapNpcData npc = new MapNpcData();
                npc.Read(reader);
                Npcs.Add(npc);
            }

            /*int reactorsCount = pReader.ReadInt32();
            Reactors = new List<MapReactorData>(reactorsCount);
            while (reactorsCount-- > 0)
            {
                MapReactorData reactor = new MapReactorData();
                reactor.Load(pReader);
                Reactors.Add(reactor);
            }*/

            int portalsCount = reader.ReadInt32();
            Portals = new List<MapPortalData>(portalsCount);
            while (portalsCount-- > 0)
            {
                MapPortalData portal = new MapPortalData();
                portal.Read(reader);
                Portals.Add(portal);
            }

            int seatsCount = reader.ReadInt32();
            Seats = new List<MapSeatData>(seatsCount);
            while (seatsCount-- > 0)
            {
                MapSeatData seat = new MapSeatData();
                seat.Read(reader);
                Seats.Add(seat);
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Identifier);
            //pWriter.Write((ushort)Flags);
            writer.Write(ShuffleName);
            writer.Write(Music);
            writer.Write(MinLevelLimit);
            writer.Write(TimeLimit);
            writer.Write(RegenRate);
            writer.Write(Traction);
            writer.Write(LeftTopX);
            writer.Write(LeftTopY);
            writer.Write(RightBottomX);
            writer.Write(RightBottomY);
            writer.Write(ReturnMapIdentifier);
            writer.Write(ForcedReturnMapIdentifier);
            //pWriter.Write((ushort)FieldTypes);
            //pWriter.Write((uint)FieldLimits);
            writer.Write(DecreaseHP);
            writer.Write(DamagePerSecond);
            writer.Write(ProtectItemIdentifier);
            writer.Write(MobRate);
            writer.Write(LinkIdentifier);

            writer.Write(Footholds.Count);
            Footholds.ForEach(f => f.Write(writer));

            writer.Write(Mobs.Count);
            Mobs.ForEach(m => m.Write(writer));

            writer.Write(Npcs.Count);
            Npcs.ForEach(n => n.Write(writer));

            /*pWriter.Write(Reactors.Count);
            Reactors.ForEach(r => r.Write(pWriter));*/

            writer.Write(Portals.Count);
            Portals.ForEach(p => p.Write(writer));

            writer.Write(Seats.Count);
            Seats.ForEach(s => s.Write(writer));
        }
    }

    internal sealed class MapDataProvider : Dictionary<int, MapData>
    {
        public MapDataProvider() : base() { }

        public void Load()
        {
            using (FileStream stream = File.Open("data/Maps.bin", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII))
                {
                    int count = reader.ReadInt32();

                    while (count-- > 0)
                    {
                        MapData map = new MapData();

                        map.Read(reader);

                        Add(map.Identifier, map);
                    }
                }
            }
        }
    }
}
