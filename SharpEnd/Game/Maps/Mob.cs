using SharpEnd.Data;
using SharpEnd.Packets;
using SharpEnd.Players;
using SharpEnd.Servers;
using System;
using System.Collections.Generic;

namespace SharpEnd.Maps
{
    internal sealed class Mob : MapEntity, IControllable
    {
        public int Identifier { get; private set; }

        public MobData Data { get; private set; }

        public int Health { get; private set; }

        public SpawnPoint SpawnPoint { get; private set; }
        
        public Player Controller { get; set; }

        public Dictionary<Player, int> Attackers { get; private set; }

        public Mob(int identifier)
        {
            Identifier = identifier;

            Data = MasterServer.Instance.Mobs[identifier];

            Health = Data.MaxHealth;

            Attackers = new Dictionary<Player, int>();
        }

        public Mob(SpawnPoint spawnPoint)
            : this(spawnPoint.Identifier)
        {
            SpawnPoint = spawnPoint;

            Position = spawnPoint.Position;
            Stance = spawnPoint.Stance;
            Foothold = spawnPoint.Foothold;
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
            amount = Math.Min(amount, Health);

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
            }

            return false;
        }

        public void Die()
        {
            Map.Mobs.Remove(this);
        }
    }
}
