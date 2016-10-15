using SharpEnd.Game.Data;
using SharpEnd.Players;

namespace SharpEnd.Game.Maps
{
    internal sealed class Map
    {
        public int Identifier { get; private set; }
        public string ShuffleName { get; private set; }
        public string Music { get; private set; }
        public byte MinLevelLimit { get; private set; }
        public ushort TimeLimit { get; private set; }
        public byte RegenRate { get; private set; }
        public float Traction { get; private set; }
        public short LeftTopX { get; private set; }
        public short LeftTopY { get; private set; }
        public short RightBottomX { get; private set; }
        public short RightBottomY { get; private set; }
        public int ReturnMapIdentifier { get; private set; }
        public int ForcedReturnMapIdentifier { get; private set; }
        public byte DecreaseHP { get; private set; }
        public ushort DamagePerSecond { get; private set; }
        public int ProtectItemIdentifier { get; private set; }
        public float MobRate { get; private set; }
        public int LinkIdentifier { get; private set; }

        public MapPlayers Players { get; private set; }
        public MapFootholds Footholds { get; private set; }
        public MapMobs Mobs { get; private set; }
        public MapNpcs Npcs { get; private set; }
        public MapPortals Portals { get; private set; }
        public MapDrops Drops { get; private set; }

        private int m_objectIdentifiers = 0;

        public Map(MapData data)
        {
            Identifier = data.Identifier;
            ShuffleName = data.ShuffleName;
            Music = data.Music;
            MinLevelLimit = data.MinLevelLimit;
            TimeLimit = data.TimeLimit;
            RegenRate = data.RegenRate;
            Traction = data.Traction;
            LeftTopX = data.LeftTopX;
            LeftTopY = data.LeftTopY;
            RightBottomX = data.RightBottomX;
            RightBottomY = data.RightBottomY;
            ReturnMapIdentifier = data.ReturnMapIdentifier;
            ForcedReturnMapIdentifier = data.ForcedReturnMapIdentifier;
            DecreaseHP = data.DecreaseHP;
            DamagePerSecond = data.DamagePerSecond;
            ProtectItemIdentifier = data.ProtectItemIdentifier;
            MobRate = data.MobRate;
            LinkIdentifier = data.LinkIdentifier;

            Players = new MapPlayers(this);
            Footholds = new MapFootholds(this);
            Mobs = new MapMobs(this);
            Npcs = new MapNpcs(this);
            Portals = new MapPortals(this);
            Drops = new MapDrops(this);

            data.Footholds.ForEach(f => Footholds.Add(new Foothold(f)));
            data.Mobs.ForEach(m => Mobs.Add(new Mob(m)));
            data.Npcs.ForEach(n => Npcs.Add(new Npc(n)));
            data.Portals.ForEach(p => Portals.Add(new Portal(p)));
        }

        public int AssignObjectIdentifier()
        {
            return ++m_objectIdentifiers;
        }

        public void Send(byte[] buffer, Player ignored = null)
        {
            foreach (Player player in Players.Values)
            {
                if (player != ignored)
                {
                    player.Send(buffer);
                }
            }
        }
    }
}
