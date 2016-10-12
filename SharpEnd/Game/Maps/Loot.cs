using SharpEnd.Utility;

namespace SharpEnd.Maps
{
    internal sealed class Loot
    {
        public int ItemIdentifier { get; set; }
        public int Chance { get; set; }
        public int MinimumQuantity { get; set; }
        public int MaximumQuantity { get; set; }
        public int QuestIdentifier { get; set; }
    }
}
