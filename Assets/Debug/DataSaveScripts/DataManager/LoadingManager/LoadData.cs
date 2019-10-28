using DChild.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoadData : ISaveData 
{
    public int listSize;
    public ListManager objectProperties;
}
