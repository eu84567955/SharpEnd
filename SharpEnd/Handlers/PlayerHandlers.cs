using SharpEnd.Data;
using SharpEnd.Drawing;
using SharpEnd.Maps;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Players;
using SharpEnd.Servers;

namespace SharpEnd.Handlers
{
    internal static class PlayerHandlers
    {
        [PacketHandler(EHeader.CMSG_CHANGE_MAP)]
        public static void ChangeMap(Client client, InPacket inPacket)
        {
            var player = client.Player;

            byte portalCount = inPacket.ReadByte();

            if (portalCount != player.PortalCount)
            {
                return;
            }

            int mode = inPacket.ReadInt();

            switch (mode)
            {
                case 0:
                    {
                        if (!player.Stats.IsAlive)
                        {
                            inPacket.ReadString();
                            inPacket.ReadByte();
                            bool wheel = inPacket.ReadBoolean();

                            if (wheel)
                            {

                            }

                            player.AcceptDeath(wheel);
                        }
                    }
                    break;

                case -1:
                    {
                        inPacket.Skip(4); // NOTE: Unknown
                        string label = inPacket.ReadString();

                        PortalData portal = MasterServer.Instance.Maps[player.Map].Portals[label];

                        if (portal == null)
                        {
                            return;
                        }

                        PortalData destinationPortal = MasterServer.Instance.Maps[portal.DestinationMap].Portals[portal.DestinationLabel];

                        player.SetMap(portal.DestinationMap, destinationPortal);
                    }
                    break;

                default:
                    {
                        // TODO: /m command
                    }
                    break;
            }
        }

        [PacketHandler(EHeader.CMSG_PLAYER_MOVE)]
        public static void PlayerMove(Client client, InPacket inPacket)
        {
            var player = client.Player;

            byte portalCount = inPacket.ReadByte();

            if (player.PortalCount != portalCount)
            {
                return;
            }

            inPacket.Skip(13);

            Point origin = inPacket.ReadPoint();

            inPacket.Skip(4);

            int rewindOffset = inPacket.Position;

            if (!player.ParseMovement(inPacket))
            {
                return;
            }

            inPacket.Position = rewindOffset;

            MasterServer.Instance.Maps[player.Map].Send(PlayersPackets.PlayerMove(player.Identifier, origin, inPacket.ReadLeftoverBytes()), player);
        }

        // TODO: Move else-where
        public sealed class ReturnDamageData
        {
            public bool IsPhysical = true;
            public byte Reduction = 0;
            public int Damage = 0;
            public int MobUniqueIdentifier = 0;
            public Point Position;
        }

        [PacketHandler(EHeader.CMSG_PLAYER_HIT)]
        public static void PlayerHit(Client client, InPacket inPacket)
        {
            var player = client.Player;

            const sbyte BumpDamage = -1;
            const sbyte MapDamage = -2;

            inPacket.Skip(4); // NOTE: Unknown
            inPacket.Skip(4); // NOTE: Ticks
            sbyte type = inPacket.ReadSByte();
            inPacket.Skip(1); // NOTE: Element: None - 0x00, Ice - 0x01, Fire - 0x02, Lightning - 0x03
            int damage = inPacket.ReadInt();
            inPacket.Skip(2); // NOTE: Unknown

            player.Stats.DamageHealth(damage);

            /*bool damageApplied = false;
            bool deadlyAttack = false;
            byte hit = 0;
            byte stance = 0;
            byte disease = 0;
            byte level = 0;
            short mpBurn = 0;
            int uniqueIdentifier = 0;
            int mobIdentifier = 0;
            int noDamageIdentifier = 0;

            ReturnDamageData pgmr = new ReturnDamageData();

            if (type != MapDamage)
            {
                mobIdentifier = inPacket.ReadInt();
                uniqueIdentifier = inPacket.ReadInt();

                Mob mob = MasterServer.Instance.Maps[player.Map].Mobs[uniqueIdentifier];

                if (mob == null || mob.Identifier != mobIdentifier)
                {
                    return;
                }

                /*if (type != BumpDamage)
                {
                    if (mob == null)
                    {
                        // TODO: Restructre so the attack works fine even if the mob dies?

                        return;
                    }

                    var attack = MasterServer.Instance.MobDataProvider.GetMobAttack(mob.Identifier, type);

                    if (attack == null)
                    {
                        return;
                    }

                    disease = (byte)attack.Disease;
                    level = (byte)attack.Level;
                    mpBurn = (short)attack.MPBurn;
                    deadlyAttack = attack.DeadlyAttack;
                }

                hit = inPacket.ReadByte(); // NOTE: Knock direction
                pgmr.Reduction = inPacket.ReadByte();
                inPacket.Skip(1); // NOTE: I think reduction is a short, but it's a byte in the S -> C packet, so..

                if (pgmr.Reduction != 0)
                {
                    pgmr.IsPhysical = inPacket.ReadBoolean();
                    pgmr.MobUniqueIdentifier = inPacket.ReadInt();

                    if (pgmr.MobUniqueIdentifier != uniqueIdentifier)
                    {
                        return;
                    }

                    inPacket.Skip(1); // NOTE: 0x06 for Power Guard, 0x00 for Mana Reflection?
                    inPacket.Skip(4); // NOTE: Mob position
                    pgmr.Position = inPacket.ReadPoint();
                    pgmr.Damage = damage;

                    if (pgmr.IsPhysical)
                    {
                        // NOTE: Only Power Guard decreases damage

                        damage = (damage - (damage * pgmr.Reduction / 100));
                    }

                    //mob.ApplyDamage(player, (uint)(pgmr.Damage * pgmr.Reduction / 100));
                }
            }

            if (type == MapDamage)
            {
                level = inPacket.ReadByte();
                disease = inPacket.ReadByte();
            }
            else
            {
                // TODO: Power Stance

                //stance = inPacket.ReadByte();

                if (stance > 0)
                {

                }
            }

            if (damage == -1)
            {
                // TODO: No Damage Skills
            }

            if (disease > 0 && damage != 0)
            {
                // TODO: Add disease
            }

            uint health = player.Stats.Health;
            uint mana = player.Stats.Mana;

            if (damage > 0)
            {
                // TODO: Meso Guard

                // TODO: Magic Guard

                // TODO: Achilles

                if (!damageApplied)
                {
                    if (deadlyAttack)
                    {
                        // TODO: Deadly attack
                    }
                    else
                    {
                        player.Stats.DamageHealth((ushort)damage);
                    }

                    if (mpBurn > 0)
                    {
                        player.Stats.DamageMana(mpBurn);
                    }
                }

                // TODO: player.Buffs.TakeDamage(damage);
            }

            //player.SendMap(PlayersPackets.DamagePlayer(player.Identifier, damage, mobIdentifier, hit, type, stance, noDamageIdentifier, pgmr));
            */
        }

        [PacketHandler(EHeader.CMSG_PLAYER_CHAT)]
        public static void PlayerChat(Client client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.ReadInt(); // NOTE: Ticks
            string text = inPacket.ReadString();
            bool shout = inPacket.ReadBoolean();

            if (!MasterServer.Instance.Commands.Execute(player, text))
            {
                MasterServer.Instance.Maps[player.Map].Send(PlayersPackets.PlayerChat(player.Identifier, text, false, shout));
            }
        }

        [PacketHandler(EHeader.CMSG_PLAYER_EMOTE)]
        public static void PlayerEmote(Client client, InPacket inPacket)
        {

        }

        [PacketHandler(EHeader.CMSG_PLAYER_DETAILS)]
        public static void PlayerDetails(Client client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.ReadInt();
            int playerIdentifier = inPacket.ReadInt();
            sbyte worldIdentifier = inPacket.ReadSByte(); // NOTE: Used for cross-world operations

            if (worldIdentifier == -1)
            {
                Player target = MasterServer.Instance.Maps[player.Map].Players.Find(p => p.Identifier == playerIdentifier);
                
                if (target == null)
                {
                    return;
                }

                client.Send(PlayersPackets.PlayerDetails(target, playerIdentifier == player.Identifier));
            }
            else
            {
                // TODO: Cross-world operations
            }
        }
    }
}
