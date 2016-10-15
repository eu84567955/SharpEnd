using SharpEnd.Drawing;
using SharpEnd.Packets;
using SharpEnd.Players;
using SharpEnd.Servers;
using System;
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

        public byte Level { get; private set; }
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public int Mana { get; private set; }
        public int MaxMana { get; private set; }
        public int HPRecovery { get; private set; }
        public int MPRecovery { get; private set; }
        public int ExplodeHP { get; private set; }
        public int Experience { get; private set; }
        public int LinkIdentifier { get; private set; }
        public byte SummonType { get; private set; }
        public int Knockback { get; private set; }
        public ushort FixedDamage { get; private set; }
        public int DeathBuffIdentifier { get; private set; }
        public int DeathAfter { get; private set; }
        public byte Traction { get; private set; }
        public int DamagedBySkillIdentifierOnly { get; private set; }
        public int DamagedByMobIdentifierOnly { get; private set; }
        public byte HPBarColor { get; private set; }
        public byte HPBarBackgroundColor { get; private set; }
        public byte CarnivalPoints { get; private set; }
        public ushort PhysicalAttack { get; private set; }
        public ushort PhysicalDefense { get; private set; }
        public ushort MagicalAttack { get; private set; }
        public ushort MagicalDefense { get; private set; }
        public short Accuracy { get; private set; }
        public ushort Avoidance { get; private set; }
        public short Speed { get; private set; }
        public short ChaseSpeed { get; private set; }
        public List<int> Summons { get; private set; }
        public List<Loot> Loots { get; private set; }

        public Mob(int identifier)
            : base()
        {
            Identifier = identifier;

            Attackers = new Dictionary<Player, int>();
            Stance = 5; // TODO.
            CanDrop = true;

            var data = MasterServer.Instance.Mobs[identifier];

            Level = data.Level;
            Health = MaxHealth = data.Health;
            Mana = MaxMana = data.Mana;
            HPRecovery = data.HealthRecovery;
            MPRecovery = data.ManaRecovery;
            ExplodeHP = data.ExplodeHP;
            Experience = data.Experience;
            LinkIdentifier = data.LinkIdentifier;
            SummonType = data.SummonType;
            Knockback = data.Knockback;
            FixedDamage = data.FixedDamage;
            DeathBuffIdentifier = data.DeathBuffIdentifier;
            DeathAfter = data.DeathAfter;
            Traction = data.Traction;
            DamagedBySkillIdentifierOnly = data.DamagedBySkillIdentifierOnly;
            DamagedByMobIdentifierOnly = data.DamagedByMobIdentifierOnly;
            HPBarColor = data.HPBarColor;
            HPBarBackgroundColor = data.HPBarBackgroundColor;
            CarnivalPoints = data.CarnivalPoints;
            PhysicalAttack = data.PhysicalAttack;
            PhysicalDefense = data.PhysicalDefense;
            MagicalAttack = data.MagicalAttack;
            MagicalDefense = data.MagicalDefense;
            Accuracy = data.Accuracy;
            Avoidance = data.Avoidance;
            Speed = data.Speed;
            ChaseSpeed = data.ChaseSpeed;

            Summons = new List<int>();
            data.Summons.ForEach(s => Summons.Add(s));

            Loots = new List<Loot>();
            data.Drops.ForEach(d => Loots.Add(new Loot(d)));
        }

        public Mob(MapMobData data)
            : this(data.Identifier)
        {
            Position = data.Position;
            Stance = (sbyte)(data.Flip ? 1 : 2); // TODO: Validate this.
            Foothold = data.Foothold;
        }

        public Mob(int identifier, MovableLife reference)
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
                    IsProvoked = false;

                    newController.ControlledMobs.Add(this);
                }
            }
        }

        public void SwitchController(Player player)
        {
            lock (this)
            {
                if (Controller != player)
                {
                    Controller.ControlledMobs.Remove(this);

                    player.ControlledMobs.Add(this);
                }
            }
        }

        public bool Damage(Player player, int amount)
        {
            int originalAmount = amount;

            amount = Math.Min(amount, Health);

            if (Attackers.ContainsKey(player))
            {
                Attackers[player] += originalAmount;
            }
            else
            {
                Attackers.Add(player, originalAmount);
            }

            Health -= amount;

            if (Health > 0)
            {
                var percent = (byte)((Health * 100) / MaxHealth);

                player.Send(MobPackets.MobHealth(ObjectIdentifier, percent));
            }

            return Health == 0;
        }

        public void Die()
        {
            Map.Mobs.Remove(this);
        }
    }
}
