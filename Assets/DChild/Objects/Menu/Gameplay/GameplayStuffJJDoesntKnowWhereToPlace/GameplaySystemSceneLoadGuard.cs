using DChild.Menu;
using Doozy.Runtime.Nody;
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
    [SerializeField]
    private FlowController m_uiFlow;

    private bool m_hasFlowStarted = false;

    private IEnumerator InitializeUIRoutine()
    {
        m_uiFlow.onStart.AddListener(SetFlowHasStarted);

        while (m_hasFlowStarted ==false)
        {
            yield return null;
        }
        m_uiFlow.onStart.RemoveListener(SetFlowHasStarted);
        yield return new WaitForSecondsRealtime(1);
        m_UIInitializeSignal.SendSignal();

        void SetFlowHasStarted() => m_hasFlowStarted = true;
    }

    private void Awake()
    {
        if (LoadingHandle.isLoading)
        {
            Debug.LogError("Wait for Loading to Be done");
            LoadingHandle.LoadingDone += OnLoadingDone;
        }
        else
        {
            Debug.LogError("No Wait for Loading to Be done");
            StartCoroutine(InitializeUIRoutine());
        }
    }

    private void OnLoadingDone(object sender, EventActionArgs eventArgs)
    {
        m_UIInitializeSignal.SendSignal();
        LoadingHandle.LoadingDone -= OnLoadingDone;
    }

}
