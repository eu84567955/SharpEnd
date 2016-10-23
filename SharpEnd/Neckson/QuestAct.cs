namespace SharpEnd.Neckson
{
    struct QuestAct
    {
        int nIncExp;
        int nIcMoney;
        int nIncPop;
        int nIncPetTameness;
        string sInfo;
        ushort usNextQuest;
        int bPetSpeed;
        int nBuffItemID;
        ActItem[] aActItem;
        ActSkill[] aActSkill;
        string sMsg;
        long[] aMapID;
        string sNpcAction;
    }
}
