using SharpEnd.Drawing;
using SharpEnd.Players;
using System.Collections.Generic;
using static SharpEnd.Game.Data.MapData;

namespace SharpEnd.Game.Maps
{
    internal sealed class Mob : MapEntity, IControllable
    {
        public int Identifier { get; private set; }
        public Player Controller { get; set; }
        public Dictionary<Player, int> Attackers { get; private set; }
        public bool IsProvoked { get; set; }
        public bool CanDrop { get; set; }

        public int Health { get; set; }
        public int Mana { get; set; }

        public Mob(int identifier)
            : base()
        {
            Identifier = identifier;

            Attackers = new Dictionary<Player, int>();
            CanDrop = true;
        }

        public Mob(MapMobData data)
            : this(data.Identifier)
        {
            Position = data.Position;
            Foothold = data.Foothold;
        }

        public Mob(int identifier, Point position)
            : this(identifier)
        {
            Position = position;
            Stance = 0;
            Foothold = 0; // TODO: FindFloor.
        }

        public void AssignController()
        {
            if (Controller == null)
            {
                int leastControlled = int.MaxValue;

                Player newController = null;

                foreach (Player player in Map.Players.Values)
                {
                    if (player.ControlledMobs.Count < leastControlled)
                    {
                        leastControlled = player.ControlledMobs.Count;

                        newController = player;
                    }
                }

                if (newController != null)
                {
                    newController.ControlledMobs.Add(this);
                }
            }
        }

        public bool Damage(Player player, int amount)
        {
            /*amount = Math.Min(amount, Health);

            if (Attackers.ContainsKey(player))
            {
                Attackers[player] += amount;
            }
            else
            {
                Attackers.Add(player, amount);
            }

            Health -= amount;

            var percent = (byte)((Health * 100) / Data.MaxHealth);

            player.Send(MobPackets.MobHealth(ObjectIdentifier, percent));

            if (Health <= 0)
            {
                return true;
            }*/

            return false;
        }

        public void Die()
        {
            Map.Mobs.Remove(this);
        }
    }
}
