namespace SharpEnd
{
    public enum EStatisticType : int
    {
        Skin = 0x1,
        Face = 0x2,
        Hair = 0x4,
        Level = 0x10,
        Job = 0x20,
        Strength = 0x40,
        Dexterity = 0x80,
        Intelligence = 0x100,
        Luck = 0x200,
        CurrentHP = 0x400,
        MaxHP = 0x800,
        CurrentMP = 0x1000,
        MaxMP = 0x2000,
        AvailableAP = 0x4000,
        AvailableSP = 0x8000,
        Experience = 0x10000,
        Fame = 0x20000,
        Meso = 0x40000,
        Pet = 0x180008
    }
}
