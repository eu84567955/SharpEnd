using SharpEnd.Drawing;
using SharpEnd.Packets;
using SharpEnd.Players;
using System;
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

                    ulong experience = (ulong)(Math.Min(mob.Experience, (attacker.Value * mob.Experience) / mob.MaxHealth));

                    attacker.Key.Stats.GiveExperience(experience, true, false);
                }
            }

            mob.Attackers.Clear();

            if (mob.CanDrop)
            {
                short i = 1;
                short mod = false ? 35 : 25; // TODO: Explosive.

                List<Drop> drops = new List<Drop>();

                foreach (Loot loot in mob.Loots)
                {
                    if (loot.QuestIdentifier != 0) continue;

                    int chance = Math.Min(loot.Chance * 1, 1000000);

                    if (Randomizer.NextInt(0, 999999) < chance)
                    {
                        drops.Add(new PlayerItem(loot.ItemIdentifier, Randomizer.NextUShort(loot.Minimum, loot.Maximum))
                        {
                            Dropper = mob,
                            Owner = owner
                        });
                    }
                }

                foreach (Drop drop in drops)
                {
                    short modX = (short)((i % 2 == 0) ?
                                      (mod * (i + 1) / 2) :
                                      -(mod * (i / 2)));

                    drop.Position = new Point(mob.Position.X + modX, mob.Position.Y);

                    i++;

                    Map.Drops.Add(drop);
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

            foreach (int summon in mob.Summons)
            {
                Map.Mobs.Add(new Mob(summon, mob));
            }
        }
    }
}
