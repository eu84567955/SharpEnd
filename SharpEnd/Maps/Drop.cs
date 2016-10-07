using SharpEnd.Drawing;
using SharpEnd.Players;
using SharpEnd.Utility;

namespace SharpEnd.Maps
{
    internal abstract class Drop : MapEntity
    {
        private MapEntity dropper;

        public Player Owner { get; set; }
        public Player Picker { get; set; }
        public Point Origin { get; set; }
        public Delay Expiry { get; set; }

        public MapEntity Dropper
        {
            get
            {
                return dropper;
            }
            set
            {
                Origin = value.Position;
                Position = value.Map.Footholds.FindBelow(value.Position);

                dropper = value;
            }
        }

        public Drop() : base() { }
    }
}
