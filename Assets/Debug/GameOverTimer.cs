using DChild;
using DChild.Gameplay;
using Doozy.Engine;
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
        m_timer.Reset();
        enabled = false;
        GameplaySystem.campaignSerializer.Load();
        GameEventMessage.SendEvent("Boss Gone");
        GameplaySystem.LoadGame(GameplaySystem.campaignSerializer.slot, DChild.Menu.LoadingHandle.LoadType.Force);
        //GameSystem.LoadMainMenu();
    }

    private void Update()
    {
        m_timer.Tick(Time.unscaledDeltaTime);
    }
}
