using SharpEnd.Drawing;
using SharpEnd.Packets;
using SharpEnd.Players;
using System.Collections.Generic;

namespace SharpEnd.Game.Maps
{
    internal sealed class MapMobs : MapEntities<Mob>
    {
        public MapMobs(Map map) : base(map) { }

        public override void Add(Mob mob)
        {
            base.Add(mob);

            Map.Send(MobPackets.MobSpawn(mob, -2));

            mob.AssignController();
        }

        // NOTE: Equivalent of mob death.
        public override void Remove(Mob mob)
        {
            uint mostDamage = 0;
            Player owner = null;

            foreach (KeyValuePair<Player, int> attacker in mob.Attackers)
            {
                if (attacker.Key.Map == Map && attacker.Key.Stats.IsAlive)
                {
                    if (attacker.Value > mostDamage)
                    {
                        owner = attacker.Key;
                    }

                    ulong experience = 0;// (ulong)(Math.Min(mob.Data.Experience, (attacker.Value * mob.Data.Experience) / mob.Data.MaxHealth));

                    attacker.Key.Stats.GiveExperience(experience, true, false);
                }
            }

            mob.Attackers.Clear();

            if (mob.CanDrop)
            {
                List<Drop> drops = new List<Drop>();

                /*foreach (Loot loot in mob.Data.Loots)
                {
                    if (Randomizer.NextInt(0, 999999) < loot.Chance)
                    {
                        drops.Add(new PlayerItem(loot.ItemIdentifier)
                        {
                            Dropper = mob,
                            Owner = owner
                        });
                    }
                }*/

                Point dropPosition = new Point(mob.Position.X, mob.Position.Y);

                dropPosition.X -= (short)(12 * drops.Count);

                foreach (Drop drop in drops)
                {
                    drop.Position = new Point(dropPosition.X, dropPosition.Y);

                    dropPosition.X += 25;

                    //Map.Drops.Add(drop);
                }
            }

            if (owner != null)
            {
                // TODO: Add mob to quests.
            }

            mob.Controller.ControlledMobs.Remove(mob);

            Map.Send(MobPackets.MobDespawn(mob.ObjectIdentifier, 1));

            base.Remove(mob);

            // TODO: Respawn.

            // TODO: Spawn death summons.
        }
    }
}
