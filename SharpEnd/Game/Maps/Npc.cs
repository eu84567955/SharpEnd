using SharpEnd.Players;
using static SharpEnd.Game.Data.MapData;

namespace SharpEnd.Game.Maps
{
    internal sealed class Npc : MapEntity, IControllable
    {
        public int Identifier { get; private set; }
        public short MinimumClickX { get; private set; }
        public short MaximumClickX { get; private set; }
        public bool Flip { get; private set; }
        public bool Hide { get; private set; }

        public Player Controller { get; set; }

        public string Script
        {
            get
            {
                return "levelUP";
            }
        }

        public Npc(MapNpcData data)
        {
            Identifier = data.Identifier;
            Position = data.Position;
            Stance = (sbyte)(data.Flip ? 1 : 2); // TODO: Validate this.
            Foothold = data.Foothold;
            MinimumClickX = data.MinimumClickX;
            MaximumClickX = data.MaximumClickX;
            Flip = data.Flip;
            Hide = data.Hide;
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
