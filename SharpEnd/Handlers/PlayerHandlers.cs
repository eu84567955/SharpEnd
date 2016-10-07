using SharpEnd.Data;
using SharpEnd.Drawing;
using SharpEnd.Maps;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Players;
using SharpEnd.Servers;
using System.Collections.Generic;

namespace SharpEnd.Handlers
{
    internal static class PlayerHandlers
    {
        [PacketHandler(EHeader.CMSG_CHANGE_MAP)]
        public static void ChangeMapHandler(Client client, InPacket inPacket)
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

                        PortalData portal = player.Map.Portals[label];

                        if (portal == null)
                        {
                            return;
                        }

                        PortalData destinationPortal = MasterServer.Instance.GetMaps(client.ChannelIdentifier)[portal.DestinationMap].Portals[portal.DestinationLabel];

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
        public static void PlayerMoveHandler(Client client, InPacket inPacket)
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

            player.Map.Send(PlayersPackets.PlayerMove(player.Identifier, origin, inPacket.ReadLeftoverBytes()), player);
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
        public static void PlayerHitHandler(Client client, InPacket inPacket)
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

                Mob mob = MasterServer.Instance.GetMapsclient.ChannelIdentifier][player.Map].Mobs[uniqueIdentifier];

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
        public static void PlayerChatHandler(Client client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.ReadInt(); // NOTE: Ticks
            string text = inPacket.ReadString();
            bool shout = inPacket.ReadBoolean();

            if (text.StartsWith(Application.CommandIndicator) || text.StartsWith(Application.PlayerCommandIndicator))
            {
                MasterServer.Instance.Commands.Execute(player, text);
            }
            else
            {
                player.Map.Send(PlayersPackets.PlayerChat(player.Identifier, text, player.IsGm, shout));
            }
        }

        [PacketHandler(EHeader.CMSG_PLAYER_EMOTE)]
        public static void PlayerEmoteHandler(Client client, InPacket inPacket)
        {

        }

        [PacketHandler(EHeader.CMSG_STAT_ADD)]
        public static void StatAddHandler(Client client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.ReadInt(); // NOTE: Ticks.

            EStatisticType type = (EStatisticType)inPacket.ReadUInt();

            client.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.None, 0, true));

            player.Stats.AddAbility(type);
        }

        [PacketHandler(EHeader.CMSG_STAT_ADD_MULTI)]
        public static void AbilityPointsAutoAddHandler(Client client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.Skip(4); // NOTE: Ticks.
            inPacket.Skip(4); // NOTE: Unknown.

            EStatisticType primaryType = (EStatisticType)inPacket.ReadULong();
            ushort primaryAmount = (ushort)inPacket.ReadUInt();
            EStatisticType secondaryType = (EStatisticType)inPacket.ReadULong();
            ushort secondaryAmount = (ushort)inPacket.ReadUInt();

            if ((primaryAmount + secondaryAmount) < player.Stats.AbilityPoints)
            {
                return;
            }

            client.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.None, 0, true));

            player.Stats.AddAbility(primaryType, primaryAmount);
            player.Stats.AddAbility(secondaryType, secondaryAmount);
        }

        [PacketHandler(EHeader.CMSG_PLAYER_HEAL)]
        public static void PlayerHealHandler(Client client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.Skip(4); // NOTE: Ticks.

            uint health = inPacket.ReadUInt();
            uint mana = inPacket.ReadUInt();

            player.Stats.ModifyHealth(health);
            player.Stats.ModifyMana(mana);
        }

        [PacketHandler(EHeader.CMSG_PLAYER_DETAILS)]
        public static void PlayerDetailsHandler(Client client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.ReadInt();
            int playerIdentifier = inPacket.ReadInt();
            sbyte worldIdentifier = inPacket.ReadSByte(); // NOTE: Used for cross-world operations

            if (worldIdentifier == -1)
            {
                Player target;

                try
                {
                    target = player.Map.Players[playerIdentifier];
                }
                catch (KeyNotFoundException)
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

        [PacketHandler(EHeader.CMSG_INSTANT_WARP)]
        public static void InstantWarpHandler(Client client, InPacket inPacket)
        {
            var player = client.Player;

            byte portalCount = inPacket.ReadByte();

            if (player.PortalCount != portalCount)
            {
                return;
            }

            string label = inPacket.ReadString();

            PortalData portal;

            try
            {
                portal = player.Map.Portals[label];
            }
            catch (KeyNotFoundException)
            {
                return;
            }

            Point destination = inPacket.ReadPoint();

            // TODO: Check portal distance relative to the player
            // TODO: Check portal data to verify the destination

            player.Map.Send(PlayersPackets.PlayerMove(player.Identifier, destination, null), player);
        }

        [PacketHandler(EHeader.CMSG_QUEST)]
        public static void QuestHandler(Client client, InPacket inPacket)
        {
            var player = client.Player;

            EQuestAction action = (EQuestAction)inPacket.ReadSByte();
            ushort questIdentifier = (ushort)inPacket.ReadInt();

            switch (action)
            {
                case EQuestAction.RestoreLostItem:
                    {

                    }
                    break;

                case EQuestAction.Start:
                    {
                        int npcIdentifier = inPacket.ReadInt();

                        player.Quests.Start(questIdentifier, npcIdentifier);
                    }
                    break;

                case EQuestAction.End:
                    {

                    }
                    break;

                case EQuestAction.Forfeit:
                    {

                    }
                    break;

                case EQuestAction.ScriptStart:
                    {

                    }
                    break;

                case EQuestAction.ScriptEnd:
                    {

                    }
                    break;
            }
        }

        [PacketHandler(EHeader.CMSG_SPECIAL_STAT)]
        public static void SpecialStatHandler(Client client, InPacket inPacket)
        {
            string type = inPacket.ReadString();
            int array = inPacket.ReadInt();
            int mode = inPacket.ReadInt();

            using (OutPacket outPacket = new OutPacket())
            {
                outPacket
                    .WriteHeader(EHeader.SMSG_SPECIAL_STAT)
                    .WriteString(type)
                    .WriteInt(array)
                    .WriteInt(mode)
                    .WriteBoolean(true)
                    .WriteInt();

                client.Send(outPacket.ToArray());
            }
        }
    }
}
