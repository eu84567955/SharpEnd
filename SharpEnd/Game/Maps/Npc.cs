using SharpEnd.Players;
using SharpEnd.Servers;
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

        public int StorageCost { get; private set; }
        public string Script { get; private set; }
        public Shop Shop { get; private set; }

        public Player Controller { get; set; }

        public bool HasShop
        {
            get
            {
                return Shop != null;
            }
        }

        public bool IsStorage
        {
            get
            {
                return StorageCost > 0;
            }
        }

        public Npc(int identifier)
            : base()
        {
            Identifier = identifier;

            var data = MasterServer.Instance.Npcs[Identifier];

            StorageCost = data.StorageCost;
            Script = data.Script;
            if (data.Shop != null) Shop = new Shop(data.Shop);
        }

        public Npc(MapNpcData data)
            : this(data.Identifier)
        {
            Position = data.Position;
            Stance = (sbyte)(data.Flip ? 1 : 2); // TODO: Validate this.
            Foothold = data.Foothold;
            MinimumClickX = data.MinimumClickX;
            MaximumClickX = data.MaximumClickX;
            Flip = data.Flip;
            Hide = data.Hide;
        }

        public Npc(int identifier, MovableLife reference)
            : this(identifier)
        {
            Position = reference.Position;
            Stance = reference.Stance;
            Foothold = reference.Foothold;
        }

        public void AssignController()
        {
            if (Controller == null)
            {
                int leastControlled = int.MaxValue;

                Player newController = null;

                foreach (Player player in Map.Players)
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
