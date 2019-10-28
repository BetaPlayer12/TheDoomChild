using DChild.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListManager : ISaveData
{

    public DataSave SavedObject;
    public GameObject[] ListObject;

}
