﻿namespace SharpEnd
{
    public static class StatusEffects
    {
        public static class Mob
        {
            public enum MobStatus : uint
            {
                // Groups of 5 for easier counting
                Watk = 0x01,
                Wdef = 0x02,
                Matk = 0x04,
                Mdef = 0x08,
                Acc = 0x10,

                Avoid = 0x20,
                Speed = 0x40,
                Stun = 0x80,
                Freeze = 0x100,
                Poison = 0x200,

                Seal = 0x400,
                NoClue1 = 0x800,
                WeaponAttackUp = 0x1000,
                WeaponDefenseUp = 0x2000,
                MagicAttackUp = 0x4000,

                MagicDefenseUp = 0x8000,
                Doom = 0x10000,
                ShadowWeb = 0x20000,
                WeaponImmunity = 0x40000,
                MagicImmunity = 0x80000,

                NoClue2 = 0x100000,
                NoClue3 = 0x200000,
                NinjaAmbush = 0x400000,
                NoClue4 = 0x800000,
                VenomousWeapon = 0x1000000,

                NoClue5 = 0x2000000,
                NoClue6 = 0x4000000,
                Empty = 0x8000000, // All mobs have this when they spawn
                Hypnotize = 0x10000000,
                WeaponDamageReflect = 0x20000000,

                MagicDamageReflect = 0x40000000,
                NoClue7 = 0x80000000 // Not any more bits you can use with 4 bytes
            }
        }

        public static class Player
        {
            public enum PlayerStatus : short
            {
                Curse = 0x01,
                Weakness = 0x02,
                Darkness = 0x04,
                Seal = 0x08,
                Poison = 0x10,
                Stun = 0x20,
                Slow = 0x40,
                Seduce = 0x80,
                Zombify = 0x100,
                CrazySkull = 0x200
            }
        }
    }

    public static class Skills
    {
        public const byte MaxComboOrbs = 5;
        public const byte MaxAdvancedComboOrbs = 10;

        public enum All : int
        {
            RegularAttack = 0
        }

        public enum Beginner : int
        {
            BlessingOfTheFairy = 12,
            EchoOfHero = 1005,
            FollowTheLead = 8,
            MonsterRider = 1004,
            NimbleFeet = 1002,
            Recovery = 1001,
            ThreeSnails = 1000,
            LegendarySpirit = 1003,
            Maker = 1007,
        }

        public enum Swordsman : int
        {
            ImprovedMaxHpIncrease = 1000001,
            IronBody = 1001003,
        }

        public enum Fighter : int
        {
            AxeBooster = 1101005,
            AxeMastery = 1100001,
            PowerGuard = 1101007,
            Rage = 1101006,
            SwordBooster = 1101004,
            SwordMastery = 1100000,
        }

        public enum Crusader : int
        {
            ArmorCrash = 1111007,
            AxeComa = 1111006,
            AxePanic = 1111004,
            ComboAttack = 1111002,
            Shout = 1111008,
            SwordComa = 1111005,
            SwordPanic = 1111003,
        }

        public enum Hero : int
        {
            Achilles = 1120004,
            AdvancedComboAttack = 1120003,
            Enrage = 1121010,
            Guardian = 1120005,
            HerosWill = 1121011,
            MapleWarrior = 1121000,
            MonsterMagnet = 1121001,
            PowerStance = 1121002,
        }

        public enum Page : int
        {
            BwBooster = 1201005,
            BwMastery = 1200001,
            PowerGuard = 1201007,
            SwordBooster = 1201004,
            SwordMastery = 1200000,
            Threaten = 1201006,
        }

        public enum WhiteKnight : int
        {
            BwFireCharge = 1211004,
            BwIceCharge = 1211006,
            BwLitCharge = 1211008,
            ChargeBlow = 1211002,
            MagicCrash = 1211009,
            SwordFireCharge = 1211003,
            SwordIceCharge = 1211005,
            SwordLitCharge = 1211007,
        }

        public enum Paladin : int
        {
            Achilles = 1220005,
            AdvancedCharge = 1220010,
            BwHolyCharge = 1221004,
            Guardian = 1220006,
            HeavensHammer = 1221011,
            HerosWill = 1221012,
            MapleWarrior = 1221000,
            MonsterMagnet = 1221001,
            PowerStance = 1221002,
            SwordHolyCharge = 1221003,
        }

        public enum Spearman : int
        {
            HyperBody = 1301007,
            IronWill = 1301006,
            PolearmBooster = 1301005,
            PolearmMastery = 1300001,
            SpearBooster = 1301004,
            SpearMastery = 1300000,
        }

        public enum DragonKnight : int
        {
            DragonBlood = 1311008,
            DragonRoar = 1311006,
            ElementalResistance = 1310000,
            PowerCrash = 1311007,
            Sacrifice = 1311005,
        }

        public enum DarkKnight : int
        {
            Achilles = 1320005,
            AuraOfBeholder = 1320008,
            Beholder = 1321007,
            Berserk = 1320006,
            HerosWill = 1321010,
            HexOfBeholder = 1320009,
            MapleWarrior = 1321000,
            MonsterMagnet = 1321001,
            PowerStance = 1321002,
        }

        public enum Magician : int
        {
            ImprovedMaxMpIncrease = 2000001,
            MagicArmor = 2001003,
            MagicGuard = 2001002,
        }

        public enum FpWizard : int
        {
            Meditation = 2101001,
            MpEater = 2100000,
            PoisonBreath = 2101005,
            Slow = 2101003,
        }

        public enum FpMage : int
        {
            ElementAmplification = 2110001,
            ElementComposition = 2111006,
            PartialResistance = 2110000,
            PoisonMist = 2111003,
            Seal = 2111004,
            SpellBooster = 2111005,
        }

        public enum FpArchMage : int
        {
            BigBang = 2121001,
            Elquines = 2121005,
            FireDemon = 2121003,
            HerosWill = 2121008,
            Infinity = 2121004,
            ManaReflection = 2121002,
            MapleWarrior = 2121000,
            Paralyze = 2121006,
        }

        public enum IlWizard : int
        {
            ColdBeam = 2201004,
            Meditation = 2201001,
            MpEater = 2200000,
            Slow = 2201003,
        }

        public enum IlMage : int
        {
            ElementAmplification = 2210001,
            ElementComposition = 2211006,
            IceStrike = 2211002,
            PartialResistance = 2210000,
            Seal = 2211004,
            SpellBooster = 2211005,
        }

        public enum IlArchMage : int
        {
            BigBang = 2221001,
            Blizzard = 2221007,
            HerosWill = 2221008,
            IceDemon = 2221003,
            Ifrit = 2221005,
            Infinity = 2221004,
            ManaReflection = 2221002,
            MapleWarrior = 2221000,
        }

        public enum Cleric : int
        {
            Bless = 2301004,
            Heal = 2301002,
            Invincible = 2301003,
            MpEater = 2300000,
        }

        public enum Priest : int
        {
            Dispel = 2311001,
            Doom = 2311005,
            ElementalResistance = 2310000,
            HolySymbol = 2311003,
            MysticDoor = 2311002,
            SummonDragon = 2311006,
        }

        public enum Bishop : int
        {
            Bahamut = 2321003,
            BigBang = 2321001,
            HerosWill = 2321009,
            HolyShield = 2321005,
            Infinity = 2321004,
            ManaReflection = 2321002,
            MapleWarrior = 2321000,
            Resurrection = 2321006,
        }

        public enum Archer : int
        {
            CriticalShot = 3000001,
            Focus = 3001003,
        }

        public enum Hunter : int
        {
            ArrowBomb = 3101005,
            BowBooster = 3101002,
            BowMastery = 3100000,
            SoulArrow = 3101004,
        }

        public enum Ranger : int
        {
            MortalBlow = 3110001,
            Puppet = 3111002,
            SilverHawk = 3111005,
        }

        public enum Bowmaster : int
        {
            Concentrate = 3121008,
            Hamstring = 3121007,
            HerosWill = 3121009,
            Hurricane = 3121004,
            MapleWarrior = 3121000,
            Phoenix = 3121006,
            SharpEyes = 3121002,
            BowExpert = 3120005,
        }

        public enum Crossbowman : int
        {
            CrossbowBooster = 3201002,
            CrossbowMastery = 3200000,
            SoulArrow = 3201004,
        }

        public enum Sniper : int
        {
            Blizzard = 3211003,
            GoldenEagle = 3211005,
            MortalBlow = 3210001,
            Puppet = 3211002,
        }

        public enum Marksman : int
        {
            Blind = 3221006,
            Frostprey = 3221005,
            HerosWill = 3221008,
            MapleWarrior = 3221000,
            PiercingArrow = 3221001,
            SharpEyes = 3221002,
            Snipe = 3221007,
        }

        public enum Rogue : int
        {
            DarkSight = 4001003,
            Disorder = 4001002,
            DoubleStab = 4001334,
            LuckySeven = 4001344,
        }

        public enum Assassin : int
        {
            ClawBooster = 4101003,
            ClawMastery = 4100000,
            CriticalThrow = 4100001,
            Drain = 4101005,
            Haste = 4101004,
        }

        public enum Hermit : int
        {
            Alchemist = 4110000,
            Avenger = 4111005,
            MesoUp = 4111001,
            ShadowMeso = 4111004,
            ShadowPartner = 4111002,
            ShadowWeb = 4111003,
        }

        public enum NightLord : int
        {
            HerosWill = 4121009,
            MapleWarrior = 4121000,
            NinjaAmbush = 4121004,
            NinjaStorm = 4121008,
            ShadowShifter = 4120002,
            ShadowStars = 4121006,
            Taunt = 4121003,
            TripleThrow = 4121007,
            VenomousStar = 4120005,
        }

        public enum Bandit : int
        {
            DaggerBooster = 4201002,
            DaggerMastery = 4200000,
            Haste = 4201003,
            SavageBlow = 4201005,
            Steal = 4201004,
        }

        public enum ChiefBandit : int
        {
            Assaulter = 4211002,
            BandOfThieves = 4211004,
            Chakra = 4211001,
            MesoExplosion = 4211006,
            MesoGuard = 4211005,
            Pickpocket = 4211003,
        }

        public enum Shadower : int
        {
            Assassinate = 4221001,
            BoomerangStep = 4221007,
            HerosWill = 4221008,
            MapleWarrior = 4221000,
            NinjaAmbush = 4221004,
            ShadowShifter = 4220002,
            Smokescreen = 4221006,
            Taunt = 4221003,
            VenomousStab = 4220005,
        }

        public enum Pirate : int
        {
            Dash = 5001005,
        }

        public enum Brawler : int
        {
            BackspinBlow = 5101002,
            CorkscrewBlow = 5101004,
            DoubleUppercut = 5101003,
            ImproveMaxHp = 5100000,
            KnucklerBooster = 5101006,
            KnucklerMastery = 5100001,
            MpRecovery = 5101005,
            OakBarrel = 5101007,
        }

        public enum Marauder : int
        {
            EnergyCharge = 5110001,
            EnergyDrain = 5111004,
            StunMastery = 5110000,
            Transformation = 5111005,
        }

        public enum Buccaneer : int
        {
            Demolition = 5121004,
            MapleWarrior = 5121000,
            HerosWill = 5121008,
            Snatch = 5121005,
            SpeedInfusion = 5121009,
            SuperTransformation = 5121003,
            TimeLeap = 5121010,
        }

        public enum Gunslinger : int
        {
            BlankShot = 5201004,
            Grenade = 5201002,
            GunBooster = 5201003,
            GunMastery = 5200000,
        }

        public enum Outlaw : int
        {
            Flamethrower = 5211004,
            Gaviota = 5211002,
            HomingBeacon = 5211006,
            IceSplitter = 5211005,
            Octopus = 5211001,
        }

        public enum Corsair : int
        {
            AerialStrike = 5221003,
            Battleship = 5221006,
            Bullseye = 5220011,
            ElementalBoost = 5220001,
            Hypnotize = 5221009,
            MapleWarrior = 5221000,
            RapidFire = 5221004,
            HerosWill = 5221010,
            WrathOfTheOctopi = 5220002,
        }

        public enum Gm : int
        {
            Haste = 9001000,
            SuperDragonRoar = 9001001,
            Teleport = 9001007,
        }

        public enum SuperGm : int
        {
            Bless = 9101003,
            Haste = 9101001,
            HealPlusDispel = 9101000,
            Hide = 9101004,
            HolySymbol = 9101002,
            HyperBody = 9101008,
            Resurrection = 9101005,
            SuperDragonRoar = 9101006,
            Teleport = 9101007,
        }
    }
}
