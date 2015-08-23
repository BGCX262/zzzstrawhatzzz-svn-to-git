using UnityEngine;
using System.Collections;

public enum eLayerName
{
    Card = 8,
    UI = 11,
}

public enum eCardType
{
    Monster = 0,
    Magic = 1    
}

public enum eBoardCellAnimation
{
    Idle = 0,
    IsCanSelect = 1,
}

public enum ePlayerType
{
    Player,
    AI,
    Other
}

public enum eCardStatus
{    
    InRow,
    InView,
    Summon,
    Set,
    Dead
}


