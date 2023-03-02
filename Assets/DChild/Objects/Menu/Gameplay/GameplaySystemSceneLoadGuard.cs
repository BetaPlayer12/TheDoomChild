using DChild.Menu;
using Doozy.Runtime.Signals;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySystemSceneLoadGuard : MonoBehaviour
{
    [SerializeField]
    private SignalSender m_UIInitializeSignal;

    void Awake()
    {
        if (LoadingHandle.isLoading)
        {
            Debug.LogError("Wait for Loading to Be done");
            LoadingHandle.LoadingDone += OnLoadingDone;
        }
        else
        {
            m_UIInitializeSignal.SendSignal();
        }
    }

    private void OnLoadingDone(object sender, EventActionArgs eventArgs)
    {
        m_UIInitializeSignal.SendSignal();
        LoadingHandle.LoadingDone -= OnLoadingDone;
    }

}
