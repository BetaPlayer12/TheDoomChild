using DChild;
using DChild.Gameplay;
using Holysoft.Collections;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTimer : MonoBehaviour
{
    [SerializeField]
    private CountdownTimer m_timer;

    private void Awake()
    {
        m_timer.CountdownEnd += OnCountdownEnd;
        m_timer.Reset();
        enabled = false;
    }

    private void OnCountdownEnd(object sender, EventActionArgs eventArgs)
    {
        ////Auto Reload the Campaign
        //GameSystem.LoadMainMenu();
        //Use This instead of the above when save point is actually saving
        GameplaySystem.ReloadGame();
    }

    private void Update()
    {
        m_timer.Tick(Time.unscaledDeltaTime);
    }
}
