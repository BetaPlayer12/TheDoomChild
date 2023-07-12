using DChildDebug;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSaveFileResetter : MonoBehaviour
{
    void Start()
    {
        var m_saveManager = FindObjectOfType<ForceSave>();

        m_saveManager.ResetSaves();
    }
}
