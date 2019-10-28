using DChild.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListDestructible : ISaveData
{
   
    public int IDNum;
    public bool[] BoolValue;
    public int[] intValue;

    public ListDestructible(ListDestructible savedata)
    {
     IDNum = savedata.IDNum;
     BoolValue = savedata.BoolValue;
     intValue = savedata.intValue;
}

}




   
