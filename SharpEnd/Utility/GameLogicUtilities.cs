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
        public static bool HasSeparatedSkillPoints(ushort job)
        {
            return true;
        }
        /*public static Jobs.JobLines GetJobLine(ushort job) => (Jobs.JobLines)(job / 100);
        public static bool IsBeginnerJob(ushort job) // TODO: Include every beginner job
        {
            return job == 0;
        }*/

        // Monster card

        // Map
        public static sbyte GetMapCluster(int mapId) => (sbyte)(mapId / 10000000);

        // Party
    }
}
