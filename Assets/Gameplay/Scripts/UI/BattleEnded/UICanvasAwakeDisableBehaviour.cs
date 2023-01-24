using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvasAwakeDisableBehaviour : MonoBehaviour
{
    [Header("Objects To Disable")]
    public GameObject[] objectsToDisable;

    void Awake()
    {
        DisableObjects();
    }

    void DisableObjects()
    {
        for(int i = 0; i < objectsToDisable.Length; i++)
        {
            objectsToDisable[i].SetActive(false);
        }
    }
}
