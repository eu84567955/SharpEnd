﻿namespace SharpEnd
{
    public enum EStatisticType : ulong
    {
        None = 0x00,
        Skin = 0x1,
        Face = 0x2,
        Hair = 0x4,
        Level = 0x10,
        Job = 0x20,
        Strength = 0x40,
        Dexterity = 0x80,
        Intelligence = 0x100,
        Luck = 0x200,
        Health = 0x400,
        MaxHealth = 0x800,
        Mana = 0x1000,
        MaxMana = 0x2000,
        AbilityPoints = 0x4000,
        SkillPoints = 0x8000,
        Experience = 0x10000,
        Fame = 0x20000,
        Meso = 0x40000,
        Pet = 0x180008
    }
}
