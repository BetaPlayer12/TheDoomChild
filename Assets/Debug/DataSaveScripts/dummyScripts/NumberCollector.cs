using DChild.Serialization;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberCollector : MonoBehaviour
{
    public scriptSample SS;

    public int Number;

    ZoneDataHandle saveFiles;


    private void Start()
    {
        GameObject go = GameObject.Find("ZoneHandler");
        saveFiles = go.GetComponent<ZoneDataHandle>();
    }




    [Button]
    public void inputNumber()
    {
        SS.storeData.intValue[0] = Number;
        saveFiles.storeDataFiles(SS);
    }


    [Button]
    public void LoadNumber()
    {
        SS = saveFiles.LoadDataFiles(SS);
         Number = SS.storeData.intValue[0];
    }
}
