using SharpEnd.Drawing;
using SharpEnd.Players;

namespace SharpEnd.Maps
{
    internal sealed class Npc : LifeEntity, IControllable
    {
        public short RX0 { get; private set; }
        public short RX1 { get; private set; }

        public string Script
        {
            get
            {
                return "levelUP";
            }
        }

        public Player Controller { get; set; }

        public Npc(int identifier, short rx0, short rx1, Point position, ushort foothold, bool flip, bool hide)
            : base(identifier, position, foothold, flip, hide)
        {
            RX0 = rx0;
            RX1 = rx1;
        }

        public void AssignController()
        {
            if (Controller == null)
            {
                int leastControlled = int.MaxValue;

                Player newController = null;

                foreach (Player player in Map.Players.Values)
                {
                    if (player.ControlledNpcs.Count < leastControlled)
                    {
                        leastControlled = player.ControlledNpcs.Count;

                        newController = player;
                    }
                }

                if (newController != null)
                {
                    newController.ControlledNpcs.Add(this);
                }
            }
        }
    }
}
