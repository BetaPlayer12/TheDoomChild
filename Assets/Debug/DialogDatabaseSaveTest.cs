using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDatabaseSaveTest : MonoBehaviour
{
    private string save;
    [Button]
    private void getsave()
    {
        save=PersistentDataManager.GetSaveData();
        Debug.Log("Result:"+save);
    }
}
