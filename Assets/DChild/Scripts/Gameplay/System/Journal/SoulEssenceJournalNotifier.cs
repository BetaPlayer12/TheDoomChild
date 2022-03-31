using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEssenceJournalNotifier : MonoBehaviour
{
    protected static Player m_player;
    public bool m_notified=false;

   
    protected virtual void OnEnable()
    {
        var currentPlayer = GameplaySystem.playerManager.player;
        if (m_player == null || m_player == currentPlayer)
        {
            m_player = GameplaySystem.playerManager.player;
            m_player.inventory.OnAmountAdded += OnNewSoulEssence;
        }
    }

    private void OnNewSoulEssence(object sender, CurrencyUpdateEventArgs eventArgs)
    {
        if (m_notified == false)
        {
            notify();
        }
        else
        {

        }
    }

    private void notify()
    {
        m_notified = true;
    }

  

}
