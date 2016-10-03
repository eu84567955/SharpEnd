using SharpEnd.Network;
using SharpEnd.Utility;

namespace SharpEnd.Players
{
    internal sealed class PlayerStats
    {
        private Player m_player;

        public byte Level { get; private set; }
        public ushort Job { get; private set; }
        public ushort Strength { get; private set; }
        public ushort Dexterity { get; private set; }
        public ushort Intelligence { get; private set; }
        public ushort Luck { get; private set; }
        public uint Health { get; private set; }
        public uint MaxHealth { get; private set; }
        public uint Mana { get; private set; }
        public uint MaxMana { get; private set; }
        public ushort AbilityPoints { get; private set; }
        public ulong Experience { get; private set; }
        public int Fame { get; private set; }

        public PlayerStats(Player player, DatabaseQuery query)
        {
            m_player = player;

            Level = query.Get<byte>("level");
            Job = query.Get<ushort>("job");
            Strength = query.Get<ushort>("strength");
            Dexterity = query.Get<ushort>("dexterity");
            Intelligence = query.Get<ushort>("intelligence");
            Luck = query.Get<ushort>("luck");
            Health = query.Get<uint>("health");
            MaxHealth = query.Get<uint>("max_health");
            Mana = query.Get<uint>("mana");
            MaxMana = query.Get<uint>("max_mana");
            AbilityPoints = query.Get<ushort>("ability_points");
            Experience = query.Get<ulong>("experience");
            Fame = query.Get<int>("fame");
        }

        public void WriteInitial(OutPacket outPacket)
        {
            outPacket
                .WriteByte(Level)
                .WriteUShort(Job)
                .WriteUShort(Strength)
                .WriteUShort(Dexterity)
                .WriteUShort(Intelligence)
                .WriteUShort(Luck)
                .WriteUInt(Health)
                .WriteUInt(MaxHealth)
                .WriteUInt(Mana)
                .WriteUInt(MaxMana)
                .WriteUShort(AbilityPoints)
                .WriteByte()
                .WriteULong(Experience)
                .WriteInt(Fame);
        }
    }
}
