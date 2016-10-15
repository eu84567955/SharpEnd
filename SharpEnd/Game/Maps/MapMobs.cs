using SharpEnd.Drawing;
using SharpEnd.Game.Life;
using SharpEnd.Packets;
using SharpEnd.Players;
using System;
using System.Collections.Generic;

namespace SharpEnd.Game.Maps
{
    internal sealed class MapMobs : MapEntities<Mob>
    {
        public MapMobs(Map map) : base(map) { }

        protected override void InsertItem(Mob item)
        {
            base.InsertItem(item);

            Map.Send(MobPackets.MobSpawn(item, -2));

            item.AssignController();
        }

        // NOTE: Equivalent of mob death.
        protected override void RemoveItem(Mob item)
        {
            uint mostDamage = 0;
            Player owner = null;

            foreach (KeyValuePair<Player, int> attacker in item.Attackers)
            {
                if (attacker.Key.Map == Map && attacker.Key.Stats.IsAlive)
                {
                    if (attacker.Value > mostDamage)
                    {
                        owner = attacker.Key;
                    }

                    ulong experience = (ulong)(Math.Min(item.Experience, (attacker.Value * item.Experience) / item.MaxHealth));

                    attacker.Key.Stats.GiveExperience(experience, true, false);
                }
            }

            item.Attackers.Clear();

            if (item.CanDrop)
            {
                short i = 1;
                short mod = false ? 35 : 25; // TODO: Explosive.

                List<Drop> drops = new List<Drop>();

                foreach (Loot loot in item.Loots)
                {
                    if (loot.QuestIdentifier != 0) continue;

                    int chance = Math.Min(loot.Chance * 1, 1000000);

                    if (Randomizer.NextInt(0, 999999) < chance)
                    {
                        drops.Add(new PlayerItem(loot.ItemIdentifier, Randomizer.NextUShort(loot.Minimum, loot.Maximum))
                        {
                            Dropper = item,
                            Owner = owner
                        });
                    }
                }

                foreach (Drop drop in drops)
                {
                    short modX = (short)((i % 2 == 0) ?
                                      (mod * (i + 1) / 2) :
                                      -(mod * (i / 2)));

                    drop.Position = new Point(item.Position.X + modX, item.Position.Y);

                    i++;

                    Map.Drops.Add(drop);
                }
            }

            if (owner != null)
            {
                // TODO: Add mob to quests.
            }

            item.Controller.ControlledMobs.Remove(item);

            Map.Send(MobPackets.MobDespawn(item.ObjectIdentifier, 1));

            base.RemoveItem(item);

            // TODO: Respawn.

            foreach (int summon in item.Summons)
            {
                Map.Mobs.Add(new Mob(summon, item));
            }
        }
    }
}
