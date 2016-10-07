using SharpEnd.Packets;

namespace SharpEnd.Maps
{
    internal sealed class MapMobs : MapEntities<Mob>
    {
        public MapMobs(Map map) : base(map) { }

        public override void Add(Mob mob)
        {
            base.Add(mob);

            Map.Send(MobPackets.MobSpawn(mob));

            mob.AssignController();
        }

        // NOTE: Equivalent of mob death.
        public override void Remove(Mob mob)
        {
            // TODO: Calculate most damage and distribute rewards accordingly.

            mob.Controller.ControlledMobs.Remove(mob);

            Map.Send(MobPackets.MobDespawn(mob.ObjectIdentifier, 1));

            base.Remove(mob);

            // TODO: Respawn.
        }
    }
}
