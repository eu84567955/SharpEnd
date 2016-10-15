using static SharpEnd.Data.MobData;

namespace SharpEnd.Game.Maps
{
    internal sealed class Loot
    {
        public int ItemIdentifier { get; private set; }
        public int Minimum { get; private set; }
        public int Maximum { get; private set; }
        public int Chance { get; private set; }
        public ushort QuestIdentifier { get; private set; }

        public Loot(MobDropData data)
        {
            ItemIdentifier = data.ItemIdentifier;
            Minimum = data.Minimum;
            Maximum = data.Maximum;
            Chance = data.Chance;
            QuestIdentifier = data.QuestIdentifier;
        }
    }
}
