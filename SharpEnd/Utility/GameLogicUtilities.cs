using System;

namespace SharpEnd.Utility
{
    public static class GameLogicUtilities
    {
        // Inventory
        public static EInventoryType GetInventory(int itemIdentifier) => (EInventoryType)(itemIdentifier / 1000000);
        public static int GetItemType(int itemIdentifier) => itemIdentifier / 10000;
        public static int GetScrollType(int itemIdentifier) => (itemIdentifier % 10000) - (itemIdentifier % 100);
        public static bool IsArrow(int itemIdentifier) => GetItemType(itemIdentifier) == 201;
        public static bool IsStar(int itemIdentifier) => GetItemType(itemIdentifier) == 207;
        public static bool IsBullet(int itemIdentifier) => GetItemType(itemIdentifier) == 233;
        public static bool IsRechargeable(int itemIdentifier) => IsBullet(itemIdentifier) || IsStar(itemIdentifier) || IsArrow(itemIdentifier);
        public static bool IsEquip(int itemIdentifier) => GetInventory(itemIdentifier) == EInventoryType.Equipment;
        public static bool IsPet(int itemIdentifier) => (itemIdentifier / 100 * 100) == 5000000;
        public static bool IsStackable(int itemIdentifier) => !(IsRechargeable(itemIdentifier) || IsEquip(itemIdentifier) || IsPet(itemIdentifier));
        public static bool IsCashSlot(short slot) => Math.Abs(slot) > 100;
        /*public static bool IsOverall(int itemIdentifier) => GetItemType(itemIdentifier) == (short)Items.Types.ArmorOverall;
        public static bool IsTop(int itemIdentifier) => GetItemType(itemIdentifier) == (short)Items.Types.ArmorTop;
        public static bool IsBottom(int itemIdentifier) => GetItemType(itemIdentifier) == (short)Items.Types.ArmorBottom;
        public static bool IsShield(int itemIdentifier) => GetItemType(itemIdentifier) == (short)Items.Types.ArmorShield;
        public static bool Is1HWeapon(int itemIdentifier) => GetItemType(itemIdentifier) / 10 == 13;
        public static bool Is2HWeapon(int itemIdentifier) => GetItemType(itemIdentifier) / 10 == 14;
        public static bool IsBow(int itemIdentifier) => GetItemType(itemIdentifier) == (short)Items.Types.WeaponBow;
        public static bool IsCrossbow(int itemIdentifier) => GetItemType(itemIdentifier) == (short)Items.Types.WeaponCrossbow;
        public static bool IsSword(int itemIdentifier) => GetItemType(itemIdentifier) == (short)Items.Types.Weapon1hSword || GetItemType(itemIdentifier) == (short)Items.Types.Weapon2hSword;
        public static bool IsMace(int itemIdentifier) => GetItemType(itemIdentifier) == (short)Items.Types.Weapon1hMace || GetItemType(itemIdentifier) == (short)Items.Types.Weapon2hMace;
        public static bool IsMount(int itemIdentifier) => GetItemType(itemIdentifier) == (short)Items.Types.Mount;
        public static bool IsMedal(int itemIdentifier) => GetItemType(itemIdentifier) == (short)Items.Types.Medal;*/
        public static short StripCashSlot(short slot) => (short)(IsCashSlot(slot) ? Math.Abs(slot) - 100 : Math.Abs(slot));

        // Player
        public static byte GetGenderIdentifier(string gender) => (byte)(gender == "male" ? 0 : 1);

        // Player skills
        public static bool IsBeginnerSkill(int skillIdentifier) => (skillIdentifier / 1000000) == (skillIdentifier < 10000000 ? 0 : 10);
        public static bool IsFourthJobSkill(int skillIdentifier) => (skillIdentifier / 10000) % 10 == 2;
        public static sbyte GetMasteryDisplay(byte level) => (sbyte)((level + 1) / 2);
        public static byte GetAdvancementFromSkill(int skillIdentifier)
        {
            int job = skillIdentifier / 10000;
            return (byte)((short)job % 100 == 0 ? 1 : ((short)job % 10) + 2);
        }

        // Mob skills

        // Jobs
        public static bool IsBeginner(ushort job) => job == 0;
        public static bool IsExplorerWarrior(ushort job) => job == 100 || (job >= 110 && job <= 112) || (job >= 120 && job <= 122) || (job >= 130 && job <= 132);
        public static bool IsExplorerMagician(ushort job) => job == 200 || (job >= 210 && job <= 212) || (job >= 220 && job <= 222) || (job >= 230 && job <= 232);
        public static bool IsExplorerBowman(ushort job) => job == 300 || (job >= 310 && job <= 312) || (job >= 320 && job <= 322);
        public static bool IsExplorerThief(ushort job) => job == 400 || (job >= 410 && job <= 412) || (job >= 420 && job <= 422) || (job / 10) == 43;
        public static bool IsExplorerPirate(ushort job) => job == 500 || (job >= 510 && job <= 512) || (job >= 520 && job <= 522);
        public static bool IsJett(ushort job) => (job / 10) == 57 || job == 508;
        public static bool IsMarkOfHonor(ushort job) => (job / 1000) == 4;
        public static bool IsKoC(ushort job) => (job / 1000) == 1;
        public static bool IsResistance(ushort job) => (job / 1000) == 3;
        public static bool IsEvan(ushort job) => (job / 100) == 22 || job == 2001;
        public static bool IsMercedes(ushort job) => (job / 100) == 23 || job == 2002;
        public static bool IsPhantom(ushort job) => (job / 100) == 24 || job == 2003;
        public static bool IsMihile(ushort job) => (job / 1000) == 5;
        public static bool IsLuminous(ushort job) => (job / 100) == 27 || job == 2004;
        public static bool IsNova(ushort job) => (job / 1000) == 6;
        public static bool IsZero(ushort job) => job == 10000 || job == 10100 || (job >= 10110 && job <= 10112);
        public static bool IsShade(ushort job) => job == 10000 || job == 10100 || (job >= 10110 && job <= 10112);
        public static bool IsAran(ushort job) => (job / 100) == 21 || job == 2000;
        public static bool IsBeastTamer(ushort job) => job == 14000 || job == 14200 || (job >= 14210 && job <= 14212);
        public static bool HasSeparatedSkillPoints(ushort job)
        {
            if (IsBeginner(job) ||
                IsExplorerWarrior(job) ||
                IsExplorerMagician(job) ||
                IsExplorerBowman(job) ||
                IsExplorerThief(job) ||
                IsExplorerPirate(job) ||
                IsJett(job) ||
                IsMarkOfHonor(job) ||
                IsKoC(job) ||
                IsResistance(job) ||
                IsEvan(job) ||
                IsMercedes(job) ||
                IsPhantom(job) ||
                IsMihile(job) ||
                IsLuminous(job) ||
                IsNova(job) ||
                IsZero(job) ||
                IsShade(job) ||
                IsAran(job) ||
                IsBeastTamer(job))
            {
                return true;
            }

            return false;
        }
        public static byte GetMaxLevel(ushort job) => 250; // TODO.

        // Monster card
        public static bool IsMonsterCard(int itemIdentifier) => itemIdentifier / 10000 == 287;

        // Map
        public static sbyte GetMapCluster(int mapId) => (sbyte)(mapId / 10000000);

        // Party
    }
}
