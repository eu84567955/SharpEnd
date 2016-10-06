using SharpEnd.Players;

namespace SharpEnd.Maps
{
    internal sealed class Drop : MapEntity
    {
        public EDropType Type { get; private set; }

        public int Meso { get; private set; }
        public bool IsMeso => Meso > 0;

        public PlayerItem Item { get; private set; }

        public Drop(EDropType type, PlayerItem item)
        {
            Type = type;

            Item = item;
        }

        public Drop(EDropType type, int meso)
        {
            Type = type;

            Meso = meso;
        }
    }
}
