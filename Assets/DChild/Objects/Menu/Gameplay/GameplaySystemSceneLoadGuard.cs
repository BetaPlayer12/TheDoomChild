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
    private SignalSender m_UIGuardSignal;
    [SerializeField]
    private SignalSender m_UIOffenseSignal;

    void Awake()
    {
        if (LoadingHandle.isLoading)
        {
            m_UIGuardSignal.SendSignal();
            LoadingHandle.LoadingDone += OnLoadingDone;
        }
        else
        {
            m_UIOffenseSignal.SendSignal();
        }
    }

    private void OnLoadingDone(object sender, EventActionArgs eventArgs)
    {
        m_UIOffenseSignal.SendSignal();
        LoadingHandle.LoadingDone -= OnLoadingDone;
    }

}
