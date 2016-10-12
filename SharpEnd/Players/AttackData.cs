using SharpEnd.Drawing;
using SharpEnd.Network;
using System.Collections.Generic;

namespace SharpEnd.Players
{
    internal sealed class AttackData
    {
        public bool IsMesoExplosion = false;
        public bool IsShadowMeso = false;
        public bool IsChargingSkill = false;
        public bool IsPiercingArrow = false;
        public bool IsHeal = false;
        public sbyte Targets = 0;
        public sbyte Hits = 0;
        public byte SkillLevel = 0;
        public byte WeaponSpeed = 0;
        public byte Animation = 0;
        public byte WeaponClass = 0;
        public byte Portals = 0;
        public short Display = 0;
        public short StarPosition = 0;
        public short CashStarPosition = 0;
        public int SkillIdentifier = 0;
        public int SummonIdentifier = 0;
        public int Charge = 0;
        public int StarIdentifier = 0;
        public int Ticks = 0;
        public ulong TotalDamage = 0;
        public Point ProjectilePosition;
        public Point PlayerPosition;
        public Dictionary<int, List<int>> Damages = new Dictionary<int, List<int>>();

        public static AttackData Compile(ESkillType skillType, Player player, InPacket inPacket)
        {
            AttackData attack = new AttackData();

            if (skillType != ESkillType.Summon)
            {
                attack.Portals = inPacket.ReadByte();
                byte tByte = inPacket.ReadByte();
                attack.SkillIdentifier = inPacket.ReadInt();
                attack.SkillLevel = inPacket.ReadByte();
                inPacket.Skip(6); // NOTE: Unknown.

                attack.Targets = (sbyte)((tByte >> 4) & 0xF);
                attack.Hits = (sbyte)(tByte & 0xF);

                switch (attack.SkillIdentifier)
                {
                    case (int)Skills.Hermit.ShadowMeso:
                        attack.IsShadowMeso = true;
                        break;

                    case (int)Skills.ChiefBandit.MesoExplosion:
                        attack.IsMesoExplosion = true;
                        break;

                    case (int)Skills.Cleric.Heal:
                        attack.IsHeal = true;
                        break;

                    case (int)Skills.Gunslinger.Grenade:
                    case (int)Skills.Brawler.CorkscrewBlow:
                    case (int)Skills.Bowmaster.Hurricane:
                    case (int)Skills.Marksman.PiercingArrow:
                    case (int)Skills.Corsair.RapidFire:
                    case (int)Skills.FpArchMage.BigBang:
                    case (int)Skills.IlArchMage.BigBang:
                    case (int)Skills.Bishop.BigBang:
                        attack.IsChargingSkill = true;
                        attack.Charge = inPacket.ReadInt();
                        break;
                }

                inPacket.Skip(1); // NOTE: Unknown.
                attack.Display = inPacket.ReadShort();
                attack.Animation = inPacket.ReadByte();
                inPacket.Skip(3); // NOTE: Unknown.
                attack.WeaponClass = inPacket.ReadByte();
                attack.WeaponSpeed = inPacket.ReadByte();
                attack.Ticks = inPacket.ReadInt();
            }
            else
            {
                attack.SummonIdentifier = inPacket.ReadInt();
                attack.Ticks = inPacket.ReadInt();
                attack.Animation = inPacket.ReadByte();
                attack.Targets = inPacket.ReadSByte();
                attack.Hits = 1;
            }

            if (skillType == ESkillType.Ranged)
            {
                attack.StarPosition = inPacket.ReadShort();
                attack.CashStarPosition = inPacket.ReadShort();
                inPacket.ReadByte(); // NOTE: 0x00 - AoE?
            }

            inPacket.Skip(8); // NOTE: Unknown.

            for (sbyte i = 0; i < attack.Targets; i++)
            {
                int uniqueIdentifier = inPacket.ReadInt();
                inPacket.Skip(20); // TODO: Figure these out. Mostly contains mob and damage positions.

                for (sbyte k = 0; k < attack.Hits; ++k)
                {
                    int damage = inPacket.ReadInt();

                    if (!attack.Damages.ContainsKey(uniqueIdentifier))
                    {
                        attack.Damages.Add(uniqueIdentifier, new List<int>());
                    }

                    attack.Damages[uniqueIdentifier].Add(damage);
                }

                if (skillType != ESkillType.Summon)
                {
                    inPacket.Skip(4); // NOTE: GetMobUpDownYRange.
                    inPacket.Skip(4); // NOTE: Mob CRC.
                    byte bSkeleton = inPacket.ReadByte();

                    if (bSkeleton == 1)
                    {

                    }
                    else if (bSkeleton == 2)
                    {

                    }
                }
            }

            if (skillType == ESkillType.Ranged)
            {
                attack.ProjectilePosition = inPacket.ReadPoint();
            }

            attack.PlayerPosition = inPacket.ReadPoint();

            return attack;
        }
    }
}
