using SharpEnd.Drawing;
using SharpEnd.Game.Life;
using SharpEnd.Game.Maps;
using SharpEnd.Game.Scripting;
using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Game.Players;
using SharpEnd.Network.Servers;
using System.Collections.Generic;

namespace SharpEnd.Handlers
{
    public static class PlayerHandlers
    {
        [PacketHandler(EHeader.CMSG_MAP_CHANGE)]
        public static void ChangeMapHandler(GameClient client, InPacket inPacket)
        {
            /*var player = client.Player;

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
                        inPacket.Skip(4); // NOTE: Unknown.
                        string label = inPacket.ReadString();

                        Portal portal;

                        try
                        {
                            portal = player.Map.Portals[label];
                        }
                        catch (KeyNotFoundException)
                        {
                            return;
                        }

                        Portal link = MasterServer.Instance.Worlds[client.WorldID][client.ChannelID].MapFactory.GetMap(portal.DestinationMapID).Portals[portal.DestinationLabel];

                        player.SetMap(portal.DestinationMapID, link);
                    }
                    break;

                default:
                    {
                        // TODO: /m command, some say.
                    }
                    break;
            }*/
        }

        [PacketHandler(EHeader.CMSG_PLAYER_MOVE)]
        public static void PlayerMoveHandler(GameClient client, InPacket inPacket)
        {
            /* var player = client.Player;

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

             player.Map.Send(PlayersPackets.PlayerMove(player.ID, origin, inPacket.ReadLeftoverBytes()), player);
         */
        }

        [PacketHandler(EHeader.CMSG_SIT)]
        public static void SitHandler(GameClient client, InPacket inPacket)
        {
            /*var player = client.Player;

            short seatID = inPacket.ReadShort();

            // TODO: Validate the seat identifier.
            // TODO: Check distance of seat relative to the player.

            if (seatID != -1)
            {
                player.Map.Send(MapPackets.MapSeat(player.ID, seatID));
            }
            else
            {
                player.Map.Send(MapPackets.MapSeatCancel(player.ID));
            }
        */
        }

        [PacketHandler(EHeader.CMSG_ATTACK_MELEE)]
        public static void AttackMeleeHandler(GameClient client, InPacket inPacket)
        {
            /*var player = client.Player;

            AttackData attack = AttackData.Compile(ESkillType.Melee, player, inPacket);

            if (attack.Portals != player.PortalCount)
            {
                return;
            }

            int masteryID = 0; // TODO: Obtain this from players' skills
            sbyte damagedTargets = 0;
            int skillID = attack.SkillID;
            byte skillLevel = attack.SkillLevel;

            if (skillID != (int)Skills.All.RegularAttack)
            {
                // TODO: Use the god-damn skill!
            }

            // TODO: Broadcast to map.

            List<Mob> dead = new List<Mob>();

            foreach (var target in attack.Damages)
            {
                Mob mob;

                try
                {
                    mob = player.Map.Mobs[target.Key];
                }
                catch (KeyNotFoundException)
                {
                    continue;
                }

                mob.IsProvoked = true;
                //mob.SwitchController(player);

                int totalDamage = 0;

                foreach (var hit in target.Value)
                {
                    totalDamage += hit;
                }

                if (mob.Damage(player, totalDamage))
                {
                    dead.Add(mob);
                }
            }

            foreach (Mob mob in dead)
            {
                mob.Die();
            }*/
        }

        // TODO: Move else-where
        public sealed class ReturnDamageData
        {
            public bool IsPhysical = true;
            public byte Reduction = 0;
            public int Damage = 0;
            public int MobUniqueID = 0;
            public Point Position;
        }

        [PacketHandler(EHeader.CMSG_PLAYER_HIT)]
        public static void PlayerHitHandler(GameClient client, InPacket inPacket)
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

            //player.Stats.DamageHealth(damage);

            /*bool damageApplied = false;
            bool deadlyAttack = false;
            byte hit = 0;
            byte stance = 0;
            byte disease = 0;
            byte level = 0;
            short mpBurn = 0;
            int uniqueID = 0;
            int mobID = 0;
            int noDamageID = 0;

            ReturnDamageData pgmr = new ReturnDamageData();

            if (type != MapDamage)
            {
                mobID = inPacket.ReadInt();
                uniqueID = inPacket.ReadInt();

                Mob mob = MasterServer.Instance.GetMapsclient.ChannelID][player.Map].Mobs[uniqueID];

                if (mob == null || mob.ID != mobID)
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

                    var attack = MasterServer.Instance.MobDataProvider.GetMobAttack(mob.ID, type);

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
                    pgmr.MobUniqueID = inPacket.ReadInt();

                    if (pgmr.MobUniqueID != uniqueID)
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

            ////player.SendMap(PlayersPackets.DamagePlayer(player.ID, damage, mobID, hit, type, stance, noDamageID, pgmr));
            */
        }

        [PacketHandler(EHeader.CMSG_PLAYER_CHAT)]
        public static void PlayerChatHandler(GameClient client, InPacket inPacket)
        {
            /*var player = client.Player;

            inPacket.ReadInt(); // NOTE: Ticks
            string text = inPacket.ReadString();
            bool shout = inPacket.ReadBoolean();

            if (text.StartsWith(Application.CommandIndicator) || text.StartsWith(Application.PlayerCommandIndicator))
            {
                //MasterServer.Instance.Commands.Execute(player, text);
            }
            else
            {
                player.Map.Send(PlayersPackets.PlayerChat(player.ID, text, player.IsGm, shout));
            }*/
        }

        [PacketHandler(EHeader.CMSG_PLAYER_EMOTE)]
        public static void PlayerEmoteHandler(GameClient client, InPacket inPacket)
        {

        }

        [PacketHandler(EHeader.CMSG_STAT_ADD)]
        public static void StatAddHandler(GameClient client, InPacket inPacket)
        {
            /*var player = client.Player;

            inPacket.ReadInt(); // NOTE: Ticks.

            EPlayerUpdate type = (EPlayerUpdate)inPacket.ReadUInt();

            player.Release();

            player.Stats.AddAbility(type);*/
        }

        [PacketHandler(EHeader.CMSG_STAT_ADD_MULTI)]
        public static void AbilityPointsAutoAddHandler(GameClient client, InPacket inPacket)
        {
            /*var player = client.Player;

            inPacket.Skip(4); // NOTE: Ticks.
            inPacket.Skip(4); // NOTE: Unknown.

            EPlayerUpdate primaryType = (EPlayerUpdate)inPacket.ReadULong();
            ushort primaryAmount = (ushort)inPacket.ReadUInt();
            EPlayerUpdate secondaryType = (EPlayerUpdate)inPacket.ReadULong();
            ushort secondaryAmount = (ushort)inPacket.ReadUInt();

            if ((primaryAmount + secondaryAmount) < player.Stats.AbilityPoints)
            {
                return;
            }

            player.Release();

            player.Stats.AddAbility(primaryType, primaryAmount);
            player.Stats.AddAbility(secondaryType, secondaryAmount);*/
        }

        [PacketHandler(EHeader.CMSG_PLAYER_HEAL)]
        public static void PlayerHealHandler(GameClient client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.Skip(4); // NOTE: Ticks.

            /*uint health = inPacket.ReadUInt();
            uint mana = inPacket.ReadUInt();

            player.Stats.ModifyHealth(health);
            player.Stats.ModifyMana(mana);*/

        }

        [PacketHandler(EHeader.CMSG_PLAYER_DETAILS)]
        public static void PlayerDetailsHandler(GameClient client, InPacket inPacket)
        {
            var player = client.Player;

            inPacket.ReadInt();
            int playerID = inPacket.ReadInt();
            sbyte worldID = inPacket.ReadSByte(); // NOTE: Used for cross-world operations

            if (worldID == -1)
            {
                Player target;

                try
                {
                    target = player.Map.Players[playerID];
                }
                catch (KeyNotFoundException)
                {
                    return;
                }

                client.Send(PlayersPackets.PlayerDetails(target, playerID == player.Id));
            }
            else
            {
                // TODO: Cross-world operations.
            }
        }

        [PacketHandler(EHeader.CMSG_CHANGE_MAP_SCRIPTED)]
        public static void ChangeMapScriptedHandler(GameClient client, InPacket inPacket)
        {
            /*var player = client.Player;

            byte portalCount = inPacket.ReadByte();

            if (portalCount != player.PortalCount)
            {
                return;
            }

            string label = inPacket.ReadString();

            Portal portal;

            try
            {
                portal = player.Map.Portals[label];
            }
            catch (KeyNotFoundException)
            {
                return;
            }

            // TODO: Check portal distance relative to the player.

            if (!PortalScriptManager.Instance.RunScript(player, portal.Script, portal.Label))
            {
                player.Release();
            }*/
        }

        [PacketHandler(EHeader.CMSG_INSTANT_WARP)]
        public static void InstantWarpHandler(GameClient client, InPacket inPacket)
        {
            /*var player = client.Player;

            byte portalCount = inPacket.ReadByte();

            if (player.PortalCount != portalCount)
            {
                return;
            }

            string label = inPacket.ReadString();

            Portal portal;

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

            player.Map.Send(PlayersPackets.PlayerMove(player.ID, destination, null), player);*/
        }

        [PacketHandler(EHeader.CMSG_QUEST)]
        public static void QuestHandler(GameClient client, InPacket inPacket)
        {
            var player = client.Player;

            EQuestAction action = (EQuestAction)inPacket.ReadSByte();
            ushort questID = (ushort)inPacket.ReadInt();

            switch (action)
            {
                case EQuestAction.RestoreLostItem:
                    {

                    }
                    break;

                case EQuestAction.Start:
                    {
                        int npcID = inPacket.ReadInt();

                        player.Quests.Start(questID, npcID);
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
        public static void SpecialStatHandler(GameClient client, InPacket inPacket)
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

        [PacketHandler(EHeader.CMSG_KEYMAP)]
        public static void KeymapHandler(GameClient client, InPacket inPacket)
        {
            /*var player = client.Player;

            int mode = inPacket.ReadInt();

            switch (mode)
            {
                case 0: // NOTE: Key change.
                    {
                        player.Keymap.IsModified = true;

                        int count = inPacket.ReadInt();

                        while (count-- > 0)
                        {
                            int keyID = inPacket.ReadInt();
                            byte type = inPacket.ReadByte();
                            int action = inPacket.ReadInt();

                            if (type != 0)
                            {
                                if (player.Keymap.ContainsKey(keyID))
                                {
                                    player.Keymap[keyID] = new Shortcut(type, action);
                                }
                                else
                                {
                                    player.Keymap.Add(keyID, new Shortcut(type, action));
                                }
                            }
                            else
                            {
                                player.Keymap.Remove(keyID);
                            }
                        }
                    }
                    break;

                case 1: // NOTE: Automatic health potions.
                    {

                    }
                    break;

                case 2: // NOTE: Automatic mana potions.
                    {

                    }
                    break;
            }*/
        }
    }
}
