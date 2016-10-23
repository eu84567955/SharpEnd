namespace SharpEnd
{
    public enum EFieldEffect : byte
    {
        FieldEffect_Summon = 0x0,
        FieldEffect_Tremble = 0x1,
        FieldEffect_Object = 0x2,
        FieldEffect_Object_Disable = 0x3,
        FieldEffect_Screen = 0x4,
        FieldEffect_Sound = 0x5,
        FieldEffect_MobHPTag = 0x6,
        FieldEffect_ChangeBGM = 0x7,
        FieldEffect_BGMVolumeOnly = 0x8,
        FieldEffect_BGMVolume = 0x9,
        FieldEffect_RewordRullet = 0xA,
        FieldEffect_TopScreen = 0xB,
        FieldEffect_Screen_Delayed = 0xC,
        FieldEffect_TopScreen_Delayed = 0xD,
        FieldEffect_Screen_AutoLetterBox = 0xE,
        FieldEffect_FloatingUI = 0xF,
        FieldEffect_Blind = 0x10,
        FieldEffect_GrayScale = 0x11,
        FieldEffect_OnOffLayer = 0x12,
        FieldEffect_Overlap = 0x13,
        FieldEffect_Overlap_Detail = 0x14,
        FieldEffect_Remove_Overlap_Detail = 0x15,
        FieldEffect_ColorChange = 0x16,
        FieldEffect_StageClear = 0x17,
        FieldEffect_TopScreen_WithOrigin = 0x18,
        FieldEffect_SpineScreen = 0x19,
        FieldEffect_OffSpineScreen = 0x1A,
    }
}
